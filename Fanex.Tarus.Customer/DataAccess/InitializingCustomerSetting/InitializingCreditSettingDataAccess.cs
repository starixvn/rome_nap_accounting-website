namespace Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Caching;
    using Fanex.Caching.Memory;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using Fanex.Tarus.Infrastructure.Caching;

    public interface IInitializingCreditSettingDataAccess : IBaseDataAccess
    {
        InitializingCustomerSettingViewModel GetSetting(GetSettingCriteria criteria, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites);

        ConfigModel GetConfigDetail(GetConfigDetailCriteria criteria);

        ValidateResultModel ValidateConfigSetting(ICriteria criteria);

        IEnumerable<InsertErrorModel> AddUpdateConfigSetting(ICriteria criteria);
    }
    public class InitializingCreditSettingDataAccess : BaseDataAccess, IInitializingCreditSettingDataAccess
    {
        public InitializingCreditSettingDataAccess() : base()
        {
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

        public ConfigModel GetConfigDetail(GetConfigDetailCriteria criteria)
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

        public IEnumerable<InsertErrorModel> AddUpdateConfigSetting(ICriteria criteria)
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
                            Id = Convert.IsDBNull(reader["id"])? -1 : Convert.ToInt32(reader["id"]),
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
