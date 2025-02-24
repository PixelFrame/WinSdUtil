using System.Text;
using System.Text.RegularExpressions;

namespace WinSdUtil.Model
{
    public class SDDL
    {
        internal static readonly string RegexPatternSddl = @"(O:(?<Owner>S-1-[^G]*|..))?(G:(?<Group>S-1-[^D]*|..))?(D:(?<DACL>(?<DAclFlags>(P|AI|AR|NO_ACCESS_CONTROL){0,3})(?<DAclAces>\(([^;]*;){5}([^\(\)]*);?(\([^\(\)]*\))?\))*))?(S:(?<SACL>(?<SAclFlags>(P|AI|AR|NO_ACCESS_CONTROL){0,3})(?<SAclAces>\(([^;]*;){5}([^\(\)]*);?(\([^\(\)]*\))?\))*))?";

        public string Value
        {
            get
            {
                var sb = new StringBuilder();
                if (Owner.Length > 0) sb.Append($"O:{Owner}");
                if (Group.Length > 0) sb.Append($"G:{Group}");
                if (DAclFlags.Length > 0 || DAclAces.Length > 0) sb.Append("D:");
                if (DAclFlags.Length > 0) sb.Append($"{DAclFlags}");
                foreach (var DAclAce in DAclAces) sb.Append(DAclAce);
                if (SAclFlags.Length > 0 || SAclAces.Length > 0) sb.Append("S:");
                if (SAclFlags.Length > 0) sb.Append($"{SAclFlags}");
                foreach (var SAclAce in SAclAces) sb.Append(SAclAce);
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
                    DAclAces = sddlMatch.Groups["DAclAces"].Captures.Select(c => c.Value).ToArray();
                if (sddlMatch.Groups.ContainsKey("SAclFlags"))
                    SAclFlags = sddlMatch.Groups["SAclFlags"].Value;
                if (sddlMatch.Groups.ContainsKey("SAclAces"))
                    SAclAces = sddlMatch.Groups["SAclAces"].Captures.Select(c => c.Value).ToArray();
            }
        }
        public string Owner { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public string DAclFlags { get; set; } = string.Empty;
        public string[] DAclAces { get; set; } = Array.Empty<string>();
        public string SAclFlags { get; set; } = string.Empty;
        public string[] SAclAces { get; set; } = Array.Empty<string>();

        public SDDL() { }

        public SDDL(string SddlString)
        {
            Value = SddlString;
        }

        public SDDL(AccessControlList acl)
        {
            if (acl.Owner != null) { Owner = acl.Owner.SddlName; }
            if (acl.Group != null) { Group = acl.Group.SddlName; }

            if ((acl.Flags & ControlFlags.DiscretionaryAclProtected) != 0)
                DAclFlags += SddlMapping.DAclFlagsMapping.Inverse[ControlFlags.DiscretionaryAclProtected];
            if ((acl.Flags & ControlFlags.DiscretionaryAclAutoInheritRequired) != 0)
                DAclFlags += SddlMapping.DAclFlagsMapping.Inverse[ControlFlags.DiscretionaryAclAutoInheritRequired];
            if ((acl.Flags & ControlFlags.DiscretionaryAclAutoInherited) != 0)
                DAclFlags += SddlMapping.DAclFlagsMapping.Inverse[ControlFlags.DiscretionaryAclAutoInherited];

            if ((acl.Flags & ControlFlags.SystemAclProtected) != 0)
                SAclFlags += SddlMapping.SAclFlagsMapping.Inverse[ControlFlags.SystemAclProtected];
            if ((acl.Flags & ControlFlags.SystemAclAutoInheritRequired) != 0)
                SAclFlags += SddlMapping.SAclFlagsMapping.Inverse[ControlFlags.SystemAclAutoInheritRequired];
            if ((acl.Flags & ControlFlags.SystemAclAutoInherited) != 0)
                SAclFlags += SddlMapping.SAclFlagsMapping.Inverse[ControlFlags.SystemAclAutoInherited];

            if (acl.DAclAces != null)
            {
                DAclAces = new string[acl.DAclAces.Length];
                for (int i = 0; i < acl.DAclAces.Length; ++i)
                {
                    DAclAces[i] = acl.DAclAces[i].ToSDDL();
                }
            }
            else if ((acl.Flags & ControlFlags.DiscretionaryAclPresent) != 0)
            {
                DAclFlags = SddlMapping.DAclFlagsMapping.Inverse[ControlFlags.None];
            }

            if (acl.SAclAces != null)
            {
                SAclAces = new string[acl.SAclAces.Length];
                for (int i = 0; i < acl.SAclAces.Length; ++i)
                {
                    SAclAces[i] = acl.SAclAces[i].ToSDDL();
                }
            }
            else if ((acl.Flags & ControlFlags.SystemAclPresent) != 0)
            {
                DAclFlags = SddlMapping.SAclFlagsMapping.Inverse[ControlFlags.None];
            }
        }

        public AccessControlList ToACL()
        {
            return new AccessControlList(this);
        }

        public BinarySecurityDescriptor ToBinarySd()
        {
            return this.ToACL().ToBinarySd();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
