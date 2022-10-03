using System;
using Fanex.Common;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Data;
using Fanex.Tarus.Customer.DangerousGrouping.Criterias;
using Fanex.Tarus.Customer.DangerousGrouping.Helpers;
using Fanex.Tarus.Customer.DangerousGrouping.Models;
using Fanex.Tarus.Infrastructure.Extensions;
using static Sunplus.Accounting.Core.Common.Enumerations;

namespace Fanex.Tarus.Customer.DangerousGrouping.DataAccess
{
    public class DangerousGroupingDataAccess
    {
        private readonly IDangerousGroupingHelper helper;

        public DangerousGroupingDataAccess()
        {
            helper = new DangerousGroupingHelper();
        }

        public CustomerDangerResult GetCustomerDanger(GetCustomerDangerCriteria criteria)
        {
            try
            {
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
                }

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(criteria.GetSpParams());
                    var reader = db.ExecuteReader();
                    var outputParams = db.GetOuputParams();

                    var customerDangerResult = new CustomerDangerResult
                    {
                        Result = new Result(Converter.ToInt(outputParams["errcode"]), Converter.ToString2(outputParams["errmsg"]))
                    };
                    if(reader.Read())
                    {
                        
                        var originDangerLevel = int.Parse(reader["danger"].ToString());
                        customerDangerResult.CustomerDanger = new CustomerDanger
                        {
                            Username = criteria.Username,
                            OriginDanger = helper.GetDangerDetailFromValue(originDangerLevel),
                            ABIDanger = helper.GetDangerDetailFromValue(int.Parse(reader["danger2"].ToString())),
                            OtherSportDanger = helper.GetDangerDetailFromValue(int.Parse(reader["danger3"].ToString())),
                            BasketballDanger = helper.GetDangerDetailFromValue(int.Parse(reader["danger4"].ToString())),
                            ESportsDanger = helper.GetDangerDetailFromValue(int.Parse(reader["danger5"].ToString())),
                            IsMember = int.Parse(reader["roleid"].ToString()) == (int)Role.Member
                        };
                    }

                    return customerDangerResult;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        public Result UpdateCustomerDanger(UpdateCustomerDangerCriteria criteria)
        {
            try
            {
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
                }

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(criteria.GetSpParams());
                    var outputParams = db.GetOuputParams();

                    var result = new Result(Converter.ToInt(outputParams["errcode"]), Converter.ToString2(outputParams["errmsg"]));

                    return result;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }

        public Result ValdiateUsernames(ValidateCustomerNamesCriteria criteria)
        {
            try
            {
                if (criteria == null)
                {
                    throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
                }

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(criteria.GetSpParams());
                    var outputParams = db.GetOuputParams();

                    var result = new Result(Converter.ToInt(outputParams["errcode"]), Converter.ToString2(outputParams["errmsg"]));

                    return result;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}