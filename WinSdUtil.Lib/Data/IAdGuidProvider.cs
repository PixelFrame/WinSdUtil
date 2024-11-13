using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    public interface IAdGuidProvider
    {
        IEnumerable<AdObjectGuid> AdGuidData { get; }
    }
}
