namespace Fanex.Tarus.Customer.DataAccess.CopySuperSetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.CopySuperSetting;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.CopySuperSetting;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Extensions;

    public interface ICopySuperSettingDataAccess
    {
        CheckSuperListResult CheckSuperList(CheckSuperListCriteria criteria);
    }
    public class CopySuperSettingDataAccess : BaseDataAccess, ICopySuperSettingDataAccess
    {
        private readonly int[] RepresentProductIds = new int[] {5};
        public CheckSuperListResult CheckSuperList(CheckSuperListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var response = new CheckSuperListResult();
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var sportList = new List<SportType>();
                    var superList = new List<BaseModel>();

                    while (reader.Read())
                    {
                        var groupId = int.Parse(reader["groupid"].ToString());
                        var productId = int.Parse(reader["productid"].ToString());
                        var sportTypeId = int.Parse(reader["SportType"].ToString());
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
                            var super = new BaseModel
                            {
                                Id = int.Parse(reader["custid"].ToString()),
                                Name = reader["username"].ToString()
                            };

                            superList.Add(super);
                        }

                        response.SuperList = superList;
                        response.SuperListIds = string.Join(",", superList.Select(sp => sp.Id.ToString()));
                    }
                }

                var output = db.GetOuputParams();
                response.CustId = int.Parse(output["custid"].ToString());
                response.Result = new Result(int.Parse(output["errcode"].ToString()), output["errmsg"].ToString());

                return response;
            }
        }
    }
}
