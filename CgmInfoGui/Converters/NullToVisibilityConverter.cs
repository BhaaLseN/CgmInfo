using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CgmInfoGui.Converters
{
    class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                if (string.Equals("inverse", value as string, StringComparison.OrdinalIgnoreCase))
                    b = !b;
                return b ? Visibility.Visible : Visibility.Collapsed;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
