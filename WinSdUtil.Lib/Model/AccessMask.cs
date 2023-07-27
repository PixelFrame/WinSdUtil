using System.Text;

namespace WinSdUtil.Lib.Model
{
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
        Unknown,
    }

    public class AccessMask
    {
        public uint Standard { get; set; }
        public uint ObjectSpecific { get; set; }
        public AccessMaskType ObjectType { get; set; }

        public uint Full
        {
            get { return Standard | ObjectSpecific; }
            set { Standard = value & 0xFFFF0000; ObjectSpecific = value & 0x0000FFFF; }
        }

        public List<string> ToStrings()
        {
            var list = new List<string>();
            var stdMask = (AccessMask_Standard)Standard;
            foreach (AccessMask_Standard __ in Enum.GetValues(typeof(AccessMask_Standard)))
            {
                if ((stdMask & __) != 0) list.Add(__.ToString());
            }

            switch (ObjectType)
            {
                case AccessMaskType.File:
                    var fileMask = (AccessMask_File)ObjectSpecific;
                    foreach (AccessMask_File __ in Enum.GetValues(typeof(AccessMask_File)))
                    {
                        if ((fileMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.Directory:
                    var dirMask = (AccessMask_Directory)ObjectSpecific;
                    foreach (AccessMask_Directory __ in Enum.GetValues(typeof(AccessMask_Directory)))
                    {
                        if ((dirMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.ActiveDirectory:
                    var adMask = (AccessMask_ActiveDirectory)ObjectSpecific;
                    foreach (AccessMask_ActiveDirectory __ in Enum.GetValues(typeof(AccessMask_ActiveDirectory)))
                    {
                        if ((adMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.Process:
                    var procMask = (AccessMask_Process)ObjectSpecific;
                    foreach (AccessMask_Process __ in Enum.GetValues(typeof(AccessMask_Process)))
                    {
                        if ((procMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.Service:
                    var svcMask = (AccessMask_Service)ObjectSpecific;
                    foreach (AccessMask_Service __ in Enum.GetValues(typeof(AccessMask_Service)))
                    {
                        if ((svcMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SCM:
                    var scmMask = (AccessMask_SCM)ObjectSpecific;
                    foreach (AccessMask_SCM __ in Enum.GetValues(typeof(AccessMask_SCM)))
                    {
                        if ((scmMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.Registry:
                    var regMask = (AccessMask_Registry)ObjectSpecific;
                    foreach (AccessMask_Registry __ in Enum.GetValues(typeof(AccessMask_Registry)))
                    {
                        if ((regMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcConfigInfo:
                    var srvsvcConfigInfoMask = (AccessMask_SrvsvcConfigInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcConfigInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcConfigInfo)))
                    {
                        if ((srvsvcConfigInfoMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcConnection:
                    var srvsvcConnMask = (AccessMask_SrvsvcConnection)ObjectSpecific;
                    foreach (AccessMask_SrvsvcConnection __ in Enum.GetValues(typeof(AccessMask_SrvsvcConnection)))
                    {
                        if ((srvsvcConnMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcFile:
                    var srvsvcFileMask = (AccessMask_SrvsvcFile)ObjectSpecific;
                    foreach (AccessMask_SrvsvcFile __ in Enum.GetValues(typeof(AccessMask_SrvsvcFile)))
                    {
                        if ((srvsvcFileMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcServerDiskEnum:
                    var srvsvcServerDiskEnumMask = (AccessMask_SrvsvcServerDiskEnum)ObjectSpecific;
                    foreach (AccessMask_SrvsvcServerDiskEnum __ in Enum.GetValues(typeof(AccessMask_SrvsvcServerDiskEnum)))
                    {
                        if ((srvsvcServerDiskEnumMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcSessionInfo:
                    var srvsvcSessionInfoMask = (AccessMask_SrvsvcSessionInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcSessionInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcSessionInfo)))
                    {
                        if ((srvsvcSessionInfoMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareAdminConnect:
                    var srvsvcShareAdminConnectMask = (AccessMask_SrvsvcShareAdminConnect)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareAdminConnect __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareAdminConnect)))
                    {
                        if ((srvsvcShareAdminConnectMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareAdminInfo:
                    var srvsvcShareAdminInfoMask = (AccessMask_SrvsvcShareAdminInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareAdminInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareAdminInfo)))
                    {
                        if ((srvsvcShareAdminInfoMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareChange:
                    var srvsvcShareChangeMask = (AccessMask_SrvsvcShareChange)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareChange __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareChange)))
                    {
                        if ((srvsvcShareChangeMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareConnect:
                    var srvsvcShareConnectMask = (AccessMask_SrvsvcShareConnect)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareConnect __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareConnect)))
                    {
                        if ((srvsvcShareConnectMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcShareFileInfo:
                    var srvsvcShareFileInfoMask = (AccessMask_SrvsvcShareFileInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcShareFileInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcShareFileInfo)))
                    {
                        if ((srvsvcShareFileInfoMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcSharePrintInfo:
                    var srvsvcSharePrintInfoMask = (AccessMask_SrvsvcSharePrintInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcSharePrintInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcSharePrintInfo)))
                    {
                        if ((srvsvcSharePrintInfoMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcStatisticsInfo:
                    var srvsvcStatisticsInfoMask = (AccessMask_SrvsvcStatisticsInfo)ObjectSpecific;
                    foreach (AccessMask_SrvsvcStatisticsInfo __ in Enum.GetValues(typeof(AccessMask_SrvsvcStatisticsInfo)))
                    {
                        if ((srvsvcStatisticsInfoMask & __) != 0) list.Add(__.ToString());
                    }
                    break;
                case AccessMaskType.SrvsvcTransportEnum:
                    var srvsvcTransportEnumMask = (AccessMask_SrvsvcTransportEnum)ObjectSpecific;
                    foreach (AccessMask_SrvsvcTransportEnum __ in Enum.GetValues(typeof(AccessMask_SrvsvcTransportEnum)))
                    {
                        if ((srvsvcTransportEnumMask & __) != 0) list.Add(__.ToString());
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

            if ((Full | 0xF00F01FF) != 0xF00F01FF)
            {
                return Full.ToString();
            }

            var sb = new StringBuilder();
            uint mask = 0x1;
            for (int i = 0; i < 31; ++i)
            {
                if ((Full & mask) != 0) sb.Append(SddlMapping.AccessMaskMapping.Inverse[mask]);
                mask <<= 1;
            }
            return sb.ToString();
        }
    }
}