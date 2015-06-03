using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace poshring
{
    public class CredentialsManager
    {
        public IEnumerable<Credential> GetCredentials()
        {
            int count;
            IntPtr ptr;
            if (!UnsafeAdvapi32.CredEnumerateW(null, 0x1, out count, out ptr))
            {
                var error = (CredentialErrors) Marshal.GetLastWin32Error();
                switch (error)
                {
                    case CredentialErrors.Success:
                        break;
                    case CredentialErrors.NotFound:
                        return Enumerable.Empty<Credential>();
                    case CredentialErrors.NoSuchLogonSession:
                    case CredentialErrors.InvalidFlags:
                        throw new Win32Exception((int)error);
                    default:
                        throw new InvalidOperationException("Unexpected error while fetching credentials.");
                }
            }

            var credentials = Enumerable.Range(0, count)
                .Select(i => Marshal.ReadIntPtr(ptr, i*IntPtr.Size))
                .Select(p => new Credential((NativeCredential)Marshal.PtrToStructure(p, typeof(NativeCredential))));
            return credentials;
        }

        public void AddPasswordCredential(string targetName, string userName, string password, string comment)
        {
            using (var credential = new Credential
            {
                TargetName = targetName,
                UserName = userName,
                CredentialBlob = password,
                Comment = comment
            })
            {
                credential.Save();
            }
        }

        public void DeleteCredential(Credential credential)
        {
            if (!UnsafeAdvapi32.CredDeleteW(credential.TargetName, credential.Type, 0))
            {
                var error = (CredentialErrors)Marshal.GetLastWin32Error();
                switch (error)
                {
                    case CredentialErrors.Success:
                    case CredentialErrors.NotFound:
                        break;
                    case CredentialErrors.NoSuchLogonSession:
                    case CredentialErrors.InvalidFlags:
                        throw new Win32Exception((int)error);
                    default:
                        throw new InvalidOperationException("Unexpected error while removing credential.");
                }
            }
        }
    }
}