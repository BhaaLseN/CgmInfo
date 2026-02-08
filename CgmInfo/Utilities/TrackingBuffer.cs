using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CgmInfo.Utilities;

public sealed class TrackingBuffer(long position)
{
    private readonly List<byte> _buffer = [];

    public long Start { get; } = position;
    public bool Unaligned { get; private set; }
    public List<string> EncodingChanges { get; } = [];

    public byte[] Buffer => [.. _buffer];

    internal void SetAlign(byte align)
    {
        Unaligned = true;
        _buffer.Add(align);
    }
    internal void SetHeader(ushort commandHeader) => _buffer.AddRange(BE(BitConverter.GetBytes(commandHeader)));
    internal void AddLongCommand(ushort longFormCommandHeader) => _buffer.AddRange(BE(BitConverter.GetBytes(longFormCommandHeader)));
    internal void AddBuffer(byte[] buffer) => _buffer.AddRange(buffer);
    internal void SwitchEncoding(Encoding oldEncoding, Encoding newEncoding)
    {
        EncodingChanges.Add($"{oldEncoding.EncodingName} -> {newEncoding.EncodingName}");
    }

    private static IEnumerable<byte> BE(IEnumerable<byte> buffer)
    {
        if (BitConverter.IsLittleEndian)
            buffer = buffer.Reverse();
        return buffer;
    }
}
