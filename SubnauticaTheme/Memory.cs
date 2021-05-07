using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SubnauticaTheme
{
    public class Memory
    {

        private readonly PerformanceCounter counter;


        public Memory()
        {
            counter = new PerformanceCounter("Memory", "Available MBytes", true);
        }


        public double GetUtilizationPercent()
        {
            ulong installedMemory = 1;
            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memStatus))
            {
                installedMemory = memStatus.ullTotalPhys;
            }

            return 100 - (100 * counter.NextValue() / ((double)installedMemory / (1024 * 1024)));
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

    }
}
