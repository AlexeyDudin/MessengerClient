using System;
using System.Globalization;
using System.Text;
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
                    StringBuilder sb = new StringBuilder();
                    sb.Append(string.Format("{0:00}:", date.Hour));
                    sb.Append(string.Format("{0:00}:", date.Minute));
                    sb.Append(string.Format("{0:00}", date.Second));
                    return sb.ToString();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(string.Format("{0:00}.", date.Day));
                    sb.Append(string.Format("{0:00}.", date.Month));
                    sb.Append(string.Format("{0:0000} ", date.Year));
                    sb.Append(string.Format("{0:00}:", date.Hour));
                    sb.Append(string.Format("{0:00}:", date.Minute));
                    sb.Append(string.Format("{0:00}", date.Second));
                    return sb.ToString();
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
