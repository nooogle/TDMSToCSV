using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMSToCSV
{
    internal class DataAnalyser1
    {
        const int rawPuckSamplesIntervalMS = 8;
        const int approximatePeriodOfPeaksS = 15;

        /// <summary>
        /// We get a sample every (subSampleEvery * rawPuckSamplesIntervalMS) milliseconds
        /// </summary>
        const int subSampleEvery = 25; 

        struct Sample
        {
            public readonly int ms;
            public readonly float uv;

            public Sample(int ms, float uv)
            {
                this.ms = ms;
                this.uv = uv;
            }

            public override string ToString() => $"{ms}, {uv}";
        }

        public static void Test()
        {
            var fullPath = @"C:\Users\jon\Dropbox\CustomerLines\CLTUV001\Profiler puck data files\2023-04-24\Bottom\20232404_1_bottom.tdms";

            var fileNamePart = Path.GetFileNameWithoutExtension(fullPath);
            (var uvaChannel, var _) = UVAChannelBuilder.Build(fullPath);

            new DataAnalyser1().Go(uvaChannel);
        }

        public void Go(UVAChannel uvaChannel)
        {
            List<float> samples = GenerateSubSamples(uvaChannel);
            var peaks = FindPeaks(samples);
        }

        private static List<float> GenerateSubSamples(UVAChannel uvaChannel)
        {
            // A sample is generated every 8ms, we'll take every 25th sample giving us
            // data at a rate of 5 per second

            int subSampleCounter = 0;

            List<float> samples = new List<float>();

            const float minUVReading = 0.5f;
            int indexOfFirstUsableSample = uvaChannel.Samples.Length;

            for(int sampleIndex = 0; sampleIndex < uvaChannel.Samples.Length; sampleIndex++)
            {
                if (uvaChannel.Samples[sampleIndex].MWPerCM2 >= minUVReading)
                {
                    indexOfFirstUsableSample = sampleIndex;
                    break;
                }
            }

            for (int sampleIndex = indexOfFirstUsableSample; sampleIndex < uvaChannel.Samples.Length; sampleIndex++)
            {
                var puckSample = uvaChannel.Samples[sampleIndex];

                if (puckSample.MWPerCM2 < minUVReading)
                {
                    break;
                }

                subSampleCounter++;
                if (subSampleCounter == subSampleEvery)
                {
                    samples.Add(puckSample.MWPerCM2);
                    subSampleCounter = 0;
                }
            }

            return samples;
        }


        private List<int> FindPeaks(List<float> data)
        {
            var samplesPerSecond = 1000 / (subSampleEvery * rawPuckSamplesIntervalMS);
            int windowSize = approximatePeriodOfPeaksS * samplesPerSecond;

            List<int> peakIndices = new List<int>();

            for (int i = windowSize / 2; i < data.Count - (windowSize / 2); i++)
            {
                int windowStart = i - windowSize / 2;

                var window = data.GetRange(windowStart, windowSize);

                int maxIndex = window.IndexOf(window.Max());
                int globalMaxIndex = windowStart + maxIndex;

                if ((globalMaxIndex == i) && 
                    (data[globalMaxIndex] >= data[globalMaxIndex - 1]) && 
                    (data[globalMaxIndex] >= data[globalMaxIndex + 1]))
                {
                    peakIndices.Add(globalMaxIndex);
                }
            }

            return peakIndices;
        }
    }
}
