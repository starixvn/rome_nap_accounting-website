using Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting;

namespace Fanex.Tarus.Customer.Business.InitializingCustomerSetting
{
    public interface IInitializingDepositSettingService : IBaseCustomerSettingService
    {

    }
    public class InitializingDepositSettingService : BaseCustomerSettingService, IInitializingDepositSettingService
    {
        public InitializingDepositSettingService() : base(new InittializingDepositSettingDataAccess())
        {
        }

        public InitializingDepositSettingService(InittializingDepositSettingDataAccess dataAccess) : base(dataAccess)
        {
        }
    }
}
