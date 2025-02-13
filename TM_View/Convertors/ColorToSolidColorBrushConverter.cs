using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace TM_View.Convertors
{
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        // Convert Color to SolidColorBrush
        public object Convert(object value, Type targetType, object parameter, string language)
        {
       
                return new SolidColorBrush(ConvertToUIColor((System.Drawing.Color)value));
            
          
        }
        private System.Drawing.Color ConvertToDrawingColor(Windows.UI.Color uiColor)
        {
            return System.Drawing.Color.FromArgb(uiColor.A, uiColor.R, uiColor.G, uiColor.B);
        }

        private Windows.UI.Color ConvertToUIColor(System.Drawing.Color drawingColor)
        {
            return Windows.UI.Color.FromArgb(
                drawingColor.A,
                drawingColor.R,
                drawingColor.G,
                drawingColor.B
            );
        }

        // Convert SolidColorBrush back to Color (optional, if needed for two-way binding)
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is SolidColorBrush brush)
            {
                return brush.Color;
            }
            return Colors.Transparent; // Default fallback color
        }
    }
}
