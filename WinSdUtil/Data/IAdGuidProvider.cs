using WinSdUtil.Model;

namespace WinSdUtil.Data
{
    public interface IAdGuidProvider
    {
        IEnumerable<AdObjectGuid> AdGuidData { get; }
    }
}
