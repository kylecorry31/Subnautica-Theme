using System;
using System.Diagnostics;
using System.Windows.Media;

namespace SubnauticaTheme
{
    class CPUPercentageWidget : PieChartWidget
    {
        private readonly PerformanceCounter cpuCounter;

        public CPUPercentageWidget(Color color, double size, double x, double y) : base(color, size, x, y)
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total", Environment.MachineName);
        }

        public override double GetPercentage()
        {
            return 1 - cpuCounter.NextValue() / 100.0;
        }

        public override long GetUpdateFrequency()
        {
            return 1000;
        }
    }
}
