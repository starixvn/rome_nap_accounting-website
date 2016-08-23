using System.Collections.Generic;
using Fanex.CustomerSetting.Module.Core;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Data;
using Fanex.Tarus.Customer.Entities.NG2;

namespace Fanex.Tarus.Customer.DataAccess.NG2
{
    public class SyncCurrencyRepository : ISyncCurrencyRepository
    {
        public Result AddSyncCurrency(SyncCurrencyEntity entity)
        {
            // Create currency to 3rd by API only.
            var result = new Result();

            return result;
        }

        public SyncCurrencyEntity GetCurrencyByName(string currencyCode)
        {
            var entity = new SyncCurrencyEntity();
            using (var db = new ObjectDb("Acc_ExchangeSel_byCurrencyName"))
            {
                using (var reader = db.ExecuteReader(new { currencyname = currencyCode }))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            entity.BaseRate = reader.ToDecimal("actualrate");
                            entity.CurrencyCode = reader.ToString("currency");
                            entity.CurrencyId = reader.ToInt32("exchangeid");
                        }
                    }
                }
            }

            return entity;
        }

        public SyncCurrencyModel SyncCurrencies()
        {
            var model = new SyncCurrencyModel();
            using (var db = new ObjectDb("Acc_ExchangeSel_Sync_NG2"))
            {
                using (var reader = db.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            model.Currencies.Add(new SyncCurrencyEntity
                            {
                                BaseRate = reader.ToDecimal("actualrate"),
                                CurrencyCode = reader.ToString("currency"),
                                CurrencyId = reader.ToInt32("exchangeid")
                            }); 
                        }
                    }
                }
            }

            return model;
        }

        public IList<Result> UpdateCurrencies(SyncCurrencyModel model)
        {
            // Updates list of currencies to 3rd by API only.
            var result = new List<Result>();
            
            return result;
        }
    }
}