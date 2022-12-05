using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    public interface ITrusteeDataProvider
    {
        IEnumerable<Trustee> TrusteeData { get; }
    }
}
