using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SubnauticaTheme
{
    class SubnauticaCanvas : CustomCanvas
    {

        public SubnauticaCanvas()
        {
            runEveryCycle = false;
        }

        private ImageSource memoryImage;
        private ImageSource batteryImage;
        private ImageSource processorImage;
        private ImageSource storageImage;
        private ImageSource topPanel;
        private ImageSource rightPanel;

        public double[] percentages = new double[] { 0, 0, 0, 0 };
        public double temperature = 0;
        public double pressure = 0;
        public double humidity = 0;

        protected override void Draw()
        {
            Clear();

            var red = Color(220, 95, 60);
            var green = Color(145, 215, 60);
            var yellow = Color(250, 170, 35);
            var blue = Color(50, 170, 215);

            var w = ActualWidth;
            var h = ActualHeight + 14;

            DonutChart(0.02 * w + 30, h - 3.5 * 72 + 40, 60, percentages[0] * 360, red, storageImage);
            DonutChart(0.02 * w + 60, h - 2 * 72 + 10, 60, percentages[1] * 360, yellow, processorImage);
            DonutChart(0.02 * w + 140, h - 1.5 * 72, 60, percentages[2] * 360, blue, memoryImage);
            DonutChart(0.02 * w + 140, h - 3.5 * 72 + 40, 120, percentages[3] * 360, green, batteryImage);

            TopPanel(w / 2, topPanel.Height * 1.5, topPanel.Width * 3, topPanel.Height * 3);
            RightPanel(w - rightPanel.Width * 0.5 - 16, h - rightPanel.Height / 2 - 1.5 * 72, rightPanel.Width, rightPanel.Height);
        }

        private void TopPanel(double x, double y, double w, double h)
        {
            Image(topPanel, x - w / 2, y - h / 4, w, h);
            Fill(Color(255));
            TextSize(48);
            Text(DateTime.Now.ToShortTimeString(), x, y + 8, true);
        }

        private void RightPanel(double x, double y, double w, double h)
        {
            Image(rightPanel, x - w / 2, y - h / 4, w, h);
            Fill(Color(255));
            TextSize(28);

            var pressureWidth = TextWidth(pressure.ToString());
            var pressureHeight = TextHeight(pressure.ToString());
            var temperatureWidth = TextWidth(temperature.ToString());
            var temperatureHeight = TextHeight(temperature.ToString());
            var humidityWidth = TextWidth(humidity.ToString());
            var humidityHeight = TextHeight(humidity.ToString());

            Text(pressure.ToString(), x - w * 0.4, y + h * 0.2 - pressureHeight / 2, false);
            Text(temperature.ToString(), x + w * 0.1, y + h * 0.55 - temperatureHeight / 2, false);
            Text(humidity.ToString(), x + w * 0.15, y - humidityHeight / 2, false);

            Fill(Color(255, 255, 0));
            Text("hPa", x - w * 0.4 + pressureWidth + 8, y + h * 0.2 - pressureHeight / 2, false);
            Text("°F", x + w * 0.1 + temperatureWidth + 8, y + h * 0.55 - temperatureHeight / 2, false);
            Text("%", x + w * 0.15 + humidityWidth + 8, y - humidityHeight / 2, false);

        }

        private void DonutChart(double x, double y, double size, double angle, Color color, ImageSource img)
        {
            var background = Color(105, 190, 190, 200);
            var foreground = Color(105, 190, 190);

            angle = Math.Min(359.9, angle);

            Fill(background);
            Circle(x, y, size);
            Fill(color);
            Arc(x - size / 2, y - size / 2, size, size, 90, 90 - angle, ArcMode.Pie);
            Fill(foreground);
            Circle(x, y, size / 2);
            Image(img, x - size / 6.0, y - size / 6.0, size / 3.0, size / 3.0);
        }

        protected override void Setup()
        {
            NoStroke();
            memoryImage = LoadImage(new Uri("pack://application:,,/Images/memory.png", UriKind.RelativeOrAbsolute));
            batteryImage = LoadImage(new Uri("pack://application:,,/Images/battery.png", UriKind.RelativeOrAbsolute));
            storageImage = LoadImage(new Uri("pack://application:,,/Images/storage.png", UriKind.RelativeOrAbsolute));
            processorImage = LoadImage(new Uri("pack://application:,,/Images/chip.png", UriKind.RelativeOrAbsolute));
            topPanel = LoadImage(new Uri("pack://application:,,/Images/top_panel.png", UriKind.RelativeOrAbsolute));
            rightPanel = LoadImage(new Uri("pack://application:,,/Images/right_panel.png", UriKind.RelativeOrAbsolute));
        }
    }
}
