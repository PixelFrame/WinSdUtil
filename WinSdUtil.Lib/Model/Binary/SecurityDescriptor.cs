using System.Runtime.InteropServices;

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
            Revision = blob[0];
            Sbz1 = blob[1];
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
            int size = 20;
            if (((ControlFlags)Control & ControlFlags.SystemAclPresent) != 0) size += SACL.AclSize;
            if (((ControlFlags)Control & ControlFlags.DiscretionaryAclPresent) != 0) size += DACL.AclSize;
            if (OffsetOwner != 0) size += (8 + OwnerSid.SubAuthorityCount * 4);
            if (OffsetGroup != 0) size += (8 + GroupSid.SubAuthorityCount * 4);
            byte[] arr = new byte[size];
            arr[0] = Revision;
            arr[1] = Sbz1;
            BitConverter.GetBytes(Control).CopyTo(arr, 2);
            BitConverter.GetBytes(OffsetOwner).CopyTo(arr, 4);
            BitConverter.GetBytes(OffsetGroup).CopyTo(arr, 8);
            BitConverter.GetBytes(OffsetSacl).CopyTo(arr, 12);
            BitConverter.GetBytes(OffsetDacl).CopyTo(arr, 16);
            if (((ControlFlags)Control & ControlFlags.SystemAclPresent) != 0 && SACL.Aces != null) SACL.GetBytes(ref arr, (int)OffsetSacl);
            if (((ControlFlags)Control & ControlFlags.DiscretionaryAclPresent) != 0 && DACL.Aces != null) DACL.GetBytes(ref arr, (int)OffsetDacl);
            if (OffsetOwner != 0) OwnerSid.GetBytes(ref arr, (int)OffsetOwner);
            if (OffsetGroup != 0) GroupSid.GetBytes(ref arr, (int)OffsetGroup);

            return arr;
        }
    }
}
