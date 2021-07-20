namespace Fanex.Tarus.Customer.DataAccess.LicenseeOddsGroup
{
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.ViewModels.LicenseeOddsGroup;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Result = CustomerSetting.Module.Core.Models.Result;

    public class LicenseeOddsGroupDataAccess : BaseDataAccess, ILicenseeOddsGroupDataAccess
    {
        public IEnumerable<BaseModel> GetAgents(ICriteria criteria)
            => ExecuteDataReader(criteria, GetAgent);

        public IEnumerable<AgentOddsGroupEntity> GetAgentOddsGroups(ICriteria criteria)
            => ExecuteDataReader(criteria, GetAgentOddsGroup);

        public Result UpdateOddsGroup(ICriteria criteria)
            => ExecuteNonQuery(criteria);

        private static AgentOddsGroupEntity GetAgentOddsGroup(IDataReader reader)
            => new AgentOddsGroupEntity
            {
                CustId = Utils.ToInt32(reader["custid"]),
                UserName = Utils.ToString(reader["username"]),
                OperatorId = Utils.ToString(reader["operatorid"]),
                OddsGroup = Utils.ToString(reader["ugroup"]),
                IsActive = Utils.ToBoolean(reader["isactive"])
            };

        private static BaseModel GetAgent(IDataReader reader)
            => new BaseModel
            {
                Id = Convert.ToInt32(reader["custid"]),
                Name = reader["username"].ToString()
            };

        private static IEnumerable<T> ExecuteDataReader<T>(ICriteria criteria, Func<IDataReader, T> parseTFunction)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return parseTFunction(reader);
                    }
                }
            }
        }
    }
}
