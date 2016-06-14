using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Tarus.Admin.Criteria.SMAManagement;
using Fanex.Tarus.Admin.Entities;
using Fanex.Tarus.Admin.Entities.SMAManagement;

namespace Fanex.Tarus.Admin.DataAccess.SMAManagement
{
    public class SMAManagementAccess : ISMAManagementAccess
    {
        public IEnumerable<IdNameEntity> GetSites()
        {
            IList<IdNameEntity> sites = new List<IdNameEntity>(32);

            using (var db = new ObjectDb("Acc_SiteSel_Credit"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        sites.Add(new IdNameEntity
                        {
                            Id = Convert.ToInt32(reader["siteid"]),
                            Name = reader["site"].ToString()
                        });
                    }
                }
            }

            return sites;
        }

        public IEnumerable<IdNameEntity> GetCurrencies()
        {
            IList<IdNameEntity> currencies = new List<IdNameEntity>(64);

            using (var db = new ObjectDb("Acc_ExchangeSel"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currencies.Add(new IdNameEntity
                        {
                            Id = Convert.ToInt32(reader["exchangeid"]),
                            Name = reader["currency"].ToString()
                        });
                    }
                }
            }

            return currencies;
        }

        public DataTable GetSMAGroupHeadlines(GetSMAGroupHeadlineCriteria criteria)
        {
            return ExecuteDataTable(criteria.GetSettingKey(), criteria.GetSPParams());
        }

        public Result UpdateHeadlineType(UpdateHeadlineTypeCriteria criteria)
        {
            return ExecuteNonQuery(criteria.GetSettingKey(), criteria.GetSPParams());
        }

        public Result UpdateOwner(UpdateOwnerCriteria criteria)
        {
            return ExecuteNonQuery(criteria.GetSettingKey(), criteria.GetSPParams());
        }

        private DataTable ExecuteDataTable(string settingKey, Hashtable inputParams = null)
        {
            using (var db = new ObjectDb(settingKey))
            {
                var ds = db.ExecuteDataSet(inputParams ?? new Hashtable());

                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }

            return null;
        }

        private Result ExecuteNonQuery(string settingKey, Hashtable inputParams)
        {
            using (var db = new ObjectDb(settingKey))
            {
                db.ExecuteNonQuery(inputParams);
                var outputParams = db.GetOuputParams();

                if (outputParams != null)
                {
                    return new Result((int)outputParams["errcode"], outputParams["errmsg"].ToString());
                }
            }

            return null;
        }
    }
}
