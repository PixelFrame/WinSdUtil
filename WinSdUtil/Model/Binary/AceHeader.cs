using System.Runtime.InteropServices;

namespace WinSdUtil.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct AceHeader
    {
        internal byte AceType;
        internal byte AceFlags;
        internal ushort AceSize;
    }
}
