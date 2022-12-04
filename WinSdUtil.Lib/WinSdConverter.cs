using WinSdUtil.Lib.Data;
using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib
{
    public class WinSdConverter
    {
        public WinSdConverter()
        {
            Trustee.DataProvider = new LibDbDataProvider();
        }
    }
}
