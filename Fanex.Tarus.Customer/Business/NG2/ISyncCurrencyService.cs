using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.Entities.NG2;

namespace Fanex.Tarus.Customer.Business.NG2
{
    public interface ISyncCurrencyService
    {
        SyncCurrencyModel SyncCurrencies();

        SyncCurrencyEntity GetCurrencyByName(string currencyCode);

        Result AddSyncCurrency(SyncCurrencyEntity entity);
    }
}