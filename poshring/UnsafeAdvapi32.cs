using System;
using System.Runtime.InteropServices;

namespace poshring
{
    public static class UnsafeAdvapi32
    {
        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CredDeleteW([In] string target, [In] CredentialType type, [In] int reservedFlag);

        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CredEnumerateW([In] string filter, [In] int flags, out int count, out IntPtr credentials);

        [DllImport("Advapi32.dll", SetLastError = true)]
        public static extern void CredFree([In] NativeCredential credential);

        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CredReadW([In] string target, [In] CredentialType type, [In] int reservedFlag,  out NativeCredential credential);

        [DllImport("Advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool CredWriteW([In] ref NativeCredential credential, [In] uint flags);
    }
}
