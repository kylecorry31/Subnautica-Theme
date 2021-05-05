using System.Windows.Controls;

namespace SubnauticaTheme
{
    interface IWidget
    {

        public void Update();

        public void Draw(Canvas canvas);

        public long GetUpdateFrequency();

    }
}
