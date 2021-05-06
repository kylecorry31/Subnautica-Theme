using System.Windows.Media;

namespace SubnauticaTheme
{
    class CPUPercentageWidget : PieChartWidget
    {
        private readonly Processor processor;

        public CPUPercentageWidget(Color color, double size, double x, double y) : base(color, size, x, y, "/Images/chip.png")
        {
            processor = new Processor();
        }

        public override double GetPercentage()
        {
            return 1 - processor.GetUtilizationPercent() / 100.0;
        }

        public override long GetUpdateFrequency()
        {
            return 1000;
        }
    }
}
