using WinSdUtil.Data;

namespace WinSdUtil.Model
{
    public class AdObjectGuid
    {
        public static IAdGuidProvider? DataProvider;

        public string Guid { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        public AdObjectGuid() { }
        public AdObjectGuid(string guid)
        {
            Guid = guid;
            if (DataProvider == null)
            {
                DisplayName = "NO AdGuid DATA SOURCE AVAILABLE";
                return;
            }
            var dataSource = DataProvider.AdGuidData;
            var dbResult = dataSource.SingleOrDefault(x => x.Guid.Equals(guid, StringComparison.OrdinalIgnoreCase));
            if (dbResult == null)
            {
                DisplayName = "(Unknown GUID)";
                return;
            }
            DisplayName = dbResult.DisplayName;
            Type = dbResult.Type;
        }
    }
}
