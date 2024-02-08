using System;

namespace TDMSToCSV
{
    public struct UVASample
    {
        public TimeSpan IntervalSinceStart { get; } 
        public float MWPerCM2 { get; }


        public override string ToString() => $"{IntervalSinceStart.TotalMilliseconds:0.000}ms, {MWPerCM2} mW/cm²";


        public UVASample(TimeSpan intervalSinceStart, float mwPerCM2)
        {
            IntervalSinceStart = intervalSinceStart;
            MWPerCM2 = mwPerCM2;
        }
    }
}
