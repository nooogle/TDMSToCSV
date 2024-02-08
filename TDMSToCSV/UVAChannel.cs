using System;
using System.Collections.Immutable;

namespace TDMSToCSV
{
    public class UVAChannel
    {
        public double EnergyDensityMJPerCM2 { get; }
        public double PeakIrradianceMWPerCM2 { get; }
        public ImmutableArray<UVASample> Samples { get; }
        public TimeSpan Duration { get; }


        public override string ToString() => 
            $"Peak {PeakIrradianceMWPerCM2:0.0} mW/cm², " +
            $"dose {EnergyDensityMJPerCM2:0.0} mJ/cm², " +
            $"duration {Duration.TotalMilliseconds:0.0} ms";



        public UVAChannel(
            double energyDensityMJPerCM2, 
            double peakIrradianceMWPerCM2,
            ImmutableArray<UVASample> samples)
        {
            EnergyDensityMJPerCM2 = energyDensityMJPerCM2;
            PeakIrradianceMWPerCM2 = peakIrradianceMWPerCM2;
            Samples = samples;

            Duration = TimeSpan.Zero;
            if(samples.Length > 0)
            {
                Duration = samples[samples.Length - 1].IntervalSinceStart;
            }
        }
    }
}
