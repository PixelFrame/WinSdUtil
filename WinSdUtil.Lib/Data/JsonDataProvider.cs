using System.Text.Json;
using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    internal class JsonDataProvider : ITrusteeDataProvider
    {
        private Trustee[] data = Array.Empty<Trustee>();
        public IEnumerable<Trustee> Data { get => data; }

        public JsonDataProvider(string jsonData)
        {
            data = JsonSerializer.Deserialize<Trustee[]>(jsonData) ?? Array.Empty<Trustee>();
        }
    }
}
