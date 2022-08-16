using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ITHome.Converters
{
    internal class NullToVisibilityInversionConverter  : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            /*string defaultInvisibility = parameter as string;
            Visibility invisibility = (defaultInvisibility != null) ?
                (Visibility)Enum.Parse(typeof(Visibility), defaultInvisibility)
                : Visibility.Collapsed;
            return value == null? invisibility : Visibility.Visible;*/
            if (value is List<string> list)
            {
                if (list.Count == 0) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
            if (value == null || value.ToString() == "")
            {
                return Visibility.Visible;
            }
            else
                return Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return DependencyProperty.UnsetValue;
        }

    }
}
