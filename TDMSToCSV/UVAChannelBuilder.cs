using System;
using System.Collections.Immutable;
using System.ComponentModel;

namespace TDMSToCSV
{
    public static class UVAChannelBuilder
    {
        public static (UVAChannel, IDictionary<string, object>) Build(string path)
        {
            UVAChannel uvaChannel;
            IDictionary<string, object> properties;

            using (var tdmsFile = new NationalInstruments.Tdms.File(path))
            {
                tdmsFile.Open();
                properties = tdmsFile.Properties;

                var group = tdmsFile.Groups["UV Signals"];
                var channel = group.Channels["UVA"];

                double secondsIncrementPerSample = (double)channel.Properties["wf_increment"];
                var numSamples = (int)channel.Properties["wf_samples"];
                var samples = new UVASample[numSamples];

                int index = 0;
                foreach (var wattsPerCM2 in channel.GetData<float>())
                {
                    samples[index] = new UVASample(
                        intervalSinceStart: TimeSpan.FromSeconds(index * secondsIncrementPerSample),
                        mwPerCM2: wattsPerCM2 * 1000.0f);

                    index++;
                }

                uvaChannel = new UVAChannel(
                    energyDensityMJPerCM2: (float)channel.Properties["Energy Density"] * 1000.0f,
                    peakIrradianceMWPerCM2: (float)channel.Properties["Peak Irradiance"] * 1000.0f,
                    samples: samples.ToImmutableArray());
            }

            return (uvaChannel, properties);
        }
    }
}
