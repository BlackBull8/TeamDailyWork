using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TeamDailyWork.Converters
{
    class TimeToDoubleConverter:IValueConverter
    {
        public double PixelsPerMinute { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan time;
            if (value is DateTime)
            {
                time = ((DateTime)value).TimeOfDay;
            }
            else if (value is TimeSpan)
            {
                time = (TimeSpan)value;
            }
            else
            {
                throw new NotSupportedException();
            }
            return time.TotalMinutes * PixelsPerMinute;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
