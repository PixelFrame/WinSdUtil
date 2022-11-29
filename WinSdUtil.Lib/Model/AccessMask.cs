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

    public enum AccessMaskType
    {
        File,
        Directory,
        ActiveDirectory,
        Process,
        Service,
        ServiceControlManager,
        Registry
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
                case AccessMaskType.ServiceControlManager:
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