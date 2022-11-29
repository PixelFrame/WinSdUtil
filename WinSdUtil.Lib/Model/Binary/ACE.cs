using System.Runtime.InteropServices;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ACE
    {
        internal AceHeader Header;
        internal uint Mask;
        internal SID Sid;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ACE_Object
    {
        AceHeader Header;
        uint Mask;
        uint Flags;
        Guid ObjectType;
        Guid InheritedObjectType;
        SID Sid;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ACE_Callback
    {
        AceHeader Header;
        uint Mask;
        SID Sid;
        byte[] ApplicationData;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ACE_Callback_Object
    {
        AceHeader Header;
        uint Mask;
        uint Flags;
        Guid ObjectType;
        Guid InheritedObjectType;
        SID Sid;
        byte[] ApplicationData;
    }
}