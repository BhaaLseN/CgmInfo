using System;
using Avalonia.Media.TextFormatting;
using AvaloniaHex.Rendering;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Controls;

internal class OffsetColumn : AvaloniaHex.Rendering.OffsetColumn
{
    public override TextLine? CreateTextLine(VisualBytesLine line)
    {
        if (HexView is null)
            throw new InvalidOperationException();

        ulong offset = line.Range.Start.ByteIndex;
        if (HexView.DataContext is NodeBase { Command.Buffer.Start: long start })
            offset += (ulong)start;
        string text = IsUppercase
            ? $"{offset:X8}:"
            : $"{offset:x8}:";

        var properties = GetTextRunProperties();
        return TextFormatter.Current.FormatLine(
            new SimpleTextSource(text, properties),
            0,
            double.MaxValue,
            new GenericTextParagraphProperties(properties)
        )!;
    }

    private readonly struct SimpleTextSource(string text, TextRunProperties defaultProperties) : ITextSource
    {
        public TextRun GetTextRun(int textSourceIndex)
        {
            if (textSourceIndex >= text.Length)
                return new TextEndOfParagraph();

            return new TextCharacters(text, defaultProperties);
        }
    }
}
