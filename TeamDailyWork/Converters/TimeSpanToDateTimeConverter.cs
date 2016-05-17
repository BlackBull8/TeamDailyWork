using System;
using System.Globalization;
using System.Windows.Data;

namespace TeamDailyWork.Converters
{
    class TimeSpanToDateTimeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan oldValue = (TimeSpan) value;
            DateTime newTime = DateTime.Parse(oldValue.ToString());
            return newTime.ToString("HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
