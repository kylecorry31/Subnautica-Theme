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
            fillBrush.SetValue(SolidColorBrush.ColorProperty, color);
        }

        protected void Stroke(Color color)
        {
            paintStyle = ShouldFill() ? PaintStyle.FillAndStroke : PaintStyle.Stroke;
            strokeBrush.SetValue(SolidColorBrush.ColorProperty, color);
        }

        protected void NoStroke(Color color)
        {
            paintStyle = ShouldFill() ? PaintStyle.Fill : PaintStyle.None;
        }

        protected void NoFill(Color color)
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
            context.DrawEllipse(ShouldFill() ? fillBrush : null, ShouldStroke() ? pen : null, new System.Windows.Point(x, y), w / 2, h / 2);
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

        protected BitmapImage LoadImage(Uri uri)
        {
            return new BitmapImage(uri);
        }

        protected void Image(BitmapImage image, double x, double y, double w, double h)
        {
            context.DrawImage(image, new System.Windows.Rect(x, y, w, h));
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
        

    }
}
