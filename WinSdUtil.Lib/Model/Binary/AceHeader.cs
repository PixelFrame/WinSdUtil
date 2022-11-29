using System.Runtime.InteropServices;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AceHeader
    {
        internal byte AceType;
        internal byte AceFlags;
        internal ushort AceSize;
    }
}
