using WinSdUtil.Lib.Data;

namespace WinSdUtil.Lib.Model
{
    public class AdSchemaGuid
    {
        public static IAdSchemaGuidProvider? DataProvider;

        public string SchemaIdGuid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public AdSchemaGuid() { }
        public AdSchemaGuid(string guid)
        {
            SchemaIdGuid = guid;
            if (DataProvider == null)
            {
                Name = "NO AdSchemaGuid DATA SOURCE AVAILABLE";
                return;
            }
            var dataSource = DataProvider.AdSchemaGuidData;
            var dbResult = dataSource.SingleOrDefault(x => x.SchemaIdGuid.Equals(guid, StringComparison.OrdinalIgnoreCase));
            if (dbResult == null)
            {
                Name = "(Unknown GUID)";
                return;
            }
            Name = dbResult.Name;
        }
    }
}
