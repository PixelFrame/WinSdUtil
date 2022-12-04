using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    internal interface ITrusteeDataProvider
    {
        IEnumerable<Trustee> Data { get; }
    }
}
