using System;
using System.Diagnostics;
using System.Management;
using System.Windows.Media;

namespace SubnauticaTheme
{
    class BatteryWidget : PieChartWidget
    {
        public BatteryWidget(Color color, double size, double x, double y) : base(color, size, x, y)
        {
        }

        public override double GetPercentage()
        {
            ManagementClass wmi = new ManagementClass("Win32_Battery");
            ManagementObjectCollection allBatteries = wmi.GetInstances();

            double batteryLevel = 0;

            foreach (var battery in allBatteries)
            {
                batteryLevel = Convert.ToDouble(battery["EstimatedChargeRemaining"]);
            }

            return batteryLevel / 100.0;

        }

        public override long GetUpdateFrequency()
        {
            return 1000;
        }
    }
}
