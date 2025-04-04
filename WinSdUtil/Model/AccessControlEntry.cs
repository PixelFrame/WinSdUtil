﻿using System.Text;
using System.Text.RegularExpressions;
using WinSdUtil.Helper;
using WinSdUtil.Model.Binary;

namespace WinSdUtil.Model
{
    public class AccessControlEntry
    {
        public static readonly string RegexPatternAce = @"\((?<AceType>[A-Z]*);(?<AceFlags>[A-Z]*);(?<Rights>0x[0-9a-fA-F]*|[0-9A-Z]*);(?<ObjectGuid>[A-Za-z\d\-]*);(?<InheritObjectGuid>[A-Za-z\d\-]*);(?<AccountSid>[A-Za-z\d\-]*);?(\((?<ResourceAttribute>[^\(\)]*)\))?\)";

        public AceType Type { get; set; } = 0;
        public AceFlags Flags { get; set; } = 0;
        public AccessMask Mask { get; set; } = new();
        public Guid ObjectGuid { get; set; }
        public AdObjectGuid AdObjectGuid
        {
            get => new AdObjectGuid(ObjectGuid.ToString());
        }
        public Guid InheritObjectGuid { get; set; }
        public AdObjectGuid AdInheritObjectGuid
        {
            get => new AdObjectGuid(InheritObjectGuid.ToString());
        }
        public Trustee Trustee { get; set; } = new();
        public string ApplicationData { get; set; } = string.Empty;

        public AccessControlEntry() { }
        public AccessControlEntry(string SddlAce)
        {
            var regexMatchAce = Regex.Match(SddlAce, RegexPatternAce);
            if (!regexMatchAce.Success)
            {
                throw new ArgumentException("Invalid ACE SDDL");
            }

            var sddlType = regexMatchAce.Groups["AceType"].Value;
            if (!SddlMapping.AceTypeMapping.TryGetValue(sddlType, out AceType type)) { throw new ArgumentException($"Invalid ACE Type: {sddlType}"); }
            Type = type;

            Flags = 0;
            var sddlFlags = regexMatchAce.Groups["AceFlags"].Value.SplitInParts(2);
            foreach (var sddlFlag in sddlFlags)
            {
                if (!SddlMapping.AceFlagsMapping.TryGetValue(sddlFlag, out AceFlags flag)) { throw new ArgumentException($"Invalid ACE Flag: {sddlFlag}"); }
                Flags |= flag;
            }

            Mask.Full = 0;
            var sddlRights = regexMatchAce.Groups["Rights"].Value;
            Mask.FromSDDL(sddlRights);

            Trustee = new(regexMatchAce.Groups["AccountSid"].Value, 0);

            if (regexMatchAce.Groups["ObjectGuid"].Value != string.Empty) ObjectGuid = new Guid(regexMatchAce.Groups["ObjectGuid"].Value);
            if (regexMatchAce.Groups["InheritObjectGuid"].Value != string.Empty) InheritObjectGuid = new Guid(regexMatchAce.Groups["InheritObjectGuid"].Value);
            if (regexMatchAce.Groups["ResourceAttribute"].Value != string.Empty) ApplicationData = regexMatchAce.Groups["ResourceAttribute"].Value;
        }

        public string ToSDDL()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append(SddlMapping.AceTypeMapping.Inverse[Type]);
            sb.Append(';');

            if (((byte)Flags | 0xDF) != 0xDF) sb.Append(Flags.ToString());
            foreach (var flag in SddlMapping.AceFlagsMapping.Inverse.Keys)
            {
                if (flag == AceFlags.None) continue;
                if ((Flags & flag) != 0) sb.Append(SddlMapping.AceFlagsMapping.Inverse[flag]);
            }
            sb.Append(';');

            sb.Append(Mask.ToSDDL());
            sb.Append(';');

            if (ObjectGuid != Guid.Empty)
            {
                sb.Append(ObjectGuid);
            }
            sb.Append(';');

            if (InheritObjectGuid != Guid.Empty)
            {
                sb.Append(InheritObjectGuid);
            }
            sb.Append(';');

            sb.Append(Trustee.SddlName);

            if (ApplicationData != string.Empty)
            {
                sb.Append(';');
                sb.Append('(');
                sb.Append(ApplicationData);
                sb.Append(')');
            }
            sb.Append(')');
            return sb.ToString();
        }

        internal byte[] ToBinary()
        {
            switch (Type)
            {
                case AceType.AccessAllowed:
                case AceType.AccessDenied:
                case AceType.SystemAudit:
                case AceType.SystemAlarm:
                    var ace = new ACE();
                    ace.Header.AceType = (byte)Type;
                    ace.Header.AceFlags = (byte)Flags;
                    ace.Mask = Mask.Full;
                    ace.Sid = Trustee.ToBinarySid();
                    ace.Header.AceSize = (ushort)(16 + 4 * ace.Sid.SubAuthorityCount);
                    return ace.GetBytes();
                case AceType.AccessAllowedObject:
                case AceType.AccessDeniedObject:
                case AceType.SystemAuditObject:
                case AceType.SystemAlarmObject:
                    var aceObj = new ACE_Object();
                    aceObj.Header.AceType = (byte)Type;
                    aceObj.Header.AceFlags |= (byte)Flags;
                    aceObj.Mask = Mask.Full;
                    if (ObjectGuid != Guid.Empty)
                    {
                        aceObj.Flags |= 0x1;
                        aceObj.ObjectType = ObjectGuid;
                        aceObj.Header.AceSize += 16;
                    }
                    if (InheritObjectGuid != Guid.Empty)
                    {
                        aceObj.Flags |= 0x2;
                        aceObj.InheritedObjectType = InheritObjectGuid;
                        aceObj.Header.AceSize += 16;
                    }
                    aceObj.Sid = Trustee.ToBinarySid();
                    aceObj.Header.AceSize += (ushort)(20 + 4 * aceObj.Sid.SubAuthorityCount);
                    return aceObj.GetBytes();
                case AceType.AccessAllowedCallback:
                case AceType.AccessDeniedCallback:
                case AceType.SystemAuditCallback:
                case AceType.SystemAlarmCallback:
                    var aceCb = new ACE_Callback();
                    aceCb.Header.AceType = (byte)Type;
                    aceCb.Header.AceFlags = (byte)Flags;
                    aceCb.Mask = Mask.Full;
                    aceCb.Sid = Trustee.ToBinarySid();
                    aceCb.ApplicationData = Encoding.Unicode.GetBytes(ApplicationData);
                    aceCb.Header.AceSize = (ushort)(16 + 4 * aceCb.Sid.SubAuthorityCount + ApplicationData.Length * 2);
                    return aceCb.GetBytes();
                case AceType.AccessAllowedCallbackObject:
                case AceType.AccessDeniedCallbackObject:
                case AceType.SystemAuditCallbackObject:
                case AceType.SystemAlarmCallbackObject:
                    var aceCbObj = new ACE_Callback_Object();
                    aceCbObj.Header.AceType = (byte)Type;
                    aceCbObj.Header.AceFlags |= (byte)Flags;
                    aceCbObj.Mask = Mask.Full;
                    if (ObjectGuid != Guid.Empty)
                    {
                        aceCbObj.Flags |= 0x1;
                        aceCbObj.ObjectType = ObjectGuid;
                        aceCbObj.Header.AceSize += 16;
                    }
                    if (InheritObjectGuid != Guid.Empty)
                    {
                        aceCbObj.Flags |= 0x2;
                        aceCbObj.InheritedObjectType = InheritObjectGuid;
                        aceCbObj.Header.AceSize += 16;
                    }
                    aceCbObj.Sid = Trustee.ToBinarySid();
                    aceCbObj.ApplicationData = Encoding.Unicode.GetBytes(ApplicationData);  // This is likely wrong from test result, but we don't have more details about how to convert it to binary for now
                    aceCbObj.Header.AceSize += (ushort)(20 + 4 * aceCbObj.Sid.SubAuthorityCount + ApplicationData.Length * 2);
                    return aceCbObj.GetBytes();
                default:
                    throw new NotImplementedException($"The specified ACE type ({Type}) is not supported to be converted to binary.");
            }
        }

        public override string ToString() => ToString("");
        public string ToString(string Indent = "")
        {
            var sb = new StringBuilder();
            sb.Append(Indent);
            sb.Append(Type);
            sb.Append('|');
            sb.Append(Flags);
            sb.Append("(0x");
            sb.Append(((byte)Flags).ToString("X"));
            sb.Append(")|");
            if (ObjectGuid != Guid.Empty)
            {
                sb.Append("Object: ");
                sb.Append(AdObjectGuid.DisplayName);
                sb.Append('(');
                sb.Append(AdObjectGuid.Guid);
                sb.Append(")|");
            }
            if (InheritObjectGuid != Guid.Empty)
            {
                sb.Append("InheritedObject: ");
                sb.Append(AdInheritObjectGuid.DisplayName);
                sb.Append('(');
                sb.Append(AdInheritObjectGuid.Guid);
                sb.Append(")|");
            }
            sb.Append($"{Trustee.DisplayName} ({Trustee.SddlName})");

            return sb.ToString();
        }
    }
}
