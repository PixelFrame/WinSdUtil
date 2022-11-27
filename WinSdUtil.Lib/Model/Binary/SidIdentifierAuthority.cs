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
    }
}
