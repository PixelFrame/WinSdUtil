﻿using System.Security.AccessControl;

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

            if ((Flags & ControlFlags.DiscretionaryAclPresent) != 0)
            {
                DAclAces = sddl.DAclAces.Select(a => new AccessControlEntry(a)).ToArray();
            }

            if ((Flags & ControlFlags.SystemAclPresent) != 0)
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
            if (Owner != null) { sddl.Owner = Owner.SddlName; }
            if (Group != null) { sddl.Group = Group.SddlName; }

            if ((Flags & ControlFlags.DiscretionaryAclProtected) != 0)
                sddl.DAclFlags += SddlMapping.DAclFlagsMapping.Inverse[ControlFlags.DiscretionaryAclProtected];
            if ((Flags & ControlFlags.DiscretionaryAclAutoInheritRequired) != 0)
                sddl.DAclFlags += SddlMapping.DAclFlagsMapping.Inverse[ControlFlags.DiscretionaryAclAutoInheritRequired];
            if ((Flags & ControlFlags.DiscretionaryAclAutoInherited) != 0) 
                sddl.DAclFlags += SddlMapping.DAclFlagsMapping.Inverse[ControlFlags.DiscretionaryAclAutoInherited];

            if ((Flags & ControlFlags.SystemAclProtected) != 0)
                sddl.SAclFlags += SddlMapping.SAclFlagsMapping.Inverse[ControlFlags.SystemAclProtected];
            if ((Flags & ControlFlags.SystemAclAutoInheritRequired) != 0)
                sddl.SAclFlags += SddlMapping.SAclFlagsMapping.Inverse[ControlFlags.SystemAclAutoInheritRequired];
            if ((Flags & ControlFlags.SystemAclAutoInherited) != 0)
                sddl.SAclFlags += SddlMapping.SAclFlagsMapping.Inverse[ControlFlags.SystemAclAutoInherited];

            if (DAclAces != null)
            {
                sddl.DAclAces = new string[DAclAces.Length];
                for (int i = 0; i < DAclAces.Length; ++i)
                {
                    sddl.DAclAces[i] = DAclAces[i].ToSDDL();
                }
            }

            if (SAclAces != null)
            {
                sddl.SAclAces = new string[SAclAces.Length];
                for (int i = 0; i < SAclAces.Length; ++i)
                {
                    sddl.SAclAces[i] = SAclAces[i].ToSDDL();
                }
            }

            return sddl;
        }
    }
}