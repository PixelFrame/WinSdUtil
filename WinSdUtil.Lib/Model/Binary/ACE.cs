using System.Runtime.InteropServices;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ACE
    {
        internal AceHeader Header;
        internal uint Mask;
        internal SID Sid;

        internal byte[] GetBytes()
        {
            var arr = new byte[Header.AceSize];
            arr[0] = Header.AceType;
            arr[1] = Header.AceFlags;
            BitConverter.GetBytes(Header.AceSize).CopyTo(arr, 2);
            BitConverter.GetBytes(Mask).CopyTo(arr, 4);
            Sid.GetBytes(ref arr, 8);
            return arr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ACE_Object
    {
        internal AceHeader Header;
        internal uint Mask;
        internal uint Flags;
        internal Guid? ObjectType;
        internal Guid? InheritedObjectType;
        internal SID Sid;

        internal byte[] GetBytes()
        {
            var arr = new byte[Header.AceSize];
            arr[0] = Header.AceType;
            arr[1] = Header.AceFlags;
            BitConverter.GetBytes(Header.AceSize).CopyTo(arr, 2);
            BitConverter.GetBytes(Mask).CopyTo(arr, 4);
            BitConverter.GetBytes(Flags).CopyTo(arr, 8);
            var offset = 12;
            if (ObjectType != null)
            {
                Buffer.BlockCopy(ObjectType.Value.ToByteArray(), 0, arr, offset, 16);
                offset += 16;
            }
            if (InheritedObjectType != null)
            {
                Buffer.BlockCopy(InheritedObjectType.Value.ToByteArray(), 0, arr, offset, 16);
                offset += 16;
            }
            Sid.GetBytes(ref arr, offset);
            return arr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ACE_Callback
    {
        internal AceHeader Header;
        internal uint Mask;
        internal SID Sid;
        internal byte[] ApplicationData;

        internal byte[] GetBytes()
        {
            var arr = new byte[Header.AceSize];
            arr[0] = Header.AceType;
            arr[1] = Header.AceFlags;
            BitConverter.GetBytes(Header.AceSize).CopyTo(arr, 2);
            BitConverter.GetBytes(Mask).CopyTo(arr, 4);
            var sidLen = Sid.GetBytes(ref arr, 8);
            Buffer.BlockCopy(ApplicationData, 0, arr, 8 + sidLen, ApplicationData.Length);
            return arr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ACE_Callback_Object
    {
        internal AceHeader Header;
        internal uint Mask;
        internal uint Flags;
        internal Guid? ObjectType;
        internal Guid? InheritedObjectType;
        internal SID Sid;
        internal byte[] ApplicationData;

        internal byte[] GetBytes()
        {
            var arr = new byte[Header.AceSize];
            arr[0] = Header.AceType;
            arr[1] = Header.AceFlags;
            BitConverter.GetBytes(Header.AceSize).CopyTo(arr, 2);
            BitConverter.GetBytes(Mask).CopyTo(arr, 4);
            BitConverter.GetBytes(Flags).CopyTo(arr, 8);
            var offset = 12;
            if (ObjectType != null)
            {
                Buffer.BlockCopy(ObjectType.Value.ToByteArray(), 0, arr, offset, 16);
                offset += 16;
            }
            if (InheritedObjectType != null)
            {
                Buffer.BlockCopy(InheritedObjectType.Value.ToByteArray(), 0, arr, offset, 16);
                offset += 16;
            }
            var sidLen = Sid.GetBytes(ref arr, offset);
            Buffer.BlockCopy(ApplicationData, 0, arr, offset + sidLen, ApplicationData.Length);
            return arr;
        }
    }
}