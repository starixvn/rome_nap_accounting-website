using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Tarus.Customer.Entities.NG2;
using ONEbook.NG2.API;
using ONEbook.NG2.API.DTO;

namespace Fanex.Tarus.Customer.Business.NG2
{
    public class SyncCurrencyService : ISyncCurrencyService
    {
        public CustomerSetting.Module.Core.Models.Result AddSyncCurrency(SyncCurrencyEntity entity)
        {
            // Do add currency in OW DB here. And then sync to 3rd.
            var service = new Services();
            var request = new Request();

            request.Auth = new Auth
            {
                SiteId = "2000"
            };
            request.Call.Currencies.Add(new KeyValuePair<int, decimal>(entity.CurrencyId, entity.BaseRate));

            var response = service.SyncExchangeRates(request);

            return new CustomerSetting.Module.Core.Models.Result
            {
                ErrorCode = response.Result.ResultDetail.ElementAt(0).ErrorCode,
                ErrorMessage = response.Result.ResultDetail.ElementAt(0).ErrorDescription
            };
        }

        public SyncCurrencyModel SyncCurrencies()
        {
            throw new NotImplementedException();
        }

        public IList<CustomerSetting.Module.Core.Models.Result> UpdateCurrencies(SyncCurrencyModel model)
        {
            throw new NotImplementedException();
        }
    }
}