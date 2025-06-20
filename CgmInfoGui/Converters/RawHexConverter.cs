using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaHex;
using AvaloniaHex.Document;
using AvaloniaHex.Rendering;
using CgmInfo.Commands;
using CgmInfo.Utilities;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Converters;

public sealed class RawHexConverter : IValueConverter
{
    public static readonly AttachedProperty<bool> ApplyHighlightsProperty =
        AvaloniaProperty.RegisterAttached<RawHexConverter, StyledElement, bool>("ApplyHighlights");

    public static void SetApplyHighlights(StyledElement element, bool value) =>
        element.SetValue(ApplyHighlightsProperty, value);
    public static bool GetApplyHighlights(StyledElement element) =>
        element.GetValue(ApplyHighlightsProperty);

    static RawHexConverter()
    {
        ApplyHighlightsProperty.Changed.AddClassHandler<HexEditor>(OnApplyHighlights);
    }
    private static void OnApplyHighlights(HexEditor editor, AvaloniaPropertyChangedEventArgs eventArgs)
    {
        if (editor is not { HexView: { } view })
            return;

        if (!(eventArgs.NewValue as bool?).GetValueOrDefault())
        {
            view.LineTransformers.Clear();
            return;
        }

        view.LineTransformers.Add(new AlignmentHighlighter());
        view.LineTransformers.Add(new CommandHeaderHighlighter());
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Command { Buffer: TrackingBuffer buffer })
            return null;

        return new TrackingBufferDocument(buffer);
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();

    private abstract class TrackingBufferHighlighter : ByteHighlighter
    {
        protected override bool IsHighlighted(HexView hexView, VisualBytesLine line, BitLocation location)
        {
            if (hexView.DataContext is not NodeBase { Command.Buffer: TrackingBuffer buffer })
                return false;
            return IsHighlighted(buffer, location);
        }
        protected abstract bool IsHighlighted(TrackingBuffer buffer, BitLocation location);
    }
    private sealed class AlignmentHighlighter : TrackingBufferHighlighter
    {
        public AlignmentHighlighter()
        {
            Background = Brushes.Red;
            Foreground = Brushes.Black;
        }
        protected override bool IsHighlighted(TrackingBuffer buffer, BitLocation location)
        {
            return buffer.Unaligned && location.ByteIndex == 0;
        }
    }
    private sealed class CommandHeaderHighlighter : TrackingBufferHighlighter
    {
        public CommandHeaderHighlighter()
        {
            Foreground = Brushes.Orange;
        }
        protected override bool IsHighlighted(TrackingBuffer buffer, BitLocation location)
        {
            if (location.ByteIndex == 1)
                return true;

            return buffer.Unaligned ? location.ByteIndex == 2 : location.ByteIndex == 0;
        }
    }
    private sealed class TrackingBufferDocument : IBinaryDocument
    {
        private readonly TrackingBuffer _buffer;
        private readonly ulong _virtualStart;

        public TrackingBufferDocument(TrackingBuffer buffer)
        {
            _buffer = buffer;
            // FIXME: AvaloniaHex currently renders this incorrectly when the start is not at zero (AvaloniaHex#20)
            _virtualStart = 0;
            //_virtualStart = (ulong)_buffer.Start;
            Length = _virtualStart + (ulong)_buffer.Buffer.Length;
            ValidRanges = new BitRangeUnion([new BitRange(_virtualStart, Length)]).AsReadOnly();
        }

        public ulong Length { get; }
        public bool IsReadOnly { get; } = true;
        public bool CanInsert { get; } = false;
        public bool CanRemove { get; } = false;
        public IReadOnlyBitRangeUnion ValidRanges { get; }

#pragma warning disable 67 // The event 'Changed' is never used. (Because this document is considered immutable.)
        public event EventHandler<BinaryDocumentChange>? Changed;
#pragma warning restore 67

        public void Dispose() { }
        public void Flush() { }
        public void InsertBytes(ulong offset, ReadOnlySpan<byte> buffer) => throw new NotSupportedException();
        public void ReadBytes(ulong offset, Span<byte> buffer)
        {
            if (offset < _virtualStart || offset >= Length)
                return;

            int realOffset = (int)(offset - _virtualStart);
            var buf = _buffer.Buffer.AsSpan();
            buf[realOffset..(realOffset + buffer.Length)].CopyTo(buffer);
        }
        public void RemoveBytes(ulong offset, ulong length) => throw new NotSupportedException();
        public void WriteBytes(ulong offset, ReadOnlySpan<byte> buffer) => throw new NotSupportedException();
    }
}
