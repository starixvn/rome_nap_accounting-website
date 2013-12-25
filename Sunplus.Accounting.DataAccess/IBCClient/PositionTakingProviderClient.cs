// --------------------------------------------------------------------------
// <copyright file="PositionTakingProviderClient.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.DataAccess.IBCClient
{
    using System;
    using System.Collections;
    using System.Web;
    using DNAide.Core;
    using DNAide.Data;
    using Fanex.Data;
    using Fanex.Tarus.Infrastructure.Extensions;
    using IBCUtils;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Configurations;
    using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
    using Sunplus.Accounting.Core.Helper;
    using Sunplus.Accounting.Entities;

    /// <summary>
    /// Position Taking Provider Client.
    /// </summary>
    public class PositionTakingProviderClient : PositionTakingProvider
    {
        /// <summary>
        /// Gets the PT.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="uplineId">The up line identifier.</param>
        /// <returns>The PT result.</returns>
        public override DbResult GetPT(int custId, Globals.AccountType accountType, int uplineId)
        {
            try
            {
                DbResult databaseResult;
                var param = new Hashtable();
                param.Add("custid", custId);
                var storeProcedureName = string.Empty;
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        storeProcedureName = "CustomerSelFIEditSuper";
                        break;
                    case Globals.AccountType.Master:
                        param.Add("srecommend", uplineId);
                        storeProcedureName = "CustomerSelFIEditMaster";
                        break;
                    case Globals.AccountType.Agent:
                        param.Add("mrecommend", uplineId);
                        storeProcedureName = "CustomerSelFIEditAgent";
                        break;
                    case Globals.AccountType.Member:
                        storeProcedureName = "CustomerSelFIEditMember";
                        break;
                    default:
                        storeProcedureName = string.Empty;
                        break;
                }

                if (string.IsNullOrEmpty(storeProcedureName))
                {
                    return null;
                }

                using (ObjectDb objectDb = new ObjectDb(storeProcedureName))
                {
                    objectDb.AssignParamValues(param);
                    var ds = objectDb.ExecuteDataSet();
                    var output = objectDb.GetOuputParams();
                    databaseResult = new DbResult(output, ds);
                }

                return databaseResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the PT master edit member.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="uplineId">The up line identifier.</param>
        /// <returns>The PT master edit member.</returns>
        public override DbResult GetPTMasterEditMember(int custId, int uplineId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("agentid", uplineId);

                DbResult databaseResult;
                using (ObjectDb objectDb = new ObjectDb("CustomerSelFIMasterEditMember"))
                {
                    objectDb.AssignParamValues(param);
                    var ds = objectDb.ExecuteDataSet();
                    var output = objectDb.GetOuputParams();
                    databaseResult = new DbResult(output, ds);
                }

                return databaseResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the PT super edit member.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="uplineId">The up line identifier.</param>
        /// <returns>The PT super edit member.</returns>
        public override DbResult GetPTSuperEditMember(int custId, int uplineId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("agentid", uplineId);
                DbResult databaseResult;
                using (ObjectDb objectDb = new ObjectDb("CustomerSelFISuperEditMember"))
                {
                    objectDb.AssignParamValues(param);
                    var ds = objectDb.ExecuteDataSet();
                    var output = objectDb.GetOuputParams();
                    databaseResult = new DbResult(output, ds);
                }

                return databaseResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Update PT.
        /// </summary>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Update result.</returns>
        public override IbcError UpdatePT(Globals.AccountType accountType, Hashtable param)
        {
            try
            {
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                var storeProcedureName = string.Empty;
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        storeProcedureName = "Acc_CustomerUpdSuperFI_All";
                        break;
                    case Globals.AccountType.Master:
                        storeProcedureName = "Acc_CustomerUpdMasterFI_All";
                        break;
                    case Globals.AccountType.Agent:
                        storeProcedureName = "Acc_CustomerUpdAgentFI_All";
                        break;
                    case Globals.AccountType.Member:
                        storeProcedureName = "Acc_CustomerUpdMemberFI_All";
                        break;
                    default:
                        storeProcedureName = string.Empty;
                        break;
                }

                Hashtable output = new Hashtable();
                using (ObjectDb objectDb = new ObjectDb(storeProcedureName))
                {
                    objectDb.AssignParamValues(param);
                    objectDb.ExecuteNonQuery();
                    output = objectDb.GetOuputParams();
                }

                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, output["errcode"].ToString(), output["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the PT master on member.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Update result.</returns>
        public override IbcError UpdatePTMasterOnMember(Hashtable param)
        {
            try
            {
                Hashtable output = new Hashtable();
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                using (ObjectDb objectDb = new ObjectDb("Acc_CustomerUpdMemberFI_MAg"))
                {
                    objectDb.AssignParamValues(param);
                    objectDb.ExecuteNonQuery();
                    output = objectDb.GetOuputParams();
                }

                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, output["errcode"].ToString(), output["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the PT super on member.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>Update result.</returns>
        public override IbcError UpdatePTSuperOnMember(Hashtable param)
        {
            try
            {
                Hashtable output = new Hashtable();

                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                using (ObjectDb objectDb = new ObjectDb("Acc_CustomerUpdMemberFI_SAg"))
                {
                    objectDb.AssignParamValues(param);
                    objectDb.ExecuteNonQuery();
                    output = objectDb.GetOuputParams();
                }

                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, output["errcode"].ToString(), output["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Gets the minimum maximum PT.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <returns>The minimum maximum PT.</returns>
        public override DbResult GetMinMaxPT(int custId, Globals.AccountType accountType)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var storeProcedureName = string.Empty;
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        storeProcedureName = "CustomerSelFISuperGetMinMaxPt";
                        break;
                    case Globals.AccountType.Master:
                        storeProcedureName = "CustomerSelFIMasterGetMinMaxPt";
                        break;
                    case Globals.AccountType.Agent:
                        storeProcedureName = "CustomerSelFIAgentGetMinMaxPt";
                        break;
                    default:
                        storeProcedureName = string.Empty;
                        break;
                }

                if (string.IsNullOrEmpty(storeProcedureName))
                {
                    return null;
                }

                DbResult databaseResult;
                using (ObjectDb objectDb = new ObjectDb(storeProcedureName))
                {
                    objectDb.AssignParamValues(param);
                    var ds = objectDb.ExecuteDataSet();
                    var output = objectDb.GetOuputParams();
                    databaseResult = new DbResult(output, ds);
                }

                return databaseResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// Updates the minimum maximum PT.
        /// </summary>
        /// <param name="accountType">Type of the account.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>Update result.</returns>
        public override ErrorDetail UpdateMinMaxPT(Globals.AccountType accountType, Hashtable param)
        {
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
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        /// <summary>
        /// SP Config.
        /// </summary>
        private class SpConfig : DataAccess
        {
            public StoreProcedureElement GetCustomerStoreProcedure(
                SportbookPtSession.StoreCollection storeCollection,
                string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
    }
}