using System.Windows.Media;

namespace SubnauticaTheme
{
    class BatteryWidget : PieChartWidget
    {

        private readonly Battery battery;

        public BatteryWidget(Color color, double size, double x, double y) : base(color, size, x, y, "/Images/battery.png")
        {
            battery = new Battery();
        }

        public override double GetPercentage()
        { 
            return battery.GetBatteryPercentage() / 100.0;

        }

        public override long GetUpdateFrequency()
        {
            return 1000;
        }
    }
}
