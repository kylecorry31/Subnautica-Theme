using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SubnauticaTheme
{
    abstract class PieChartWidget : IWidget
    {

        private double _percentage;
        private double _size;
        private Color _color;
        private double _x;
        private double _y;

        public PieChartWidget(Color color, double size, double x, double y)
        {
            _size = size;
            _color = color;
            _x = x;
            _y = y;
        }

        public void Draw(Canvas canvas)
        {
            DrawPieChart(canvas, _y, _x, _size, 360 * _percentage, _color);
        }

        public void Update()
        {
            _percentage = GetPercentage();
        }

        public abstract long GetUpdateFrequency();
        public abstract double GetPercentage();

        private void DrawPieChart(Canvas canvas, double top, double left, double size, double angle, Color color)
        {
            var background = new Color
            {
                R = 105,
                G = 190,
                B = 190,
                A = 200
            };

            var strokeSize = size / 4;

            var actualAngle = Math.Min(angle, 359.5) - 90.0;

            var angleRadians = actualAngle * Math.PI / 180.0;

            var endX = left + size / 2 + (Math.Cos(angleRadians) * size / 2) - (Math.Cos(angleRadians) * strokeSize / 2);
            var endY = top + size / 2 + (Math.Sin(angleRadians) * size / 2) - (Math.Sin(angleRadians) * strokeSize / 2);

            Ellipse newEllipse = new Ellipse();
            newEllipse.Width = size;
            newEllipse.Height = size;
            newEllipse.Fill = new SolidColorBrush(background);
            newEllipse.SetValue(Canvas.LeftProperty, left);
            newEllipse.SetValue(Canvas.TopProperty, top);
            canvas.Children.Add(newEllipse);

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

            var path = new System.Windows.Shapes.Path
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = strokeSize,
                Data = g
            };

            canvas.Children.Add(path);

            Ellipse center = new Ellipse();
            center.Width = size / 2;
            center.Height = size / 2;
            center.Fill = new SolidColorBrush(background);
            center.SetValue(Canvas.LeftProperty, left + size / 4);
            center.SetValue(Canvas.TopProperty, top + size / 4);
            canvas.Children.Add(center);

            /*
             * 
             * BitmapImage img = new BitmapImage (new Uri ("c:\\demo.jpg"));
      dc.DrawImage (img, new Rect (0, 0, img.PixelWidth, img.PixelHeight));
             */
        }

    }
}
