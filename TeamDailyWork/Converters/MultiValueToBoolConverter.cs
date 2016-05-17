using System;
using System.Globalization;
using System.Windows.Data;

namespace TeamDailyWork.Converters
{
    class MultiValueToBoolConverter:IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0].ToString() == "" || values[1] == null)
            {
                return false;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
