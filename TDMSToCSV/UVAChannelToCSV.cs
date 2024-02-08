using System;
using System.IO;

namespace TDMSToCSV
{
    public static class UVAChannelToCSV
    {
        public static void Export(
            UVAChannel uvaChannel,
            UVAChannelExportMode exportMode,
            string fileName)
        {
            using (var streamWriter = File.CreateText(fileName))
            {
                switch(exportMode)
                {
                    case UVAChannelExportMode.Raw: 
                        ExportRawSamples(uvaChannel, streamWriter);
                        break;

                    case UVAChannelExportMode.NSamplesPerSecond:
                        ExportNPerSecond(uvaChannel, streamWriter);
                        break;

                    case UVAChannelExportMode.Summary:
                        ExportSummary(uvaChannel, streamWriter);
                        break;
                }
            }
        }

        private static void ExportSummary(UVAChannel uvaChannel, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"Duration (s), {uvaChannel.Duration.TotalSeconds}");
            streamWriter.WriteLine($"Peak irradiance (mW/cm²), {uvaChannel.PeakIrradianceMWPerCM2}");
            streamWriter.WriteLine($"Energy density (mJ/cm²), {uvaChannel.EnergyDensityMJPerCM2}");
            streamWriter.WriteLine($"Total samples, {uvaChannel.Samples.Length}");
        }

        private static void ExportNPerSecond(UVAChannel uvaChannel, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"Time (ms), Time (s), Time, mW/cm²");

            const int subSampleEvery = 10;
            int sampleCount = 0;

            foreach (var sample in uvaChannel.Samples)
            {
                sampleCount++;
                if((sampleCount % subSampleEvery) == 0)
                {
                    WriteOneSampleToFile(sample, streamWriter);
                }
            }
        }


        private static void ExportRawSamples(UVAChannel uvaChannel, StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"Time (ms), Time (s), Time, mW/cm²");

            foreach (var sample in uvaChannel.Samples)
            {
                WriteOneSampleToFile(sample, streamWriter);
            }
        }

        private static void WriteOneSampleToFile(
            UVASample sample,
            StreamWriter streamWriter)
        {
            streamWriter.WriteLine(
                $"{sample.IntervalSinceStart.TotalMilliseconds}, " +
                $"{sample.IntervalSinceStart.TotalSeconds}, " +
                $"{sample.IntervalSinceStart:hh\\:mm\\:ss\\.fff}, " +
                $"{sample.MWPerCM2}");
        }
    }
}
