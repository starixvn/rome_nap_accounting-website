namespace Fanex.Tarus.Customer.DataAccess.InitializingCustomerSetting
{
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.InitializingCustomerSetting;
    using Fanex.Tarus.Customer.ViewModels.InitializingCustomerSetting;
    using System;
    using System.Collections.Generic;


    public interface IInittializingDepositSettingDataAccess: IBaseDataAccess
    {
        InitializingCustomerSettingViewModel GetSetting(GetSettingCriteria criteria, IEnumerable<BaseModel> currencies, IEnumerable<BaseModel> sites);
    }
    public class InittializingDepositSettingDataAccess : BaseDataAccess, IInittializingDepositSettingDataAccess
    {
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
    }
}
