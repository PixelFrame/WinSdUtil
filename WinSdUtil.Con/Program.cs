﻿using System.Management;
using WinSdUtil.Lib;
using WinSdUtil.Lib.Data;
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

using var sdConv = new WinSdConverter();

Console.WriteLine("---------------------------------------- SDDL CONVERSION TEST ----------------------------------------");
foreach (var test in TestSddls)
{
    Console.WriteLine($"Test SDDL: {test}");
    var acl = sdConv.FromSddlToAcl(test);
    Console.WriteLine(acl.ToString());
    Console.WriteLine($"Baseline Binary: {BitConverter.ToString(WmiGetBinarySd(test))}");
    Console.WriteLine($"Converted Binary:{BitConverter.ToString(acl.ToBinarySd().Value)}");
}
Console.WriteLine("------------------------------------------------------------------------------------------------------");

Console.WriteLine("--------------------------------------- BINARY CONVERSION TEST ---------------------------------------");
foreach (var test in TestBytes)
{
    var acl = sdConv.FromBinaryToAcl(test);
    Console.WriteLine(acl.ToString());
    Console.WriteLine($"Baseline SDDL: {WmiGetSddl(test)}");
    Console.WriteLine($"Converted SDDL:{acl.ToSDDL().Value}");
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