using System.Management;
using WinSdUtil.Lib.Model;

var TestSddls = new List<string>()
{
    "O:AOG:SYD:(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(OA;;CCDC;bf967aba-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;bf967a9c-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;6da8a4ff-0e52-11d0-a286-00aa003049e2;;AO)(OA;;CCDC;bf967aa8-0de6-11d0-a285-00aa003049e2;;PO)(A;;RPLCRC;;;AU)S:(AU;SAFA;WDWOSDWPCCDCSW;;;WD)",
    "G:SYD:(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(OA;;CCDC;bf967aba-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;bf967a9c-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;6da8a4ff-0e52-11d0-a286-00aa003049e2;;AO)(OA;;CCDC;bf967aa8-0de6-11d0-a285-00aa003049e2;;PO)(A;;RPLCRC;;;AU)S:(AU;SAFA;WDWOSDWPCCDCSW;;;WD)",
    "D:(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)",
    "S:(AU;SAFA;WDWOSDWPCCDCSW;;;WD)"
};

foreach (var test in TestSddls)
{
    var sddl = new SDDL(test);
    var acl = sddl.ToACL();
    var binsd = acl.ToBinarySd();
    var acl2 = binsd.ToACL();
    var sddl2 = acl2.ToSDDL();
    Console.WriteLine($"Original SDDL  : {test}");
    Console.WriteLine($"Converted SDDL : {sddl2}");
    var wmibin = WmiGetBinarySd(test);
    Console.WriteLine($"WMI Binary SD  : {BitConverter.ToString(wmibin)}");
    Console.WriteLine($"Conv Binary SD : {BitConverter.ToString(binsd.Value)}");
}

static byte[] WmiGetBinarySd(string SDDL)
{
#pragma warning disable CA1416 // Validate platform compatibility

    using ManagementClass Win32SdHelper = new ManagementClass("Win32_SecurityDescriptorHelper");

    var inparam = Win32SdHelper.GetMethodParameters("SDDLToBinarySD");
    inparam["SDDL"] = SDDL;
    var outparam = Win32SdHelper.InvokeMethod("SDDLToBinarySD", inparam, null);
    var rtValue = (uint)outparam["ReturnValue"];
    var bin = (byte[])outparam["BinarySD"];
    if (rtValue == 0) return bin;
    else
    {
        throw new Exception($"WMI SD conversion failure: {rtValue:X}");
    }
#pragma warning restore CA1416 // Validate platform compatibility
}