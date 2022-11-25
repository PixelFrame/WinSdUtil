﻿using WinSdUtil.Lib.Model;

var sddl = new SDDL("O:SYG:SYD:(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(A;;RPWPCCDCLCRCWOWDSDSW;;;SY)(OA;;CCDC;bf967aba-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;bf967a9c-0de6-11d0-a285-00aa003049e2;;AO)(OA;;CCDC;6da8a4ff-0e52-11d0-a286-00aa003049e2;;AO)(OA;;CCDC;bf967aa8-0de6-11d0-a285-00aa003049e2;;PO)(A;;RPLCRC;;;AU)S:(AU;SAFA;WDWOSDWPCCDCSW;;;WD)");
var binsd = sddl.ToBinarySd();
Console.WriteLine(BitConverter.ToString(binsd.Value));
var acl = sddl.ToACL();
var convertedSddl = acl.ToSDDL();
Console.WriteLine(convertedSddl.Value);