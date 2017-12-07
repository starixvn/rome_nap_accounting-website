using System;
using System.Collections;
using Fanex.CustomerSetting.Module.Core;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.CustomerSetting.Module.Core.Web;
using Fanex.Data;
using Fanex.Logging.Extensions.Web;
using Fanex.Tarus.Customer.Entities.NG2;

namespace Fanex.Tarus.Customer.DataAccess.NG2
{
    public class SyncCustomerRepository : ISyncCustomerRepository
    {
        public SyncCustomerModel GetCustomersByCustIds(string custIds)
        {
            var result = new SyncCustomerModel();
            SyncCustomerEntity entity = null;

            using (var db = new ObjectDb("Acc_CustSetting_SyncAPICustomerProfileSelect_CustID"))
            {
                var param = new Hashtable();
                param.Add("custids", custIds);

                try
                {
                    using (var reader = db.ExecuteReader(param))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entity = new SyncCustomerEntity();
                                entity.Currency = reader.ToInt32("Currency");
                                entity.CustId = reader.ToInt32("CustId");
                                entity.RoleId = reader.ToInt32("RoleId");
                                entity.Username = reader.ToString("Username");
                                entity.CurrencyName = reader.ToString("CurrencyName");
                                entity.RoleName = reader.ToString("RoleName");
                                entity.UplineUserName = reader.ToString("UplineUserName");
                                entity.UplineId = reader.ToInt32("UplineId");
                                entity.IsSync = reader.ToBoolean("IsSync");

                                result.Usernames.Add(entity);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }                                
            }

            return result;
        }

        public SyncCustomerModel GetCustomersByUsernames(string usernames)
        {
            var result = new SyncCustomerModel();
            SyncCustomerEntity entity = null;

            try
            {
                using (var db = new ObjectDb("Acc_CustSetting_SyncAPICustomerProfileSelect_UserName"))
                {
                    var param = new Hashtable();
                    param.Add("usernamelist", usernames);

                    using (var reader = db.ExecuteReader(param))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                entity = new SyncCustomerEntity();
                                entity.CustId = reader.ToInt32("CustId");
                                entity.Username = reader.ToString("Username");

                                result.Usernames.Add(entity);
                            }

                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    result.NotExistUsernames.Add(reader.ToString("Username"));
                                }                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return result;
        }

        public Result SyncCustomers(string custIds, bool isSync, int productId)
        {
            var result = new Result();

            try
            {
                using (var db = new ObjectDb("Acc_CustSetting_SyncGameStatusUpd_Multiple"))
                {
                    var param = new Hashtable();
                    param.Add("custids", custIds);
                    param.Add("issync", isSync);
                    param.Add("productid", productId);
                    param.Add("userid", AppSession.CurrentSession.UserId);
                    param.Add("ip", AppSession.CurrentContext.Ip4Address);
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);

                    db.ExecuteNonQuery(param);

                    result.ErrorCode = Utils.ToInt32(db.GetParameterValue("errcode"));
                    result.ErrorMessage = Utils.ToString(db.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return result;
        }
    }
}