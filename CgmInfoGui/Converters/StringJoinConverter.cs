using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace CgmInfoGui.Converters;

public sealed class StringJoinConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (parameter is not string glue)
        {
            if (parameter is char glueChar)
                glue = glueChar.ToString();
            else
                glue = Environment.NewLine;
        }

        if (value is IEnumerable<object> values)
        {
            string str = string.Join(glue, values);
            // don't return an empty string; bindings might want to overrule this.
            if (string.IsNullOrWhiteSpace(str))
                return AvaloniaProperty.UnsetValue;

            return str;
        }

        return value;
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
