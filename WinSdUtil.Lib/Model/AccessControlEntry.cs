using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinSdUtil.Lib.Helper;

namespace WinSdUtil.Lib.Model
{
    public class AccessControlEntry
    {
        public static readonly string RegexPatternAce = @"\((?<AceType>[A-Z]*);(?<AceFlags>[A-Z]*);(?<Rights>[0-9A-Z]*);(?<ObjectGuid>[A-Za-z\d\-]*);(?<InheritObjectGuid>[A-Za-z\d\-]*);(?<AccountSid>[A-Za-z\d\-]*);?(?<ResourceAttribute>\([^\(\)]\))?\)";

        public AceType Type { get; set; } = 0;
        public AceFlags Flags { get; set; } = 0; 
        public AccessMask AccessMask { get; set; } = new();
        public Guid ObjectGuid { get; set; }
        public Guid InheritObjectGuid { get; set; }
        public Trustee Trustee { get; set; } = new();

        public AccessControlEntry(string SddlAce)
        {
            var regexMatchAce = Regex.Match(SddlAce, RegexPatternAce);
            if (!regexMatchAce.Success)
            {
                throw new ArgumentException("Invalid ACE SDDL");
            }

            var sddlType = regexMatchAce.Groups["AceType"].Value;
            if (!SddlMapping.AceTypeMapping.TryGetValue(sddlType, out AceType type)) { throw new ArgumentException($"Invalid ACE Type: {sddlType}"); }
            Type = type;

            Flags = 0;
            var sddlFlags = regexMatchAce.Groups["AceFlags"].Value.SplitInParts(2);
            foreach(var sddlFlag in sddlFlags)
            {
                if(!SddlMapping.AceFlagsMapping.TryGetValue(sddlFlag, out AceFlags flag)) { throw new ArgumentException($"Invalid ACE Flag: {sddlFlag}"); }
                Flags |= flag;
            }

            AccessMask.Full = 0;
            var sddlRights = regexMatchAce.Groups["Rights"].Value;
            if (Regex.IsMatch(sddlRights, @"\d*"))
            {
                if (!uint.TryParse(sddlRights, out uint accessMask))
                { throw new ArgumentException($"Invalid ACE Right: {sddlRights}"); }
                AccessMask.Full = accessMask;
            }
            else
            {
                var sddlRightsList = sddlRights.SplitInParts(2);
                foreach (var sddlRight in sddlRightsList)
                {
                    if (!SddlMapping.AccessMaskMapping.TryGetValue(sddlRight, out uint accessBit)) { throw new ArgumentException($"Invalid ACE Right: {sddlRight}"); }
                    AccessMask.Full |= accessBit;
                }
            }
            
            Trustee = new(regexMatchAce.Groups["AccountSid"].Value);

            if(regexMatchAce.Groups["ObjectGuid"].Value != string.Empty) ObjectGuid = new Guid(regexMatchAce.Groups["ObjectGuid"].Value);
            if (regexMatchAce.Groups["InheritObjectGuid"].Value != string.Empty) InheritObjectGuid = new Guid(regexMatchAce.Groups["InheritObjectGuid"].Value);
        }
    }
}
