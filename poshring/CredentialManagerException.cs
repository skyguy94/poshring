using System;

namespace poshring
{
    class CredentialManagerException : Exception
    {
        public CredentialErrors Error;

        public CredentialManagerException(CredentialErrors error, string message)
            : base(message)
        {
            Error = error;
        }
    }
}
