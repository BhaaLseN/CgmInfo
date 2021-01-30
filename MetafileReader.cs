using System;
using System.IO;
using CgmInfo.Commands;
using CgmInfo.Traversal;
using BinaryMetafileReader = CgmInfo.BinaryEncoding.MetafileReader;
using TextMetafileReader = CgmInfo.TextEncoding.MetafileReader;

namespace CgmInfo
{
    public abstract class MetafileReader : IDisposable
    {
        private readonly MetafilePropertyVisitor _propertyVisitor = new MetafilePropertyVisitor();
        private readonly FileStream? _fileStream;

        public MetafileDescriptor Descriptor { get; } = new MetafileDescriptor();
        public MetafileProperties Properties { get; }

        protected MetafileReader(string fileName, bool isBinaryEncoding)
        {
            _fileStream = File.OpenRead(fileName);
            Properties = new MetafileProperties(isBinaryEncoding, _fileStream.Length);
        }
        protected MetafileReader(MetafileReader other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            Descriptor = other.Descriptor;
            Properties = other.Properties;
        }

        public Command? Read()
        {
            if (_fileStream == null)
                throw new InvalidOperationException("Attempted to read a Command from a Sub-Buffer reader.");

            var command = ReadCommand(_fileStream);
            command?.Accept(_propertyVisitor, Properties);
            return command;
        }

        protected abstract Command? ReadCommand(Stream stream);

        public static MetafileReader Create(string fileName)
        {
            bool isBinary;
            using (var fs = File.OpenRead(fileName))
                isBinary = BinaryMetafileReader.IsBinaryMetafile(fs);

            if (isBinary)
                return new BinaryMetafileReader(fileName);
            else
                return new TextMetafileReader(fileName);
        }

        #region IDisposable
        private bool _isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _fileStream?.Dispose();
                }

                _isDisposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
