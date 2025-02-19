using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using WinSdUtil.Data;
using WinSdUtil.Helper;
using WinSdUtil.Model.Binary;

namespace WinSdUtil.Model
{
    public class Trustee
    {
        internal static string RegexPatternSid = @"S-(?<Revision>\d+)-(?<IdentifierAuthority>\d+)(-(?<SubAuthority>\d+))+";

        internal static ITrusteeDataProvider? DataProvider;

        public string Sid { get; set; } = "S-1-0-0";
        public string Name { get; set; } = "NULL";
        public string DisplayName { get; set; } = "NULL";
        public string SddlName { get; set; } = "S-1-0-0";
        public string Description { get; set; } = "NO TRUSTEE DATA SOURCE AVAILABLE";
        [JsonConverter(typeof(JsonBoolConverter))]
        public bool IsLocal { get; set; } = true;
        public string? DomainId { get; set; } = null;

        public Trustee() { }
        public Trustee(string SddlTrusteeOrSid, int Type)
        {
            if (DataProvider == null) return;
            var dataSource = DataProvider.TrusteeData;
            Trustee? dbResult = null;
            if (Type == 0) dbResult = dataSource.FirstOrDefault(t => t.SddlName == SddlTrusteeOrSid);
            else dbResult = dataSource.FirstOrDefault(t => t.Sid == SddlTrusteeOrSid);
            if (dbResult == null)
            {
                if (SddlTrusteeOrSid.StartsWith("S-1-5-5"))
                {
                    dbResult = dataSource.Single(t => t.SddlName == "S-1-5-5-x-y");
                    dbResult.Sid = SddlTrusteeOrSid;
                    dbResult.SddlName = SddlTrusteeOrSid;
                }
                else if (SddlTrusteeOrSid.StartsWith("S-1-5-21"))
                {
                    var domainSidMatch = Regex.Match(SddlTrusteeOrSid, @"S-1-5-21-(?<DomainId>.*-.*-.*)-(?<RID>.*)");
                    var RID = domainSidMatch.Groups["RID"].Value;
                    dbResult = dataSource.FirstOrDefault(t => t.Sid == $"S-1-5-21-<machine>-{RID}");
                    dbResult ??= dataSource.FirstOrDefault(t => t.Sid == $"S-1-5-21-<root domain>-{RID}");
                    dbResult ??= dataSource.FirstOrDefault(t => t.Sid == $"S-1-5-21-<domain>-{RID}");
                    dbResult ??= new Trustee()
                    {
                        Name = "DOMAIN_ACCOUNT",
                        DisplayName = "Unknown (Domain Account)",
                        Description = "An account from Active Directory or local computer.",
                        IsLocal = false,
                        DomainId = domainSidMatch.Groups["DomainId"].Value
                    };
                    dbResult.Sid = SddlTrusteeOrSid;
                    dbResult.SddlName = SddlTrusteeOrSid;
                }
                else if (SddlTrusteeOrSid.StartsWith("S-1-5-80"))
                {
                    dbResult = dataSource.FirstOrDefault(t => t.Sid == $"S-1-5-80-<SERVICE>");
                    dbResult ??= new Trustee()
                    {
                        Name = "NT_SERVICE",
                        DisplayName = "NT Service",
                        Description = "An NT Service account.",
                        IsLocal = false,
                        DomainId = null
                    };
                    dbResult.Sid = SddlTrusteeOrSid;
                    dbResult.SddlName = SddlTrusteeOrSid;
                }
                else if (SddlTrusteeOrSid.StartsWith("S-1-15-2"))
                {
                    dbResult = dataSource.FirstOrDefault(t => t.Sid == $"S-1-15-2-<AppNameHash>");
                    dbResult ??= new Trustee()
                    {
                        Name = "APP_PACKAGE",
                        DisplayName = "Application Package",
                        Description = "An application running in an app package context.",
                        IsLocal = true,
                        DomainId = null
                    };
                    dbResult.Sid = SddlTrusteeOrSid;
                    dbResult.SddlName = SddlTrusteeOrSid;
                }
                else if (SddlTrusteeOrSid.StartsWith("S-1-15-3-1024"))
                {
                    dbResult = dataSource.FirstOrDefault(t => t.Sid == $"S-1-15-3-1024-<AppCapNameHash>");
                    dbResult ??= new Trustee()
                    {
                        Name = "APP_CAPABILITY_APP_CAPABILITY",
                        DisplayName = "App Capability: app capability",
                        Description = "App Capability: app capability",
                        IsLocal = true,
                        DomainId = null
                    };
                    dbResult.Sid = SddlTrusteeOrSid;
                    dbResult.SddlName = SddlTrusteeOrSid;
                }
                else if (SddlTrusteeOrSid.StartsWith("S-1-15-3"))
                {
                    dbResult = dataSource.FirstOrDefault(t => t.Sid == $"S-1-15-3-<GUID>");
                    dbResult ??= new Trustee()
                    {
                        Name = "APP_CAPABILITY_DEVICE_CAPABILITY",
                        DisplayName = "App Capability: device capability",
                        Description = "App Capability: device capability.",
                        IsLocal = true,
                        DomainId = null
                    };
                    dbResult.Sid = SddlTrusteeOrSid;
                    dbResult.SddlName = SddlTrusteeOrSid;
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
            var sidstr = Sid;
            if (Sid.StartsWith("S-1-5-21"))
            {
                sidstr = sidstr.Replace("<domain>", "0-0-0");
                sidstr = sidstr.Replace("<root domain>", "0-0-0");
                sidstr = sidstr.Replace("<machine>", "0-0-0");
            }
            var sid = new SID();
            var regexMatch = Regex.Match(sidstr, RegexPatternSid);
            sid.Revision = byte.Parse(regexMatch.Groups["Revision"].Value);
            sid.IdentifierAuthority = new(ulong.Parse(regexMatch.Groups["IdentifierAuthority"].Value));
            sid.SubAuthorityCount = (byte)regexMatch.Groups["SubAuthority"].Captures.Count;
            sid.SubAuthority = regexMatch.Groups["SubAuthority"].Captures.Select(x => uint.Parse(x.Value)).ToArray();
            return sid;
        }

        public static IEnumerable<Trustee> EnumAllTrustees()
        {
            return DataProvider?.TrusteeData ?? [];
        }
    }
}
