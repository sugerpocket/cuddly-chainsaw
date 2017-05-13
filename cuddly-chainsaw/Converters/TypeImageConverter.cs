using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace cuddly_chainsaw.Converters
{
    class TypeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((uint)value != (uint)0)
            {
                return "Assets/" + value + ".jpg";
            }
            return "Assets/doing.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
            throw new NotImplementedException();
        }
    }

    class TypeImageConverter_Done : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((uint)value != (uint)0)
            {
                return "Assets/" + value + ".jpg";
            }
            return "Assets/done.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
