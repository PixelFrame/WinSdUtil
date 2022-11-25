using System.Security.AccessControl;

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
            else Owner = new Trustee(sddl.Owner);

            if (sddl.Group.Length == 0) Group = null;
            else Group = new Trustee(sddl.Group);

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

            if ((Flags & ControlFlags.DiscretionaryAclPresent) == ControlFlags.DiscretionaryAclPresent)
            {
                DAclAces = sddl.DAclAces.Select(a => new AccessControlEntry(a)).ToArray();
            }

            if ((Flags & ControlFlags.SystemAclPresent) == ControlFlags.SystemAclPresent)
            {
                SAclAces = sddl.SAclAces.Select(a => new AccessControlEntry(a)).ToArray();
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
            var sddl = new SDDL();
            throw new NotImplementedException();
        }
    }
}