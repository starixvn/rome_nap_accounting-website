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

        public IEnumerable<SMAGroupHeadline> GetSMAGroupHeadlines(GetSMAGroupHeadlineCriteria criteria)
        {
            IDictionary<int, SMAGroupHeadline> groupHeadlinesHash = new Dictionary<int, SMAGroupHeadline>(128);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSPParams()))
                {
                    SMAEntity entity;
                    SMAGroupHeadline groupHeadline;

                    while (reader.Read())
                    {
                        entity = ParseSMAEntity(reader);

                        if (!groupHeadlinesHash.TryGetValue(entity.GroupId, out groupHeadline))
                        {
                            groupHeadline = new SMAGroupHeadline
                            {
                                GroupId = entity.GroupId,
                                GroupName = entity.GroupName,
                                Owner = entity.Owner,
                                SMAEntities = new List<SMAEntity>(64)
                            };

                            groupHeadlinesHash.Add(entity.GroupId, groupHeadline);
                        }

                        groupHeadline.SMAEntities.Add(entity);
                    }
                }
            }

            return groupHeadlinesHash.Values;
        }

        public Result UpdateHeadlineType(UpdateHeadlineTypeCriteria criteria)
        {
            return ExecuteNonQuery(criteria.GetSettingKey(), criteria.GetSPParams());
        }

        public Result UpdateOwner(UpdateOwnerCriteria criteria)
        {
            return ExecuteNonQuery(criteria.GetSettingKey(), criteria.GetSPParams());
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

        private SMAEntity ParseSMAEntity(IDataReader reader)
        {
            var entity = new SMAEntity
            {
                GroupId = Convert.ToInt32(reader["custheadlineid"]),
                GroupName = reader["groupname"].ToString(),
                CustId = Convert.ToInt32(reader["custid"]),
                Username = reader["username"].ToString(),
                IsSuspend = Convert.ToBoolean(reader["issuspend"]),
                IsClosed = Convert.ToBoolean(reader["isclosed"]),
                IsEnable = Convert.ToBoolean(reader["isenable"]),
                Site = reader["site"].ToString(),
                Currency = reader["currencyname"].ToString(),
                RoleId = Convert.ToInt32(reader["roleid"]),
                Owner = reader["ownername"].ToString()
            };

            object headlineType = reader["headlinetype"];

            if (headlineType == null || headlineType is DBNull)
            {
                entity.HeadlineTypeId = null;
            }
            else
            {
                entity.HeadlineTypeId = Convert.ToInt32(headlineType);
            }

            return entity;
        }
    }
}
