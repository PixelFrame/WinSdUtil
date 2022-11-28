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
        byte[] Aces;

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
    }
}
