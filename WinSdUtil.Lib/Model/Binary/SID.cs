﻿using System.Runtime.InteropServices;
using WinSdUtil.Lib.Helper;

namespace WinSdUtil.Lib.Model.Binary
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SID
    {
        internal byte Revision;
        internal byte SubAuthorityCount;
        internal SidIdentifierAuthority IdentifierAuthority;
        internal uint[] SubAuthority;

        internal SID(byte[] blob, int offset)
        {
            Revision = blob[offset];
            SubAuthorityCount = blob[offset + 1];
            IdentifierAuthority = new SidIdentifierAuthority(blob, offset + 2);
            SubAuthority = new uint[SubAuthorityCount];
            Buffer.BlockCopy(blob, offset + 8, SubAuthority, 0, SubAuthorityCount * 4);
        }

        internal void GetBytes(ref byte[] target, int offset)
        {
            target[offset] = Revision;
            target[offset + 1] = SubAuthorityCount;
            IntPtr identifierAuthorityPtr = Marshal.AllocHGlobal(6);
            Marshal.StructureToPtr(IdentifierAuthority, identifierAuthorityPtr, false);
            Marshal.Copy(identifierAuthorityPtr, target, offset + 2, 6);
            Marshal.FreeHGlobal(identifierAuthorityPtr);
            Buffer.BlockCopy(SubAuthority, 0, target, offset + 8, SubAuthorityCount);
        }
    }
}
