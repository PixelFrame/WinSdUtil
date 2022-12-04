using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    internal class LibDbDataProvider : ITrusteeDataProvider
    {
        private readonly LibDbContext dbContext = new();
        public IEnumerable<Trustee> Data { get => dbContext.Trustees; }
    }
}
