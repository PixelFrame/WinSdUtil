using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    public class LibDbDataProvider : ITrusteeDataProvider, IAdGuidProvider, IDisposable
    {
        private readonly LibDbContext dbContext = new();
        public IEnumerable<Trustee> TrusteeData { get => dbContext.Trustees; }
        public IEnumerable<AdObjectGuid> AdGuidData { get => dbContext.AdGuids; }

        public LibDbDataProvider() { }
        public LibDbDataProvider(string ConnString)
        {
            dbContext = new(ConnString);
        }

        public void Dispose()
        {
            ((IDisposable)dbContext).Dispose();
        }
    }
}
