using System.IO;
using System.Linq;
using System.Windows.Media;

namespace SubnauticaTheme
{
    class DiskUsageWidget : PieChartWidget
    {

        public DiskUsageWidget(Color color, double size, double x, double y) : base(color, size, x, y)
        {
        }

        public override double GetPercentage()
        {
            var drives = DriveInfo.GetDrives();
            var total = drives.Sum(it => it.TotalSize);
            var free = drives.Sum(it => it.AvailableFreeSpace);

            return 1 - free / (double)total;
        }

        public override long GetUpdateFrequency()
        {
            return 15000;
        }
    }
}
