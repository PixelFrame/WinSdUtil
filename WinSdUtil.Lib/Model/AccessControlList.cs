using WinSdUtil.Lib.Model.Binary;

namespace WinSdUtil.Lib.Model
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

            if (sddl.DAclFlags.Contains("NO_ACCESS_CONTROL")) Flags ^= ControlFlags.DiscretionaryAclPresent;
            else
            {
                Flags |= parseSddlControlFlags(sddl.DAclFlags, 0);
                Flags |= ControlFlags.DiscretionaryAclPresent;
            }

            if (sddl.SAclFlags.Contains("NO_ACCESS_CONTROL")) Flags ^= ControlFlags.SystemAclPresent;
            else
            {
                Flags |= parseSddlControlFlags(sddl.SAclFlags, 1);
                Flags |= ControlFlags.SystemAclPresent;
            }

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
            Owner = new Trustee(sd.OwnerSid.ToString(), 1);
            Group = new Trustee(sd.GroupSid.ToString(), 1);
            Flags = (ControlFlags)sd.Control;
            if ((Flags & ControlFlags.DiscretionaryAclPresent) != 0)
            {
                DAclAces = AceGen.FromBytes(sd.DACL.Aces, 0, sd.DACL.Aces.Length).Select(x => x.ToManagedAce()).ToArray();
            }
            if ((Flags & ControlFlags.SystemAclPresent) != 0)
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
                if (sddlControlFlags.Contains("P")) result |= ControlFlags.DiscretionaryAclProtected;
                if (sddlControlFlags.Contains("AR")) result |= ControlFlags.DiscretionaryAclAutoInheritRequired;
                if (sddlControlFlags.Contains("AI")) result |= ControlFlags.DiscretionaryAclAutoInherited;
            }
            return result;
        }

        public SDDL ToSDDL()
        {
            return new SDDL(this);
        }

        public BinarySecurityDescriptor ToBinarySd()
        {
            return new BinarySecurityDescriptor(this.ToBinary().GetBytes());
        }

        internal SecurityDescriptor ToBinary()
        {
            var sd = new SecurityDescriptor();
            sd.Revision = 1;
            sd.Sbz1 = 0;
            sd.Control = (ushort)Flags;
            uint offset = 20;

            if (SAclAces != null)
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

            if (DAclAces != null)
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
    }
}