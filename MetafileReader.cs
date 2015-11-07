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
        private readonly MetafileDescriptor _descriptor = new MetafileDescriptor();
        private readonly MetafileProperties _properties;
        private readonly MetafilePropertyVisitor _propertyVisitor = new MetafilePropertyVisitor();

        private readonly FileStream _fileStream;

        public MetafileDescriptor Descriptor
        {
            get { return _descriptor; }
        }
        public MetafileProperties Properties
        {
            get { return _properties; }
        }

        protected MetafileReader(string fileName, bool isBinaryEncoding)
        {
            _fileStream = File.OpenRead(fileName);
            _properties = new MetafileProperties(isBinaryEncoding, _fileStream.Length);
        }

        public Command Read()
        {
            var command = ReadCommand(_fileStream);
            if (command != null)
                command.Accept(_propertyVisitor, _properties);
            return command;
        }

        protected abstract Command ReadCommand(Stream stream);

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
                    _fileStream.Dispose();
                }

                _isDisposed = true;
            }
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
