using System.IO;
using System.Linq;

namespace SubnauticaTheme
{
    public class Disk
    {

        public double GetPercentFree()
        {
            var drives = DriveInfo.GetDrives();
            var total = drives.Sum(it => it.TotalSize);
            var free = drives.Sum(it => it.AvailableFreeSpace);

            return 100 * free / (double)total;
        }

    }
}
