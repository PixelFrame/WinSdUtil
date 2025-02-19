using System.Runtime.InteropServices;

namespace WinSdUtil.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ACL
    {
        internal byte AclRevision;
        internal byte Sbz1;
        internal ushort AclSize;
        internal ushort AceCount;
        internal ushort Sbz2;
        internal byte[] Aces;

        internal ACL(byte[] blob, int offset)
        {
            AclRevision = blob[offset];
            Sbz1 = blob[offset + 1];
            AclSize = BitConverter.ToUInt16(blob, offset + 2);
            AceCount = BitConverter.ToUInt16(blob, offset + 4);
            Sbz2 = BitConverter.ToUInt16(blob, offset + 6);
            Aces = new byte[AclSize - 8];
            Buffer.BlockCopy(blob, offset + 8, Aces, 0, AclSize - 8);
        }

        internal void GetBytes(ref byte[] target, int offset)
        {
            target[offset] = AclRevision;
            target[offset + 1] = Sbz1;
            BitConverter.GetBytes(AclSize).CopyTo(target, offset + 2);
            BitConverter.GetBytes(AceCount).CopyTo(target, offset + 4);
            BitConverter.GetBytes(Sbz2).CopyTo(target, offset + 6);
            Buffer.BlockCopy(Aces, 0, target, offset + 8, Aces.Length);
        }
    }
}
