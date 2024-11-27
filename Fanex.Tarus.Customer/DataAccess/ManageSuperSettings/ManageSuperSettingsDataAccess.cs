namespace Fanex.Tarus.Customer.DataAccess.ManageSuperSettings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.ManageSuperSettings;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.ManageSuperSettings;
    using Fanex.Tarus.Customer.Entities.MultipleUpdateSuperStatus;
    using Fanex.Tarus.Infrastructure.Caching;

    public interface IManageSuperSettingsDataAccess
    {
        CheckSuperListResult CheckSuperList(CheckSuperListCriteria criteria);

        WinLossLimitModel GetWinLossLimit(GetWinLossLimitCriteria criteria);
    }

    public class ManageSuperSettingsDataAccess : BaseDataAccess, IManageSuperSettingsDataAccess
    {
        private readonly int[] RepresentProductIds = new int[] { 5 };

        public CheckSuperListResult CheckSuperList(CheckSuperListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var response = new CheckSuperListResult();
                var superList = new List<SuperStatusEntity>();

                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var sportList = new List<SportType>();

                    while (reader.Read())
                    {
                        superList.Add(new SuperStatusEntity
                        {
                            CustId = Utils.ToInt32(reader["custid"]),
                            UserName = reader["username"].ToString(),
                            Site = reader["site"].ToString(),
                            SiteLabel = reader["sitelabel"].ToString(),
                            Closed = Utils.ToBoolean(reader["closed"]),
                            Suspended = Utils.ToBoolean(reader["suspended"]),
                            Protected = Utils.ToBoolean(reader["IsProtectedMode"])
                        });
                    }

                    response.SuperList = superList;
                    response.SuperListIds = string.Join(",", superList.Select(sp => sp.CustId.ToString()));

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var groupId = Utils.ToInt32(reader["groupid"]);
                            var productId = Utils.ToInt32(reader["productid"]);
                            var sportTypeId = Utils.ToInt32(reader["SportType"]);
                            var sportTypeName = RepresentProductIds.Contains(productId) ? CacheRef.GetProductName(productId) : CacheRef.GetSportName(sportTypeId);
                            var sport = new SportType
                            {
                                GroupId = groupId,
                                ProductId = productId,
                                SportTypeId = sportTypeId,
                                SportTypeName = sportTypeName
                            };

                            sportList.Add(sport);
                        }
                    }

                    response.SportTypes = sportList;
                }

                var output = db.GetOuputParams();
                response.CustId = int.Parse(output["custid"].ToString());
                response.Result = new Result(int.Parse(output["errcode"].ToString()), output["errmsg"].ToString());

                return response;
            }
        }

        public WinLossLimitModel GetWinLossLimit(GetWinLossLimitCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var response = new WinLossLimitModel();
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var sportList = new List<SportType>();
                    var superList = new List<SuperStatusEntity>();

                    while (reader.Read())
                    {
                        var groupId = Utils.ToInt32(reader["groupid"]);
                        var productId = Utils.ToInt32(reader["productid"]);
                        var sportTypeId = Utils.ToInt32(reader["SportType"]);
                        var sportTypeName = RepresentProductIds.Contains(productId) ? CacheRef.GetProductName(productId) : CacheRef.GetSportName(sportTypeId);
                        var sport = new SportType
                        {
                            GroupId = groupId,
                            ProductId = productId,
                            SportTypeId = sportTypeId,
                            SportTypeName = sportTypeName
                        };

                        sportList.Add(sport);
                    }

                    response.SportTypes = sportList;
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var super = new SuperStatusEntity
                            {
                                CustId = int.Parse(reader["custid"].ToString()),
                                UserName = reader["username"].ToString()
                            };

                            superList.Add(super);
                        }

                        response.SuperList = superList;
                        response.SuperListIds = string.Join(",", superList.Select(sp => sp.CustId.ToString()));
                    }
                }

                var output = db.GetOuputParams();
                response.Result = new Result(int.Parse(output["errcode"].ToString()), output["errmsg"].ToString());

                return response;
            }
        }
    }
}
