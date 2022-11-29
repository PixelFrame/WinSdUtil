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
        internal ACL SACL;
        internal ACL DACL;
        internal SID OwnerSid;
        internal SID GroupSid;

        internal SecurityDescriptor(byte[] blob)
        {
            Revision= blob[0];
            Sbz1= blob[1];
            Control = BitConverter.ToUInt16(blob, 2);
            OffsetOwner = BitConverter.ToUInt32(blob, 4);
            OffsetGroup = BitConverter.ToUInt32(blob, 8);
            OffsetSacl = BitConverter.ToUInt32(blob, 12);
            OffsetDacl = BitConverter.ToUInt32(blob, 16);
            SACL = new ACL(blob, (int)OffsetSacl);
            DACL = new ACL(blob, (int)OffsetDacl);
            OwnerSid = new SID(blob, (int)OffsetOwner);
            GroupSid = new SID(blob, (int)OffsetGroup);
        }

        internal byte[] GetBytes()
        {
            int size = 20 + SACL.AclSize + DACL.AclSize + 8 + OwnerSid.SubAuthorityCount * 4 + 8 + GroupSid.SubAuthorityCount * 4;
            byte[] arr = new byte[size];
            arr[0] = Revision; 
            arr[1] = Sbz1;
            BitConverter.GetBytes(Control) .CopyTo(arr, 2);
            BitConverter.GetBytes(OffsetOwner).CopyTo(arr, 4);
            BitConverter.GetBytes(OffsetGroup).CopyTo(arr, 8);
            BitConverter.GetBytes(OffsetSacl).CopyTo(arr, 12);
            BitConverter.GetBytes(OffsetDacl).CopyTo(arr, 16);
            SACL.GetBytes(ref arr, (int)OffsetSacl);
            DACL.GetBytes(ref arr, (int)OffsetDacl);
            OwnerSid.GetBytes(ref arr, (int)OffsetOwner);
            GroupSid.GetBytes(ref arr, (int)OffsetGroup);

            return arr;
        }
    }
}
