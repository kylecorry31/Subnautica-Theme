using System;
using System.Diagnostics;

namespace SubnauticaTheme
{
    public class Processor
    {

        private readonly PerformanceCounter counter;

        public Processor()
        {
            counter = new PerformanceCounter("Processor", "% Processor Time", "_Total", Environment.MachineName);
        }

        public double GetUtilizationPercent()
        {
            return counter.NextValue();
        }


    }
}
