namespace Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using Fanex.Tarus.Infrastructure.Caching;

    public interface IInitializingCustomerSettingDataAccess
    {
        InitializingCustomerSettingViewModel GetSetting(GetSettingCriteria criteria, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites);

        IEnumerable<BaseModel> GetSites();

        IEnumerable<BaseModel> GetExchangeCurrency();

        Result ExecuteNonQuery(ICriteria criteria);

        IEnumerable<ProductModel> GetProductSportBettype(DcsProductMatrixCriteria criteria);
    }
    public class InitializingCustomerSettingDataAccess : IInitializingCustomerSettingDataAccess
    {
        private const string FormatDisplay = "[{0}] {1}";
        
        public IEnumerable<BaseModel> GetExchangeCurrency()
        {
            var currencies = new List<BaseModel>
            {
                new BaseModel
                {
                    Id = 0,
                    Name = "[0] All Currencies"
                }
            };

            using (ObjectDb objDB = new ObjectDb("Acc_ExchangeCurrencySel"))
            {
                using (var reader = objDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var id = reader["exchangeid"].ToString();
                            currencies.Add(new BaseModel
                            {
                                Id = Convert.ToInt32(id),
                                Name = string.Format(FormatDisplay, id, reader["currency"].ToString())
                            });
                        }
                    }
                }
            }

            return currencies;
        }

        public InitializingCustomerSettingViewModel GetSetting(GetSettingCriteria criteria, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites)
        {
            var result = new InitializingCustomerSettingViewModel();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var configSettings = new List<SettingConfigModel>();
                    while (reader.Read())
                    {
                        configSettings.Add(MappingConfig(reader, currencies, sites));
                    }
                    result.ConfigSettings = configSettings;

                    var steps = new List<BaseModel>();
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            steps.Add(new BaseModel
                            {
                                Id = Convert.ToInt32(reader["stepid"].ToString()),
                                Name = string.Format(reader["stepname"].ToString())
                            });
                        }
                        
                    }

                    result.Steps = steps;
                }
            }

            return result;
        }

        public IEnumerable<BaseModel> GetSites()
        {
            var sites = new List<BaseModel>
            {
                new BaseModel
                {
                    Id = 0,
                    Name = "[0] All Sites"
                }
            };

            using (ObjectDb objDB = new ObjectDb("Acc_SiteSel_Credit"))
            {
                using (var reader = objDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var id = reader["siteid"].ToString();
                            sites.Add(new BaseModel
                            {
                                Id = Convert.ToInt32(id),
                                Name = string.Format(FormatDisplay, id, reader["site"].ToString())
                            });
                        }
                    }
                }
            }

            return sites;
        }

        private SettingConfigModel MappingConfig(IDataReader reader, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites)
        {
            var model = new SettingConfigModel
            {
                ConfigId = Convert.ToInt32(reader["configid"].ToString()),
                ProductId = Convert.ToInt32(reader["configid"].ToString()),
                IsTest = Convert.ToBoolean(reader["istest"].ToString()),
                Remark = reader["remark"].ToString(),
                StartTime = reader["starttime"].ToString(),
                EndTime = reader["endtime"].ToString(),
                CurrentStep = reader["currentstep"].ToString(),
                ProcessedSteps = reader["processedstep"].ToString(),
                BetTypeIdList = new List<int>(),
                BetTypeList = new List<string>(),
                CurrencyIdList = new List<int>(),
                CurrencyList = new List<string>(),
                SiteIdList = new List<int>(),
                SiteList = new List<string>()

            };

            model.ProductName = string.Format(FormatDisplay, model.ProductId, CacheRef.GetProductName(model.ProductId));
            var sportTypeIds = reader["sporttypelist"].ToString().Split(',').Select(id => Convert.ToInt32(id));
            var sportTypeList = new List<string>();
            foreach (var sportType in sportTypeIds)
            {
                sportTypeList.Add(string.Format(FormatDisplay, sportType, CacheRef.GetSportName(sportType.ToString())));
            }
            model.SportTypeIdList = sportTypeIds;
            model.SportTypeList = sportTypeList;

            var betTypeIds = reader["bettypelist"].ToString().Split(',').Select(id => Convert.ToInt32(id));
            var betTypeList = new List<string>();
            foreach (var betType in betTypeIds)
            {
                betTypeList.Add(string.Format(FormatDisplay, betType, CacheRef.GetBettype(betType.ToString())));
            }
            model.BetTypeIdList = betTypeIds;
            model.BetTypeList = betTypeList;

            var currencyIds = reader["currency"].ToString().Split(',').Select(id => Convert.ToInt32(id));
            var currencyList = new List<string>();
            foreach (var currency in currencyIds)
            {
                currencyList.Add(currencies.FirstOrDefault(c => c.Id == currency).Name);
            }
            model.CurrencyIdList = currencyIds;
            model.CurrencyList = currencyList;

            var siteIds = reader["site"].ToString().Split(',').Select(id => Convert.ToInt32(id));
            var siteList = new List<string>();
            foreach (var site in siteIds)
            {
                siteList.Add(sites.FirstOrDefault(c => c.Id == site).Name);
            }
            model.SiteIdList = siteIds;
            model.SiteList = siteList;

            return model;
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var objDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objDB.AssignParamValues(criteria.GetSpParams());
                objDB.ExecuteNonQuery();
                var outputs = objDB.GetOuputParams();
                return new Result()
                {
                    ErrorCode = (int)outputs["errcode"],
                    ErrorMessage = outputs["errmsg"]?.ToString()
                };
            }
        }

        public IEnumerable<ProductModel> GetProductSportBettype(DcsProductMatrixCriteria criteria)
        {
            var result = new List<ProductModel>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        var productId = Convert.ToInt32(reader["productid"]);
                        var product = new ProductModel(productId, CacheRef.GetProductName(productId));
                        var sportId = Convert.ToInt32(reader["sporttype"]);
                        var sport = new SportModel(sportId, CacheRef.GetSportName(sportId));
                        var bettypeId = Convert.ToInt32(reader["bettype"]);
                        var bettype = new BettypeModel(bettypeId, CacheRef.GetBettype(bettypeId));
                        var setting = new SettingTypeModel(Convert.ToInt32(reader["settingtype"]), reader["settingname"].ToString());
                        var existProduct = result.FirstOrDefault(pd => pd.ProductId == productId);
                        if(existProduct == null)
                        {
                            bettype.SettingTypes.Add(setting);
                            sport.Bettypes.Add(bettype);
                            product.Sports.Add(sport);
                            result.Add(product);
                        } else
                        {
                            BuildExistSettingType(existProduct, sport, bettype, setting);
                        }
                    }
                }
            }

            return result;
        }

        private void BuildExistSettingType(ProductModel product, SportModel sport, BettypeModel bettype, SettingTypeModel setting)
        {
            var existSport = product.Sports.FirstOrDefault(sp => sp.SportId == sport.SportId);
            if(existSport == null)
            {
                bettype.SettingTypes.Add(setting);
                sport.Bettypes.Add(bettype);
                product.Sports.Add(sport);
            } else
            {
                var existBettype = existSport.Bettypes.FirstOrDefault(bt => bt.BettypeId == bettype.BettypeId);
                if(existBettype == null)
                {
                    bettype.SettingTypes.Add(setting);
                    existSport.Bettypes.Add(bettype);
                } else
                {
                    existBettype.SettingTypes.Add(setting);
                }
            }
        }
    }
}
