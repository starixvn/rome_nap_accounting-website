using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.CustomerSetting.Module.Core.Web;
using Fanex.Tarus.Customer.DataAccess.NG2;
using Fanex.Tarus.Customer.Entities.NG2;
using ONEbook.NG2.API;

namespace Fanex.Tarus.Customer.Business.NG2
{
    public class SyncCurrencyService : ISyncCurrencyService
    {
        private readonly ISyncCurrencyRepository _repository = new SyncCurrencyRepository();

        public CustomerSetting.Module.Core.Models.Result AddSyncCurrency(SyncCurrencyEntity entity)
        {
            var service = new Services();
            var req = new ONEbook.NG2.API.DTO.SyncExchangeRate.Request();
            req.Auth.SiteId = 2000;
            req.Call.CurrencyArray.Add(new ONEbook.NG2.API.DTO.SyncExchangeRate.CurrencyInfo { CurrencyId = entity.CurrencyId, ExchangeRate = (decimal)entity.BaseRate });
            req.UserId = AppSession.CurrentSession.UserId;

            var res = service.SyncExchangeRates(req);

            return new Result(res.Result.ErrorCode, res.Result.ErrorCode != 0 ? "Update base rate failed. Please try it again later" : res.Result.ErrorDescription);
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