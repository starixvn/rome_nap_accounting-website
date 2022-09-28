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

    public class InittializingDepositSettingDataAccess : InitializingCreditSettingDataAccess
    {
        public InittializingDepositSettingDataAccess() : base()
        {
        }

        public override IEnumerable<BaseModel> GetSites()
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

            return sites;
        }

        public override IEnumerable<ProductDcsModel> GetProductDcsModel()
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

        public override IEnumerable<BetSettingModel> GetBetSetting(int configId)
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

        public override ConfigModel GetConfigDetail(ICriteria criteria)
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

        protected override SettingConfigModel MappingConfig(IDataReader reader, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites)
        {
            var model = base.MappingConfig(reader, currencies, sites);
            model.IsExistBetSetting = Convert.ToBoolean(reader["existbetsetting"].ToString());

            return model;
        }

        public override IEnumerable<InsertErrorModel> AddUpdateConfigSetting(ICriteria criteria)
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
    }
}
