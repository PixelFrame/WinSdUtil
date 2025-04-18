﻿using System.Text;
using WinSdUtil.Model.Binary;

namespace WinSdUtil.Model
{
    public class AccessControlList
    {
        public Trustee? Owner { get; set; }
        public Trustee? Group { get; set; }
        public ControlFlags Flags { get; set; } = 0;
        public AccessControlEntry[]? DAclAces { get; set; }
        public AccessControlEntry[]? SAclAces { get; set; }

        public AccessControlList(SDDL sddl)
        {
            if (sddl.Owner.Length == 0) Owner = null;
            else Owner = new Trustee(sddl.Owner, 0);

            if (sddl.Group.Length == 0) Group = null;
            else Group = new Trustee(sddl.Group, 0);

            if (sddl.DAclFlags.Contains("NO_ACCESS_CONTROL")) Flags |= ControlFlags.DiscretionaryAclPresent;
            else if (sddl.DAclAces.Length == 0) Flags &= ~ControlFlags.DiscretionaryAclPresent;
            else
            {
                Flags |= parseSddlControlFlags(sddl.DAclFlags, 0);
                Flags |= ControlFlags.DiscretionaryAclPresent;
            }

            if (sddl.SAclFlags.Contains("NO_ACCESS_CONTROL")) Flags |= ControlFlags.SystemAclPresent;
            else if (sddl.SAclAces.Length == 0) Flags &= ~ControlFlags.SystemAclPresent;
            else
            {
                Flags |= parseSddlControlFlags(sddl.SAclFlags, 1);
                Flags |= ControlFlags.SystemAclPresent;
            }

            Flags |= ControlFlags.SelfRelative;

            if ((Flags & ControlFlags.DiscretionaryAclPresent) != 0)
            {
                DAclAces = sddl.DAclAces.Select(a => new AccessControlEntry(a)).ToArray();
            }

            if ((Flags & ControlFlags.SystemAclPresent) != 0)
            {
                SAclAces = sddl.SAclAces.Select(a => new AccessControlEntry(a)).ToArray();
            }
        }

        public AccessControlList(BinarySecurityDescriptor binsd) : this(binsd.SD) { }

        internal AccessControlList(SecurityDescriptor sd)
        {
            if (sd.OwnerSid.Revision == 1) Owner = new Trustee(sd.OwnerSid.ToString(), 1);
            if (sd.GroupSid.Revision == 1) Group = new Trustee(sd.GroupSid.ToString(), 1);
            Flags = (ControlFlags)sd.Control;
            if ((Flags & ControlFlags.DiscretionaryAclPresent) != 0 && sd.DACL.Aces != null)
            {
                DAclAces = AceGen.FromBytes(sd.DACL.Aces, 0, sd.DACL.Aces.Length).Select(x => x.ToManagedAce()).ToArray();
            }
            if ((Flags & ControlFlags.SystemAclPresent) != 0 && sd.SACL.Aces != null)
            {
                SAclAces = AceGen.FromBytes(sd.SACL.Aces, 0, sd.SACL.Aces.Length).Select(x => x.ToManagedAce()).ToArray();
            }
        }

        private ControlFlags parseSddlControlFlags(string sddlControlFlags, int type)
        {
            ControlFlags result = 0;
            if (type == 0)
            {
                if (sddlControlFlags.Contains("P")) result |= ControlFlags.DiscretionaryAclProtected;
                if (sddlControlFlags.Contains("AR")) result |= ControlFlags.DiscretionaryAclAutoInheritRequired;
                if (sddlControlFlags.Contains("AI")) result |= ControlFlags.DiscretionaryAclAutoInherited;
            }
            else
            {
                if (sddlControlFlags.Contains("P")) result |= ControlFlags.SystemAclProtected;
                if (sddlControlFlags.Contains("AR")) result |= ControlFlags.SystemAclAutoInheritRequired;
                if (sddlControlFlags.Contains("AI")) result |= ControlFlags.SystemAclAutoInherited;
            }
            return result;
        }

        public SDDL ToSDDL()
        {
            return new SDDL(this);
        }

        public BinarySecurityDescriptor ToBinarySd()
        {
            return new BinarySecurityDescriptor(this.ToBinary());
        }

        internal SecurityDescriptor ToBinary()
        {
            var sd = new SecurityDescriptor();
            sd.Revision = 1;
            sd.Sbz1 = 0;
            sd.Control = (ushort)Flags;
            uint offset = 20;

            if (SAclAces != null && SAclAces.Length > 0)
            {
                sd.OffsetSacl = offset;
                var sacl = new ACL();
                sacl.AclRevision = 2;
                sacl.Sbz1 = 0;
                sacl.AceCount = (ushort)SAclAces.Length;
                var aceBytes = SAclAces
                    .Select(x => x.ToBinary())
                    .Aggregate((x, y) => x.Concat(y).ToArray());
                sacl.Aces = aceBytes;
                sacl.AclSize = (ushort)(8 + aceBytes.Length);
                sd.SACL = sacl;
                offset += sacl.AclSize;
            }
            else
            {
                sd.OffsetSacl = 0;
            }

            if (DAclAces != null && DAclAces.Length > 0)
            {
                sd.OffsetDacl = offset;
                var dacl = new ACL();
                dacl.AclRevision = 2;
                dacl.Sbz1 = 0;
                dacl.AceCount = (ushort)DAclAces.Length;
                var aceBytes = DAclAces
                    .Select(x => x.ToBinary())
                    .Aggregate((x, y) => x.Concat(y).ToArray());
                dacl.Aces = aceBytes;
                dacl.AclSize = (ushort)(8 + aceBytes.Length);
                sd.DACL = dacl;
                offset += dacl.AclSize;
            }
            else
            {
                sd.OffsetDacl = 0;
            }

            if (Owner != null)
            {
                sd.OffsetOwner = offset;
                sd.OwnerSid = Owner.ToBinarySid();
                offset += (uint)(8 + sd.OwnerSid.SubAuthorityCount * 4);
            }

            if (Group != null)
            {
                sd.OffsetGroup = offset;
                sd.GroupSid = Group.ToBinarySid();
                offset += (uint)(8 + sd.GroupSid.SubAuthorityCount * 4);
            }

            return sd;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Owner != null) sb.AppendLine($"Owner: {Owner.DisplayName} ({Owner.SddlName})");
            if (Group != null) sb.AppendLine($"Owner: {Group.DisplayName} ({Group.SddlName})");
            sb.AppendLine($"ControlFlags: {Flags} (0x{((ushort)Flags).ToString("X")})");
            if (DAclAces != null)
            {
                sb.AppendLine("DACL:");
                foreach (var ace in DAclAces)
                {
                    sb.AppendLine(ace.ToString("  "));
                }
            }
            if (SAclAces != null)
            {
                sb.AppendLine("SACL:");
                foreach (var ace in SAclAces)
                {
                    sb.AppendLine(ace.ToString("  "));
                }
            }
            return sb.ToString();
        }
    }
}