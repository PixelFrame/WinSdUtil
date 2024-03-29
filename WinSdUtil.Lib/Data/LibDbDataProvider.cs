﻿using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    public class LibDbDataProvider : ITrusteeDataProvider, IAdSchemaGuidProvider, IDisposable
    {
        private readonly LibDbContext dbContext = new();
        public IEnumerable<Trustee> TrusteeData { get => dbContext.Trustees; }
        public IEnumerable<AdSchemaGuid> AdSchemaGuidData { get => dbContext.AdSchemaGuids; }

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
