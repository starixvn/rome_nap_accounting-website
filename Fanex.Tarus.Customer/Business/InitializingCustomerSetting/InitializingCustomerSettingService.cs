namespace Fanex.Tarus.Customer.Business.InitializingCustomerSetting
{
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using DNAide.Web;
    using System.Text;

    public interface IInitializingCustomerSettingService
    {
        InitializingCustomerSettingViewModel GetSetting();

        Result DeleteConfigSetting(int configId, int userId, string ip);

        Result AbortConfigSetting(int configId, int userId, string ip);

        IEnumerable<ProductModel> GetProductSportBettype();

        string GetConfigDetail(int configId);
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

        public string GetConfigDetail(int configId)
        {
            var criteria = new GetConfigDetailCriteria(configId);
            var config = dataAccess.GetConfigDetail(criteria);
            var configSettingHtml = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Templates/InitializingCustomerSetting/ConfigDetail.htm");
            var settingsHtml = new StringBuilder();
            foreach (var setting in config.Settings)
            {
                settingsHtml.Append(BuildSettingTempalte(setting));
            }

            TemplateHandler.Assign(ref configSettingHtml, "{settings}", settingsHtml.ToString());

            return configSettingHtml;
        }

        public IEnumerable<ProductModel> GetProductSportBettype()
        {
            var criteria = new DcsProductMatrixCriteria();

            return dataAccess.GetProductSportBettype(criteria);
        }

        public InitializingCustomerSettingViewModel GetSetting()
        {
            var currencies = dataAccess.GetExchangeCurrency();
            var sites = dataAccess.GetSites();
            return dataAccess.GetSetting(new GetSettingCriteria(), currencies, sites);
        }

        private string BuildSettingTempalte(SettingInforModel setting)
        {
            return "";
        }
    }
}
