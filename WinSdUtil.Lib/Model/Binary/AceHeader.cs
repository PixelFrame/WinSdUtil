using System.Runtime.InteropServices;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AceHeader
    {
        byte AceType;
        byte AceFlags;
        ushort AceSize;
    }
}
