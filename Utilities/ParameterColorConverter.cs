using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace WPFApplication.Utilities
{
    public class ParameterColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new SolidColorBrush(Colors.White);
            var param = (Parameter)value;
            if (param.StorageType == StorageType.String) 
                return new SolidColorBrush(Color.FromRgb( 114, 245, 66)) ; 
            if (param.StorageType == StorageType.Integer) 
                return new SolidColorBrush(Color.FromRgb( 245, 173, 66)); 
            if (param.StorageType == StorageType.Double) 
                return new SolidColorBrush(Color.FromRgb(66, 150, 245)); 
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }    
}
