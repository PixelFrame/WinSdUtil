using System.Runtime.InteropServices;
using WinSdUtil.Lib.Helper;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SID
    {
        internal byte Revision;
        internal byte SubAuthorityCount;
        internal SidIdentifierAuthority IdentifierAuthority;
        internal uint[] SubAuthority;

        internal SID(byte[] blob, int offset)
        {
            Revision = blob[offset];
            SubAuthorityCount = blob[offset + 1];
            IdentifierAuthority = new SidIdentifierAuthority(blob, offset + 2);
            SubAuthority = new uint[SubAuthorityCount];
            Buffer.BlockCopy(blob, offset + 8, SubAuthority, 0, SubAuthorityCount * 4);
        }
    }
}
