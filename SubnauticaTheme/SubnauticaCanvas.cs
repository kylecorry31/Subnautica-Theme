using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace SubnauticaTheme
{
    class SubnauticaCanvas : CustomCanvas
    {

        public SubnauticaCanvas()
        {
            runEveryCycle = false;
        }

        private BitmapImage image;

        protected override void Draw()
        {
            Clear();
            Fill(Color(0));
            Stroke(Color(255));
        }

        protected override void Setup()
        {
        }
    }
}
