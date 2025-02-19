namespace WinSdUtil.Model
{
    internal static class SddlMapping
    {
        internal static readonly BidirectionalDictionary<string, uint> AccessMaskMapping = new()
        {
            ["GR"] = 0x80000000,
            ["GW"] = 0x40000000,
            ["GX"] = 0x20000000,
            ["GA"] = 0x10000000,
            ["WO"] = 0x00080000,
            ["WD"] = 0x00040000,
            ["RC"] = 0x00020000,
            ["SD"] = 0x00010000,
            ["CR"] = 0x00000100,
            ["LO"] = 0x00000080,
            ["DT"] = 0x00000040,
            ["WP"] = 0x00000020,
            ["RP"] = 0x00000010,
            ["SW"] = 0x00000008,
            ["LC"] = 0x00000004,
            ["DC"] = 0x00000002,
            ["CC"] = 0x00000001,
        };

        internal static readonly BidirectionalDictionary<string, uint> FileAccessMaskMapping = new()
        {
            ["FA"] = 0x001F01FF,
            ["FR"] = 0x00120089,
            ["FW"] = 0x00120016,
            ["FX"] = 0x001200A0,
        };

        internal static readonly Dictionary<string, uint> RegistryAccessMaskMapping = new()
        {
            ["KA"] = 0x000F003F,
            ["KR"] = 0x00020019,
            ["KW"] = 0x00020006,
            ["KX"] = 0x00020019,
        };

        internal static readonly Dictionary<uint, string> InverseRegistryAccessMaskMapping = new()
        {
            [0x000F003F] = "KA",
            [0x00020019] = "KR",
            [0x00020006] = "KW",
        };

        internal static readonly BidirectionalDictionary<string, ControlFlags> DAclFlagsMapping = new()
        {
            ["P"] = ControlFlags.DiscretionaryAclProtected,
            ["AR"] = ControlFlags.DiscretionaryAclAutoInheritRequired,
            ["AI"] = ControlFlags.DiscretionaryAclAutoInherited,
            ["NO_ACCESS_CONTROL"] = ControlFlags.None,
        };

        internal static readonly BidirectionalDictionary<string, ControlFlags> SAclFlagsMapping = new()
        {
            ["P"] = ControlFlags.SystemAclProtected,
            ["AR"] = ControlFlags.SystemAclAutoInheritRequired,
            ["AI"] = ControlFlags.SystemAclAutoInherited,
            ["NO_ACCESS_CONTROL"] = ControlFlags.None,
        };

        internal static readonly BidirectionalDictionary<string, AceType> AceTypeMapping = new()
        {
            ["A"] = AceType.AccessAllowed,
            ["D"] = AceType.AccessDenied,
            ["OA"] = AceType.AccessAllowedObject,
            ["OD"] = AceType.AccessDeniedObject,
            ["AU"] = AceType.SystemAudit,
            ["AL"] = AceType.SystemAlarm,
            ["OU"] = AceType.SystemAuditObject,
            ["OL"] = AceType.SystemAlarmObject,
            ["ML"] = AceType.SystemMandatoryLabel,
            ["XA"] = AceType.AccessAllowedCallback,
            ["XD"] = AceType.AccessDeniedCallback,
            ["RA"] = AceType.SystemResourceAttribute,
            ["SP"] = AceType.SystemScopedPolicy,
            ["XU"] = AceType.SystemAuditCallback,
            ["ZA"] = AceType.AccessAllowedCallbackObject,
            ["TL"] = AceType.SystemProcessTrustLabel,
            ["FL"] = AceType.SystemAccessFilter,
        };

        internal static readonly BidirectionalDictionary<string, AceFlags> AceFlagsMapping = new()
        {
            ["CI"] = AceFlags.ContainerInherit,
            ["OI"] = AceFlags.ObjectInherit,
            ["NP"] = AceFlags.NoPropagateInherit,
            ["IO"] = AceFlags.InheritOnly,
            ["ID"] = AceFlags.Inherited,
            ["SA"] = AceFlags.SuccessfulAccess,
            ["FA"] = AceFlags.FailedAccess,
        };
    }
}
