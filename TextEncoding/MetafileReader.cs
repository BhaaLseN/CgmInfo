using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using BaseMetafileReader = CgmInfo.MetafileReader;

namespace CgmInfo.TextEncoding
{
    public class MetafileReader : BaseMetafileReader
    {
        private readonly Dictionary<string, Func<MetafileReader, Command>> _commandTable = new Dictionary<string, Func<MetafileReader, Command>>
        {
            // delimiter elements [ISO/IEC 8632-4 7.1]
            { "BEGMF", DelimiterElementReader.BeginMetafile },
            { "ENDMF", DelimiterElementReader.EndMetafile },
            { "BEGPIC", DelimiterElementReader.BeginPicture },
            { "BEGPICBODY", DelimiterElementReader.BeginPictureBody },
            { "ENDPIC", DelimiterElementReader.EndPicture },
            { "BEGSEG", DelimiterElementReader.BeginSegment },
            { "ENDSEG", DelimiterElementReader.EndSegment },
            { "BEGFIGURE", DelimiterElementReader.BeginFigure },
            { "ENDFIGURE", DelimiterElementReader.EndFigure },
            { "BEGPROTREGION", DelimiterElementReader.BeginProtectionRegion },
            { "ENDPROTREGION", DelimiterElementReader.EndProtectionRegion },
            { "BEGCOMPOLINE", DelimiterElementReader.BeginCompoundLine },
            { "ENDCOMPOLINE", DelimiterElementReader.EndCompoundLine },
            { "BEGCOMPTEXTPATH", DelimiterElementReader.BeginCompoundTextPath },
            { "ENDCOMPTEXTPATH", DelimiterElementReader.EndCompoundTextPath },
            { "BEGTILEARRAY", DelimiterElementReader.BeginTileArray },
            { "ENDTILEARRAY", DelimiterElementReader.EndTileArray },
            { "BEGAPS", DelimiterElementReader.BeginApplicationStructure },
            { "BEGAPSBODY", DelimiterElementReader.BeginApplicationStructureBody },
            { "ENDAPS", DelimiterElementReader.EndApplicationStructure },

            // metafile descriptor elements [ISO/IEC 8632-4 7.2]
            { "MFVERSION", MetafileDescriptorReader.MetafileVersion },
            { "MFDESC", MetafileDescriptorReader.MetafileDescription },
            { "VDCTYPE", ReadVdcType },
            { "INTEGERPREC", ReadIntegerPrecision },
            { "REALPREC", ReadRealPrecision },
            { "INDEXPREC", ReadIndexPrecision },
            { "COLRPREC", ReadColorPrecision },
            { "COLRINDEXPREC", MetafileDescriptorReader.ColorIndexPrecision },
            { "MAXCOLRINDEX", MetafileDescriptorReader.MaximumColorIndex },
            { "COLRVALUEEXT", MetafileDescriptorReader.ColorValueExtent },
            { "FONTLIST", MetafileDescriptorReader.FontList },
            { "CHARSETLIST", MetafileDescriptorReader.CharacterSetList },
            { "CHARCODING", MetafileDescriptorReader.CharacterCodingAnnouncer },
            { "NAMEPREC", MetafileDescriptorReader.NamePrecision },
            { "MAXVDCEXT", MetafileDescriptorReader.MaximumVdcExtent },
            { "COLRMODEL", MetafileDescriptorReader.ColorModelCommand },

            // control elements [ISO/IEC 8632-4 7.4]
            { "VDCINTEGERPREC", ReadVdcIntegerPrecision },
            { "VDCREALPREC", ReadVdcRealPrecision },
            // FIXME: disabled for now (at least until COLOUR SELECTION MODE is implemented)
            //{ "AUXCOLR", ControlElementReader.AuxiliaryColor },
            { "TRANSPARENCY", ControlElementReader.Transparency },
            { "CLIPRECT", ControlElementReader.ClipRectangle },
            { "CLIP", ControlElementReader.ClipIndicator },
            { "LINECLIPMODE", ControlElementReader.LineClippingMode },
            { "MARKERCLIPMODE", ControlElementReader.MarkerClippingMode },
            { "EDGECLIPMODE", ControlElementReader.EdgeClippingMode },
            { "NEWREGION", ControlElementReader.NewRegion },
            { "SAVEPRIMCONT", ControlElementReader.SavePrimitiveContext },
            { "RESPRIMCONT", ControlElementReader.RestorePrimitiveContext },

            // graphical primitive elements [ISO/IEC 8632-4 7.5]
            { "TEXT", GraphicalPrimitiveReader.Text },
            { "RESTRTEXT", GraphicalPrimitiveReader.RestrictedText },
            { "APNDTEXT", GraphicalPrimitiveReader.AppendText },

            // application structure descriptor elements [ISO/IEC 8632-4 7.10]
            { "APSATTR", ApplicationStructureDescriptorReader.ApplicationStructureAttribute },
        };

        public MetafileReader(string fileName)
            : base(fileName, false)
        {
        }

        protected override Command ReadCommand()
        {
            string token;
            TokenState state = ReadToken(out token);

            if (state == TokenState.EndOfFile)
                return null;

            Func<MetafileReader, Command> commandHandler;
            if (!_commandTable.TryGetValue(token.ToUpperInvariant(), out commandHandler))
            {
                if (state == TokenState.EndOfToken)
                    commandHandler = r => UnsupportedCommand(token);
                else
                    commandHandler = r => UnsupportedCommandNoParameters(token);
            }

            return commandHandler(this);
        }

        private Command UnsupportedCommand(string elementName)
        {
            var rawParameters = new StringBuilder();

            TokenState state;
            do
            {
                string token;
                state = ReadToken(out token);

                rawParameters.Append(token).Append(' ');
            } while (state == TokenState.EndOfToken);

            return new UnsupportedCommand(elementName, rawParameters.ToString().Trim());
        }
        private Command UnsupportedCommandNoParameters(string elementName)
        {
            return new UnsupportedCommand(elementName, null);
        }

        private static Command ReadVdcType(MetafileReader reader)
        {
            var vdcType = MetafileDescriptorReader.VdcType(reader);
            reader.Descriptor.VdcType = vdcType.Specification;
            return vdcType;
        }
        private static Command ReadIntegerPrecision(MetafileReader reader)
        {
            var integerPrecision = MetafileDescriptorReader.IntegerPrecision(reader);
            reader.Descriptor.IntegerPrecision = integerPrecision.Precision;
            return integerPrecision;
        }
        private static Command ReadRealPrecision(MetafileReader reader)
        {
            var realPrecision = MetafileDescriptorReader.RealPrecision(reader);
            reader.Descriptor.RealPrecision = realPrecision.Specification;
            return realPrecision;
        }
        private static Command ReadIndexPrecision(MetafileReader reader)
        {
            var indexPrecision = MetafileDescriptorReader.IndexPrecision(reader);
            reader.Descriptor.IndexPrecision = indexPrecision.Precision;
            return indexPrecision;
        }
        private static Command ReadColorPrecision(MetafileReader reader)
        {
            var colorPrecision = MetafileDescriptorReader.ColorPrecision(reader);
            reader.Descriptor.ColorPrecision = colorPrecision.Precision;
            return colorPrecision;
        }
        private static Command ReadVdcIntegerPrecision(MetafileReader reader)
        {
            var vdcIntegerPrecision = ControlElementReader.VdcIntegerPrecision(reader);
            reader.Descriptor.VdcIntegerPrecision = vdcIntegerPrecision.Precision;
            return vdcIntegerPrecision;
        }
        private static Command ReadVdcRealPrecision(MetafileReader reader)
        {
            var vdcRealPrecision = ControlElementReader.VdcRealPrecision(reader);
            reader.Descriptor.VdcRealPrecision = vdcRealPrecision.Specification;
            return vdcRealPrecision;
        }

        internal string ReadString()
        {
            return ReadToken();
        }

        private string ReadToken()
        {
            string token;
            ReadToken(out token);
            return token;
        }

        /// <summary>
        /// Reads tokens until the end of the current element (or end of file)
        /// and returns a list of read tokens.
        /// </summary>
        /// <returns>List of tokens, encoded as string</returns>
        internal IEnumerable<string> ReadToEndOfElement()
        {
            string token;
            TokenState state;
            var ret = new List<string>();
            do
            {
                state = ReadToken(out token);
                ret.Add(token);
            } while (state == TokenState.EndOfToken);

            return ret;
        }
        internal string ReadEnum()
        {
            return ReadToken();
        }
        internal int ReadIndex()
        {
            return ReadInteger();
        }
        private static readonly Regex DecimalInteger = new Regex(@"^(?<sign>[+\-])?(?<digits>[0-9]+)$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        private static readonly Regex BasedInteger = new Regex(@"^(?<sign>[+\-])?(?<radix>(?:[2-9]|1[0-6]))#(?<digits>[0-9A-F]+)$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        private const string ExtendedDigits = "0123456789ABCDEF";
        internal int ReadInteger()
        {
            string number = ReadToken();
            var match = DecimalInteger.Match(number);
            if (match.Success)
            {
                int num;
                if (!int.TryParse(match.Groups["digits"].Value, out num))
                    throw new FormatException(string.Format("Invalid Decimal Integer digits '{0}' at position {1}", number, _fileStream.Position - number.Length));
                if (match.Groups["sign"].Success && match.Groups["sign"].Value == "-")
                    num = -num;
                return num;
            }

            match = BasedInteger.Match(number);
            if (match.Success)
            {
                int radix;
                if (!int.TryParse(match.Groups["radix"].Value, out radix))
                    throw new FormatException(string.Format("Invalid Based Integer radix '{0}' at position {1}", number, _fileStream.Position - number.Length));
                int num = 0;
                string digits = match.Groups["digits"].Value.ToUpperInvariant();
                for (int i = 0; i < digits.Length; i++)
                {
                    int charValue = ExtendedDigits.IndexOf(digits[i]);
                    if (charValue < 0 || charValue >= radix)
                        throw new ArgumentOutOfRangeException("BasedInteger", digits[charValue],
                            string.Format("Invalid Based Integer digits '{0}' at position {1}", number, _fileStream.Position - number.Length));

                    num = num * radix + charValue;
                }
                if (match.Groups["sign"].Success && match.Groups["sign"].Value == "-")
                    num = -num;
                return num;
            }

            throw new FormatException(string.Format("Unsupported Integer Format '{0}' at position {1}", number, _fileStream.Position - number.Length));
        }
        // according to spec, explicit point must have either 1 digit integer or 1 digit fraction. lets hope we can get away with that...[ISO/IEC 8632-4 6.3.2]
        private static readonly Regex ExplicitPointNumber = new Regex(@"^(?<sign>[+\-])?(?<integer>[0-9]*)\.(?<fraction>[0-9]*)$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        private static readonly Regex ScaledRealNumber = new Regex(@"^(?<sign>[+\-])?(?<integer>[0-9]*)(?:\.(?<fraction>[0-9]*))?[Ee](?<exponent>[0-9]+)$", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
        internal double ReadReal()
        {
            string number = ReadToken();

            // all 3 formats can be parsed by double.Parse, so simply throw if the number itself doesn't match either.
            if (!ExplicitPointNumber.IsMatch(number) && !ScaledRealNumber.IsMatch(number) && !DecimalInteger.IsMatch(number))
                throw new FormatException(string.Format("Invalid Real number '{0}' at position {1}", number, _fileStream.Position - number.Length));

            return double.Parse(number, CultureInfo.GetCultureInfo("en"));
        }
        internal double ReadVdc()
        {
            // a VDC is either an int or a double; depending on what VDC TYPE said [ISO/IEC 8632-4 6.3.5]
            if (Descriptor.VdcType == VdcTypeSpecification.Integer)
            {
                return ReadInteger();
            }
            else if (Descriptor.VdcType == VdcTypeSpecification.Real)
            {
                return ReadReal();
            }

            throw new NotSupportedException("The current VDC TYPE is not supported");
        }
        internal PointF ReadPoint()
        {
            double x = ReadVdc();
            double y = ReadVdc();
            return new PointF((float)x, (float)y);
        }
        internal Color ReadColor()
        {
            if (Descriptor.ColorModel == ColorModel.RGB)
            {
                int r = ReadColorValue();
                int g = ReadColorValue();
                int b = ReadColorValue();
                return Color.FromArgb(r, g, b);
            }
            else if (Descriptor.ColorModel == ColorModel.CMYK)
            {
                int c = ReadColorValue();
                int m = ReadColorValue();
                int y = ReadColorValue();
                int k = ReadColorValue();
                return BinaryEncoding.MetafileReader.ColorFromCMYK(c, m, y, k);
            }
            else
            {
                // CIELAB/CIELUV/RGB-related are not exactly .NET Color values, but we'll return them anyways.
                // TODO: actually convert them to RGB (using CIEXYZ for example, [ISO/IEC 8632-1 Annex G])
                double first = ReadReal();
                double second = ReadReal();
                double third = ReadReal();

                return Color.FromArgb((int)(first / 255), (int)(second / 255), (int)(third / 255));
            }
        }
        internal int ReadColorValue()
        {
            // FIXME: color component in CIELAB/CIELUV/RGB-related is reals, not ints
            return ReadInteger();
        }

        private TokenState ReadToken(out string token)
        {
            var sb = new StringBuilder();
            try
            {
                while (_fileStream.Position < _fileStream.Length)
                {
                    char c = (char)_fileStream.ReadByte();
                    switch (c)
                    {
                        // null characters; skip them [ISO/IEC 8632-4 6.1]
                        case '_':
                        case '$':
                            break;

                        // element seperator characters; those end the current element [ISO/IEC 8632-4 6.2.1]
                        case ';':
                        case '/':
                            return TokenState.EndOfElement;

                        // whitespace; skip them unless there is content (since it also counts as a soft seperator) [ISO/IEC 8632-4 6.2.2]
                        case ' ':
                        case '\r':
                        case '\n':
                        case '\t':
                            if (sb.Length > 0)
                                return TokenState.EndOfToken;
                            break;

                        // comma counts as a hard separator [ISO/IEC 8632-4 6.2.2]
                        case ',':
                        // point values might be enclosed in parentheses for readability [ISO/IEC 8632-4 6.3.5]
                        // TODO: spec says there must be exactly two numbers inside the parentheses, or the file is non-conforming.
                        //       this isn't validated at this point, and would require rewriting ReadPoint (and possibly others).
                        case '(':
                        case ')':
                            return TokenState.EndOfToken;

                        // strings; fully read and return them as they are [ISO/IEC 8632-4 6.3.3]
                        case '\'':
                        case '"':
                            char stringDelimiter = c;
                            do
                            {
                                c = (char)_fileStream.ReadByte();

                                // in case the delimiter appears:
                                // either end of string, or double the delimiter to include a literal one
                                if (c == stringDelimiter)
                                {
                                    if (_fileStream.Position >= _fileStream.Length)
                                        return TokenState.EndOfFile;

                                    char nextChar = (char)_fileStream.ReadByte();
                                    if (nextChar == stringDelimiter)
                                    {
                                        // literal delimiter; append it once
                                        sb.Append(c);
                                    }
                                    else
                                    {
                                        // end of string; reset back by the one character read ahead
                                        _fileStream.Seek(-1, SeekOrigin.Current);
                                        break;
                                    }
                                }
                                else
                                {
                                    // any other character: simply append to the token string
                                    sb.Append(c);
                                }

                                if (_fileStream.Position >= _fileStream.Length)
                                    return TokenState.EndOfFile;
                            } while (c != stringDelimiter);

                            // end of string might also mean end of element;
                            // we need to do another read, or we'd end up with an empty string at the next read
                            // simply break for another loop; and pray we get either a "/" or ";" character next.
                            break;

                        // comment; skip them completely [ISO/IEC 8632-4 6.2.3]
                        case '%':
                            do
                            {
                                c = (char)_fileStream.ReadByte();
                                if (_fileStream.Position >= _fileStream.Length)
                                    return TokenState.EndOfFile;
                            } while (c != '%');
                            // Comments may be included any place that a separator may be used, and are equivalent to a <SOFTSEP>; they
                            // may be replaced by a SPACE character in parsing, without affecting the meaning of the metafile.
                            return TokenState.EndOfToken;

                        default:
                            sb.Append(c);
                            break;
                    }
                }
            }
            finally
            {
                token = sb.ToString();
            }
            return TokenState.EndOfFile;
        }

        private enum TokenState
        {
            // obviously EOF
            EndOfFile,
            // reached an element seperator (either ";" or "/")
            EndOfElement,
            // reached something that delimits tokens from each other; but doesn't end the element
            EndOfToken,
        }
    }
}
