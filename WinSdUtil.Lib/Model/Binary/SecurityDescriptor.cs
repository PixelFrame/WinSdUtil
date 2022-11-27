using System.Runtime.InteropServices;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityDescriptor
    {
        byte Revision;
        byte Sbz1;
        ushort Control;
        uint OffsetOwner;
        uint OffsetGroup;
        uint OffsetSacl;
        uint OffsetDacl;
        SID OwnerSid;
        SID GroupSid;
    }
}
