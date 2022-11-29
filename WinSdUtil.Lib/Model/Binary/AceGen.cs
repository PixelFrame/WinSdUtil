using WinSdUtil.Lib.Helper;

namespace WinSdUtil.Lib.Model.Binary
{
    internal static class AceGen
    {
        internal static List<ACE_Callback_Object> FromBytes(in byte[] bytes, int offset, int length)
        {
            var result = new List<ACE_Callback_Object>();
            var offsetLimit = offset + length;
            while (offset < offsetLimit)
            {
                var newAce = new ACE_Callback_Object();
                newAce.Header = new AceHeader()
                {
                    AceType = bytes[offset],
                    AceFlags = bytes[offset + 1],
                    AceSize = BitConverter.ToUInt16(bytes, offset + 2),
                };
                switch ((AceType)newAce.Header.AceType)
                {
                    case AceType.AccessAllowed:
                    case AceType.AccessDenied:
                    case AceType.SystemAudit:
                    case AceType.SystemAlarm:
                        newAce.Mask = BitConverter.ToUInt32(bytes, offset + 4);
                        newAce.Sid = new SID(bytes, offset + 8);
                        offset += newAce.Header.AceSize;
                        break;

                    case AceType.AccessAllowedObject:
                    case AceType.AccessDeniedObject:
                    case AceType.SystemAuditObject:
                    case AceType.SystemAlarmObject:
                        newAce.Mask = BitConverter.ToUInt32(bytes, offset + 4);
                        newAce.Flags = BitConverter.ToUInt32(bytes, offset + 8);
                        var _offset = offset + 12;
                        if ((newAce.Flags & 0x1) != 0)
                        {
                            newAce.ObjectType = new Guid(bytes.RangeSubset(_offset, 16));
                            _offset += 16;
                        }
                        if ((newAce.Flags & 0x2) != 0)
                        {
                            newAce.InheritedObjectType = new Guid(bytes.RangeSubset(_offset, 16));
                            _offset += 16;
                        }
                        newAce.Sid = new SID(bytes, _offset);
                        offset += newAce.Header.AceSize;
                        break;

                    case AceType.AccessAllowedCallback:
                    case AceType.AccessDeniedCallback:
                    case AceType.SystemAuditCallback:
                    case AceType.SystemAlarmCallback:
                        newAce.Mask = BitConverter.ToUInt32(bytes, offset + 4);
                        newAce.Sid = new SID(bytes, offset + 8);
                        var sidLen = 8 + 4 * newAce.Sid.SubAuthorityCount;
                        var appDataLen = newAce.Header.AceSize - 8 - sidLen;
                        newAce.ApplicationData = new byte[appDataLen];
                        Buffer.BlockCopy(bytes, offset + 8 + sidLen, newAce.ApplicationData, 0, appDataLen);
                        offset += newAce.Header.AceSize;
                        break;

                    case AceType.AccessAllowedCallbackObject:
                    case AceType.AccessDeniedCallbackObject:
                    case AceType.SystemAuditCallbackObject:
                    case AceType.SystemAlarmCallbackObject:
                        newAce.Mask = BitConverter.ToUInt32(bytes, offset + 4);
                        newAce.Flags = BitConverter.ToUInt32(bytes, offset + 8);
                        var __offset = offset + 12;
                        if ((newAce.Flags & 0x1) != 0)
                        {
                            newAce.ObjectType = new Guid(bytes.RangeSubset(__offset, 16));
                            __offset += 16;
                        }
                        if ((newAce.Flags & 0x2) != 0)
                        {
                            newAce.InheritedObjectType = new Guid(bytes.RangeSubset(__offset, 16));
                            __offset += 16;
                        }
                        newAce.Sid = new SID(bytes, __offset);
                        var __sidLen = 8 + 4 * newAce.Sid.SubAuthorityCount;
                        var __appDataLen = newAce.Header.AceSize - (__offset - offset) - __sidLen;
                        newAce.ApplicationData = new byte[__appDataLen];
                        Buffer.BlockCopy(bytes, __offset + __sidLen, newAce.ApplicationData, 0, __appDataLen);
                        offset += newAce.Header.AceSize;
                        break;
                    default:
                        throw new NotImplementedException($"The specified ACE type ({(AceType)newAce.Header.AceType}) is not supported to be converted to binary.");
                }
                result.Add(newAce);
            }
            return result;
        }

        internal static AccessControlEntry ToManagedAce(this ACE_Callback_Object structAce)
        {
            var manAce = new AccessControlEntry()
            {
                Type = (AceType)structAce.Header.AceType,
                Flags = (AceFlags)structAce.Header.AceFlags,
                Mask = new AccessMask() { Full = structAce.Mask },
                ObjectGuid = structAce.ObjectType.GetValueOrDefault(),
                InheritObjectGuid = structAce.InheritedObjectType.GetValueOrDefault(),
                Trustee = new Trustee(structAce.Sid.ToString(), 1),
                ApplicationData = structAce.ApplicationData
            };
            return manAce;
        }
    }
}
