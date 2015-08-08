using System;
using System.IO;
using CgmInfo.Commands;

namespace CgmInfo.Binary
{
    public class MetafileReader : IDisposable
    {
        private readonly string _fileName;
        private readonly BinaryReader _reader;

        private bool _insideMetafile;

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
                    result = ReadDelimiterElement(commandHeader);
                    break;
                case 1: // metafile descriptor
                    result = ReadMetafileDescriptorElement(commandHeader);
                    break;
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

            if (result != null && !_insideMetafile)
            {
                if (result.ElementClass != 0)
                    throw new FormatException("Expected Element Class 0 (Delimiter) at the beginning of a Metafile");
                if (result.ElementId != 1)
                    throw new FormatException("Expected Element Id 1 (BEGIN METAFILE) at the beginning of a Metafile");
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

        private Command ReadDelimiterElement(CommandHeader commandHeader)
        {
            Command result;
            // ISO/IEC 8632-3 8.2, Table 3
            switch (commandHeader.ElementId)
            {
                //case 0: // no-op; these are skipped already while reading the command header
                case 1: // BEGIN METAFILE
                    result = DelimiterElementReader.BeginMetafile(this, commandHeader);
                    _insideMetafile = result != null;
                    break;
                case 2: // END METAFILE
                case 3: // BEGIN PICTURE
                case 4: // BEGIN PICTURE BODY
                case 5: // END PICTURE
                case 6: // BEGIN SEGMENT
                case 7: // END SEGMENT
                case 8: // BEGIN FIGURE
                case 9: // END FIGURE
                case 13: // BEGIN PROTECTION REGION
                case 14: // END PROTECTION REGION
                case 15: // BEGIN COMPOUND LINE
                case 16: // END COMPOUND LINE
                case 17: // BEGIN COMPOUND TEXT PATH
                case 18: // END COMPOUND TEXT PATH
                case 19: // BEGIN TILE ARRAY
                case 20: // END TILE ARRAY
                case 21: // BEGIN APPLICATION STRUCTURE
                case 22: // BEGIN APPLICATION STRUCTURE BODY
                case 23: // END APPLICATION STRUCTURE
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }
            return result;
        }

        private Command ReadMetafileDescriptorElement(CommandHeader commandHeader)
        {
            Command result;
            // ISO/IEC 8632-3 8.3, Table 4
            switch (commandHeader.ElementId)
            {
                case 1: // METAFILE VERSION
                    result = MetafileDescriptorReader.MetafileVersion(this, commandHeader);
                    break;
                case 2: // METAFILE DESCRIPTION
                    result = MetafileDescriptorReader.MetafileDescription(this, commandHeader);
                    break;
                case 3: // VDC TYPE
                case 4: // INTEGER PRECISION
                case 5: // REAL PRECISION
                case 6: // INDEX PRECISION
                case 7: // COLOUR PRECISION
                case 8: // COLOUR INDEX PRECISION
                case 9: // MAXIMUM COLOUR INDEX
                case 10: // COLOUR VALUE EXTENT
                case 11: // METAFILE ELEMENT LIST
                case 12: // METAFILE DEFAULTS REPLACEMENT
                case 13: // FONT LIST
                case 14: // CHARACTER SET LIST
                case 15: // CHARACTER CODING ANNOUNCER
                case 16: // NAME PRECISION
                case 17: // MAXIMUM VDC EXTENT
                case 18: // SEGMENT PRIORITY EXTENT
                case 19: // COLOUR MODEL
                case 20: // COLOUR CALIBRATION
                case 21: // FONT PROPERTIES
                case 22: // GLYPH MAPPING
                case 23: // SYMBOL LIBRARY LIST
                case 24: // PICTURE DIRECTORY
                default:
                    result = ReadUnsupportedElement(commandHeader);
                    break;
            }
            return result;
        }

        internal int ReadInteger(int numBytes)
        {
            if (numBytes < 1 || numBytes > 4)
                throw new ArgumentOutOfRangeException("numBytes", numBytes, "Number of bytes must be between 1 and 4");
            int ret = 0;
            while (numBytes-- > 0)
                ret = (ret << 8) | _reader.ReadByte();
            return ret;
        }

        internal ushort ReadWord()
        {
            return (ushort)((_reader.ReadByte() << 8) | _reader.ReadByte());
        }

        internal string ReadString()
        {
            return _reader.ReadString();
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
