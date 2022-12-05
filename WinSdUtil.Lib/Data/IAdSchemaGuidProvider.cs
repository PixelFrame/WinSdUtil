using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    public interface IAdSchemaGuidProvider
    {
        IEnumerable<AdSchemaGuid> AdSchemaGuidData { get; }
    }
}
