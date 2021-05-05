using System;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
           
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Main_Canvas.Dispatcher.Invoke(
                () =>
                {
                    var red = new Color
                    {
                        R = 220,
                        G = 95,
                        B = 60,
                        A = 255
                    };
                    var random = new Random();
                    Main_Canvas.Children.Clear();
                    DrawPieChart(Main_Canvas.ActualHeight - 260.0, 95.0, 120, random.Next(360), red);
                    DrawPieChart(Main_Canvas.ActualHeight - 220.0, 32.0, 60, random.Next(360), red);
                });
        }


        private void DrawPieChart(double top, double left, double size, double angle, Color color)
        {
            var background = new Color
            {
                R = 105,
                G = 190,
                B = 190,
                A = 200
            };

            var strokeSize = size / 4;

            var actualAngle = angle - 90.0;

            var angleRadians = actualAngle * Math.PI / 180.0;

            var endX = left + size / 2 + (Math.Cos(angleRadians) * size / 2) - (Math.Cos(angleRadians) * strokeSize / 2);
            var endY = top + size / 2 + (Math.Sin(angleRadians) * size / 2) - (Math.Sin(angleRadians) * strokeSize / 2);

            Ellipse newEllipse = new Ellipse();
            newEllipse.Width = size;
            newEllipse.Height = size;
            newEllipse.Fill = new SolidColorBrush(background);
            newEllipse.SetValue(Canvas.LeftProperty, left);
            newEllipse.SetValue(Canvas.TopProperty, top);
            Main_Canvas.Children.Add(newEllipse);

            var g = new StreamGeometry();

            using (var gc = g.Open())
            {
                gc.BeginFigure(
                    startPoint: new Point(left + size / 2, top + strokeSize / 2),
                    isFilled: false,
                    isClosed: false);

                gc.ArcTo(
                    point: new Point(endX, endY),
                    size: new Size(size / 2 - strokeSize / 2, size / 2 - strokeSize / 2),
                    rotationAngle: 0d,
                    isLargeArc: angle > 180,
                    sweepDirection: SweepDirection.Clockwise,
                    isStroked: true,
                    isSmoothJoin: false);
            }

            var path = new Path
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = strokeSize,
                Data = g
            };

            Main_Canvas.Children.Add(path);

            Ellipse center = new Ellipse();
            center.Width = size / 2;
            center.Height = size / 2;
            center.Fill = new SolidColorBrush(background);
            center.SetValue(Canvas.LeftProperty, left + size / 4);
            center.SetValue(Canvas.TopProperty, top + size / 4);
            Main_Canvas.Children.Add(center);
        }

    }
}
