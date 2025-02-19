using System.Text.Json;
using WinSdUtil.Model;

namespace WinSdUtil.Data
{
    public class JsonDataProvider : ITrusteeDataProvider, IAdGuidProvider
    {
        private List<Trustee> trusteeData = [];
        private List<AdObjectGuid> adGuidData = [];
        public IEnumerable<Trustee> TrusteeData { get => trusteeData; }
        public IEnumerable<AdObjectGuid> AdGuidData { get => adGuidData; }

        public JsonDataProvider(IEnumerable<string> trusteeJsonDataSources, string adGuidJsonData)
        {
            foreach (var trusteeJsonData in trusteeJsonDataSources)
            {
                trusteeData.AddRange(JsonSerializer.Deserialize<Trustee[]>(trusteeJsonData) ?? []);
            }
            adGuidData = JsonSerializer.Deserialize<List<AdObjectGuid>>(adGuidJsonData) ?? [];
        }
    }
}
