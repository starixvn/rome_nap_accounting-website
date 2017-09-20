using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeDcsSetting
{
    public class LicenseeDcsSettingDataAccess : ILicenseeDcsSettingDataAccess
    {
        public IEnumerable<string> GetSites(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return reader["licenseename"].ToString();
                    }
                }
            }
        }

        public IEnumerable<BaseModel> GetSupers(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetSuper(reader);
                    }
                }
            }
        }

        public IEnumerable<ProductSettingType> GetProducts(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<ProductSettingType>(criteria.GetSpParams());
            }
        }

        public decimal GetPositionTaking(ICriteria criteria)
        {
            decimal superPT = 0;

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        superPT = Convert.ToDecimal(reader["superpt"]);
                    }
                }
            }

            return superPT;
        }

        public IEnumerable<BetSetting> GetBetSettings(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetBetSetting(reader);
                    }
                }
            }
        }

        public StatusEntity GetStatus(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.QueryEntity<StatusEntity>(criteria.GetSpParams());
            }
        }

        public Result UpdatePositionTaking(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result UpdateBetSetting(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result UpdateStatus(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        private static BaseModel GetSuper(IDataReader reader)
        {
            return new BaseModel
            {
                Id = Convert.ToInt32(reader["custid"]),
                Name = reader["username"].ToString()
            };
        }

        private static BetSetting GetBetSetting(IDataReader reader)
        {
            return new BetSetting
            {
                SportType = Convert.ToInt32(reader["sporttype"]),
                BetType = Convert.ToInt32(reader["bettype"]),
                MinBet = Convert.ToDecimal(reader["minbet"]),
                MaxBet = Convert.ToDecimal(reader["maxbet"]),
                MaxPerMatch = Convert.ToDecimal(reader["maxpermatch"])
            };
        }

        private static Result ExecuteNonQuery(ICriteria criteria)
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
    }
}
