using System;
using System.Timers;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Reflection;
using System.IO;

namespace SubnauticaTheme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly Battery battery;
        private readonly Processor processor;
        private readonly Memory memory;
        private readonly Disk storage;
        private readonly Weather weather;

        private DateTime lastWeatherCall = DateTime.MinValue;
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOACTIVATE = 0x0010;
        const UInt32 SWP_NOOWNERZORDER = 0x0200;
        const UInt32 SWP_NOSENDCHANGING = 0x0400;

        IntPtr windowHandle;

        public MainWindow()
        {
            battery = new Battery();
            processor = new Processor();
            memory = new Memory();
            storage = new Disk();
            weather = new Weather(GetStation("KPVD"));
            InitializeComponent();
        }

        private string GetStation(string defaultStation)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Subnautica Theme\station.txt");

            try
            {
                if (!File.Exists(path))
                {
                    SetStation(defaultStation);
                }
                var station = File.ReadAllText(path).Trim();
                if (string.IsNullOrEmpty(station))
                {
                    SetStation(defaultStation);
                    return defaultStation;
                }
                return station;
            } catch (Exception)
            {
                return defaultStation;
            }
        }

        private void SetStation(string station)
        {
            try
            {
                var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Subnautica Theme");
                Directory.CreateDirectory(directory);
                var path = Path.Combine(directory, @"station.txt");
                var writer = File.CreateText(path);
                writer.Write(station);
                writer.Close();
            } catch (Exception)
            {
                // Do nothing
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // https://stackoverflow.com/questions/357076/best-way-to-hide-a-window-from-the-alt-tab-program-switcher
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);


            windowHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            SendToBottom();
            var timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Canvas2.Dispatcher.Invoke(() => {
                if (DateTime.Now - lastWeatherCall > TimeSpan.FromMinutes(5))
                {
                    weather.Update().Wait();
                    lastWeatherCall = DateTime.Now;
                }

                Canvas2.percentages[0] = 1 - storage.GetPercentFree() / 100.0;
                Canvas2.percentages[1] = 1 - processor.GetUtilizationPercent() / 100.0;
                Canvas2.percentages[2] = 1 - memory.GetUtilizationPercent() / 100.0;
                Canvas2.percentages[3] = battery.GetBatteryPercentage() / 100.0;
                Canvas2.temperature = Math.Round((weather.temperature * 9 / 5.0) + 32);
                Canvas2.pressure = Math.Round(weather.pressure);
                Canvas2.humidity = Math.Round(weather.humidity);

                Canvas2.InvalidateVisual();
            });
        }

        private void SendToBottom()
        {
            SetWindowPos(windowHandle, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_NOSENDCHANGING);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            SendToBottom();
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

        #region Window styles
        [Flags]
        public enum ExtendedWindowStyles
        {
            WS_EX_TOOLWINDOW = 0x00000080,
        }

        public enum GetWindowLongFields
        {
            GWL_EXSTYLE = (-20),
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion
    }
}
