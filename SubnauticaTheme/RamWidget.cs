using System.Windows.Media;

namespace SubnauticaTheme
{
    class RamWidget : PieChartWidget
    {
        private readonly Memory memory;

        public RamWidget(Color color, double size, double x, double y) : base(color, size, x, y, "/Images/memory.png")
        {
            memory = new Memory();
        }

        public override double GetPercentage()
        {
            return 1 - memory.GetUtilizationPercent() / 100.0;
        }

        public override long GetUpdateFrequency()
        {
            return 1000;
        }

        

    }
}
