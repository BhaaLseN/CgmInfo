using System;
using System.Collections.Generic;
using System.IO;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Utilities;
using BaseMetafileWriter = CgmInfo.MetafileWriter;

namespace CgmInfo.TextEncoding
{
    public class MetafileWriter : BaseMetafileWriter
    {
        private const char HardSeparator = ',';

        private readonly Dictionary<(int ElementClass, int ElementId), Action<MetafileWriter>> _indentChangeBefore = new Dictionary<(int, int), Action<MetafileWriter>>
        {
            // delimiter elements [ISO/IEC 8632-3 8.2, Table 3]
            { (0, 2), DecreaseLevel },
            { (0, 4), DecreaseLevel },
            { (0, 5), DecreaseLevel },
            { (0, 7), DecreaseLevel },
            { (0, 9), DecreaseLevel },
            { (0, 14), DecreaseLevel },
            { (0, 16), DecreaseLevel },
            { (0, 18), DecreaseLevel },
            { (0, 20), DecreaseLevel },
            { (0, 22), DecreaseLevel },
            { (0, 23), DecreaseLevel },
        };
        private readonly Dictionary<(int ElementClass, int ElementId), Action<MetafileWriter>> _indentChangeAfter = new Dictionary<(int, int), Action<MetafileWriter>>
        {
            // delimiter elements [ISO/IEC 8632-3 8.2, Table 3]
            { (0, 1), IncreaseLevel },
            { (0, 3), IncreaseLevel },
            { (0, 4), IncreaseLevel },
            { (0, 6), IncreaseLevel },
            { (0, 8), IncreaseLevel },
            { (0, 13), IncreaseLevel },
            { (0, 15), IncreaseLevel },
            { (0, 17), IncreaseLevel },
            { (0, 19), IncreaseLevel },
            { (0, 21), IncreaseLevel },
            { (0, 22), IncreaseLevel },
        };
        private readonly Dictionary<(int ElementClass, int ElementId), Action<MetafileWriter, Command>> _commandHandlerReplacement = new Dictionary<(int, int), Action<MetafileWriter, Command>>
        {
            // metafile descriptor elements [ISO/IEC 8632-3 8.3, Table 4]
            { (1, 4), WriteIntegerPrecision },
            { (1, 5), WriteRealPrecision },
            { (1, 6), WriteIndexPrecision },
            { (1, 7), WriteColorPrecision },
            { (1, 8), WriteColorIndexPrecision },
            { (1, 11), WriteMetafileElementsList },
            { (1, 16), WriteNamePrecision },

            // control elements [ISO/IEC 8632-3 8.5, Table 6]
            { (3, 1), WriteVdcIntegerPrecision },
            { (3, 2), WriteVdcRealPrecision },
        };

        public string IndentSequence { get; set; } = " ";

        private int _indent;
        private readonly StreamWriter _writer;

        public MetafileWriter(string fileName)
            : base(fileName)
        {
            _writer = new StreamWriter(_stream);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _writer.Flush();
                _writer.Dispose();
            }
            base.Dispose(disposing);
        }
        protected override void WriteInternal(Command command, Action<BaseMetafileWriter, Command> commandHandler)
        {
            string? elementName = TextTokenAttribute.GetToken(command);
            if (string.IsNullOrWhiteSpace(elementName))
            {
                // we didn't find this command in the table; means we don't support it (yet).
                // we can however write a comment instead.
                WriteIndent();
                WriteComment($"Unsupported Command {command.GetType().Name} ({command.ElementClass}/{command.ElementId})");
                return;
            }

            var classId = (command.ElementClass, command.ElementId);
            if (_indentChangeBefore.TryGetValue(classId, out var changeIndentBefore) && changeIndentBefore != null)
            {
                changeIndentBefore(this);
            }

            WriteIndent();
            _writer.Write(elementName);

            if (_commandHandlerReplacement.TryGetValue(classId, out var commandHandlerReplacement) && commandHandlerReplacement != null)
            {
                commandHandlerReplacement(this, command);
            }
            else
            {
                commandHandler(this, command);
            }

            // TODO: support '/' as alternate terminator
            _writer.WriteLine(';');

            if (_indentChangeAfter.TryGetValue(classId, out var changeIndentAfter) && changeIndentAfter != null)
            {
                changeIndentAfter(this);
            }
        }

        private static void IncreaseLevel(MetafileWriter writer) => writer._indent++;
        private static void DecreaseLevel(MetafileWriter writer)
        {
            writer._indent--;
            if (writer._indent <= 0)
                writer._indent = 0;
        }
        private void WriteIndent()
        {
            for (int i = 0; i < _indent; i++)
                _writer.Write(IndentSequence);
        }
        private void WriteComment(string commentText)
        {
            _writer.Write('%');
            _writer.Write(' ');
            _writer.Write(commentText?.Replace("%", "(percent)"));
            _writer.Write(' ');
            _writer.Write('%');
        }
        private void WriteIntegerLiteral(int value, char prefixedSeparator = ' ')
        {
            _writer.Write(prefixedSeparator);
            _writer.Write(value);
        }
        private void WriteRealLiteral(double value, char prefixedSeparator = ' ')
        {
            _writer.Write(prefixedSeparator);
            _writer.Write(value.ToString(TextEncodingHelper.Culture));
        }
        private void WriteStringLiteral(string value, char delimiter = '\'', char prefixedSeparator = ' ')
        {
            _writer.Write(prefixedSeparator);

            // If an APOSTROPHE is required in a string delimited with APOSTROPHES, it is represented by two adjacent
            // APOSTROPHES at that position in the string.Likewise, if a DOUBLE QUOTE character is required in a string
            // delimited with DOUBLE QUOTE characters, it is represented by two adjacent DOUBLE QUOTE characters [ISO/IEC 8632-4 6.3.3]
            _writer.Write(delimiter);
            string escapedValue = value.Replace(delimiter.ToString(), "" + delimiter + delimiter);
            // inside a Structured Data Record, strings may be nested. in case we're inside an SDR, we also have to escape the other type of quote.
            if (_inSDR)
            {
                char altDelimiter = delimiter == '\'' ? '"' : '\'';
                escapedValue = escapedValue.Replace(altDelimiter.ToString(), "" + altDelimiter + altDelimiter);
            }
            _writer.Write(escapedValue);
            _writer.Write(delimiter);
        }

        internal override void EnsureWordAligned()
        {
            // text encoding does not require word alignment
        }

        private static readonly string _bitStreamMap = "0123456789ABCDEF";
        internal override void WriteBitStream(byte[] value)
        {
            _writer.Write(' ');

            // The parameters of type Bitstream, of tile array elements, shall be represented as follows. The bits taken 4 at a time
            // are represented by a single hexidecimal digit in the Clear Text metafile. Null characters, SPACE, and format
            // effector characters may be interspersed in the stream for readability. For example, a space character every 4 digits
            // and a newline every 60 digits would provide well-formatted output. [ISO/IEC 8632-4 6.3.6]

            // we'll be using this recommendation here (except that we're counting bytes, not digits)
            int bytesWritten = 0;
            foreach (byte b in value)
            {
                _writer.Write(_bitStreamMap[(b >> 4) & 0xF]);
                _writer.Write(_bitStreamMap[b & 0xF]);

                bytesWritten++;

                if (bytesWritten % 2 == 0)
                    _writer.Write(' ');

                if (bytesWritten % 30 == 0)
                {
                    _writer.WriteLine();
                    WriteIndent();
                }
            }
        }
        internal override void WriteEnum<TEnum>(TEnum value)
        {
            // CELL ARRAY only has the representation mode in binary encoding; it doesn't exist in text.
            if (value is CellRepresentationMode)
                return;

            // Structured Data Elements don't serialize as enumerations but as indices; so write the numeric value instead.
            if (value is DataTypeIndex dti)
            {
                // when inside an SDR, add a delimiting comma if this isn't the first SDE.
                if (_inSDR && _sdeCount > 0)
                    _writer.Write(HardSeparator);
                // skip the leading space when we're just starting the Structured Data Element; for better formatting.
                if (!_inSDR || _sdeCount > 0)
                    _writer.Write(' ');
                _writer.Write((int)dti);
                return;
            }

            _writer.Write(' ');
            _writer.Write(TextTokenAttribute.GetToken(value));
        }

        private bool _inSDR;
        private int _sdeCount;
        internal override void WriteStructuredDataRecord(StructuredDataRecord value)
        {
            _writer.Write(' ');
            _writer.Write('"');
            _inSDR = true;
            _sdeCount = 0;
            base.WriteStructuredDataRecord(value);
            _inSDR = false;
            _writer.Write('"');
        }
        internal override void WriteStructuredDataElement(StructuredDataElement value)
        {
            base.WriteStructuredDataElement(value);
            _sdeCount++;
        }

        internal override void WriteDirectColor(MetafileColor value, int colorDirectPrecision)
        {
            if (value is MetafileColorIndexed colorIndexed)
            {
                WriteColorIndex(colorIndexed.Index);
                return;
            }

            if (Descriptor.ColorModel == ColorModel.RGB)
            {
                var colorRGB = (MetafileColorRGB)value;
                WriteIntegerLiteral(colorRGB.Red);
                WriteIntegerLiteral(colorRGB.Green, HardSeparator);
                WriteIntegerLiteral(colorRGB.Blue, HardSeparator);
            }
            else if (Descriptor.ColorModel == ColorModel.CMYK)
            {
                var colorCMYK = (MetafileColorCMYK)value;
                WriteIntegerLiteral(colorCMYK.Cyan);
                WriteIntegerLiteral(colorCMYK.Magenta, HardSeparator);
                WriteIntegerLiteral(colorCMYK.Yellow, HardSeparator);
                WriteIntegerLiteral(colorCMYK.Black, HardSeparator);
            }
            else // assume CIE*
            {
                var colorCIE = (MetafileColorCIE)value;
                WriteRealLiteral(colorCIE.Component1);
                WriteRealLiteral(colorCIE.Component2, HardSeparator);
                WriteRealLiteral(colorCIE.Component3, HardSeparator);
            }
        }
        internal override void WritePoint(MetafilePoint value)
        {
            WriteRealLiteral(value.X);
            WriteRealLiteral(value.Y, HardSeparator);
        }
        // ViewportPoint looks just like Point, so we can use that one.
        internal override void WriteViewportPoint(MetafilePoint point) => WritePoint(point);

        internal override void WriteByte(byte value) => WriteIntegerLiteral(value);
        internal override void WriteColorIndex(int value, int indexPrecision) => WriteIntegerLiteral(value);
        internal override void WriteColorValue(int value, int colorPrecision) => WriteIntegerLiteral(value);
        internal override void WriteFixedPoint(double value, int precision) => WriteRealLiteral(value);
        internal override void WriteFloatingPoint(double value, int precision) => WriteRealLiteral(value);
        internal override void WriteIndex(int value, int indexPrecision) => WriteIntegerLiteral(value);
        internal override void WriteInteger(int value, int integerPrecision) => WriteIntegerLiteral(value);
        internal override void WriteName(int value) => WriteIntegerLiteral(value);
        internal override void WriteString(string value) => WriteStringLiteral(value);
        internal override void WriteVdc(double value) => WriteRealLiteral(value);
        internal override void WriteWord(ushort value) => WriteIntegerLiteral(value);

        private static void WriteIntegerPrecision(MetafileWriter writer, Command command)
        {
            // Text encoding writes the INTEGER PRECISION as min/max pairs instead of the actual precision in bits.
            var integerPrecision = (IntegerPrecision)command;
            int maxValue = TextEncodingHelper.GetMaximumForPrecisionSigned(integerPrecision.Precision);

            writer.WriteIntegerLiteral(-maxValue);
            writer.WriteIntegerLiteral(maxValue);

            writer.Descriptor.IntegerPrecision = integerPrecision.Precision;
        }
        private static void WriteRealPrecision(MetafileWriter writer, Command command)
        {
            // Text encoding writes the REAL PRECISION as min/max pairs instead of the actual precision in bits.
            // it also has an indicator at the end of how many significant digits this encoding uses.
            var realPrecision = (RealPrecision)command;
            double maxValue = TextEncodingHelper.GetMaximumForPrecisionSigned(realPrecision.Specification);

            writer.WriteRealLiteral(-maxValue);
            writer.WriteRealLiteral(maxValue);
            // we don't really know how much we have, lets assume _something_
            // TODO: does (or should) this depend on RealPrecision.Specification?
            writer.WriteIntegerLiteral(8);

            writer.Descriptor.RealPrecision = realPrecision.Specification;
        }
        private static void WriteIndexPrecision(MetafileWriter writer, Command command)
        {
            // Text encoding writes the INDEX PRECISION as max value instead of the actual precision in bits.
            var indexPrecision = (IndexPrecision)command;
            uint maxValue = TextEncodingHelper.GetMaximumForPrecisionUnsigned(indexPrecision.Precision);

            writer.WriteIntegerLiteral((int)maxValue);

            writer.Descriptor.IndexPrecision = indexPrecision.Precision;
        }
        private static void WriteColorPrecision(MetafileWriter writer, Command command)
        {
            // Text encoding writes the COLOUR PRECISION as max value instead of the actual precision in bits.
            var colorPrecision = (ColorPrecision)command;
            uint maxValue = TextEncodingHelper.GetMaximumForPrecisionUnsigned(colorPrecision.Precision);

            writer.WriteIntegerLiteral((int)maxValue);

            writer.Descriptor.ColorPrecision = colorPrecision.Precision;
        }
        private static void WriteColorIndexPrecision(MetafileWriter writer, Command command)
        {
            // Text encoding writes the COLOUR INDEX PRECISION as max value instead of the actual precision in bits.
            var colorIndexPrecision = (ColorIndexPrecision)command;
            uint maxValue = TextEncodingHelper.GetMaximumForPrecisionUnsigned(colorIndexPrecision.Precision);

            writer.WriteIntegerLiteral((int)maxValue);

            writer.Descriptor.ColorIndexPrecision = colorIndexPrecision.Precision;
        }
        private static void WriteNamePrecision(MetafileWriter writer, Command command)
        {
            // Text encoding writes the NAME PRECISION as max value instead of the actual precision in bits.
            var namePrecision = (NamePrecision)command;
            uint maxValue = TextEncodingHelper.GetMaximumForPrecisionUnsigned(namePrecision.Precision);

            writer.WriteIntegerLiteral((int)maxValue);

            writer.Descriptor.NamePrecision = namePrecision.Precision;
        }
        private static void WriteMetafileElementsList(MetafileWriter writer, Command command)
        {
            // Text encoding writes the METAFILE ELEMENT LIST as names instead of the internal class/id from Part 3.
            var metafileElementsList = (MetafileElementsList)command;

            bool isFirst = true;
            foreach (var element in metafileElementsList.Elements)
            {
                writer.WriteStringLiteral(element.GetTextEncodingString(), prefixedSeparator: isFirst ? ' ' : HardSeparator);
                isFirst = false;
            }
        }
        private static void WriteVdcIntegerPrecision(MetafileWriter writer, Command command)
        {
            // Text encoding writes the VDC INTEGER PRECISION as min/max pairs instead of the actual precision in bits.
            var vdcIntegerPrecision = (VdcIntegerPrecision)command;
            int maxValue = TextEncodingHelper.GetMaximumForPrecisionSigned(vdcIntegerPrecision.Precision);

            writer.WriteIntegerLiteral(-maxValue);
            writer.WriteIntegerLiteral(maxValue);

            writer.Descriptor.VdcIntegerPrecision = vdcIntegerPrecision.Precision;
        }

        private static void WriteVdcRealPrecision(MetafileWriter writer, Command command)
        {
            // Text encoding writes the VDC REAL PRECISION as min/max pairs instead of the actual precision in bits.
            // it also has an indicator at the end of how many significant digits this encoding uses.
            var vdcRealPrecision = (VdcRealPrecision)command;
            double maxValue = TextEncodingHelper.GetMaximumForPrecisionSigned(vdcRealPrecision.Specification);

            writer.WriteRealLiteral(-maxValue);
            writer.WriteRealLiteral(maxValue);
            // we don't really know how much we have, lets assume _something_
            // TODO: does (or should) this depend on VdcRealPrecision.Specification?
            writer.WriteIntegerLiteral(8);

            writer.Descriptor.VdcRealPrecision = vdcRealPrecision.Specification;
        }
    }
}
