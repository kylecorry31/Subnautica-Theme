using System.Windows.Media;

namespace SubnauticaTheme
{
    class DiskUsageWidget : PieChartWidget
    {

        private readonly Disk disk;

        public DiskUsageWidget(Color color, double size, double x, double y) : base(color, size, x, y, "/Images/storage.png")
        {
            disk = new Disk();
        }

        public override double GetPercentage()
        {
            return 1 - disk.GetPercentFree() / 100.0;
        }

        public override long GetUpdateFrequency()
        {
            return 15000;
        }
    }
}
