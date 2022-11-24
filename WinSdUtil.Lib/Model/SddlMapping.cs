using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace WinSdUtil.Lib.Model
{
    internal static class SddlMapping
    {
        internal static readonly Dictionary<string, uint> AccessMaskMapping = new()
        {
            { "GR", 0x80000000 },
            { "GW", 0x40000000 },
            { "GX", 0x20000000 },
            { "GA", 0x10000000 },
            { "WO", 0x00080000 },
            { "WD", 0x00040000 },
            { "RC", 0x00020000 },
            { "SD", 0x00010000 },
            { "CR", 0x00000100 },
            { "LO", 0x00000080 },
            { "DT", 0x00000040 },
            { "WP", 0x00000020 },
            { "RP", 0x00000010 },
            { "SW", 0x00000008 },
            { "LC", 0x00000004 },
            { "DC", 0x00000002 },
            { "CC", 0x00000001 },
            { "FA", 0x001F01FF },
            { "FR", 0x00120089 },
            { "FW", 0x00120016 },
            { "FX", 0x001200A0 },
            { "KA", 0x000F003F },
            { "KR", 0x00020019 },
            { "KW", 0x00020006 },
            { "KX", 0x00020019 }
        };

        internal static readonly Dictionary<string, string> AclFlagsMapping = new()
        {
            { "P" , "SE_DACL_PROTECTED" },
            { "AR", "SE_DACL_AUTO_INHERIT_REQ" },
            { "AI", "SE_DACL_AUTO_INHERITED" },
            { "NO_ACCESS_CONTROL", "The ACL is null." },
        };

        internal static readonly Dictionary<string, AceType> AceTypeMapping = new()
        {
            {"A", AceType.AccessAllowed},
            {"D", AceType.AccessDenied},
            {"OA", AceType.AccessAllowedObject},
            {"OD", AceType.AccessDeniedObject},
            {"AU", AceType.SystemAudit},
            {"AL", AceType.SystemAlarm},
            {"OU", AceType.SystemAuditObject},
            {"OL", AceType.SystemAlarmObject},
        };

        internal static readonly Dictionary<string, AceFlags> AceFlagsMapping = new()
        {
            {"CI", AceFlags.ContainerInherit},
            {"OI", AceFlags.ObjectInherit},
            {"NP", AceFlags.NoPropagateInherit},
            {"IO", AceFlags.InheritOnly},
            {"ID", AceFlags.Inherited},
            {"SA", AceFlags.SuccessfulAccess},
            {"FA", AceFlags.FailedAccess},
        };
    }
}
