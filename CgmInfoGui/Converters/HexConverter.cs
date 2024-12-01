using System;
using System.Globalization;
using System.Text;
using Avalonia.Data.Converters;
using AvaloniaHex.Document;
using CgmInfo.Commands;

namespace CgmInfoGui.Converters;

public sealed class HexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not UnsupportedCommand cmd)
            return null;

        if (cmd.IsTextEncoding)
            return new MemoryBinaryDocument(Encoding.UTF8.GetBytes(cmd.RawParameters), isReadOnly: true);

        return new MemoryBinaryDocument(cmd.RawBuffer, isReadOnly: true);
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
