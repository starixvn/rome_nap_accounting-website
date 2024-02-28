namespace Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting.Deposit;
    using Fanex.NAP.Framework.Data;
    using System.Linq;
    using System.Data;
    using Fanex.Caching.Memory;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Caching;
    using System.Collections;

    public interface IInittializingDepositSettingDataAccess
    {

        Result ExecuteNonQuery(ICriteria criteria);

        IEnumerable<BaseModel> GetSites(int AccId);

        IEnumerable<BaseModel> GetExchangeCurrency(bool isDisplayMode = true);

        IEnumerable<ProductDcsModel> GetProductDcsModel();

        ConfigModel GetConfigDetail(ICriteria criteria);

        InitializingCustomerSettingViewModel GetSetting(ICriteria criteria, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites);

        ValidateResultModel ValidateConfigSetting(ICriteria criteria);

        IEnumerable<InsertErrorModel> AddUpdateConfigSetting(ICriteria criteria);

        IEnumerable<BetSettingModel> GetBetSetting(int configId);
    }

    public class InittializingDepositSettingDataAccess : IInittializingDepositSettingDataAccess
    {
        protected const string FormatDisplay = "[{0}] {1}";
        protected const string ProductCacheKey = "InitCust-Product-{0}";
        protected const string SportCacheKey = "InitCust-Sport-{0}";
        protected const string BettypeCacheKey = "InitCust-BetType-{0}";
        protected readonly MemoryCache _MemoryCache;

        public InittializingDepositSettingDataAccess()
        {
            _MemoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public IEnumerable<BaseModel> GetSites(int AccId)
        {
            var sites = new List<BaseModel>
            {
                new BaseModel
                {
                    Id = 0,
                    Name = "[0] All Sites"
                }
            };

            using (ObjectDb objDB = new ObjectDb("Acc_SiteSel_Licensee"))
            {
                using (var reader = objDB.ExecuteReader())
                {
                    objDB.AssignParamValues(new Hashtable
                    {
                        { "userid", AccId }
                    });
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var id = reader["licenseeid"].ToString();
                            sites.Add(new BaseModel
                            {
                                Id = Convert.ToInt32(id),
                                Name = string.Format(FormatDisplay, id, reader["licenseename"].ToString())
                            });
                        }
                    }
                }
            }

            return sites.OrderBy(site => site.Id);
        }

        public IEnumerable<ProductDcsModel> GetProductDcsModel()
        {
            var result = new List<ProductDcsModel>();
            using (var db = new ObjectDb("Acc_Dep_Initial_DCS_ProductProfile_Sel"))
            {
                using (var reader = db.ExecuteReader())
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
                            IsLive = Convert.ToBoolean(reader["islive"])
                        });
                    }
                }
            }

            return result;
        }

        public IEnumerable<BetSettingModel> GetBetSetting(int configId)
        {
            var criteria = new GetDepositBetSettingCriteria(configId);
            var result = new List<BetSettingModel>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        var sportId = Convert.ToInt32(reader["sporttype"]);
                        var bettypeId = Convert.ToInt32(reader["bettype"]);
                        result.Add(new BetSettingModel
                        {
                            SportTypeId = sportId,
                            SportTypeName = GetSportName(sportId),
                            BetTypeId = bettypeId,
                            BetTypeName = GetBetTypeName(bettypeId),
                            ConfigId = Convert.ToInt32(reader["configid"]),
                            SettingType = Convert.ToInt32(reader["settingtype"]),
                            Currency = Convert.ToInt32(reader["currency"]),
                            CurrencyCode = reader["currencycode"].ToString(),
                            CurrencyName = reader["currencyname"].ToString(),
                            MaxBet = Convert.IsDBNull(reader["maxbet"]) ? (decimal?)null : Convert.ToDecimal(reader["maxbet"]),
                            MinBet = Convert.IsDBNull(reader["minbet"]) ? (decimal?)null : Convert.ToDecimal(reader["minbet"]),
                            MaxPerMatch = Convert.IsDBNull(reader["maxpermatch"]) ? (decimal?)null : Convert.ToDecimal(reader["maxpermatch"]),
                            MaxPerNumber = Convert.IsDBNull(reader["maxpernumber"]) ? (decimal?)null : Convert.ToDecimal(reader["maxpernumber"]),
                            MaxPayoutPerMatch = Convert.IsDBNull(reader["maxpayoutpermatch"]) ? (decimal?)null : Convert.ToDecimal(reader["maxpayoutpermatch"])
                        });
                    }
                }
            }

            return result;
        }

        public ConfigModel GetConfigDetail(ICriteria criteria)
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

        public IEnumerable<InsertErrorModel> AddUpdateConfigSetting(ICriteria criteria)
        {
            var errorLists = new List<InsertErrorModel>();
            var betSettingErrors = new List<BetSettingErrorModel>();
            using (ObjectDb objDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDB.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        errorLists.Add(new InsertErrorModel
                        {
                            Id = Convert.IsDBNull(reader["id"]) ? -1 : Convert.ToInt32(reader["id"]),
                            ErrorCode = Convert.ToInt32(reader["errorcode"]),
                            ErrorMsg = reader["errormsg"].ToString(),
                        });
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            betSettingErrors.Add(new BetSettingErrorModel
                            {
                                ConfigRowID = Convert.IsDBNull(reader["ConfigRowID"]) ? -1 : Convert.ToInt32(reader["ConfigRowID"]),
                                BSRowID = Convert.IsDBNull(reader["BSRowID"]) ? -1 : Convert.ToInt32(reader["BSRowID"]),
                                SettingType = Convert.IsDBNull(reader["SettingType"]) ? -1 : Convert.ToInt32(reader["SettingType"]),
                                SportType = Convert.IsDBNull(reader["SportType"]) ? -1 : Convert.ToInt32(reader["SportType"]),
                                BetType = Convert.IsDBNull(reader["BetType"]) ? -1 : Convert.ToInt32(reader["BetType"]),
                                Currency = Convert.IsDBNull(reader["Currency"]) ? -1 : Convert.ToInt32(reader["Currency"]),
                                ErrCode = Convert.IsDBNull(reader["ErrCode"]) ? -1 : Convert.ToInt32(reader["ErrCode"])
                            });
                        }
                    }

                    if(betSettingErrors.Count <= 0)
                    {
                        return errorLists;
                    }

                    var result = new List<InsertErrorModel>();
                    foreach (var betSettingError in betSettingErrors)
                    {
                        result.Add(new InsertErrorModel
                        {
                            SettingType = betSettingError.SettingType,
                            ErrorCode = betSettingError.ErrCode,
                            ErrorMsg = errorLists.FirstOrDefault(err => err.ErrorCode == betSettingError.ErrCode)?.ErrorMsg,
                        });
                    }

                    return result;
                }
            }
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

        public IEnumerable<BaseModel> GetExchangeCurrency(bool isDisplayMode = true)
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
                                Name = isDisplayMode ? string.Format(FormatDisplay, id, reader["currency"].ToString()) : reader["currency"].ToString().ToUpperInvariant()
                            });
                        }
                    }
                }
            }

            return currencies;
        }

        public InitializingCustomerSettingViewModel GetSetting(ICriteria criteria, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites)
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

        public ValidateResultModel ValidateConfigSetting(ICriteria criteria)
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
        
        private string GetProductName(int productId)
        {
            var key = string.Format(ProductCacheKey, productId);
            if (!_MemoryCache.TryGet(key, out string productName))
            {
                productName = CacheRef.GetProductName(productId);
                _MemoryCache.Set(key, productName, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return productName;
        }

        private string GetSportName(object sportId)
        {
            var key = string.Format(SportCacheKey, sportId);
            if (!_MemoryCache.TryGet(key, out string sportName))
            {
                sportName = CacheRef.GetSportName(sportId);
                _MemoryCache.Set(key, sportName, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return sportName;
        }

        private string GetBetTypeName(object bettypeId)
        {
            var key = string.Format(BettypeCacheKey, bettypeId);
            if (!_MemoryCache.TryGet(key, out string bettypeName))
            {
                bettypeName = CacheRef.GetBettype(bettypeId);
                _MemoryCache.Set(key, bettypeName, new CacheItemOptions { SlidingExpiration = TimeSpan.FromHours(1) });
            }

            return bettypeName;
        }

        private SettingConfigModel MappingConfig(IDataReader reader, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites)
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
            model.IsExistBetSetting = Convert.ToBoolean(reader["existbetsetting"].ToString());

            return model;
        }
    }
}
