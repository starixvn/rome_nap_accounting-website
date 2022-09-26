namespace Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting
{
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using Fanex.Tarus.Infrastructure.Caching;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public interface IBaseDataAccess
    {
        Result ExecuteNonQuery(ICriteria criteria);

        IEnumerable<BaseModel> GetSites();

        IEnumerable<BaseModel> GetExchangeCurrency();

        IEnumerable<ProductDcsModel> GetProductDcsModel(DcsProductMatrixCriteria criteria);

        ConfigModel GetConfigDetail(ICriteria criteria);

        InitializingCustomerSettingViewModel GetSetting(ICriteria criteria, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites);

        ValidateResultModel ValidateConfigSetting(ICriteria criteria);

        IEnumerable<InsertErrorModel> AddUpdateConfigSetting(ICriteria criteria);
    }
    public class BaseDataAccess : IBaseDataAccess
    {
        protected const string FormatDisplay = "[{0}] {1}";
        protected const string ProductCacheKey = "InitCust-Product-{0}";
        protected const string SportCacheKey = "InitCust-Sport-{0}";
        protected const string BettypeCacheKey = "InitCust-BetType-{0}";
        protected readonly MemoryCache _MemoryCache;

        public BaseDataAccess()
        {
            _MemoryCache = new MemoryCache(new MemoryCacheOptions());
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
                Hashtable hashtableParams = new Hashtable
                {
                    { "mode", 2 }
                };
                objDB.AssignParamValues(hashtableParams);
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

        public IEnumerable<ProductDcsModel> GetProductDcsModel(DcsProductMatrixCriteria criteria)
        {
            var result = new List<ProductDcsModel>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        var productId = Convert.ToInt32(reader["productid"]);
                        var sportId = Convert.ToInt32(reader["sporttype"]);
                        var bettypeId = Convert.ToInt32(reader["bettype"]);
                        result.Add(new ProductDcsModel
                        {
                            ProductId = productId,
                            ProductName = GetProductName(productId),
                            SportId = sportId,
                            SportName = GetSportName(sportId),
                            BettypeId = bettypeId,
                            BettypeName = GetBetTypeName(bettypeId),
                            SettingTypeId = Convert.ToInt32(reader["settingtype"]),
                            SettingTypeName = reader["settingname"].ToString(),
                            IsLive = Convert.ToBoolean(reader["islive"]),
                            MaxDiscount = Convert.IsDBNull(reader["maxdiscount"]) ? 0 : Convert.ToDecimal(reader["maxdiscount"]),
                            IntervalValue = Convert.IsDBNull(reader["intervalvalue"]) ? 0 : Convert.ToDecimal(reader["intervalvalue"])
                        });
                    }
                }
            }

            return result;
        }

        protected SettingConfigModel MappingConfig(IDataReader reader, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites)
        {
            var model = new SettingConfigModel
            {
                ConfigId = Convert.ToInt32(reader["configid"].ToString()),
                ProductId = Convert.ToInt32(reader["productid"].ToString()),
                IsTest = Convert.ToBoolean(reader["istest"].ToString()),
                Remark = reader["remark"].ToString(),
                StartTime = Convert.IsDBNull(reader["starttime"]) ? string.Empty : Convert.ToDateTime(reader["starttime"]).ToString("MM/dd/yyyy HH:mm:ss"),
                EndTime = Convert.IsDBNull(reader["endtime"]) ? string.Empty : Convert.ToDateTime(reader["endtime"]).ToString("MM/dd/yyyy HH:mm:ss"),
                CurrentStep = reader["currentstep"].ToString(),
                ProcessedSteps = reader["processedstep"].ToString(),
                Status = Convert.ToInt32(reader["status"].ToString()),
                BetTypeIdList = new List<int>(),
                BetTypeList = new List<string>(),
                CurrencyIdList = new List<int>(),
                CurrencyList = new List<string>(),
                SiteIdList = new List<int>(),
                SiteList = new List<string>()
            };

            model.ProductName = string.Format(FormatDisplay, model.ProductId, GetProductName(model.ProductId));
            var sportTypeIds = reader["sporttypelist"].ToString().Split(',').Select(id => Convert.ToInt32(id));
            var sportTypeList = new List<string>();
            foreach (var sportType in sportTypeIds)
            {
                sportTypeList.Add(string.Format(FormatDisplay, sportType, GetSportName(sportType)));
            }
            model.SportTypeIdList = sportTypeIds;
            model.SportTypeList = sportTypeList;

            var betTypeIds = reader["bettypelist"].ToString().Split(',').Select(id => Convert.ToInt32(id));
            var betTypeList = new List<string>();
            foreach (var betType in betTypeIds)
            {
                betTypeList.Add(string.Format(FormatDisplay, betType, GetBetTypeName(betType)));
            }
            model.BetTypeIdList = betTypeIds;
            model.BetTypeList = betTypeList;

            var currencyIds = reader["currency"].ToString().Split(',').Select(id => Convert.ToInt32(id));
            var currencyList = new List<string>();
            foreach (var currency in currencyIds)
            {
                var targetCurrency = currencies.FirstOrDefault(c => c.Id == currency);
                currencyList.Add(targetCurrency != null ? targetCurrency.Name : string.Format(FormatDisplay, currency, string.Empty));
            }
            model.CurrencyIdList = currencyIds;
            model.CurrencyList = currencyList;

            var siteIds = reader["site"].ToString().Split(',').Select(id => Convert.ToInt32(id));
            var siteList = new List<string>();
            foreach (var site in siteIds)
            {
                var targetSite = sites.FirstOrDefault(c => c.Id == site);
                siteList.Add(targetSite != null ? targetSite.Name : string.Format(FormatDisplay, site, string.Empty));
            }
            model.SiteIdList = siteIds;
            model.SiteList = siteList;

            return model;
        }


        protected string GetProductName(int productId)
        {
            var key = string.Format(ProductCacheKey, productId);
            if (!_MemoryCache.TryGet(key, out string productName))
            {
                productName = CacheRef.GetProductName(productId);
                _MemoryCache.Set(key, productName, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return productName;
        }

        protected string GetSportName(object sportId)
        {
            var key = string.Format(SportCacheKey, sportId);
            if (!_MemoryCache.TryGet(key, out string sportName))
            {
                sportName = CacheRef.GetSportName(sportId);
                _MemoryCache.Set(key, sportName, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return sportName;
        }

        protected string GetBetTypeName(object bettypeId)
        {
            var key = string.Format(BettypeCacheKey, bettypeId);
            if (!_MemoryCache.TryGet(key, out string bettypeName))
            {
                bettypeName = CacheRef.GetBettype(bettypeId);
                _MemoryCache.Set(key, bettypeName, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return bettypeName;
        }

        public virtual ConfigModel GetConfigDetail(ICriteria criteria)
        {
            var result = new ConfigModel();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Config = new SettingConfigModel
                        {
                            ConfigId = Convert.ToInt32(reader["configid"].ToString()),
                            ProductId = Convert.ToInt32(reader["productid"].ToString()),
                            IsTest = Convert.ToBoolean(reader["istest"].ToString()),
                            Remark = reader["remark"].ToString(),
                            StartTime = Convert.IsDBNull(reader["starttime"]) ? string.Empty : Convert.ToDateTime(reader["starttime"]).ToString("MM/dd/yyyy HH:mm:ss"),
                            EndTime = Convert.IsDBNull(reader["endtime"]) ? string.Empty : Convert.ToDateTime(reader["endtime"]).ToString("MM/dd/yyyy HH:mm:ss"),
                            CurrentStep = reader["currentstep"].ToString(),
                            ProcessedSteps = reader["processedstep"].ToString(),
                            SportTypeIdList = reader["sporttypelist"].ToString().Split(',').Select(id => Convert.ToInt32(id)),
                            BetTypeIdList = reader["bettypelist"].ToString().Split(',').Select(id => Convert.ToInt32(id)),
                            CurrencyIdList = reader["currency"].ToString().Split(',').Select(id => Convert.ToInt32(id)),
                            SiteIdList = reader["site"].ToString().Split(',').Select(id => Convert.ToInt32(id))
                        };
                    }

                    if (reader.NextResult())
                    {
                        var configDetails = new List<SettingInforModel>();
                        while (reader.Read())
                        {
                            var setting = new SettingInforModel
                            {
                                ConfigId = Convert.ToInt32(reader["configid"].ToString()),
                                BetType = Convert.ToInt32(reader["BetType"].ToString()),
                                CopyFromBetType = Convert.IsDBNull(reader["CopyFromBetType"]) ? 0 : Convert.ToInt32(reader["CopyFromBetType"].ToString()),
                                CopyFromSportType = Convert.IsDBNull(reader["CopyFromSportType"]) ? 0 : Convert.ToInt32(reader["CopyFromSportType"].ToString()),
                                DefaultValueAgent = reader["DefaultValueAgent"].ToString(),
                                DefaultValueMaster = reader["DefaultValueMaster"].ToString(),
                                DefaultValueMember = reader["DefaultValueMember"].ToString(),
                                DefaultValueSuper = reader["DefaultValueSuper"].ToString(),
                                IsDefault = Convert.ToBoolean(reader["IsDefault"].ToString()),
                                IsCopy = !Convert.ToBoolean(reader["IsDefault"].ToString()),
                                IsLive = Convert.ToBoolean(reader["IsLive"].ToString()),
                                SettingName = reader["SettingName"].ToString(),
                                SettingType = Convert.ToInt32(reader["SettingType"].ToString()),
                                SpecialRequirement = reader["SpecialRequirement"].ToString(),
                                SportType = Convert.ToInt32(reader["SportType"].ToString()),
                                SportName = GetSportName(reader["SportType"].ToString()),
                                BetTypeName = GetBetTypeName(reader["BetType"].ToString()),
                                BetTypeOptions = new List<System.Web.Mvc.SelectListItem>(),
                                SportTypeOptions = new List<System.Web.Mvc.SelectListItem>()
                            };

                            configDetails.Add(setting);
                        }

                        result.Settings = configDetails;
                    }
                }
            }

            return result;
        }

        public virtual InitializingCustomerSettingViewModel GetSetting(ICriteria criteria, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites)
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

        public virtual ValidateResultModel ValidateConfigSetting(ICriteria criteria)
        {
            var result = new List<InsertErrorModel>();
            using (ObjectDb objDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDB.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new InsertErrorModel
                        {
                            Id = Convert.IsDBNull(reader["id"]) ? -1 : Convert.ToInt32(reader["id"]),
                            ErrorCode = Convert.ToInt32(reader["errorcode"]),
                            ErrorMsg = reader["errormsg"].ToString(),
                        });
                    }

                    var outputParams = objDB.GetOuputParams();

                    return new ValidateResultModel
                    {
                        IsWarning = Convert.ToBoolean(outputParams["iswarning"]),
                        Errors = result
                    };
                }
            }
        }

        public virtual IEnumerable<InsertErrorModel> AddUpdateConfigSetting(ICriteria criteria)
        {
            var result = new List<InsertErrorModel>();
            using (ObjectDb objDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDB.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new InsertErrorModel
                        {
                            Id = Convert.IsDBNull(reader["id"]) ? -1 : Convert.ToInt32(reader["id"]),
                            ErrorCode = Convert.ToInt32(reader["errorcode"]),
                            ErrorMsg = reader["errormsg"].ToString(),
                        });
                    }
                }
            }

            return result;
        }
    }
}
