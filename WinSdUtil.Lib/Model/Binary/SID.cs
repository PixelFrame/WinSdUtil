using System.Runtime.InteropServices;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SID
    {
        byte Revision;
        byte SubAuthorityCount;
        SidIdentifierAuthority IdentifierAuthority;
        uint[] SubAuthority;
    }
}
