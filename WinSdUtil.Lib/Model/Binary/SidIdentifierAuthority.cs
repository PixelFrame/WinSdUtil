using System.Runtime.InteropServices;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SidIdentifierAuthority
    {
        byte b0;
        byte b1;
        byte b2;
        byte b3;
        byte b4;
        byte b5;

        internal SidIdentifierAuthority(byte[] blob, int offset)
        {
            b0 = blob[offset];
            b1 = blob[offset + 1];
            b2 = blob[offset + 2];
            b3 = blob[offset + 3];
            b4 = blob[offset + 4];
            b5 = blob[offset + 5];
        }
    }
}
