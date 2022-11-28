using System.Runtime.InteropServices;
using WinSdUtil.Lib.Helper;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SecurityDescriptor
    {
        internal byte Revision;
        internal byte Sbz1;
        internal ushort Control;
        internal uint OffsetOwner;
        internal uint OffsetGroup;
        internal uint OffsetSacl;
        internal uint OffsetDacl;
        internal SID OwnerSid;
        internal SID GroupSid;
        internal ACL SACL;
        internal ACL DACL;

        internal SecurityDescriptor(byte[] blob)
        {
            Revision= blob[0];
            Sbz1= blob[1];
            Control = BitConverter.ToUInt16(blob, 2);
            OffsetOwner = BitConverter.ToUInt32(blob, 4);
            OffsetGroup = BitConverter.ToUInt32(blob, 8);
            OffsetSacl = BitConverter.ToUInt32(blob, 12);
            OffsetDacl = BitConverter.ToUInt32(blob, 16);
            OwnerSid = new SID(blob, (int)OffsetOwner);
            GroupSid = new SID(blob, (int)OffsetGroup);
            SACL = new ACL(blob, (int)OffsetSacl);
            DACL = new ACL(blob, (int)OffsetDacl);
        }
    }
}
