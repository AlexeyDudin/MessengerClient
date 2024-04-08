using System;
using System.Globalization;
using System.Windows.Data;

namespace MessengerClient.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                var date = (DateTime)value;
                if (date.Date == DateTime.Now.Date)
                {
                    return $"{date.Hour}:{date.Minute}:{date.Second}";
                }
                else
                {
                    return $"{date.Day}.{date.Month}.{date.Year} {date.Hour}:{date.Minute}:{date.Second}";
                }
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
