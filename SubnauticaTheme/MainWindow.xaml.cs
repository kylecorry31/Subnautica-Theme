using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SubnauticaTheme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<IWidget> widgets = new List<IWidget>();

        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var red = new Color
            {
                R = 220,
                G = 95,
                B = 60,
                A = 255
            };

            var green = new Color
            {
                R = 145,
                G = 215,
                B = 60,
                A = 255
            };

            var yellow = new Color
            {
                R = 250,
                G = 170,
                B = 35,
                A = 255
            };

            var blue = new Color
            {
                R = 50,
                G = 170,
                B = 215,
                A = 255
            };


            var w = Main_Canvas.ActualWidth;
            var h = Main_Canvas.ActualHeight - 16;

            // 1
            widgets.Add(new DiskUsageWidget(red, 60, 0.02 * w, h - 3 * 72));

            // 2
            widgets.Add(new CPUPercentageWidget(yellow, 60, 0.02 * w + 30, h - 2 * 72));

            // 3
            widgets.Add(new RamWidget(blue, 60, 0.02 * w + 100, h - 1.5 * 72));

            // Main
            widgets.Add(new BatteryWidget(green, 120, 0.02 * w + 60 + 16, h - 3.5 * 72));

            var timer = new Timer(widgets.Min(it => it.GetUpdateFrequency()));
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Main_Canvas.Dispatcher.Invoke(
                () =>
                {
                    
                    Main_Canvas.Children.Clear();
                    foreach (var widget in widgets)
                    {
                        widget.Update();
                        widget.Draw(Main_Canvas);
                    }

                });
        }

    }
}
