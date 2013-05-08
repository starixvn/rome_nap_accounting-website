using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.PT.Casino;
using Sunplus.Accounting.Entities.Casino;

namespace Sunplus.Accounting.WcfServiceLibrary.PT.Casino
{
    public class CasinoService:ICasinoService
    {
        public void AutoInsertCasinoAccount(CSCustomerDetail customerInfo, int userId, string userName, string ip)
        {
            var csService = CommonBootStrapper.Locator.GetInstance<CasinoManager>();
            csService.AutoInsertCasinoAccount(customerInfo, userId, userName, ip);
        }
    }
}
