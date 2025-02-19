using WinSdUtil.Model;

namespace WinSdUtil.Data
{
    public interface ITrusteeDataProvider
    {
        IEnumerable<Trustee> TrusteeData { get; }
    }
}
