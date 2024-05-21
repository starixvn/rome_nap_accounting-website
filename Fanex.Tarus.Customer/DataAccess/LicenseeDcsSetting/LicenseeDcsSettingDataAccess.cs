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

        public IEnumerable<BaseModelExtra> GetAgents(ICriteria criteria)
            => ExecuteDataReader(criteria, GetAgent);

        public IEnumerable<ProductSettingTypeEntity> GetProducts(ICriteria criteria)
            => ExecuteDataReader(criteria, GetProductSettingType);

        public IEnumerable<PTEntity> GetPositionTaking(ICriteria criteria)
            =>ExecuteDataReader(criteria, GetPositionTaking);

        public IEnumerable<ChargePTEntity> GetChargePositionTaking(ICriteria criteria)
            => ExecuteDataReader(criteria, GetChargePositionTaking);

        public IEnumerable<ChargeCommissionEntity> GetChargeCommission(ICriteria criteria)
            => ExecuteDataReader(criteria, GetChargeCommission);

        public IEnumerable<BetSettingEntity> GetBetSettings(ICriteria criteria)
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

            var result = from super in supers
                         join agent in agents
                         on new { super.SportType, super.BetType }
                         equals new { agent.SportType, agent.BetType }
                         select new BetSettingEntity()
                         {
                             SportType = super.SportType,
                             BetType = super.BetType,
                             MinBet = agent.MinBet,
                             SuperMinBet = super.MinBet,
                             MaxBet = agent.MaxBet,
                             SuperMaxBet = super.MaxBet,
                             MaxPerMatch = agent.MaxPerMatch,
                             SuperMaxPerMatch = super.MaxPerMatch
                         };

            return result.ToList();
        }

        public IEnumerable<BetSettingWithMaxPayoutPerMatchEntity> GetBetSettingsWithMaxPayoutPerMatch(ICriteria criteria)
        {
            IList<BetSettingWithMaxPayoutPerMatchEntity> supers = new List<BetSettingWithMaxPayoutPerMatchEntity>();
            IList<BetSettingWithMaxPayoutPerMatchEntity> agents = new List<BetSettingWithMaxPayoutPerMatchEntity>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        supers.Add(GetBetSettingWithMaxPayoutPerMatch(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            agents.Add(GetBetSettingWithMaxPayoutPerMatch(reader));
                        }
                    }
                }
            }

            var result = from super in supers
                         join agent in agents
                         on new { super.SportType, super.BetType }
                         equals new { agent.SportType, agent.BetType }
                         select new BetSettingWithMaxPayoutPerMatchEntity()
                         {
                             SportType = super.SportType,
                             BetType = super.BetType,
                             MinBet = agent.MinBet,
                             SuperMinBet = super.MinBet,
                             MaxBet = agent.MaxBet,
                             SuperMaxBet = super.MaxBet,
                             MaxPerMatch = agent.MaxPerMatch,
                             SuperMaxPerMatch = super.MaxPerMatch,
                             MaxPayoutPerMatch = agent.MaxPayoutPerMatch,
                             SuperMaxPayoutPerMatch = super.MaxPayoutPerMatch
                         };

            return result.ToList();
        }

        public IEnumerable<BetSettingWithMaxPerBallEntity> GetBetSettingsWithMaxPerBall(ICriteria criteria)
        {
            IList<BetSettingWithMaxPerBallEntity> supers = new List<BetSettingWithMaxPerBallEntity>();
            IList<BetSettingWithMaxPerBallEntity> agents = new List<BetSettingWithMaxPerBallEntity>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        supers.Add(GetBetSettingWithMaxPerBall(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            agents.Add(GetBetSettingWithMaxPerBall(reader));
                        }
                    }
                }
            }

            var result = from super in supers
                         join agent in agents
                         on new { super.SportType, super.BetType }
                         equals new { agent.SportType, agent.BetType }
                         select new BetSettingWithMaxPerBallEntity()
                         {
                             SportType = super.SportType,
                             BetType = super.BetType,
                             MinBet = agent.MinBet,
                             SuperMinBet = super.MinBet,
                             MaxBet = agent.MaxBet,
                             SuperMaxBet = super.MaxBet,
                             MaxPerMatch = agent.MaxPerMatch,
                             SuperMaxPerMatch = super.MaxPerMatch,
                             MaxPerBall = agent.MaxPerBall,
                             SuperMaxPerBall = super.MaxPerBall
                         };

            return result.ToList();
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

        private static BaseModelExtra GetAgent(IDataReader reader)
        {
            var agent = GetSuper(reader);

            if (Convert.ToBoolean(reader["issub"]))
            {
                agent.Name += " (sub)";
            }

            return new BaseModelExtra
            {
                Id = agent.Id,
                Name = agent.Name,
                SuperId = reader["srecommend"].ToString()
            };
        }

        private static ProductSettingTypeEntity GetProductSettingType(IDataReader reader)
            => new ProductSettingTypeEntity(
                Convert.ToInt32(reader["productid"]),
                reader["productname"].ToString(),
                Convert.ToByte(reader["settingtype"]));

        private static PTEntity GetPositionTaking(IDataReader reader)
            => new PTEntity
            {
                AgentPT = Convert.ToDecimal(reader["agentpt"]),
                SportType = Convert.ToInt32(reader["sporttype"]),
                BetType = Convert.ToInt32(reader["bettype"]),
                IsLive = Convert.ToBoolean(reader["islive"])
            };

        private static ChargePTEntity GetChargePositionTaking(IDataReader reader)
            => new ChargePTEntity
            {
                PT = Convert.ToDecimal(reader["pt"]),
                SportType = Convert.ToInt32(reader["sporttype"])
            };

        private static ChargeCommissionEntity GetChargeCommission(IDataReader reader)
            => new ChargeCommissionEntity
            {
                Discount = Convert.ToDecimal(reader["discount"]),
                SportType = Convert.ToInt32(reader["sporttype"]),
                BetType = Convert.ToInt32(reader["bettype"])
            };

        private static BetSettingEntity GetBetSetting(IDataReader reader)
            => new BetSettingEntity(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToDecimal(reader["minbet"]),
                Convert.ToDecimal(reader["maxbet"]),
                Convert.ToDecimal(reader["maxpermatch"]));

        private static BetSettingWithMaxPayoutPerMatchEntity GetBetSettingWithMaxPayoutPerMatch(IDataReader reader)
            => new BetSettingWithMaxPayoutPerMatchEntity(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToDecimal(reader["minbet"]),
                Convert.ToDecimal(reader["maxbet"]),
                Convert.ToDecimal(reader["maxpermatch"]),
                Convert.ToDecimal(reader["maxpayoutpermatch"]));

        private static BetSettingWithMaxPerBallEntity GetBetSettingWithMaxPerBall(IDataReader reader)
            => new BetSettingWithMaxPerBallEntity(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToDecimal(reader["minbet"]),
                Convert.ToDecimal(reader["maxbet"]),
                Convert.ToDecimal(reader["maxpermatch"]),
                Convert.ToDecimal(reader["maxpernumber"]));

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