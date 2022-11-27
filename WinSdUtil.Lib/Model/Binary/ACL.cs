using System.Runtime.InteropServices;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ACL
    {
        byte AclRevision;
        byte Sbz1;
        ushort AclSize;
        ushort AceCount;
        ushort Sbz2;
    }
}
