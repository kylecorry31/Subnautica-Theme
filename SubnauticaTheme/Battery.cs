using System;
using System.Management;

namespace SubnauticaTheme
{
    public class Battery
    {

        public double GetBatteryPercentage()
        {
            var wmi = new ManagementClass("Win32_Battery");
            var allBatteries = wmi.GetInstances();

            double batteryLevel = 0;

            foreach (var battery in allBatteries)
            {
                batteryLevel = Convert.ToDouble(battery["EstimatedChargeRemaining"]);
            }

            return batteryLevel;
        }

    }
}
