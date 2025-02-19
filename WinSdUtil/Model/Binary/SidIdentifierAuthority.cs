using System.Runtime.InteropServices;

namespace WinSdUtil.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SidIdentifierAuthority
    {
        internal byte b0;
        internal byte b1;
        internal byte b2;
        internal byte b3;
        internal byte b4;
        internal byte b5;

        internal SidIdentifierAuthority(byte[] blob, int offset)
        {
            b0 = blob[offset];
            b1 = blob[offset + 1];
            b2 = blob[offset + 2];
            b3 = blob[offset + 3];
            b4 = blob[offset + 4];
            b5 = blob[offset + 5];
        }

        internal SidIdentifierAuthority(ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                b0 = bytes[5];
                b1 = bytes[4];
                b2 = bytes[3];
                b3 = bytes[2];
                b4 = bytes[1];
                b5 = bytes[0];
            }
            else
            {
                b0 = bytes[0];
                b1 = bytes[1];
                b2 = bytes[2];
                b3 = bytes[3];
                b4 = bytes[4];
                b5 = bytes[5];
            }
        }

        public override string ToString()
        {
            var value = (b0 << 40) + (b1 << 32) + (b2 << 24) + (b3 << 16) + (b4 << 8) + b5;
            return value.ToString();
        }
    }
}
