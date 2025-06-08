using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Data;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.PresentationView
{
    class RgbToBrushConverter : IValueConverter
    {
        public object Convert(object v, Type _, object __, CultureInfo ___)
        {
            var rgb = v is Rgb c ? c : new Rgb(0, 0, 0);
            return new SolidColorBrush(Color.FromRgb(rgb.R, rgb.G, rgb.B));
        }
        public object ConvertBack(object v, Type _, object __, CultureInfo ___)
            => throw new NotSupportedException();
    }
}
