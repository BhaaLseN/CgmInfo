using System;
using System.IO;
using CgmInfo.Commands;
using BinaryMetafileReader = CgmInfo.BinaryEncoding.MetafileReader;
using TextMetafileReader = CgmInfo.TextEncoding.MetafileReader;

namespace CgmInfo
{
    public abstract class MetafileReader : IDisposable
    {
        private readonly MetafileDescriptor _descriptor = new MetafileDescriptor();

        protected readonly FileStream _fileStream;

        public MetafileDescriptor Descriptor
        {
            get { return _descriptor; }
        }

        protected MetafileReader(string fileName)
        {
            _fileStream = File.OpenRead(fileName);
        }

        public abstract Command ReadCommand();

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
