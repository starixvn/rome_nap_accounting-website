using System;
using System.Collections;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using Fanex.Data;
using IBCUtils;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Entities;

/// <summary>
/// Summary description for PositionTakingProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.IBCClient
{
    public class PositionTakingProviderClient : PositionTakingProvider
    {
        public override DbResult GetPT(int custId, Globals.AccountType accountType, int uplineId)
        {
            //return base.GetPT(custId);
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        var spElementS =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIEditSuper");
                        return DbHelper.Execute(spElementS.Name, param, spElementS.DbName);
                    case Globals.AccountType.Master:
                        param.Add("srecommend", uplineId);
                        var spElementM =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIEditMaster");
                        return DbHelper.Execute(spElementM.Name, param, spElementM.DbName);
                    case Globals.AccountType.Agent:
                        param.Add("mrecommend", uplineId);
                        var spElementA =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIEditAgent");
                        return DbHelper.Execute(spElementA.Name, param, spElementA.DbName);
                    case Globals.AccountType.Member:
                        var spElementMe =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIEditMember");
                        return DbHelper.Execute(spElementMe.Name, param, spElementMe.DbName);
                }
                return null;
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override DbResult GetPTMasterEditMember(int custId, int uplineId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("agentid", uplineId);

                var spElement =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIMasterEditMember");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override DbResult GetPTSuperEditMember(int custId, int uplineId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("agentid", uplineId);
                var spElement =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFISuperEditMember");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        /// <summary>
        /// Update PT
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="accountType"></param>
        /// <param name="uplineId"></param>
        /// <returns></returns>
        public override IbcError UpdatePT(Globals.AccountType accountType, Hashtable param)
        {
            try
            {
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                var key = "";
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        key = "Acc_CustomerUpdSuperFI_All";
                        break;

                    case Globals.AccountType.Master:
                        key = "Acc_CustomerUpdMasterFI_All";
                        break;

                    case Globals.AccountType.Agent:
                        key = "Acc_CustomerUpdAgentFI_All";
                        break;

                    case Globals.AccountType.Member:
                        key = "Acc_CustomerUpdMemberFI_All";
                        break;
                }

                Hashtable output = new Hashtable();
                using (ObjectDb db = new ObjectDb(key))
                {
                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();

                    output = db.GetOuputParams();
                }

                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, output["errcode"].ToString(), output["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override IbcError UpdatePTMasterOnMember(Hashtable param)
        {
            try
            {
                Hashtable output = new Hashtable();
                using (ObjectDb db = new ObjectDb("Acc_CustomerUpdMemberFI_MAg"))
                {
                    param.Add("ip", HttpContext.Current.Request.UserHostAddress);

                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();

                    output = db.GetOuputParams();
                }

                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, output["errcode"].ToString(), output["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override IbcError UpdatePTSuperOnMember(Hashtable param)
        {
            try
            {
                Hashtable output = new Hashtable();

                using (ObjectDb db = new ObjectDb("Acc_CustomerUpdMemberFI_SAg"))
                {
                    param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                    db.AssignParamValues(param);

                    db.ExecuteNonQuery();
                    output = db.GetOuputParams();
                }

                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, output["errcode"].ToString(), output["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override DbResult GetMinMaxPT(int custId, Globals.AccountType accountType)
        {
            //return base.GetPT(custId);
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);

                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        var spElementS = new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFISuperGetMinMaxPt");
                        return DbHelper.Execute(spElementS.Name, param, spElementS.DbName);
                    case Globals.AccountType.Master:
                        var spElementM = new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIMasterGetMinMaxPt");
                        return DbHelper.Execute(spElementM.Name, param, spElementM.DbName);
                    case Globals.AccountType.Agent:
                        var spElementA = new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIAgentGetMinMaxPt");
                        return DbHelper.Execute(spElementA.Name, param, spElementA.DbName);
                }
                return null;
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override ErrorDetail UpdateMinMaxPT(Globals.AccountType accountType, Hashtable param)
        {
            //return base.GetPT(custId);
            try
            {
                Hashtable output = new Hashtable();
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);

                using (ObjectDb db = new ObjectDb("Acc_CustOverrideUpdFI"))
                {
                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();
                    output = db.GetOuputParams();
                }

                var error = new ErrorDetail();
                error.ErrorCode = CastAide.AsInt32(output["errcode"]);
                error.ErrorMessage = CastAide.AsString(output["errmsg"]);

                return error;
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        private class SpConfig : DataAccess
        {
            public StoreProcedureElement GetCustomerStoreProcedure(Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
    }
}