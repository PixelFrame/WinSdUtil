using System.Text.Json;
using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    public class JsonDataProvider : ITrusteeDataProvider, IAdGuidProvider
    {
        private Trustee[] trusteeData = Array.Empty<Trustee>();
        private AdObjectGuid[] adGuidData = Array.Empty<AdObjectGuid>();
        public IEnumerable<Trustee> TrusteeData { get => trusteeData; }
        public IEnumerable<AdObjectGuid> AdGuidData { get => adGuidData; }

        public JsonDataProvider(string trusteeJsonData, string adGuidJsonData)
        {
            trusteeData = JsonSerializer.Deserialize<Trustee[]>(trusteeJsonData) ?? [];
            adGuidData = JsonSerializer.Deserialize<AdObjectGuid[]>(adGuidJsonData) ?? [];
        }
    }
}
