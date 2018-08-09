using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Resources;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeDcsSetting
{
    internal class LicenseeDcsSettingDataAccess : ILicenseeDcsSettingDataAccess
    {
        public IEnumerable<string> GetSites(ICriteria criteria)
            => ExecuteDataReader(criteria, GetSite);

        public IEnumerable<BaseModel> GetSupers(ICriteria criteria)
            => ExecuteDataReader(criteria, GetSuper);

        public IEnumerable<BaseModel> GetAgents(ICriteria criteria)
            => ExecuteDataReader(criteria, GetAgent);

        public IEnumerable<ProductSettingTypeEntity> GetProducts(ICriteria criteria)
            => ExecuteDataReader(criteria, GetProductSettingType);

        public decimal GetPositionTaking(ICriteria criteria)
        {
            var entities = ExecuteDataReader(criteria, GetPositionTaking);

            return entities == null || !entities.Any() ? 0 : entities.First();
        }

        public IEnumerable<BetSettingEntity>[] GetBetSettings(ICriteria criteria)
        {
            IList<BetSettingEntity> supers = new List<BetSettingEntity>();
            IList<BetSettingEntity> agents = new List<BetSettingEntity>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        supers.Add(GetBetSetting(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            agents.Add(GetBetSetting(reader));
                        }
                    }
                }
            }

            return new[] { supers, agents };
        }

        public StatusEntity GetStatus(ICriteria criteria)
            => ExecuteDataReader(criteria, GetStatusEntity).FirstOrDefault();

        public IEnumerable<GlobalStakeLevelEntity> GetGlobalStakeLevels(ICriteria criteria)
            => ExecuteDataReader(criteria, GetGlobalStakeLevelEntity);

        public IEnumerable<StakeLevelEntity> GetStakeLevels(GetStakeLevelCriteria criteria)
        {
            IList<StakeLevelEntity> stakeLevels = new List<StakeLevelEntity>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        stakeLevels.Add(GetStakeLevelEntity(reader));
                    }
                }

                var ouputParams = db.GetOuputParams();
                criteria.ErrCode = Convert.ToInt32(ouputParams["errcode"]);
                criteria.ErrMsg = ouputParams["errmsg"].ToString();
            }

            return stakeLevels;
        }

        public IEnumerable<StakeLevelGroupEntity> GetStakeLevelGroups(GetStakeLevelGroupCriteria criteria)
        {
            IList<StakeLevelGroupEntity> stakeLevelGroups = new List<StakeLevelGroupEntity>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        stakeLevelGroups.Add(GetStakeLevelGroupEntity(reader));
                    }
                }

                var ouputParams = db.GetOuputParams();
                criteria.ErrCode = Convert.ToInt32(ouputParams["errcode"]);
                criteria.ErrMsg = ouputParams["errmsg"].ToString();
            }

            return stakeLevelGroups;
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                return new Result(400, LicenseeDcsSettingLabels.BadRequest);
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    outputParams["errmsg"].ToString());
            }
        }

        private static string GetSite(IDataReader reader)
            => reader["licenseename"].ToString();

        private static BaseModel GetSuper(IDataReader reader)
            => new BaseModel
            {
                Id = Convert.ToInt32(reader["custid"]),
                Name = reader["username"].ToString()
            };

        private static BaseModel GetAgent(IDataReader reader)
        {
            var agent = GetSuper(reader);

            if (Convert.ToBoolean(reader["issub"]))
            {
                agent.Name += " (sub)";
            }

            return agent;
        }

        private static ProductSettingTypeEntity GetProductSettingType(IDataReader reader)
            => new ProductSettingTypeEntity(
                Convert.ToInt32(reader["productid"]),
                reader["productname"].ToString(),
                Convert.ToByte(reader["settingtype"]));

        private static decimal GetPositionTaking(IDataReader reader)
            => Convert.ToDecimal(reader["agentpt"]);

        private static BetSettingEntity GetBetSetting(IDataReader reader)
            => new BetSettingEntity(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToDecimal(reader["minbet"]),
                Convert.ToDecimal(reader["maxbet"]),
                Convert.ToDecimal(reader["maxpermatch"]));

        private static StatusEntity GetStatusEntity(IDataReader reader)
            => new StatusEntity(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToByte(reader["bitmaporder"]),
                Convert.ToBoolean(reader["isdisabled"]),
                Convert.ToBoolean(reader["isdisableddownline"]),
                Convert.ToBoolean(reader["hasdownline"]),
                Convert.ToBoolean(reader["isdisabledupline"]));

        private static GlobalStakeLevelEntity GetGlobalStakeLevelEntity(IDataReader reader)
            => new GlobalStakeLevelEntity(
                Convert.ToInt32(reader["gametypeid"]),
                Convert.ToInt32(reader["stakelevelid"]),
                Convert.ToDecimal(reader["minstake"]),
                Convert.ToDecimal(reader["maxstake"]),
                reader["minlabel"].ToString(),
                reader["maxlabel"].ToString());

        private static StakeLevelEntity GetStakeLevelEntity(IDataReader reader)
            => new StakeLevelEntity(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToInt32(reader["stakelevelid"]),
                reader["stakelevelname"].ToString(),
                Convert.ToBoolean(reader["ischecked"]));

        private static StakeLevelGroupEntity GetStakeLevelGroupEntity(IDataReader reader)
            => new StakeLevelGroupEntity(
                Convert.ToInt32(reader["stakelevelgroupid"]),
                reader["stakelevelgroupname"].ToString(),
                Convert.ToInt32(reader["bettyperef"]),
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToInt32(reader["stakelevelid"]),
                reader["stakelevel"].ToString(),
                Convert.ToBoolean(reader["ischecked"]));

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
