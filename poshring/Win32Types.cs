using System;
using System.Runtime.InteropServices;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace poshring
{
    [Flags]
    public enum CredentialFlags : uint
    {
        None = 0x0,
        PromptNow = 0x2,
        UsernameTarget = 0x4
    }

    public enum CredentialErrors : uint
    {
        Success = 0x0,
        InvalidParameter = 0x0057,
        InvalidFlags = 0x03EC,
        NotFound = 0x0490,
        NoSuchLogonSession = 0x0520,
        BadUsername = 0x089A
    }

    public enum CredentialPersist : uint
    {
        Session = 1,
        LocalMachine = 2,
        Enterprise = 3
    }

    public enum CredentialType : uint
    {
        Generic = 1,
        DomainPassword = 2,
        DomainCertificate = 3,
        DomainVisiblePassword = 4,
        GenericCertificate = 5,
        DomainExtended = 6,
        Maximum = 7,
        MaximumEx = Maximum + 1000
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NativeCredential
    {
        public CredentialFlags Flags;
        public CredentialType Type;
        public string TargetName;
        public string Comment;
        public FILETIME LastWritten;
        public uint CredentialBlobSize;
        public IntPtr CredentialBlob;
        public CredentialPersist Persist;
        public uint AttributeCount;
        public IntPtr Attributes;
        public string TargetAlias;
        public string UserName;
    }
}
