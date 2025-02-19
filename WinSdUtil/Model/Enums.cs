// Enums copied from System.Security.AccessControl

namespace WinSdUtil
{
    [Flags]
    public enum ControlFlags : ushort
    {
        None = 0,
        OwnerDefaulted = 1,
        GroupDefaulted = 2,
        DiscretionaryAclPresent = 4,
        DiscretionaryAclDefaulted = 8,
        SystemAclPresent = 16,
        SystemAclDefaulted = 32,
        DiscretionaryAclUntrusted = 64,
        ServerSecurity = 128,
        DiscretionaryAclAutoInheritRequired = 256,
        SystemAclAutoInheritRequired = 512,
        DiscretionaryAclAutoInherited = 1024,
        SystemAclAutoInherited = 2048,
        DiscretionaryAclProtected = 4096,
        SystemAclProtected = 8192,
        RMControlValid = 16384,
        SelfRelative = 32768
    }

    public enum AceType : byte
    {
        AccessAllowed = 0,
        AccessDenied = 1,
        SystemAudit = 2,
        SystemAlarm = 3,
        AccessAllowedCompound = 4,
        AccessAllowedObject = 5,
        AccessDeniedObject = 6,
        SystemAuditObject = 7,
        SystemAlarmObject = 8,
        AccessAllowedCallback = 9,
        AccessDeniedCallback = 10,
        AccessAllowedCallbackObject = 11,
        AccessDeniedCallbackObject = 12,
        SystemAuditCallback = 13,
        SystemAlarmCallback = 14,
        SystemAuditCallbackObject = 15,
        SystemAlarmCallbackObject = 16,

        // ACE types after vista
        SystemMandatoryLabel = 17,
        SystemResourceAttribute = 18,
        SystemScopedPolicy = 19,
        SystemProcessTrustLabel = 20,
        SystemAccessFilter = 21,
    }

    [Flags]
    public enum AceFlags : byte
    {
        None = 0,
        ObjectInherit = 1,
        ContainerInherit = 2,
        NoPropagateInherit = 4,
        InheritOnly = 8,
        InheritanceFlags = 15,
        Inherited = 16,
        SuccessfulAccess = 64,
        FailedAccess = 128,
        AuditFlags = 192
    }
}