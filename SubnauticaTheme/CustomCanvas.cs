using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SubnauticaTheme
{
    public abstract class CustomCanvas: Canvas
    {

        private DrawingContext context;
        private bool isSetup = false;
        private Brush fillBrush;
        private Brush strokeBrush;
        private Brush backgroundBrush;
        private Pen pen;
        private double textSize = 12;

        protected bool runEveryCycle = true;

        private PaintStyle paintStyle = PaintStyle.Fill;

        protected override void OnRender(DrawingContext dc)
        {
            context = dc;

            if (!isSetup)
            {
                fillBrush = new SolidColorBrush();
                strokeBrush = new SolidColorBrush();
                pen = new Pen(strokeBrush, 1);
                backgroundBrush = new SolidColorBrush();
                Setup();
                isSetup = true;
            }

            Draw();

            if (runEveryCycle)
            {
                InvalidateVisual();
            }
        }

        protected abstract void Setup();
        protected abstract void Draw();

        protected Color Color(byte r, byte? g = null, byte? b = null, byte? a = null)
        {
            if (g == null)
            {
                g = r;
            }

            if (b == null)
            {
                b = r;
            }

            if (a == null)
            {
                a = 255;
            }

            return new Color
            {
                R = r,
                G = (byte)g,
                B = (byte)b,
                A = (byte)a
            };
        }

        protected void Clear()
        {
            Background(Color(0, 0, 0, 0));
        }

        protected void Background(Color color)
        {
            backgroundBrush.SetValue(SolidColorBrush.ColorProperty, color);
            context.DrawRectangle(backgroundBrush, null, new System.Windows.Rect(0.0, 0.0, ActualWidth, ActualHeight));
        }

        protected void Fill(Color color)
        {
            paintStyle = ShouldStroke() ? PaintStyle.FillAndStroke : PaintStyle.Fill;
            fillBrush = fillBrush.Clone();
            fillBrush.SetValue(SolidColorBrush.ColorProperty, color);
        }

        protected void Stroke(Color color)
        {
            paintStyle = ShouldFill() ? PaintStyle.FillAndStroke : PaintStyle.Stroke;
            strokeBrush = strokeBrush.Clone();
            strokeBrush.SetValue(SolidColorBrush.ColorProperty, color);
        }

        protected void NoStroke()
        {
            paintStyle = ShouldFill() ? PaintStyle.Fill : PaintStyle.None;
        }

        protected void NoFill()
        {
            paintStyle = ShouldStroke() ? PaintStyle.Stroke : PaintStyle.None;
        }

        protected void StrokeWeight(double pixels)
        {
            pen.Thickness = pixels;
        }

        // Shapes

        protected void Ellipse(double x, double y, double w, double h)
        {
            context.DrawEllipse(ShouldFill() ? fillBrush : null, ShouldStroke() ? pen : null, new System.Windows.Point(x + w / 2, y + h / 2), w / 2, h / 2);
        }

        protected void Circle(double x, double y, double diameter)
        {
            Ellipse(x - diameter / 2, y - diameter / 2, diameter, diameter);
        }

        protected void Rect(double x, double y, double w, double h)
        {
            context.DrawRectangle(ShouldFill() ? fillBrush : null, ShouldStroke() ? pen : null, new System.Windows.Rect(x, y, w, h));
        }

        protected void Square(double x, double y, double size)
        {
            Rect(x, y, size, size);
        }

        protected void Line(double x1, double y1, double x2, double y2)
        {
            context.DrawLine(pen, new System.Windows.Point(x1, y1), new System.Windows.Point(x2, y2));
        }

        protected void Arc(double x, double y, double w, double h, double start, double stop, ArcMode mode = ArcMode.Pie)
        {
            double startRadians = -start * Math.PI / 180.0;
            double sweepRadians = -(stop - start) * Math.PI / 180.0;
            
            double rx = w / 2;
            double ry = h / 2;

            double startX = x + rx + (Math.Cos(startRadians) * rx);
            double startY = y + ry + (Math.Sin(startRadians) * ry);

            double endX = x + rx + (Math.Cos(startRadians + sweepRadians) * rx);
            double endY = y + ry + (Math.Sin(startRadians + sweepRadians) * ry);

            var geometry = new StreamGeometry();
            using (var ctx = geometry.Open())
            {
                bool isLargeArc = Math.Abs(stop - start) > 180;
                var direction = sweepRadians < 0 ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
                ctx.BeginFigure(new System.Windows.Point(startX, startY), mode != ArcMode.Open, mode != ArcMode.Open);
                ctx.ArcTo(new System.Windows.Point(endX, endY), new System.Windows.Size(rx, ry), 0, isLargeArc, direction, ShouldStroke(), false);
                if (mode == ArcMode.Pie)
                {
                    ctx.LineTo(new System.Windows.Point(x + rx, y + ry), ShouldStroke(), false);
                    ctx.LineTo(new System.Windows.Point(startX, startY), ShouldStroke(), false);
                }
            }

            var drawing = new GeometryDrawing();
            drawing.Geometry = geometry;
            drawing.Pen = ShouldStroke() ? pen : null;
            drawing.Brush = ShouldFill() ? fillBrush : null;
            context.DrawDrawing(drawing);
        }

        protected ImageSource LoadImage(Uri uri)
        {
            return new BitmapImage(uri);
        }

        protected void Image(ImageSource img, double x, double y, double w, double h)
        { 
            context.DrawImage(img, new System.Windows.Rect(x, y, w, h));
        }

        protected void TextSize(double size)
        {
            textSize = size;
        }

        protected double TextWidth(string str)
        {
            var formattedText = new FormattedText(str, System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Sans Serif"), textSize, fillBrush);
            return formattedText.Width;
        }

        protected double TextHeight(string str)
        {
            var formattedText = new FormattedText(str, System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Sans Serif"), textSize, fillBrush);
            return formattedText.Height;
        }

        protected void Text(string str, double x, double y, bool useCenter = false)
        {
            var formattedText = new FormattedText(str, System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Sans Serif"), textSize, fillBrush);
            x = useCenter ? x - formattedText.Width / 2 : x;
            y = useCenter ? y - formattedText.Height / 2 : y;
            context.DrawText(formattedText, new System.Windows.Point(x, y));
        }

        private bool ShouldStroke()
        {
            return paintStyle == PaintStyle.FillAndStroke || paintStyle == PaintStyle.Stroke;
        }

        private bool ShouldFill()
        {
            return paintStyle == PaintStyle.FillAndStroke || paintStyle == PaintStyle.Fill;
        }


        protected enum PaintStyle
        {
            Fill,
            Stroke,
            FillAndStroke,
            None
        }

        protected enum ArcMode
        {
            Pie,
            Open,
            Chord
        }


    }
}
