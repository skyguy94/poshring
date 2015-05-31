using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace poshring
{
    public class Credential : IDisposable
    {
        private NativeCredential _nativeCredential;
        private bool _disposed;

        public Credential()
            : this(new NativeCredential())
        {
            _nativeCredential.Flags = 0;
            Type = CredentialType.Generic;
            _nativeCredential.AttributeCount = 0;
            _nativeCredential.Persist = CredentialPersist.LocalMachine;
        }

        public Credential(NativeCredential nativeCredential)
        {
            _nativeCredential = nativeCredential;
        }

        public CredentialType Type
        {
            get { return _nativeCredential.Type; }
            set { _nativeCredential.Type = value; }
        }

        public string TargetName
        {
            get { return _nativeCredential.TargetName.Substring(_nativeCredential.TargetName.IndexOf("=", StringComparison.InvariantCultureIgnoreCase)+1); }
            set { _nativeCredential.TargetName = value; }
        }

        public string UserName
        {
            get { return _nativeCredential.UserName; }
            set { _nativeCredential.UserName = value; }
        }

        public string Comment
        {
            get { return _nativeCredential.Comment; }
            set { _nativeCredential.Comment = value; }
        }

        public DateTime LastWritten
        {
            get
            {
                long lastWritten = _nativeCredential.LastWritten.dwHighDateTime;
                lastWritten = (lastWritten << 32) + _nativeCredential.LastWritten.dwLowDateTime;
                return DateTime.FromFileTime(lastWritten);
            }
        }

        public string CredentialBlob
        {
            get
            {
                if (_nativeCredential.CredentialBlobSize == 0) return string.Empty;
                var length = (int)_nativeCredential.CredentialBlobSize / sizeof(char);
                char[] chars = new char[length];
                Marshal.Copy(_nativeCredential.CredentialBlob, chars, 0, length);
                return new string(chars);
            }
            set
            {
                var length = value.Length*sizeof (char);
                _nativeCredential.CredentialBlob = Marshal.AllocHGlobal(length);
                var bytes = value.SelectMany(BitConverter.GetBytes).ToArray();
                Marshal.Copy(bytes, 0, _nativeCredential.CredentialBlob, length);
                _nativeCredential.CredentialBlobSize = (uint)length;
            }

        }

        public void Save()
        {
            var result = UnsafeAdvapi32.CredWriteW(ref _nativeCredential, 0);
            if (!result)
            {
                var error = (CredentialErrors) Marshal.GetLastWin32Error();
                throw new CredentialManagerException(error, "Could not save credential.");
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~Credential()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;
            UnsafeAdvapi32.CredFree(_nativeCredential);
        }
    }
}
