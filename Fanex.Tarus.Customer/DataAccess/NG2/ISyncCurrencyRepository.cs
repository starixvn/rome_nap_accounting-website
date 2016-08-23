using System.Collections.Generic;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.Entities.NG2;

namespace Fanex.Tarus.Customer.DataAccess.NG2
{
    public interface ISyncCurrencyRepository
    {
        SyncCurrencyModel SyncCurrencies();

        IList<Result> UpdateCurrencies(SyncCurrencyModel model);

        Result AddSyncCurrency(SyncCurrencyEntity entity);

        SyncCurrencyEntity GetCurrencyByName(string currencyCode);
    }
}