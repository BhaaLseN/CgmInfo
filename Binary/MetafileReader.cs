using System;
using System.IO;
using CgmInfo.Commands;

namespace CgmInfo.Binary
{
    public class MetafileReader : IDisposable
    {
        private readonly string _fileName;
        private readonly BinaryReader _reader;

        public MetafileReader(string fileName)
        {
            _fileName = fileName;
            _reader = new BinaryReader(File.OpenRead(fileName));
        }

        public Command ReadCommand()
        {
            // stop at EOF; or when we cannot at least read another command header
            if (_reader.BaseStream.Position + 2 >= _reader.BaseStream.Length)
                return null;

            Command result;
            CommandHeader commandHeader = ReadCommandHeader();
            // ISO/IEC 8632-3 8.1, Table 2
            switch (commandHeader.ElementClass)
            {
                case 0: // delimiter
                case 1: // metafile descriptor
                case 2: // picture descriptor
                case 3: // control
                case 4: // graphical primitive
                case 5: // attribute
                case 6: // escape
                case 7: // external
                case 8: // segment control/segment attribute
                case 9: // application structure descriptor
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }

            return result;
        }

        private CommandHeader ReadCommandHeader()
        {
            // commands are always word aligned [ISO/IEC 8632-3 5.4]
            if (_reader.BaseStream.Position % 2 == 1)
                _reader.BaseStream.Seek(1, SeekOrigin.Current);

            ushort commandHeader = ReadWord();
            int elementClass = (commandHeader >> 12) & 0xF;
            int elementId = (commandHeader >> 5) & 0x7F;
            int parameterListLength = commandHeader & 0x1F;

            bool isLongFormat = parameterListLength == 0x1F;
            if (isLongFormat)
            {
                ushort longFormCommandHeader = ReadWord();
                int partitionFlag = (longFormCommandHeader >> 15) & 0x1;
                bool isLastPartition = partitionFlag == 0;
                if (!isLastPartition)
                    throw new InvalidOperationException("Sorry, cannot read command headers with parameters larger than 32767 octets");
                parameterListLength = longFormCommandHeader & 0x7FFF;
            }

            bool isNoop = elementClass == 0 && elementId == 0;
            if (isNoop)
            {
                if (parameterListLength > 2)
                    // TODO: length seems to include the 2 bytes already read for the header?
                    //       spec is not exactly clear there =/ [ISO/IEC 8632-3 8.2 Table 3 add.]
                    _reader.BaseStream.Seek(parameterListLength - 2, SeekOrigin.Current);
                return ReadCommandHeader();
            }

            return new CommandHeader(elementClass, elementId, parameterListLength);
        }

        private Command ReadUnsupportedElement(CommandHeader commandHeader)
        {
            // skip the command parameter bytes we don't know
            _reader.BaseStream.Seek(commandHeader.ParameterListLength, SeekOrigin.Current);
            return new UnsupportedCommand(commandHeader.ElementClass, commandHeader.ElementId);
        }

        private int ReadInteger(int numBytes)
        {
            if (numBytes < 1 || numBytes > 4)
                throw new ArgumentOutOfRangeException("numBytes", numBytes, "Number of bytes must be between 1 and 4");
            int ret = 0;
            while (numBytes-- > 0)
                ret = (ret << 8) | _reader.ReadByte();
            return ret;
        }

        private ushort ReadWord()
        {
            return (ushort)((_reader.ReadByte() << 8) | _reader.ReadByte());
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
