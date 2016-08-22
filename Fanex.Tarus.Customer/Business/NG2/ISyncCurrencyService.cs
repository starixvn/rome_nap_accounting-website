using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.Entities.NG2;

namespace Fanex.Tarus.Customer.Business.NG2
{
    public interface ISyncCurrencyService
    {
        SyncCurrencyModel SyncCurrencies();

        IList<Result> UpdateCurrencies(SyncCurrencyModel model);

        Result AddSyncCurrency(SyncCurrencyEntity entity);
    }
}