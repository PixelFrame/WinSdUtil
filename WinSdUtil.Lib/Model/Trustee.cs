using System.Text.RegularExpressions;
using WinSdUtil.Lib.Data;
using WinSdUtil.Lib.Model.Binary;

namespace WinSdUtil.Lib.Model
{
    public class Trustee
    {
        internal static string RegexPatternSid = @"S-(?<Revision>\d+)-(?<IdentifierAuthority>\d+)(-(?<SubAuthority>\d+))+";
        public string Sid { get; private set; } = "S-1-0-0";
        public string Name { get; private set; } = "NULL";
        public string DisplayName { get; private set; } = "NULL";
        public string SddlName { get; private set; } = "S-1-0-0";
        public string Description { get; private set; } = "";
        public bool IsLocal { get; private set; } = true;
        public string? DomainId { get; private set; } = null;

        public Trustee() { }
        public Trustee(string SddlTrusteeOrSid, int Type)
        {
            using var context = new LibDbContext();
            Trustee? dbResult = null;
            if (Type==0) dbResult = context.Trustees.FirstOrDefault(t => t.SddlName == SddlTrusteeOrSid);
            else dbResult = context.Trustees.FirstOrDefault(t => t.Sid == SddlTrusteeOrSid);
            if (dbResult == null)
            {
                if (SddlTrusteeOrSid.StartsWith("S-1-5-5"))
                {
                    dbResult = context.Trustees.Single(t => t.SddlName == "S-1-5-5-x-y");
                    dbResult.Sid = SddlTrusteeOrSid;
                    dbResult.SddlName = SddlTrusteeOrSid;
                }
                else if (SddlTrusteeOrSid.StartsWith("S-1-5-21"))
                {
                    var domainSidMatch = Regex.Match(SddlTrusteeOrSid, @"S-1-5-21-(?<DomainId>.*-.*-.*)-(?<RID>.*)");
                    dbResult = new Trustee()
                    {
                        Sid = SddlTrusteeOrSid,
                        Name = "DOMAIN_ACCOUNT",
                        DisplayName = "Unknown (Domain Account)",
                        SddlName = SddlTrusteeOrSid,
                        Description = "An account from Active Directory or local computer.",
                        IsLocal = false,
                        DomainId = domainSidMatch.Groups["DomainId"].Value
                    };
                }
                else
                {
                    dbResult = new Trustee()
                    {
                        Sid = SddlTrusteeOrSid,
                        Name = "UNKNOWN",
                        DisplayName = "(Unknwon Account)",
                        SddlName = SddlTrusteeOrSid,
                        Description = "Unknown Account",
                        IsLocal = false,
                        DomainId = null
                    };
                }
            }
            else if (!dbResult.IsLocal && SddlTrusteeOrSid.Length > 2)
            {
                dbResult.Sid = SddlTrusteeOrSid;
                dbResult.SddlName = SddlTrusteeOrSid;
            }
            this.Copy(dbResult!);
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

        internal SID ToBinarySid()
        {
            var sid = new SID();
            var regexMatch = Regex.Match(Sid, RegexPatternSid);
            sid.Revision = byte.Parse(regexMatch.Groups["Revision"].Value);
            sid.IdentifierAuthority = new(ulong.Parse(regexMatch.Groups["IdentifierAuthority"].Value));
            sid.SubAuthorityCount = (byte)regexMatch.Groups["SubAuthority"].Captures.Count;
            sid.SubAuthority = regexMatch.Groups["SubAuthority"].Captures.Select(x => uint.Parse(x.Value)).ToArray();
            return sid;
        }
    }
}
