using WinSdUtil.Lib.Data;
using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib
{
    public class WinSdConverter : IDisposable
    {
        private LibDbDataProvider? libDbDataProvider;
        private JsonDataProvider? jsonDataProvider;

        public WinSdConverter()
        {
            initLibDbDataProvider();
        }

        public WinSdConverter(bool UseJsonDataProvider, string ConnString)
        {
            if (UseJsonDataProvider)
            {
                var trusteeJsonData = File.ReadAllText("Data/WinSdTrustee.json");
                var adSchemaGuidJsonData = File.ReadAllText("Data/WinSdAdSchemaGuid.json");
                jsonDataProvider = new JsonDataProvider(trusteeJsonData, adSchemaGuidJsonData);
                Trustee.DataProvider = jsonDataProvider;
                AdObjectGuid.DataProvider = jsonDataProvider;
            }
            else
            {
                initLibDbDataProvider(ConnString);
            }
        }

        public WinSdConverter(bool UseJsonDataProvider, string TrusteeJsonData, string AdSchemaGuidJsonData)
        {
            if (UseJsonDataProvider)
            {
                jsonDataProvider = new JsonDataProvider(TrusteeJsonData, AdSchemaGuidJsonData);
                Trustee.DataProvider = jsonDataProvider;
                AdObjectGuid.DataProvider = jsonDataProvider;
            }
            else
            {
                initLibDbDataProvider();
            }
        }

        private void initLibDbDataProvider()
        {
            if (libDbDataProvider == null) { libDbDataProvider = new LibDbDataProvider(); }
            Trustee.DataProvider = libDbDataProvider;
            AdObjectGuid.DataProvider = libDbDataProvider;
        }

        private void initLibDbDataProvider(string ConnString)
        {
            if (libDbDataProvider == null) { libDbDataProvider = new LibDbDataProvider(ConnString); }
            Trustee.DataProvider = libDbDataProvider;
            AdObjectGuid.DataProvider = libDbDataProvider;
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

        public void Dispose()
        {
            if (libDbDataProvider != null) ((IDisposable)libDbDataProvider).Dispose();
        }
    }
}
