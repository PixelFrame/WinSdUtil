using System.Management;
using WinSdUtil.Lib.Model;

var TestSddls = new List<string>()
{
    "O:AOG:SYD:(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(OA;;CCDC;bf967aba-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;bf967a9c-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;6da8a4ff-0e52-11d0-a286-00aa003049e2;;AO)(OA;;CCDC;bf967aa8-0de6-11d0-a285-00aa003049e2;;PO)(A;;RPLCRC;;;AU)S:(AU;SAFA;WDWOSDWPCCDCSW;;;WD)",
    "O:AOG:SYD:PARAI(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)S:PARAI(AU;SAFA;WDWOSDWPCCDCSW;;;WD)",
    "G:SYD:(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(OA;;CCDC;bf967aba-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;bf967a9c-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;6da8a4ff-0e52-11d0-a286-00aa003049e2;;AO)(OA;;CCDC;bf967aa8-0de6-11d0-a285-00aa003049e2;;PO)(A;;RPLCRC;;;AU)S:(AU;SAFA;WDWOSDWPCCDCSW;;;WD)",
    "D:(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)",
    "S:(AU;SAFA;WDWOSDWPCCDCSW;;;WD)",
    "G:SYD:NO_ACCESS_CONTROL",
};

var TestBytes = new List<byte[]>()
{
    new byte[] { 0x01, 0x00, 0x04, 0x80, 0xA0, 0x00, 0x00, 0x00, 0xAC, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x02, 0x00, 0x8C, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18, 0x00, 0x17, 0x00, 0x0F, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x20, 0x00, 0x00, 0x00, 0x20, 0x02, 0x00, 0x00, 0x00, 0x00, 0x18, 0x00, 0x17, 0x00, 0x0F, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x20, 0x00, 0x00, 0x00, 0x25, 0x02, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x17, 0x00, 0x0F, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x12, 0x00, 0x00, 0x00, 0x00, 0x00, 0x18, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x20, 0x00, 0x00, 0x00, 0x23, 0x02, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x07, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x12, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x12, 0x00, 0x00, 0x00 },
};

Console.WriteLine("---------------------------------------- SDDL CONVERSION TEST ----------------------------------------");
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
    Console.WriteLine();
}
Console.WriteLine("------------------------------------------------------------------------------------------------------");

Console.WriteLine("--------------------------------------- BINARY CONVERSION TEST ---------------------------------------");
foreach (var test in TestBytes)
{
    var binsd = new BinarySecurityDescriptor(test);
    var sddl = binsd.ToSDDL();
    var wmisddl = WmiGetSddl(test);
    Console.WriteLine($"Converted SDDL    : {sddl}");
    Console.WriteLine($"WMI Baseline SDDL : {wmisddl}");
    Console.WriteLine();
}
Console.WriteLine("------------------------------------------------------------------------------------------------------");

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

static string WmiGetSddl(byte[] BinarySd)
{
#pragma warning disable CA1416 // Validate platform compatibility

    using ManagementClass Win32SdHelper = new ManagementClass("Win32_SecurityDescriptorHelper");

    var inparam = Win32SdHelper.GetMethodParameters("BinarySDToSDDL");
    inparam["BinarySD"] = BinarySd;
    var outparam = Win32SdHelper.InvokeMethod("BinarySDToSDDL", inparam, null);
    var rtValue = (uint)outparam["ReturnValue"];
    var sddl = (string)outparam["SDDL"];
    if (rtValue == 0) return sddl;
    else
    {
        throw new Exception($"WMI SD conversion failure: {rtValue:X}");
    }
#pragma warning restore CA1416 // Validate platform compatibility
}