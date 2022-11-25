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
    public class AccessControlList
    {
        public Trustee? Owner { get; set; }
        public Trustee? Group { get; set; }
        public ControlFlags DAclFlags { get; set; } = 0;
        public AccessControlEntry[]? DAclAces { get; set; }
        public ControlFlags SAclFlags { get; set; } = 0;
        public AccessControlEntry[]? SAclAces { get; set; }

        public AccessControlList(SDDL sddl)
        {
            if(sddl.Owner.Length == 0) Owner = null;
            else Owner = new Trustee(sddl.Owner);

            if (sddl.Group.Length == 0) Group = null;
            else Group = new Trustee(sddl.Group);
        }
    }
}