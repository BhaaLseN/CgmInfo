using System;
using System.Globalization;
using System.Windows.Data;

namespace CgmInfoGui.Converters
{
    class FileSizeConverter : IValueConverter
    {
        private static readonly string[] Suffixes = { "B", "KB", "MB", "GB", "TB" };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is long))
                return value;

            double fileSize = (long)value;
            int suffixIndex = 0;
            while (fileSize > 1024 && suffixIndex < Suffixes.Length)
            {
                fileSize /= 1024;
                suffixIndex++;
            }
            return string.Format("{0:0.##} {1}", fileSize, Suffixes[suffixIndex]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
