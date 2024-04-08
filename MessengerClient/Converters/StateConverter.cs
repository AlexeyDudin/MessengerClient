using Domain.Dtos;
using System;
using System.Globalization;
using System.Windows.Data;

namespace MessengerClient.Converters
{
    public class StateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value is UserState)
                {
                    return (UserState)value == UserState.Online;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
