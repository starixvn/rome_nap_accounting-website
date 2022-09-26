namespace Fanex.Tarus.Customer.Business.InitializingCustomerSetting
{
    using Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting;

    public interface IInitializingCreditSettingService : IBaseCustomerSettingService
    {
    }
    public class InitializingCreditSettingService : BaseCustomerSettingService, IInitializingCreditSettingService
    {
        public InitializingCreditSettingService() : base(new InitializingCreditSettingDataAccess())
        {
        }

        public InitializingCreditSettingService(InitializingCreditSettingDataAccess dataAccess) : base(dataAccess)
        {
        }
    }
}
