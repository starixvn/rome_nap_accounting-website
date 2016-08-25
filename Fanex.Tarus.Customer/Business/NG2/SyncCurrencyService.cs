using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.CustomerSetting.Module.Core.Web;
using Fanex.Tarus.Customer.DataAccess.NG2;
using Fanex.Tarus.Customer.Entities.NG2;
using ONEbook.NG2.API;
using ONEbook.NG2.API.DTO;

namespace Fanex.Tarus.Customer.Business.NG2
{
    public class SyncCurrencyService : ISyncCurrencyService
    {
        private readonly ISyncCurrencyRepository _repository = new SyncCurrencyRepository();
        public CustomerSetting.Module.Core.Models.Result AddSyncCurrency(SyncCurrencyEntity entity)
        {
            // Do add currency in OW DB here. And then sync to 3rd.
            var service = new Services();
            var request = new Request();

            request.Auth.SiteId = "2000";
            request.Call.UserId = AppSession.AccId;
            request.Call.Currencies.Add(new KeyValuePair<int, decimal>(entity.CurrencyId, entity.BaseRate));

            var response = service.CreateCurrency(request);

            return new CustomerSetting.Module.Core.Models.Result
            {
                ErrorCode = response.Result.ResultDetail.ElementAt(0).ErrorCode,
                ErrorMessage = response.Result.ResultDetail.ElementAt(0).ErrorDescription
            };
        }

        public SyncCurrencyEntity GetCurrencyByName(string currencyCode)
        {
            return _repository.GetCurrencyByName(currencyCode);
        }

        public SyncCurrencyModel SyncCurrencies()
        {
            return _repository.SyncCurrencies();
        }

        public IList<CustomerSetting.Module.Core.Models.Result> UpdateCurrencies(SyncCurrencyModel model)
        {
            return _repository.UpdateCurrencies(model);
        }
    }
}