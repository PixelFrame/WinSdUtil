using WinSdUtil.Data;
using WinSdUtil.Model;

namespace WinSdUtil
{
    public class WinSdConverter
    {
        private JsonDataProvider? jsonDataProvider;

        public WinSdConverter()
        {
            var trusteeData = File.ReadAllText("./Data/WinSdTrustee.json");
            var svcTrusteeData = File.ReadAllText("./Data/ServiceTrustee.json");
            var adGuidData = File.ReadAllText("./Data/WinSdAdGuid.json");
            jsonDataProvider = new([trusteeData, svcTrusteeData], adGuidData);
            Trustee.DataProvider = jsonDataProvider;
            AdObjectGuid.DataProvider = jsonDataProvider;
        }

        public WinSdConverter(IEnumerable<string> TrusteeJsonDataSources, string AdSchemaGuidJsonData)
        {
            jsonDataProvider = new JsonDataProvider(TrusteeJsonDataSources, AdSchemaGuidJsonData);
            Trustee.DataProvider = jsonDataProvider;
            AdObjectGuid.DataProvider = jsonDataProvider;
        }

        public AccessControlList FromSddlToAcl(string SDDL)
        {
            return new SDDL(SDDL).ToACL();
        }

        public byte[] FromSddlToBinary(string SDDL)
        {
            return new SDDL(SDDL).ToBinarySd().Value;
        }

        public AccessControlList FromBinaryToAcl(byte[] BinarySD)
        {
            return new BinarySecurityDescriptor(BinarySD).ToACL();
        }

        public string FromBinaryToSddl(byte[] BinarySD)
        {
            return new BinarySecurityDescriptor(BinarySD).ToSDDL().Value;
        }
    }
}
