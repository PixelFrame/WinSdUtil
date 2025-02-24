using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using WinSdUtil.Helper;

namespace WinSdUtil.Model
{
    #region Enums
    [Flags]
    public enum AccessMask_Standard : uint
    {
        DELETE = 0x10000,
        READ_CONTROL = 0x20000,
        WRITE_DAC = 0x40000,
        WRITE_OWNER = 0x80000,
        SYNCHRONIZE = 0x100000,
        ACCESS_SYSTEM_SECURITY = 0x1000000,
        MAXIMUM_ALLOWED = 0x2000000,
        GENERIC_ALL = 0x10000000,
        GENERIC_EXECUTE = 0x20000000,
        GENERIC_WRITE = 0x40000000,
        GENERIC_READ = 0x80000000,
    }

    [Flags]
    public enum AccessMask_File : uint
    {
        FILE_ALL_ACCESS = 0x1F01FF,
        FILE_READ_DATA = 0x1,
        FILE_WRITE_DATA = 0x2,
        FILE_APPEND_DATA = 0x4,
        FILE_READ_EA = 0x8,
        FILE_WRITE_EA = 0x10,
        FILE_EXECUTE = 0x20,
        FILE_DELETE_CHILD = 0x40,
        FILE_READ_ATTRIBUTES = 0x80,
        FILE_WRITE_ATTRIBUTES = 0x100,
    }

    [Flags]
    public enum AccessMask_Directory : uint
    {
        FILE_ALL_ACCESS = 0x1F01FF,
        FILE_LIST_DIRECTORY = 0x1,
        FILE_ADD_FILE = 0x2,
        FILE_ADD_SUBDIRECTORY = 0x4,
        FILE_READ_EA = 0x8,
        FILE_WRITE_EA = 0x10,
        FILE_TRAVERSE = 0x20,
        FILE_DELETE_CHILD = 0x40,
        FILE_READ_ATTRIBUTES = 0x80,
        FILE_WRITE_ATTRIBUTES = 0x100
    }

    [Flags]
    public enum AccessMask_ActiveDirectory : uint
    {
        DS_CREATE_CHILD = 0x1,
        DS_DELETE_CHILD = 0x2,
        DS_LIST_CONTENTS = 0x4,
        DS_WRITE_PROPERTY_EXTENDED = 0x8,
        DS_READ_PROPERTY = 0x10,
        DS_WRITE_PROPERTY = 0x20,
        DS_DELETE_TREE = 0x40,
        DS_LIST_OBJECT = 0x80,
        DS_CONTROL_ACCESS = 0x100
    }

    [Flags]
    public enum AccessMask_Process : uint
    {
        PROCESS_ALL_ACCESS = 0x1FFFFF,
        PROCESS_TERMINATE = 0x1,
        PROCESS_CREATE_THREAD = 0x2,
        PROCESS_VM_OPERATION = 0x8,
        PROCESS_VM_READ = 0x10,
        PROCESS_VM_WRITE = 0x20,
        PROCESS_DUP_HANDLE = 0x40,
        PROCESS_CREATE_PROCESS = 0x80,
        PROCESS_SET_QUOTA = 0x100,
        PROCESS_SET_INFORAMTION = 0x200,
        PROCESS_QUERY_INFORMATION = 0x400,
        PROCESS_QUERY_LIMITED_INFORMATION = 0x1000
    }

    [Flags]
    public enum AccessMask_Service : uint
    {
        SERVICE_ALL_ACCESS = 0xF01FF,
        SERVICE_QUERY_CONFIG = 0x1,
        SERVICE_CHANGE_CONFIG = 0x2,
        SERVICE_QUERY_STATUS = 0x4,
        SERVICE_ENUMERATE_DEPENDENTS = 0x8,
        SERVICE_START = 0x10,
        SERVICE_STOP = 0x20,
        SERVICE_PAUSE_CONTINUE = 0x40,
        SERVICE_INTERROGATE = 0x80,
        SERVICE_USER_DEFINED_CONTROL = 0x100
    }

    [Flags]
    public enum AccessMask_SCM : uint
    {
        SC_MANAGER_ALL_ACCESS = 0xF003F,
        SC_MANAGER_CONNECT = 0x1,
        SC_MANAGER_CREATE_SERVICE = 0x2,
        SC_MANAGER_ENUMERATE_SERVICE = 0x4,
        SC_MANAGER_LOCK = 0x8,
        SC_MANAGER_QUERY_LOCK_STATUS = 0x10,
        SC_MANAGER_MODIFY_BOOT_CONFIG = 0x20
    }

    [Flags]
    public enum AccessMask_Registry : uint
    {
        KEY_ALL_ACCESS = 0xF003F,
        KEY_QUERY_VALUE = 0x1,
        KEY_SET_VALUE = 0x2,
        KEY_CREATE_SUB_KEY = 0x4,
        KEY_ENUMERATE_SUB_KEYS = 0x8,
        KEY_EXECUTE = 0x20019,
        KEY_READ = 0x20019,
        KEY_WRITE = 0x20006,
        KEY_NOTIFY = 0x10,
        KEY_CREATE_LINK = 0x20,
        KEY_WOW64_64KEY = 0x100,
        KEY_WOW64_32KEY = 0x200
    }

    [Flags]
    public enum AccessMask_SrvsvcConfigInfo : uint
    {
        ReadServerInfo = 0x1,
        ReadAdvancedServerInfo = 0x2,
        ReadAdministrativeServerInfo = 0x4,
        ChangeServerInfo = 0x10,
    }

    [Flags]
    public enum AccessMask_SrvsvcConnection : uint
    {
        EnumerateConnections = 0x1,
    }

    [Flags]
    public enum AccessMask_SrvsvcFile : uint
    {
        EnumerateOpenFiles = 0x1,
        ForceFilesClosed = 0x10,
    }

    [Flags]
    public enum AccessMask_SrvsvcServerDiskEnum : uint
    {
        EnumerateDisks = 0x1,
    }

    [Flags]
    public enum AccessMask_SrvsvcSessionInfo : uint
    {
        ReadSessionInfo = 0x1,
        ReadAdministrativeSessionInfo = 0x2,
        ChangeServerInfo = 0x10,
    }

    [Flags]
    public enum AccessMask_SrvsvcShareAdminConnect : uint
    {
        ConnectToServer = 0x1,
        ConnectToPausedServer = 0x2,
    }

    [Flags]
    public enum AccessMask_SrvsvcShareAdminInfo : uint
    {
        ReadShareInfo = 0x1,
        ReadAdministrativeShareInfo = 0x2,
        ChangeShareInfo = 0x10,
    }

    [Flags]
    public enum AccessMask_SrvsvcShareChange : uint
    {
        ReadShareUserInfo = 0x1,
        ReadAdminShareUserInfo = 0x2,
        SetShareInfo = 0x10,
    }

    [Flags]
    public enum AccessMask_SrvsvcShareConnect : uint
    {
        ConnectToServer = 0x1,
        ConnectToPausedServer = 0x2,
    }

    [Flags]
    public enum AccessMask_SrvsvcShareFileInfo : uint
    {
        ReadShareInfo = 0x1,
        ReadAdministrativeShareInfo = 0x2,
        ChangeShareInfo = 0x10,
    }

    [Flags]
    public enum AccessMask_SrvsvcSharePrintInfo : uint
    {
        ReadShareInfo = 0x1,
        ReadAdministrativeShareInfo = 0x2,
        ChangeShareInfo = 0x10,
    }

    [Flags]
    public enum AccessMask_SrvsvcStatisticsInfo : uint
    {
        ReadStatistics = 0x1,
    }

    [Flags]
    public enum AccessMask_SrvsvcTransportEnum : uint
    {
        Enumerate = 0x1,
        AdvancedEnumerate = 0x2,
        SetInfo = 0x10,
    }

    // From WinSDK wmistr.h
    [Flags]
    public enum AccessMask_WmiGuidObject : uint
    {
        WMIGUID_QUERY = 0x0001,
        WMIGUID_SET = 0x0002,
        WMIGUID_NOTIFICATION = 0x0004,
        WMIGUID_READ_DESCRIPTION = 0x0008,
        WMIGUID_EXECUTE = 0x0010,
        TRACELOG_CREATE_REALTIME = 0x0020,
        TRACELOG_CREATE_ONDISK = 0x0040,
        TRACELOG_GUID_ENABLE = 0x0080,
        TRACELOG_ACCESS_KERNEL_LOGGER = 0x0100,
        //TRACELOG_LOG_EVENT = 0x0200,           //Pre-Vista
        TRACELOG_CREATE_INPROC = 0x0200,
        TRACELOG_ACCESS_REALTIME = 0x0400,
        TRACELOG_REGISTER_GUIDS = 0x0800,
        TRACELOG_JOIN_GROUP = 0x1000,
    }

    [Flags]
    public enum AccessMask_FwpFilterCondition : uint
    {
        FWP_ACTRL_MATCH_FILTER = 0x1
    }

    [Flags]
    public enum AccessMask_Unknown : uint
    {
        BIT0001 = 0x1,
        BIT0002 = 0x2,
        BIT0004 = 0x4,
        BIT0008 = 0x8,
        BIT0010 = 0x10,
        BIT0020 = 0x20,
        BIT0040 = 0x40,
        BIT0080 = 0x80,
        BIT0100 = 0x100,
        BIT0200 = 0x200,
        BIT0400 = 0x400,
        BIT0800 = 0x800,
        BIT1000 = 0x1000,
        BIT2000 = 0x2000,
        BIT4000 = 0x4000,
        BIT8000 = 0x8000,
    }

    public enum AccessMaskType
    {
        File,
        Directory,
        ActiveDirectory,
        Process,
        Service,
        SCM,
        Registry,
        SrvsvcConfigInfo,
        SrvsvcConnection,
        SrvsvcFile,
        SrvsvcServerDiskEnum,
        SrvsvcSessionInfo,
        SrvsvcShareAdminConnect,
        SrvsvcShareAdminInfo,
        SrvsvcShareChange,
        SrvsvcShareConnect,
        SrvsvcShareFileInfo,
        SrvsvcSharePrintInfo,
        SrvsvcStatisticsInfo,
        SrvsvcTransportEnum,
        WmiGuidObject,
        FwpFilterCondition,
        Unknown,
    }
    #endregion

    public class AccessMask : IEnumerable<(int, bool)>
    {
        public uint Standard { get; set; }
        public uint ObjectSpecific { get; set; }

        private AccessMaskType objectType = AccessMaskType.Unknown;
        public AccessMaskType ObjectType
        {
            get => objectType;
            set
            {
                objectType = value;
                objectTypeType = Type.GetType($"WinSdUtil.Model.AccessMask_{value}", true, true) ?? typeof(AccessMask_Unknown);
            }
        }

        private Type objectTypeType = typeof(AccessMask_Unknown);
        public Type ObjectTypeType => objectTypeType;

        public uint Full
        {
            get { return Standard | ObjectSpecific; }
            set { Standard = value & 0xFFFF0000; ObjectSpecific = value & 0x0000FFFF; }
        }
        
        public bool this[int index]
        {
            get
            {
                if (index < 0 || index > 31) throw new ArgumentOutOfRangeException("index");
                uint bit = 1;
                bit <<= index;
                return (Full & bit) == bit;
            }
            set
            {
                if (index < 0 || index > 31) throw new ArgumentOutOfRangeException("index");
                uint bit = 1;
                bit <<= index;
                if (value)
                {
                    Full |= bit;
                }
                else
                {
                    Full &= ~bit;
                }
            }
        }
        
        public IEnumerable<string> BitNames
        {
            get
            {
                for (int j = 0; j < 16; ++j)
                {
                    var i = unchecked((uint)(1 << j));
                    yield return Enum.GetName(ObjectTypeType, i) ?? "Undefined";
                }
                for (int j = 16; j < 32; ++j)
                {
                    var i = unchecked((uint)(1 << j));
                    yield return Enum.GetName(typeof(AccessMask_Standard), i) ?? "Undefined";
                }
            }
        }

        public IEnumerable<string> ToStrings()
        {
            var list = new List<string>();
            var stdMask = (AccessMask_Standard)Standard;
            foreach (AccessMask_Standard __ in Enum.GetValues(typeof(AccessMask_Standard)))
            {
                if ((stdMask & __) == __) list.Add(__.ToString());
            }

            switch (ObjectType)
            {
                case AccessMaskType.File:
                    var fileMask = (AccessMask_File)Full;                                                       // As FILE_ALL_ACCESS overlaps generic flag, we need to use Full here. Same to Directory, Process, Service, SCM, Registry.
                    foreach (AccessMask_File __ in Enum.GetValues(typeof(AccessMask_File)))
                    {
                        if ((fileMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.Directory:
                    var dirMask = (AccessMask_Directory)Full;
                    foreach (AccessMask_Directory __ in Enum.GetValues(typeof(AccessMask_Directory)))
                    {
                        if ((dirMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.ActiveDirectory:
                    var adMask = (AccessMask_ActiveDirectory)ObjectSpecific;
                    foreach (AccessMask_ActiveDirectory __ in Enum.GetValues(typeof(AccessMask_ActiveDirectory)))
                    {
                        if ((adMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.Process:
                    var procMask = (AccessMask_Process)Full;
                    foreach (AccessMask_Process __ in Enum.GetValues(typeof(AccessMask_Process)))
                    {
                        if ((procMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.Service:
                    var svcMask = (AccessMask_Service)Full;
                    foreach (AccessMask_Service __ in Enum.GetValues(typeof(AccessMask_Service)))
                    {
                        if ((svcMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SCM:
                    var scmMask = (AccessMask_SCM)Full;
                    foreach (AccessMask_SCM __ in Enum.GetValues(typeof(AccessMask_SCM)))
                    {
                        if ((scmMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.Registry:
                    var regMask = (AccessMask_Registry)Full;
                    foreach (AccessMask_Registry __ in Enum.GetValues(typeof(AccessMask_Registry)))
                    {
                        if ((regMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcConfigInfo:
                    var srvsvcConfigInfoMask = (AccessMask_SrvsvcConfigInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcConfigInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcConfigInfo)))
                    {
                        if ((srvsvcConfigInfoMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcConnection:
                    var srvsvcConnMask = (AccessMask_SrvsvcConnection)ObjectSpecific;
                    foreach (AccessMask_SrvsvcConnection __ in Enum.GetValues(typeof(AccessMask_SrvsvcConnection)))
                    {
                        if ((srvsvcConnMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcFile:
                    var srvsvcFileMask = (AccessMask_SrvsvcFile)ObjectSpecific;
                    foreach (AccessMask_SrvsvcFile __ in Enum.GetValues(typeof(AccessMask_SrvsvcFile)))
                    {
                        if ((srvsvcFileMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcServerDiskEnum:
                    var srvsvcServerDiskEnumMask = (AccessMask_SrvsvcServerDiskEnum)ObjectSpecific;
                    foreach (AccessMask_SrvsvcServerDiskEnum __ in Enum.GetValues(typeof(AccessMask_SrvsvcServerDiskEnum)))
                    {
                        if ((srvsvcServerDiskEnumMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcSessionInfo:
                    var srvsvcSessionInfoMask = (AccessMask_SrvsvcSessionInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcSessionInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcSessionInfo)))
                    {
                        if ((srvsvcSessionInfoMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareAdminConnect:
                    var srvsvcShareAdminConnectMask = (AccessMask_SrvsvcShareAdminConnect)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareAdminConnect __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareAdminConnect)))
                    {
                        if ((srvsvcShareAdminConnectMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareAdminInfo:
                    var srvsvcShareAdminInfoMask = (AccessMask_SrvsvcShareAdminInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareAdminInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareAdminInfo)))
                    {
                        if ((srvsvcShareAdminInfoMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareChange:
                    var srvsvcShareChangeMask = (AccessMask_SrvsvcShareChange)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareChange __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareChange)))
                    {
                        if ((srvsvcShareChangeMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareConnect:
                    var srvsvcShareConnectMask = (AccessMask_SrvsvcShareConnect)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareConnect __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareConnect)))
                    {
                        if ((srvsvcShareConnectMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareFileInfo:
                    var srvsvcShareFileInfoMask = (AccessMask_SrvsvcShareFileInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareFileInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareFileInfo)))
                    {
                        if ((srvsvcShareFileInfoMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcSharePrintInfo:
                    var srvsvcSharePrintInfoMask = (AccessMask_SrvsvcSharePrintInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcSharePrintInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcSharePrintInfo)))
                    {
                        if ((srvsvcSharePrintInfoMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcStatisticsInfo:
                    var srvsvcStatisticsInfoMask = (AccessMask_SrvsvcStatisticsInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcStatisticsInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcStatisticsInfo)))
                    {
                        if ((srvsvcStatisticsInfoMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcTransportEnum:
                    var srvsvcTransportEnumMask = (AccessMask_SrvsvcTransportEnum)ObjectSpecific;
                    foreach (AccessMask_SrvsvcTransportEnum __ in Enum.GetValues(typeof(AccessMask_SrvsvcTransportEnum)))
                    {
                        if ((srvsvcTransportEnumMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.WmiGuidObject:
                    var wmiGuidObjectEnumMask = (AccessMask_WmiGuidObject)ObjectSpecific;
                    foreach (AccessMask_WmiGuidObject __ in Enum.GetValues(typeof(AccessMask_WmiGuidObject)))
                    {
                        if ((wmiGuidObjectEnumMask & __) == __) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.FwpFilterCondition:
                    var fwpFilterConditionEnumMask = (AccessMask_FwpFilterCondition)ObjectSpecific;
                    foreach (AccessMask_FwpFilterCondition __ in Enum.GetValues(typeof(AccessMask_FwpFilterCondition)))
                    {
                        if ((fwpFilterConditionEnumMask & __) == __) list.Add(__.ToString());
                    }
                    break;
            }

            return list;
        }

        public string ToSDDL()
        {
            if (SddlMapping.AccessMaskMapping.Inverse.TryGetValue(Full, out string right))
            {
                return right;
            }

            if (SddlMapping.FileAccessMaskMapping.Inverse.TryGetValue(Full, out string fright))
            {
                return fright;
            }

            if (SddlMapping.InverseRegistryAccessMaskMapping.TryGetValue(Full, out string? rright))
            {
                return rright;
            }

            // Not covered by SDDL, use number directly
            if ((Full | 0xF00F01FF) != 0xF00F01FF)
            {
                return $"0x{Full:x}";
            }

            var sb = new StringBuilder();
            uint mask = 0x1;
            for (int i = 0; i < 32; ++i)
            {
                if ((Full & mask) != 0) sb.Append(SddlMapping.AccessMaskMapping.Inverse[mask]);
                mask <<= 1;
            }
            return sb.ToString();
        }

        public void FromSDDL(string sddlRights)
        {
            if (Regex.IsMatch(sddlRights, @"0x[0-9a-fA-F]"))
            {
                Full = Convert.ToUInt32(sddlRights, 16);
            }
            else if (Regex.IsMatch(sddlRights, @"\d+"))
            {
                if (!uint.TryParse(sddlRights, out uint accessMask))
                { throw new ArgumentException($"Invalid ACE Right: {sddlRights}"); }
                Full = accessMask;
            }
            else
            {
                Full = 0;
                var sddlRightsList = sddlRights.SplitInParts(2);
                foreach (var sddlRight in sddlRightsList)
                {
                    if (SddlMapping.AccessMaskMapping.TryGetValue(sddlRight, out uint accessBit)
                        || SddlMapping.FileAccessMaskMapping.TryGetValue(sddlRight, out accessBit)
                        || SddlMapping.RegistryAccessMaskMapping.TryGetValue(sddlRight, out accessBit)
                        ) { Full |= accessBit; }
                    else
                    {
                        throw new ArgumentException($"Invalid ACE Right: {sddlRight}");
                    }
                }
            }
        }

        public IEnumerator<(int, bool)> GetEnumerator()
        {
            for (int j = 0; j < 32; ++j)
            {
                var i = unchecked((uint)(1 << j));
                yield return (j, (i & Full) == i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int j = 0; j < 32; ++j)
            {
                var i = unchecked((uint)(1 << j));
                yield return (j, (i & Full) == i);
            }
        }
    }
}