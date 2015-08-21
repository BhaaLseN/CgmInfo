using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CgmInfo.Commands;

namespace CgmInfo.TextEncoding
{
    public class MetafileReader : IDisposable
    {
        private readonly string _fileName;
        private readonly FileStream _fileStream;
        private readonly MetafileDescriptor _descriptor = new MetafileDescriptor();

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
            { "BEGCOMPOLINE", DelimiterElementReader.BeginCompoundLine },
            { "ENDCOMPOLINE", DelimiterElementReader.EndCompoundLine },
            { "BEGCOMPTEXTPATH", DelimiterElementReader.BeginCompoundTextPath },
            { "ENDCOMPTEXTPATH", DelimiterElementReader.EndCompoundTextPath },
            { "BEGAPS", DelimiterElementReader.BeginApplicationStructure },
            { "BEGAPSBODY", DelimiterElementReader.BeginApplicationStructureBody },
            { "ENDAPS", DelimiterElementReader.EndApplicationStructure },
        };

        public MetafileDescriptor Descriptor
        {
            get { return _descriptor; }
        }

        public MetafileReader(string fileName)
        {
            _fileName = fileName;
            _fileStream = File.OpenRead(fileName);
        }

        public Command ReadCommand()
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

        internal string ReadEnum()
        {
            return ReadToken();
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
        public void Dispose()
        {
            _fileStream.Dispose();
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
