namespace Fanex.Tarus.Customer.DataAccess.CopySuperSetting
{
    using System;
    using System.Collections.Generic;
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
        public CheckSuperListResult CheckSuperList(CheckSuperListCriteria criteria)
        {
            try
            {
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
                }

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    var response = new CheckSuperListResult();
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        var sportList = new List<BaseModel>();
                        var superList = new List<BaseModel>();

                        while (reader.Read())
                        {
                            var sportTypeId = int.Parse(reader["SportType"].ToString());
                            var sport = new BaseModel
                            {
                                Id = sportTypeId,
                                Name = CacheRef.GetSportName(sportTypeId)
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
                        }
                    }

                    var output = db.GetOuputParams();
                    response.CustId = int.Parse(output["custid"].ToString());
                    response.Result = new Result(int.Parse(output["errcode"].ToString()), output["errmsg"].ToString());

                    return response;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

                return new CheckSuperListResult
                {
                    Result = new Result(-1, "Internal Server Error.")
                };
            }
        }
    }
}
