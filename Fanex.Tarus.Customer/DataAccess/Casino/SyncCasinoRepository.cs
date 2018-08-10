using CasinoLibrary.Model.UserException;
using Fanex.CustomerSetting.Module.Casino.Services.Impl;
using Fanex.CustomerSetting.Module.Core;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.CustomerSetting.Module.Core.Web;
using Fanex.Data;
using Fanex.Logging.Extensions.Web;
using Fanex.Tarus.Customer.Entities.NG2;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Entities.Casino;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fanex.Tarus.Customer.DataAccess.Casino
{
    public class SyncCasinoRepository : ISyncCasinoRepository
    {
        public SyncCustomerModel GetCustomersByUsernames(string usernames)
        {
            var result = new SyncCustomerModel();
            SyncCustomerEntity entity = null;

            using (var db = new ObjectDb("Acc_CustomerSelRnGStatus"))
            {
                var param = new Hashtable();
                param.Add("strusername", usernames);

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
                            entity.RoleName = GetRoleName(reader.ToInt32("RoleId"));
                            entity.Username = reader.ToString("Username");
                            entity.CurrencyName = reader.ToString("CurrencyName");
                            entity.UplineUserName = reader.ToString("UplineUserName");
                            entity.UplineId = reader.ToInt32("UplineId");
                            entity.IsSync = reader.ToBoolean("SyncCS");
                            entity.IsSync3rd = reader.ToBoolean("ExistsCSDB");

                            result.Usernames.Add(entity);
                        }
                    }
                }
            }

            return result;
        }

        public SyncCustomerModel GetUnsyncedCustomers()
        {
            var result = new SyncCustomerModel();
            SyncCustomerEntity entity = null;

            using (var db = new ObjectDb("Acc_CustomerSelUnSyncRNG"))
            {
                var param = new Hashtable();

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
                            entity.RoleName = GetRoleName(reader.ToInt32("RoleId"));
                            entity.Username = reader.ToString("Username");
                            entity.CurrencyName = reader.ToString("CurrencyName");
                            entity.UplineUserName = reader.ToString("UplineUserName");
                            entity.UplineId = reader.ToInt32("UplineId");
                            entity.IsSync = reader.ToBoolean("SyncCS");
                            entity.IsSync3rd = reader.ToBoolean("ExistsCSDB");

                            result.Usernames.Add(entity);
                        }
                    }
                }
            }

            return result;
        }

        public IDictionary<string, Result> SyncCustomers(IList<SyncCustomerEntity> customers)
        {
            var result = new Dictionary<string, Result>();
            var custIBC = new SuperServicesImpl();

            foreach (var customer in customers)
            {
                bool inserted = false;
                CSCustomerDetail cust = new CSCustomerDetail();
                try
                {
                    cust = CasinoManager.GetCustInfoIBC(customer.CustId);
                    var enableJackpot = custIBC.IsEnabledJackpot(customer.UplineId);
                    inserted = new CasinoLibrary.SBAgent.Customer().
                        AutoInsertAccount("01",
                            cust.IBCCustID,
                            cust.UserName,
                            cust.UserName2,
                            cust.FirstName,
                            cust.LastName,
                            cust.UserPwd,
                            cust.Phone,
                            cust.MobilePhone,
                            cust.Currency,
                            cust.Fax,
                            cust.Closed,
                            cust.Suspend,
                            cust.IsEnablePT,
                            cust.SIBCCustID,
                            cust.MIBCCustID,
                            cust.AIBCCustID,
                            cust.RoleId.ToString(),
                            false,
                            enableJackpot,
                            cust.DisableAutoMasterPT,
                            DateTime.Now,
                            AppSession.CurrentContext.Ip4Address,
                            AppSession.CurrentSession.UserId,
                            AppSession.CurrentSession.UserName,
                            AppSession.CurrentSession.FromSystem);
                }
                catch (ErrorException e)
                {
                    ExceptionHandler.Handle(new Exception(e.Message, e), TraceEventType.Information);
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }

                if (inserted || customer.IsSync3rd)
                {
                    try
                    {
                        var disabled = CasinoManager.GetCustInfoIBC(customer.UplineId).CasinoDisabled;
                        var res = CasinoManager.UpdateCSStatus(customer.CustId, true, disabled, true, cust.SiteName);
                        result.Add(customer.Username, new Result { ErrorCode = res ? 0 : 1 });
                    }
                    catch (ErrorException e)
                    {
                        ExceptionHandler.Handle(new Exception(e.Message, e), TraceEventType.Information);
                        result.Add(customer.Username, new Result { ErrorCode = 1 });
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.Handle(ex);
                        result.Add(customer.Username, new Result { ErrorCode = 1 });
                    }
                }
                else
                {
                    result.Add(customer.Username, new Result { ErrorCode = 1 });
                }
            }

            return result;
        }

        private static string GetRoleName(int roleId) =>
            roleId == 4 ? "Super" : 
            roleId == 3 ? "Master" : 
            roleId == 2 ? "Agent" : "Member";
    }
}