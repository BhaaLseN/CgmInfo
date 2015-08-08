using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgmInfo.Parameters;

namespace CgmInfo.Binary
{
    public class MetafileParameterReader : IDisposable
    {
        private readonly string _fileName;
        private readonly BinaryReader _reader;

        public MetafileState State { get; private set; }
        public List<object> Parameters { get; private set; }

        public MetafileParameterReader(string fileName)
        {
            _fileName = fileName;
            _reader = new BinaryReader(File.OpenRead(fileName));
            State = MetafileState.Start;
        }

        public bool Next()
        {
            var parameters = new List<object>();
            try
            {
                switch (State)
                {
                    case MetafileState.Start:
                        var beginMetafile = ReadCommandHeader();
                        if (beginMetafile.ElementClass != 0)
                            throw new FormatException("Expected Element Class 0 (Delimiter) at the beginning of a Metafile");
                        if (beginMetafile.ElementId != 1)
                            throw new FormatException("Expected Element Id 1 (BEGIN METAFILE) at the beginning of a Metafile");

                        State = MetafileState.BeginMetafile;
                        string identifier = ReadBString(beginMetafile.ParameterListLength);
                        parameters.Add(identifier);
                        return true;
                    case MetafileState.BeginMetafile:
                    case MetafileState.MetafileDescriptor:
                        var metafileDescriptor = ReadCommandHeader();
                        if (metafileDescriptor.ElementClass == 1)
                        {
                            State = MetafileState.MetafileDescriptor;
                            parameters.Add(ReadMetafileDescriptorParameters(metafileDescriptor));
                            return true;
                        }
                        else
                        {
                            State = 0;
                        }
                        break;
                    case MetafileState.EndMetafile:
                        break;
                    default:
                        break;
                }

                return false;
            }
            finally
            {
                Parameters = parameters;
            }
        }

        private MetafileDescriptorParameter ReadMetafileDescriptorParameters(CommandHeader metafileDescriptor)
        {
            MetafileDescriptorType type = (MetafileDescriptorType)metafileDescriptor.ElementId;
            switch (metafileDescriptor.ElementId)
            {
                case 1: // METAFILE VERSION (I)
                case 4: // INTEGER PRECISION (I)
                case 6: // INDEX PRECISION (I)
                case 7: // COLOUR PRECISION (I)
                case 8: // COLOUR INDEX PRECISION (I)
                case 16: // NAME PRECISION (I)
                case 3: // VDC TYPE (E)
                case 15: // CHARACTER CODING ANNOUNCER (E)
                case 9: // MAXIMUM COLOUR INDEX (CI)
                    int @int = ReadInteger(metafileDescriptor.ParameterListLength);
                    return new IntegerMetafileDescriptorParameter(@int, type);
                case 2: // METAFILE DESCRIPTION (SF)
                    string @string = ReadBString(metafileDescriptor.ParameterListLength);
                    return new StringMetafileDescriptorParameter(@string, type);
                case 5: // REAL PRECISION (E, 2I)
                    int realFormat = ReadInteger(2);
                    int realExponent = ReadInteger(2);
                    int realFraction = ReadInteger(2);
                    return new RealPrecisionMetafileDescriptorParameter(realFormat, realExponent, realFraction, type);
                case 10: // COLOUR VALUE EXTENT (2CD or 6R)
                case 11: // METAFILE ELEMENT LIST (I, 2nIX)
                case 12: // METAFILE DEFAULTS REPLACEMENT (Metafile elements)
                case 13: // FONT LIST (nSF)
                case 14: // CHARACTER SET LIST (n(E, SF))
                case 17: // MAXIMUM VDC EXTENT (2P)
                case 18: // SEGMENT PRIORTY EXTENT (2I)
                case 19: // COLOUR MODEL (IX)
                case 20: // COLOUR CALIBRATION (IX, 3R, 18R, I, 6nCCO, I, mCD, 3mR)
                case 21: // FONT PROPERTIES (n[IX, I, SDR])
                case 22: // GLYPH MAPPING (IX, E, SF, I, IX, SDR)
                case 23: // SYMBOL LIBRARY List (nSF)
                case 24: // PICTURE DIRECTORY (E, n(SF, 2[ldt]))
                default:
                    byte[] raw = _reader.ReadBytes(metafileDescriptor.ParameterListLength);
                    return new UnsupportedMetafileDescriptorParameter(raw, type);
            }
        }
        private string ReadBString(int maxLength)
        {
            string str = _reader.ReadString();
            return str;
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
        #region IDisposable Members

        public void Dispose()
        {
            _reader.Dispose();
        }

        #endregion
    }
}
