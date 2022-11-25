using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinSdUtil.Lib.Model
{
    public class SDDL
    {
        public static readonly string RegexPatternSddl = @"(O:(?<Owner>..))?(G:(?<Group>..))?(D:(?<DACL>(?<DAclFlags>(P|AI|AR|NO_ACCESS_CONTROL){0,3})(?<DAclAces>\([^\(\)]*\))*))?(S:(?<SACL>(?<SAclFlags>(P|AI|AR|NO_ACCESS_CONTROL){0,3})(?<SAclAces>\([^\(\)]*\))*))?";
        public string Value
        {
            get
            {
                var sb = new StringBuilder();
                if (Owner.Length > 0) sb.Append($"O:{Owner}");
                if (Group.Length > 0) sb.Append($"G:{Group}");
                if (DAclFlags.Length > 0) sb.Append($"D:{DAclFlags}");
                sb.Append(DAclAces);
                if (SAclFlags.Length > 0) sb.Append($"S:{SAclFlags}");
                sb.Append(SAclAces);
                return sb.ToString();
            }
            set
            {
                var sddlMatch = Regex.Match(value, RegexPatternSddl);
                if (!sddlMatch.Success) { throw new ArgumentException("Invalid SDDL"); }
                if (sddlMatch.Groups.ContainsKey("Owner"))
                    Owner = sddlMatch.Groups["Owner"].Value;
                if (sddlMatch.Groups.ContainsKey("Group"))
                    Group = sddlMatch.Groups["Group"].Value;
                if (sddlMatch.Groups.ContainsKey("DAclFlags"))
                    DAclFlags = sddlMatch.Groups["DAclFlags"].Value;
                if (sddlMatch.Groups.ContainsKey("DAclAces"))
                    DAclAces = sddlMatch.Groups["DAclAces"].Value;
                if (sddlMatch.Groups.ContainsKey("SAclFlags"))
                    SAclFlags = sddlMatch.Groups["SAclFlags"].Value;
                if (sddlMatch.Groups.ContainsKey("SAclAces"))
                    SAclAces = sddlMatch.Groups["SAclAces"].Value;
            }
        }
        public string Owner { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public string DAclFlags { get; set; } = string.Empty;
        public string DAclAces { get; set; } = string.Empty;
        public string SAclFlags { get; set; } = string.Empty;
        public string SAclAces { get; set; } = string.Empty;

        public SDDL(byte[] BinarySd)
        {
            using ManagementClass Win32SdHelper = new ManagementClass("Win32_SecurityDescriptorHelper");

            var inparam = Win32SdHelper.GetMethodParameters("BinarySDToSDDL");
            inparam["BinarySD"] = BinarySd;
            var outparam = Win32SdHelper.InvokeMethod("BinarySDToSDDL", inparam, null);
            var rtValue = (uint)outparam["ReturnValue"];
            Value = (string)outparam["SDDL"];
            if (rtValue != 0) { throw new ArgumentException($"Given byte array is not valid Windows SD. Win32 error: {rtValue}"); }
        }

        public AccessControlList ToACL()
        {
            return new AccessControlList(this);
        }
    }
}
