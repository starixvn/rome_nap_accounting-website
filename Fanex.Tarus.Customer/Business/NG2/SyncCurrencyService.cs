using System.Collections.Generic;
using System.Linq;
using Fanex.CustomerSetting.Module.Core.Web;
using Fanex.Tarus.Customer.DataAccess.NG2;
using Fanex.Tarus.Customer.Entities.NG2;
using ONEbook.NG2.API;
using ONEbook.NG2.API.DTO;
using ONEbook.NG2.API.DTO.CreateCurrency;

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

            request.Auth.SiteId = 2000;
            request.UserId = AppSession.CurrentSession.UserId;
            request.Call.CurrencyArray.Add(new CurrencyInfo() { CurrencyId = entity.CurrencyId, ExchangeRate = entity.BaseRate });

            var response = service.CreateCurrency(request);

            return new CustomerSetting.Module.Core.Models.Result
            {
                ErrorCode = response.Result.ErrorCode,
                ErrorMessage = response.Result.ErrorDescription
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
    }
}