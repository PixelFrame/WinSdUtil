using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinSdUtil.Lib.Data;

namespace WinSdUtil.Lib.Model
{
    public class Trustee
    {
        public string Sid { get; private set; } = "S-1-0-0";
        public string Name { get; private set; } = "NULL";
        public string DisplayName { get; private set; } = "NULL";
        public string SddlName { get; private set; } = "S-1-0-0";
        public string Description { get; private set; } = "";
        public bool   IsLocal { get; private set; } = true;
        public string? DomainId { get; private set; } = null;

        public Trustee() { }
        public Trustee(string SddlTrustee)
        {
            using (var context = new LibDbContext())
            {
                var dbResult = context.Trustees.Single(t => t.SddlName == SddlTrustee);
                this.Copy(dbResult);
            }
        }

        private void Copy(Trustee source)
        {
            Sid = source.Sid;
            Name = source.Name;
            DisplayName = source.DisplayName;
            SddlName = source.SddlName;
            Description = source.Description;
            IsLocal = source.IsLocal;
            DomainId = source.DomainId;
        }
    }
}
