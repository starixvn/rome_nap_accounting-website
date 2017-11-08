using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeDcsSetting
{
    public class LicenseeDcsSettingDataAccess : ILicenseeDcsSettingDataAccess
    {
        public IEnumerable<string> GetSites(ICriteria criteria)
        {
            return ExecuteDataReader(criteria, GetSite);
        }

        public IEnumerable<BaseModel> GetSupers(ICriteria criteria)
        {
            return ExecuteDataReader(criteria, GetSuper);
        }

        public IEnumerable<ProductSettingTypeEntity> GetProducts(ICriteria criteria)
        {
            return ExecuteDataReader(criteria, GetProductSettingType);
        }

        public decimal GetPositionTaking(ICriteria criteria)
        {
            var entities = ExecuteDataReader(criteria, GetPositionTaking);

            return entities == null || !entities.Any() ? 0 : entities.First();
        }

        public IEnumerable<BetSettingEntity> GetBetSettings(ICriteria criteria)
        {
            return ExecuteDataReader(criteria, GetBetSetting);
        }

        public StatusEntity GetStatus(ICriteria criteria)
        {
            var entities = ExecuteDataReader(criteria, GetStatusEntity);

            return entities == null || !entities.Any() ? null : entities.First();
        }

        public IEnumerable<GlobalStakeLevelEntity> GetGlobalStakeLevels(ICriteria criteria)
        {
            return ExecuteDataReader(criteria, GetGlobalStakeLevelEntity);
        }

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
                return new Result(400, string.Empty);
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
        {
            return reader["licenseename"].ToString();
        }

        private static BaseModel GetSuper(IDataReader reader)
        {
            return new BaseModel
            {
                Id = Convert.ToInt32(reader["custid"]),
                Name = reader["username"].ToString()
            };
        }

        private static ProductSettingTypeEntity GetProductSettingType(IDataReader reader)
        {
            return new ProductSettingTypeEntity(
                Convert.ToInt32(reader["productid"]),
                reader["productname"].ToString(),
                Convert.ToByte(reader["settingtype"]));
        }

        private static decimal GetPositionTaking(IDataReader reader)
        {
            return Convert.ToDecimal(reader["superpt"]);
        }

        private static BetSettingEntity GetBetSetting(IDataReader reader)
        {
            return new BetSettingEntity(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToDecimal(reader["minbet"]),
                Convert.ToDecimal(reader["maxbet"]),
                Convert.ToDecimal(reader["maxpermatch"]));
        }

        private static StatusEntity GetStatusEntity(IDataReader reader)
        {
            return new StatusEntity(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToByte(reader["bitmaporder"]),
                Convert.ToBoolean(reader["isdisabled"]),
                Convert.ToBoolean(reader["isdisableddownline"]),
                Convert.ToBoolean(reader["hasdownline"]));
        }

        private static GlobalStakeLevelEntity GetGlobalStakeLevelEntity(IDataReader reader)
        {
            return new GlobalStakeLevelEntity(
                Convert.ToInt32(reader["gametypeid"]),
                Convert.ToInt32(reader["stakelevelid"]),
                Convert.ToDecimal(reader["minstake"]),
                Convert.ToDecimal(reader["maxstake"]),
                reader["minlabel"].ToString(),
                reader["maxlabel"].ToString());
        }

        private static StakeLevelEntity GetStakeLevelEntity(IDataReader reader)
        {
            return new StakeLevelEntity(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToInt32(reader["stakelevelid"]),
                reader["stakelevelname"].ToString(),
                Convert.ToBoolean(reader["ischecked"]));
        }

        private static StakeLevelGroupEntity GetStakeLevelGroupEntity(IDataReader reader)
        {
            return new StakeLevelGroupEntity(
                Convert.ToInt32(reader["stakelevelgroupid"]),
                reader["stakelevelgroupname"].ToString(),
                Convert.ToInt32(reader["bettyperef"]),
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToInt32(reader["stakelevelid"]),
                reader["stakelevel"].ToString(),
                Convert.ToBoolean(reader["ischecked"]));
        }
        
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
