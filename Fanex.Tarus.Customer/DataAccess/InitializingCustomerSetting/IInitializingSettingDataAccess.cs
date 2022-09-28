namespace Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using System.Collections.Generic;

    public interface IInitializingSettingDataAccess
    {
        Result ExecuteNonQuery(ICriteria criteria);

        IEnumerable<BaseModel> GetSites();

        IEnumerable<BaseModel> GetExchangeCurrency(bool isDisplayMode = true);

        IEnumerable<ProductDcsModel> GetProductDcsModel();

        ConfigModel GetConfigDetail(ICriteria criteria);

        InitializingCustomerSettingViewModel GetSetting(ICriteria criteria, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites);

        ValidateResultModel ValidateConfigSetting(ICriteria criteria);

        IEnumerable<InsertErrorModel> AddUpdateConfigSetting(ICriteria criteria);

        IEnumerable<BetSettingModel> GetBetSetting(int configId);
    }
}
