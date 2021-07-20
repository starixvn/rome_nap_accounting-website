namespace Fanex.Tarus.Customer.Business.InitializingCustomerSetting
{
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;

    public interface IInitializingCustomerSettingService
    {
        InitializingCustomerSettingViewModel GetSetting();

        Result DeleteConfigSetting(int configId, int userId, string ip);

        Result AbortConfigSetting(int configId, int userId, string ip);
    }
    public class InitializingCustomerSettingService : IInitializingCustomerSettingService
    {
        private readonly IInitializingCustomerSettingDataAccess dataAccess;

        public InitializingCustomerSettingService()
        {
            dataAccess = new InitializingCustomerSettingDataAccess();
        }

        public Result AbortConfigSetting(int configId, int userId, string ip)
        {
            var criteria = new UpdateConfigStatusCriteria
            {
                ConfigId = configId,
                UserId = userId,
                Status = 3,
                IP = ip
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public Result DeleteConfigSetting(int configId, int userId, string ip)
        {
            var criteria = new DeleteConfigCriteria
            {
                ConfigId = configId,
                UserId = userId,
                IP = ip
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public InitializingCustomerSettingViewModel GetSetting()
        {
            var currencies = dataAccess.GetExchangeCurrency();
            var sites = dataAccess.GetSites();
            return dataAccess.GetSetting(new GetSettingCriteria(), currencies, sites);
        }
    }
}
