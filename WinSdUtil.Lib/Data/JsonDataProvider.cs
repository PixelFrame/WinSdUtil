using System.Text.Json;
using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    public class JsonDataProvider : ITrusteeDataProvider, IAdSchemaGuidProvider
    {
        private Trustee[] trusteeData = Array.Empty<Trustee>();
        private AdSchemaGuid[] adSchemaGuidData = Array.Empty<AdSchemaGuid>();
        public IEnumerable<Trustee> TrusteeData { get => trusteeData; }
        public IEnumerable<AdSchemaGuid> AdSchemaGuidData { get => adSchemaGuidData; }

        public JsonDataProvider(string trusteeJsonData, string adSchemaGuidJsonData)
        {
            trusteeData = JsonSerializer.Deserialize<Trustee[]>(trusteeJsonData) ?? Array.Empty<Trustee>();
            adSchemaGuidData = JsonSerializer.Deserialize<AdSchemaGuid[]>(adSchemaGuidJsonData) ?? Array.Empty<AdSchemaGuid>();
        }
    }
}
