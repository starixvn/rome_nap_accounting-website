using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.Entities;



namespace Sunplus.Accounting.Business.PositionTaking
{
    public partial class HRPositionTakingManager
    {
        #region Private Variable

        private static bool EnabledCopyAll = true;
        private static CustomerDetail custDetail;

        #endregion Private Variable

        #region Get Data

        /// <summary>
        /// Get PT data for edit
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="accountType"></param>
        /// <param name="uplineId"></param>
        /// <returns></returns>
        ///
        public static DbResult GetInfoData(int custId, int roleId)
        {
            try
            {
                return Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerManager.GetAllHRSetting(custId, roleId);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        public static DbResult GetPT(int custId, Globals.AccountType accountType, int uplineId)
        {
            try
            {
                return IBCProvider.PositionTakingHR.GetPT(custId, accountType, uplineId);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        public static DbResult GetMinMaxPT(int custId, Globals.AccountType accountType)
        {
            try
            {
                return IBCProvider.PositionTakingHR.GetMinMaxPT(custId, accountType);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        public static DbResult GetPTMasterEditMember(int custId, int uplineId)
        {
            try
            {
                return IBCProvider.PositionTakingHR.GetPTMasterEditMember(custId, uplineId);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        public static DbResult GetPTSuperEditMember(int custId, int uplineId)
        {
            try
            {
                return IBCProvider.PositionTakingHR.GetPTSuperEditMember(custId, uplineId);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        #endregion Get Data

        #region Update data

        public static IbcError UpdatePT(Globals.AccountType accountType, Hashtable param)
        {
            try
            {
                return IBCProvider.PositionTakingHR.UpdatePT(accountType, param);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new IbcError(-1, ex.Message);
            }
        }

        public static ErrorDetail UpdateMinMaxPT(Globals.AccountType accountType, Hashtable param)
        {
            try
            {
                return IBCProvider.PositionTakingHR.UpdateMinMaxPT(accountType, param);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new ErrorDetail(-1, ex.Message);
            }
        }

        public static IbcError UpdatePTMasterOnMember(Hashtable param)
        {
            try
            {
                return IBCProvider.PositionTakingHR.UpdatePTMasterOnMember(param);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new IbcError(-1, ex.Message);
            }
        }

        public static IbcError UpdatePTSuperOnMember(Hashtable param)
        {
            try
            {
                return IBCProvider.PositionTakingHR.UpdatePTSuperOnMember(param);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new IbcError(-1, ex.Message);
            }
        }

        #endregion Update data

        /// <summary>
        /// Didn't synchronize data to CS system
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="roleId"></param>
        /// <param name="uplineId"></param>
        /// <param name="ptType"></param>
        /// <param name="isInsert"></param>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static string GenHTML(int custId, int roleId, int uplineId, Globals.PTType ptType, bool isInsert,
                                     int errorCode)
        {
            int sporttype = 151; //hardcode for HR
            try
            {
                if (WebSettings.disabledHR)
                    return string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUnAvailable"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "HR"));
                bool enableAutoMpt = true;
                custDetail = new CustomerDetail { CustID = custId, RoleId = roleId, UplineId = uplineId };
                var betSetting = new Dictionary<string, string>();
                var custComm = new Dictionary<string, string>();
                var custPt = new Dictionary<string, double>();

                string tplSetting = WebUtils.LoadTemplate(TPL_HR_SETTING);
                var accountType = (Globals.AccountType)(roleId);
                DbResult data = null;
                switch (ptType)
                {
                    case Globals.PTType.Default:
                        //data = GetInfoData(custId, roleId);
                        data = GetPT(custId, accountType, uplineId);
                        TemplateHandler.Assign(ref tplSetting, "{FuncOnClick}", "UpdatePT();");
                        break;

                    case Globals.PTType.MasterOnMember:
                        data = GetPTMasterEditMember(custId, uplineId);
                        TemplateHandler.Assign(ref tplSetting, "{FuncOnClick}", "UpdatePTHRMasterOnMember();");

                        break;

                    case Globals.PTType.SuperOnMember:
                        data = GetPTSuperEditMember(custId, uplineId);
                        TemplateHandler.Assign(ref tplSetting, "{FuncOnClick}", "UpdatePTHRSuperOnMember();");
                        break;
                }

                if (data == null)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                if (data.ResultSet.Tables.Count < 1)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                if (data.ResultSet.Tables[0].Rows.Count < 1)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                DataTable dt = data.ResultSet.Tables[0];
                if (accountType == Globals.AccountType.Agent || ptType == Globals.PTType.MasterOnMember)
                    enableAutoMpt = !CastAide.AsBoolean(dt.Rows[0]["DisableAutoMPT"]);
                //Get data from database
                //dt = //GenSampleDataTablePT();
                //assign data to object
                List<int> listbettypeTote = GetBetTypeList(dt, Convert.ToChar(Delimiter), HRType.TOTE);
                listbettypeTote.Sort();
                //listbettypeTote.Reverse();

                List<int> listbettypeFixed = GetBetTypeList(dt, Convert.ToChar(Delimiter), HRType.FIXED);
                listbettypeFixed.Sort();
                //listbettypeFixed.Reverse();

                AssignData(sporttype, ref custDetail, ref betSetting, ref custComm, ref custPt, dt);
                //Assign cust info

                TemplateHandler.Assign(ref tplSetting, "{lblCustInfo}", GetRoleName(roleId) + ": " + custDetail.UserName);
                TemplateHandler.Assign(ref tplSetting, "{custinfo}", AssignCustInfo(custDetail, roleId, ptType));
                if (isInsert)
                {
                    TemplateHandler.RemoveBlock(ref tplSetting, "chkdownline");
                }

                //Assign Commission
                AssignCommission(ref tplSetting, data, ptType);

                var ptData = new StringBuilder();
                string tplToteSetting = WebUtils.LoadTemplate(TPL_HR_PT);
                string tplFixedSetting = WebUtils.LoadTemplate(TPL_HR_PT);

                bool allowSATote = AllowSaTote(custDetail.Currency.ToString());
                if (allowSATote)
                {
                    TemplateHandler.Assign(ref tplSetting, "{isDisabledSA}", "0");
                }
                else
                {
                    bettypeMemberPTMaxHR = 41;
                    TemplateHandler.Assign(ref tplSetting, "{isDisabledSA}", "1");
                }
                if (!WebSettings.DisableHRPT_Tote && allowSATote)
                {
                    #region TOTE Setting

                    //title
                    TemplateHandler.Assign(ref tplToteSetting, "{type}", GetHRTypeString(HRType.TOTE));
                    TemplateHandler.Assign(ref tplToteSetting, "{lblType}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTote"));

                    //status
                    if (ptType == Globals.PTType.SuperOnMember || ptType == Globals.PTType.MasterOnMember)
                    {
                        TemplateHandler.RemoveBlock(ref tplToteSetting, "status");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref tplToteSetting, "{HRType}", "HR");
                        TemplateHandler.Assign(ref tplToteSetting, "{lbltype}", GetRoleName(roleId));
                        TemplateHandler.Assign(ref tplToteSetting, "{lbltypeDL}", GetRoleName(roleId) + "'s downlines");
                        if (roleId == 4)
                        {
                            TemplateHandler.Assign(ref tplToteSetting, "{disabledChangeHR}", "");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplToteSetting, "{disabledChangeHR}",
                                                   custDetail.IsDisabledHRUL ? " disabled='disabled' " : " ");
                        }
                        TemplateHandler.Assign(ref tplToteSetting, "{status}",
                                               custDetail.IsDisabledHR ? "" : "checked='checked'");
                        TemplateHandler.Assign(ref tplToteSetting, "{statusDL}",
                                               (custDetail.IsDisabledHR || custDetail.IsDisabledHRDL)
                                                   ? ""
                                                   : "checked='checked'");
                        TemplateHandler.Assign(ref tplToteSetting, "{isDisabledHR}",
                                               custDetail.IsDisabledHR ? "yes" : "no");
                        TemplateHandler.Assign(ref tplToteSetting, "{isDisabledHRDL}",
                                               (custDetail.IsDisabledHR || custDetail.IsDisabledHRDL) ? "yes" : "no");
                        if (roleId == 1 || isInsert)
                        {
                            TemplateHandler.Assign(ref tplToteSetting, "{visibleInsert}", "display:none;");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplToteSetting, "{visibleInsert}", "");
                        }
                    }
                    //End status

                    //// Bet Setting
                    TemplateHandler.Assign(ref tplToteSetting, "{lblBetSetting}", "Bet Setting");
                    TemplateHandler.Assign(ref tplToteSetting, "{BetSetting}",
                                           GenHtmlBetSetting(betSetting, sporttype, roleId, ptType, 31));
                    ////Assign PT
                    TemplateHandler.Assign(ref tplToteSetting, "{lblPositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    switch (accountType)
                    {
                        case Globals.AccountType.Super:
                            TemplateHandler.RemoveBlock(ref tplToteSetting, TAG_copy_preset);
                            TemplateHandler.Assign(ref tplToteSetting, TAG_PT_Data,
                                                   GenHTMLSuperPT(custPt, sporttype, listbettypeTote, HRType.TOTE));
                            break;

                        case Globals.AccountType.Master:
                            GenHTMLMasterPT(ref tplToteSetting, custPt, sporttype, listbettypeTote, HRType.TOTE,
                                            isInsert);
                            break;

                        case Globals.AccountType.Agent:
                            GenHTMLAgentPT(ref tplToteSetting, custPt, sporttype, listbettypeTote, HRType.TOTE, isInsert,
                                           enableAutoMpt);
                            //TemplateHandler.Assign(ref tplToteSetting, TAG_PT_Data, GenHTMLAgentPT(custPT, sporttype, listbettypeTote, HRType.TOTE));
                            break;

                        case Globals.AccountType.Member:
                            TemplateHandler.RemoveBlock(ref tplToteSetting, TAG_copy_preset);
                            TemplateHandler.Assign(ref tplToteSetting, TAG_PT_Data,
                                                   GenHTMLMemberPT(custPt, ptType, sporttype, listbettypeTote,
                                                                   HRType.TOTE, enableAutoMpt));
                            break;
                    }
                    ptData.Append(tplToteSetting);

                    #endregion TOTE Setting
                }

                if (!WebSettings.DisableHRPT_OpenTote)
                {
                    #region FIXED Setting

                    //title
                    TemplateHandler.Assign(ref tplFixedSetting, "{type}", GetHRTypeString(HRType.FIXED));
                    TemplateHandler.Assign(ref tplFixedSetting, "{lblType}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOpenTote"));

                    //status
                    if (ptType == Globals.PTType.SuperOnMember || ptType == Globals.PTType.MasterOnMember)
                    {
                        TemplateHandler.RemoveBlock(ref tplFixedSetting, "status");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref tplFixedSetting, "{HRType}", "OpenTote");
                        TemplateHandler.Assign(ref tplFixedSetting, "{lbltype}", GetRoleName(roleId));
                        TemplateHandler.Assign(ref tplFixedSetting, "{lbltypeDL}", GetRoleName(roleId) + "'s downlines");
                        if (roleId == 4)
                        {
                            TemplateHandler.Assign(ref tplFixedSetting, "{disabledChangeHR}", "");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplFixedSetting, "{disabledChangeHR}",
                                                   custDetail.IsDisabledOpenToteUL ? " disabled='disabled' " : " ");
                        }
                        TemplateHandler.Assign(ref tplFixedSetting, "{status}",
                                               custDetail.IsDisabledOpenTote ? "" : "checked='checked'");
                        TemplateHandler.Assign(ref tplFixedSetting, "{statusDL}",
                                               (custDetail.IsDisabledOpenTote || custDetail.IsDisabledOpenToteDL)
                                                   ? ""
                                                   : "checked='checked'");
                        TemplateHandler.Assign(ref tplFixedSetting, "{isDisabledHR}",
                                               custDetail.IsDisabledOpenTote ? "yes" : "no");
                        TemplateHandler.Assign(ref tplFixedSetting, "{isDisabledHRDL}",
                                               (custDetail.IsDisabledOpenTote || custDetail.IsDisabledOpenToteDL)
                                                   ? "yes"
                                                   : "no");
                        if (roleId == 1 || isInsert)
                        {
                            TemplateHandler.Assign(ref tplFixedSetting, "{visibleInsert}", "display:none;");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplFixedSetting, "{visibleInsert}", "");
                        }
                    }
                    //End status

                    //bet setting
                    TemplateHandler.Assign(ref tplFixedSetting, "{lblBetSetting}", "Bet Setting");
                    TemplateHandler.Assign(ref tplFixedSetting, "{BetSetting}",
                                           GenHtmlBetSetting(betSetting, sporttype, roleId, ptType, 41));

                    ////Assign PT
                    TemplateHandler.Assign(ref tplFixedSetting, "{lblPositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    switch (accountType)
                    {
                        case Globals.AccountType.Super:
                            TemplateHandler.RemoveBlock(ref tplFixedSetting, TAG_copy_preset);
                            TemplateHandler.Assign(ref tplFixedSetting, TAG_PT_Data,
                                                   GenHTMLSuperPT(custPt, sporttype, listbettypeFixed, HRType.FIXED));
                            break;

                        case Globals.AccountType.Master:
                            GenHTMLMasterPT(ref tplFixedSetting, custPt, sporttype, listbettypeFixed, HRType.FIXED,
                                            isInsert);
                            //TemplateHandler.Assign(ref tplFixedSetting, TAG_PT_Data, GenHTMLMasterPT(custPT, sporttype, listbettypeFixed, HRType.FIXED));
                            break;

                        case Globals.AccountType.Agent:
                            GenHTMLAgentPT(ref tplFixedSetting, custPt, sporttype, listbettypeFixed, HRType.FIXED,
                                           isInsert, enableAutoMpt);
                            //TemplateHandler.Assign(ref tplFixedSetting, TAG_PT_Data, GenHTMLAgentPT(custPT, sporttype, listbettypeFixed, HRType.FIXED));
                            break;

                        case Globals.AccountType.Member:
                            TemplateHandler.RemoveBlock(ref tplFixedSetting, TAG_copy_preset);
                            TemplateHandler.Assign(ref tplFixedSetting, TAG_PT_Data,
                                                   GenHTMLMemberPT(custPt, ptType, sporttype, listbettypeFixed,
                                                                   HRType.FIXED, enableAutoMpt));
                            break;
                    }

                    #endregion FIXED Setting

                    ptData.Append(tplFixedSetting);
                }

                TemplateHandler.Assign(ref tplSetting, "pt_data", ptData.ToString());

                //assign Copy All
                if (!WebSettings.DisableHRPT_OpenTote && allowSATote)
                {
                    string copyFromId = sporttype + Delimiter + TAG_CHK_CopyAll + Delimiter + BETTYPE_COPY_ALL;
                    string funcChkCopyAllClick = "ChkCopyAll(this)";
                    TemplateHandler.Assign(ref tplSetting, "copy-all",
                                           Sunplus.Accounting.Core.Web.HtmlHelper.GenInput(Globals.InputType.CheckBox, copyFromId, "", "", "",
                                                           !EnabledCopyAll, false, funcChkCopyAllClick));
                    TemplateHandler.Assign(ref tplSetting, "lblCopyAll",
                                           string.Format(
                                               (accountType == Globals.AccountType.Super)
                                                   ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblHRCopyAllSuper")
                                                   : ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblHRCopyAll"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTote"),
                                               GetBettypeName(BETTYPE_COPY_ALL)));
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref tplSetting, "copy-all");
                }
                //assign common header
                //AssignCommonHeader(ref htmlContent);
                //assign javascript here
                var lsBettype = new List<string> { JS_arrBetType151_Tote, JS_arrBetType151_Fixed };

                string js = AssignJavaScript(ref tplSetting, custDetail);
                js += string.Format(Globals.JS_VariableFormat, "insert", (isInsert) ? "true" : "false");
                js += string.Format(Globals.JS_VariableFormat, JS_arrBetType, GenJSArray(lsBettype));
                js += string.Format(Globals.JS_VariableFormat, JS_Var_bettypeMemberPTMax, bettypeMemberPTMaxHR);
                js = string.Format(Globals.JS_Block, js);

                TemplateHandler.Assign(ref tplSetting, "jsdata", js);
                TemplateHandler.Assign(ref tplSetting, "lblErrMsg",
                                       (errorCode == 0)
                                           ? "<script>alert_error('" +
                                             AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUpdatedSuccessfully").Replace("* ", ""),
                                                                 AlertMessage.MsgType.Success) + "');</script>"
                                           : "");
                return tplSetting;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        public static string GenHTMLMinMax(int custId, int uplineId, int roleId, Globals.PTType ptType, int errorCode)
        {
            try
            {
                int sporttype = 151; //hardcode for HR
                var custDetail = new CustomerDetail { CustID = custId, RoleId = roleId, UplineId = uplineId };
                var betSetting = new Dictionary<string, string>();
                var custComm = new Dictionary<string, string>();
                var custPt = new Dictionary<string, double>();
                var accountType = (Globals.AccountType)(roleId);
                string tplSetting = WebUtils.LoadTemplate(TPL_HR_MINMAX_SETTING);
                DbResult data = GetMinMaxPT(custId, accountType);

                if (data == null)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                if (data.ResultSet.Tables.Count < 1)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                if (data.ResultSet.Tables[0].Rows.Count < 1)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                DataTable dt = data.ResultSet.Tables[0];
                //Get data from database
                //dt = //GenSampleDataTablePT();
                //assign data to object
                List<int> listbettypeTote = GetBetTypeList(dt, Convert.ToChar(Delimiter), HRType.TOTE);
                listbettypeTote.Sort();
                //listbettypeTote.Reverse();

                List<int> listbettypeFixed = GetBetTypeList(dt, Convert.ToChar(Delimiter), HRType.FIXED);
                listbettypeFixed.Sort();
                //listbettypeFixed.Reverse();
                AssignData(sporttype, ref custDetail, ref betSetting, ref custComm, ref custPt, dt);
                //Assign cust info

                TemplateHandler.Assign(ref tplSetting, "{lblCustInfo}", GetRoleName(roleId) + ": " + custDetail.UserName);
                TemplateHandler.Assign(ref tplSetting, "{custinfo}", AssignCustInfo(custDetail, roleId, ptType));

                TemplateHandler.RemoveBlock(ref tplSetting, "UpdatePT");

                bool allowSATote = AllowSaTote(custDetail.Currency.ToString());
                //Assign PT
                var ptData = new StringBuilder();
                string tplToteSetting = WebUtils.LoadTemplate(TPL_HR_PT);
                string tplFixedSetting = WebUtils.LoadTemplate(TPL_HR_PT);

                if (!WebSettings.DisableHRPT_Tote && allowSATote)
                {
                    #region TOTE Setting

                    TemplateHandler.RemoveBlock(ref tplToteSetting, "status");
                    TemplateHandler.RemoveBlock(ref tplToteSetting, "betsetting");

                    TemplateHandler.Assign(ref tplToteSetting, "{type}", GetHRTypeString(HRType.TOTE));
                    TemplateHandler.Assign(ref tplToteSetting, "{lblType}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTote"));
                    ////Assign PT
                    TemplateHandler.Assign(ref tplToteSetting, "{lblPositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    TemplateHandler.Assign(ref tplToteSetting, TAG_PT_Data,
                                           GenMinMaxPT(custPt, sporttype, listbettypeTote, accountType, HRType.TOTE));
                    ptData.Append(tplToteSetting);

                    #endregion TOTE Setting
                }

                if (!WebSettings.DisableHRPT_OpenTote)
                {
                    #region FIXED Setting

                    TemplateHandler.RemoveBlock(ref tplFixedSetting, "status");
                    TemplateHandler.RemoveBlock(ref tplFixedSetting, "betsetting");

                    TemplateHandler.Assign(ref tplFixedSetting, "{type}", GetHRTypeString(HRType.FIXED));
                    TemplateHandler.Assign(ref tplFixedSetting, "{lblType}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOpenTote"));

                    ////Assign PT
                    TemplateHandler.Assign(ref tplFixedSetting, "{lblPositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    TemplateHandler.Assign(ref tplFixedSetting, TAG_PT_Data,
                                           GenMinMaxPT(custPt, sporttype, listbettypeFixed, accountType, HRType.FIXED));

                    #endregion FIXED Setting

                    ptData.Append(tplFixedSetting);
                }

                TemplateHandler.Assign(ref tplSetting, "pt_data", ptData.ToString());
                TemplateHandler.RemoveBlock(ref tplSetting, TAG_copy_preset);
                //assign Copy All
                if (!WebSettings.DisableHRPT_OpenTote && allowSATote)
                {
                    string copyFromId = sporttype + Delimiter + TAG_CHK_CopyAll + Delimiter + BETTYPE_COPY_ALL;
                    string funcChkCopyAllClick = "ChkCopyAll(this)";
                    TemplateHandler.Assign(ref tplSetting, "copy-all",
                                           HtmlHelper.GenInput(Globals.InputType.CheckBox, copyFromId, "", "", "", false,
                                                           false, funcChkCopyAllClick));
                    TemplateHandler.Assign(ref tplSetting, "lblCopyAll",
                                           string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblHRCopyAll"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTote"),
                                                         GetBettypeName(BETTYPE_COPY_ALL)));
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref tplSetting, "copy-all");
                }
                //assign common header
                //AssignCommonHeader(ref htmlContent);
                //assign javascript here
                var lsBettype = new List<string> { JS_arrBetType151_Tote, JS_arrBetType151_Fixed };

                //assign common header
                //AssignCommonHeader(ref htmlContent);
                //assign javascript here
                string js = AssignJavaScript(ref tplSetting, custDetail);
                //js += string.Format(Globals.JS_VariableFormat, "insert", (isInsert) ? "true" : "false");
                js += string.Format(Globals.JS_VariableFormat, JS_arrBetType, GenJSArray(lsBettype));
                js = string.Format(Globals.JS_Block, js);

                TemplateHandler.Assign(ref tplSetting, "jsdata", js);
                TemplateHandler.Assign(ref tplSetting, "lblErrMsg",
                                       (errorCode == 0)
                                           ? "<script>alert_error('" +
                                             AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUpdatedSuccessfully").Replace("* ", ""),
                                                                 AlertMessage.MsgType.Success) + "');</script>"
                                           : "");
                return tplSetting;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        private static string GenHTMLSuperPT(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype,
                                             HRType hrType)
        {
            try
            {
                if (lsBettype.Count < 1)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                ;
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_HR_PT_SUPER);
                string tplPTValue = WebUtils.LoadTemplate(TPL_HR_PT_Value);
                string tplIBCPTValue = WebUtils.LoadTemplate(TPL_HR_IBCPT_Value);
                //Remove unuse row
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_superautopt);
                //Assign Label
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_IBCPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAutoPositionTaking"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IBC")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperPT, string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_MasterPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_AgentPT, string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));

                var dataHeader = new StringBuilder();
                var dataIbcpt = new StringBuilder();
                var dataSpt = new StringBuilder();
                var dataMpt = new StringBuilder();
                var dataApt = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);

                double ibcptMax = maxIBCPT, sptMax = maxPTHR, mptMax = maxPTHR, aptMax = maxPTHR;
                double ibcptMin = minPT, sptMin = minPT, mptMin = minPT, aptMin = minPT;
                double ibcptCurrent = minPT, sptCurrent = minPT, mptCurrent = minPT, aptCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype + Delimiter + GetHRTypeString(hrType);
                //arrBettype151_tote [fixed]
                string arrBettypeData = "";

                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueIBCPT = tplIBCPTValue;
                    string valueSPT = tplPTValue;
                    string valueMPT = tplPTValue;
                    string valueAPT = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, GetBettypeName(bettype));
                    dataHeader.Append(valueHeader);
                    s4 = maxPTHR;
                    l4 = maxPTHR;
                    n4 = datapt[PREFIX_SPORT + sporttype + SPT + Delimiter + bettype + Delimiter + "min"];
                    ;

                    l4 = datapt[PREFIX_SPORT + sporttype + SPT + Delimiter + bettype + Delimiter + "max"];
                    if (totalBettype == 0)
                    {
                        ibcptMax = datapt[PREFIX_SPORT + sporttype + IBCPT + Delimiter + bettype + Delimiter + "max"];
                        ibcptCurrent = datapt[PREFIX_SPORT + sporttype + IBCPT + Delimiter + bettype];
                    }

                    sptMax = Math.Min(l4, s4);
                    sptMin = n4;
                    sptCurrent = datapt[PREFIX_SPORT + sporttype + SPT + Delimiter + bettype]; //s4;

                    mptMax = sptCurrent;
                    mptMin = minPT;
                    mptCurrent = datapt[PREFIX_SPORT + sporttype + MPT + Delimiter + bettype];

                    aptMax = mptCurrent;
                    aptMin = minPT;
                    aptCurrent = datapt[PREFIX_SPORT + sporttype + APT + Delimiter + bettype];
                    //SPT
                    string ibcptId = sporttype + Delimiter + bettype + Delimiter + IBCPT;
                    string sptId = sporttype + Delimiter + bettype + Delimiter + SPT;
                    string mptId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string aptId = sporttype + Delimiter + bettype + Delimiter + APT;
                    string ibcptValue = HtmlHelper.GenSelectOption(ibcptMax, ibcptMin, ibcptCurrent, Globals.INTERVAL_PT_HR,
                                                               (bettype == 32 || bettype == 42), ibcptId, funcOnChange,
                                                               CLASS_Select_PT);
                    string sptValue = HtmlHelper.GenSelectOption(sptMax, sptMin, sptCurrent, Globals.INTERVAL_PT_HR, false,
                                                             sptId, funcOnChange, CLASS_Select_PT);
                    string mptValue = HtmlHelper.GenSelectOption(mptMax, mptMin, mptCurrent, Globals.INTERVAL_PT_HR, false,
                                                             mptId, funcOnChange, CLASS_Select_PT);
                    string aptValue = HtmlHelper.GenSelectOption(aptMax, aptMin, aptCurrent, Globals.INTERVAL_PT_HR, false,
                                                             aptId, funcOnChange, CLASS_Select_PT);
                    if (totalBettype == 0)
                    {
                        ibcptValue += HtmlHelper.GenChangePTButtonsHR(ibcptId, false);
                        sptValue += HtmlHelper.GenChangePTButtonsHR(sptId, false);
                        mptValue += HtmlHelper.GenChangePTButtonsHR(mptId, false);
                        aptValue += HtmlHelper.GenChangePTButtonsHR(aptId, false);
                        copyFromBettype = GetBettypeName(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }
                    if (IsIBCPTBettype(bettype))
                    {
                        TemplateHandler.Assign(ref valueIBCPT, TAG_Value, ibcptValue);
                        TemplateHandler.Assign(ref valueIBCPT, "colspan", lsBettype.Count);
                        dataIbcpt.Append(valueIBCPT);
                    }

                    TemplateHandler.Assign(ref valueSPT, TAG_Value, sptValue);
                    dataSpt.Append(valueSPT);
                    TemplateHandler.Assign(ref valueMPT, TAG_Value, mptValue);
                    dataMpt.Append(valueMPT);
                    TemplateHandler.Assign(ref valueAPT, TAG_Value, aptValue);
                    dataApt.Append(valueAPT);

                    totalBettype++;
                    if (totalBettype == lsBettype.Count)
                    {
                        if (totalBettype == 1)
                        {
                            TemplateHandler.RemoveBlock(ref tplPTData, TAG_CopyPT);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplPTData, TAG_CHK_Copy,
                                                   HtmlHelper.GenInput(Globals.InputType.CheckBox, copyFromId, "", "", "",
                                                                   false, false, funcChkCopyClick));
                            TemplateHandler.Assign(ref tplPTData, TAG_LBL_CopyFrom,
                                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyFrom"), copyFromBettype));
                            TemplateHandler.Assign(ref tplPTData, TAG_Copy_Colspan, lsBettype.Count);
                        }
                    }
                }
                TemplateHandler.Assign(ref tplPTData, TAG_PT_HEADER_Value, dataHeader.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_ibcpt_value, dataIbcpt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superpt_value, dataSpt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_masterpt_value, dataMpt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_agentpt_value, dataApt.ToString());

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                arrPTType = "'" + IBCPT + "'," + "'" + SPT + "','" + MPT + "','" + APT + "'";
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = string.Format(Globals.JS_arrayFormat, arrPTType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js = string.Format(Globals.JS_Block, js);

                TemplateHandler.Assign(ref tplPTData, TAG_PT_JS, js);
                return tplPTData;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        //master
        /// <summary>
        /// Edit Master PT
        /// </summary>
        /// <param name="_cust"></param>
        /// <returns></returns>
        private static void GenHTMLMasterPT(ref string tplSetting, Dictionary<string, double> datapt, int sporttype,
                                            List<int> lsBettype, HRType hrType, bool isInsert)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_HR_PTDATA);
                string tplPTValue = WebUtils.LoadTemplate(TPL_HR_PT_Value);
                //Remove unuse row
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_agentpt);
                //Assign Label
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPresetPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_MasterPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPresetPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperAutoPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAutoPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));

                var dataHeader = new StringBuilder();
                var dataSpt = new StringBuilder();
                var dataMPT = new StringBuilder();
                var dataSAutoPT = new StringBuilder();
                var dataIsSAutoPT = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);
                string funcChkSAutoClick = string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);

                double sptMax = maxPTHR, mptMax = maxPTHR, sautoMax = maxPTHR;
                double sptMin = minPT, mptMin = minPT, sautoMin = minPT;
                double sptCurrent = minPT, mptCurrent = minPT, sautoCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype + Delimiter + GetHRTypeString(hrType);
                string arrBettypeData = "";
                var lsPTData = new List<PositionTakingDetail>();
                string js = "";
                bool isUseAuto = false;
                bool isCopyPreset = false;
                bool isDisabledCopyPreset = false;
                int intTotalUsedAuto = 0;
                int intTotalSPTZero = 0;
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueSpt = tplPTValue;
                    string valueMpt = tplPTValue;
                    string valueSAutoPt = tplPTValue;
                    string valueIsAutoPt = tplPTValue;
                    bool isAuto = CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + bettype]);
                    if (isAuto)
                        intTotalUsedAuto++;

                    TemplateHandler.Assign(ref valueHeader, TAG_Value, GetBettypeName(bettype));
                    dataHeader.Append(valueHeader);

                    sptMax = Math.Min(
                        CastAide.AsDouble(datapt[prefix + SPT + Delimiter + bettype + Delimiter + MAXPT]), maxPTHR);
                    sptMin = 0;
                    sptCurrent = datapt[prefix + SPT + Delimiter + bettype];
                    //count total bettype got zero value
                    if (sptCurrent == 0)
                        intTotalSPTZero++;

                    mptMax = Math.Min(
                        CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype + Delimiter + MAXPT]), maxPTHR);
                    mptMax = Math.Min(mptMax, sptMax);
                    //assign max MPT to javascript
                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MAXPT,
                                                 mptMax));
                    mptMax = (isAuto) ? mptMax : (Math.Min(mptMax, sptMax - sptCurrent));
                    mptMin = CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype + Delimiter + MINPT]);
                    mptCurrent = datapt[prefix + MPT + Delimiter + bettype];
                    //Math.Min(datapt[prefix + MPT + Delimiter + bettype], mptMax);

                    sautoMax =
                        CastAide.AsDouble(
                            datapt[Auto + Delimiter + prefix + SPT + Delimiter + bettype + Delimiter + MAXPT]);
                    sautoMin = 0;
                    sautoCurrent = CastAide.AsDouble(datapt[Auto + Delimiter + prefix + SPT + Delimiter + bettype]);

                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + MAXPT,
                                                 sptMax));
                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MINPT,
                                                 mptMin));

                    if (bettype == bettypeMemberPTMaxHR)
                        lsPTData.Add(new PositionTakingDetail(JS_Var_msPTMax, sptMax));

                    isUseAuto = (isUseAuto == false) ? isAuto : isUseAuto;

                    string sptId = sporttype + Delimiter + bettype + Delimiter + SPT;
                    string mptId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string sAutoId = sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + Auto;
                    string isAutoId = sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + STR_IsAuto;
                    lsPTData.Add(new PositionTakingDetail(isAutoId, (isAuto) ? 1 : 0));
                    string sptValue = HtmlHelper.GenSelectOption(sptMax - mptMin, sptMin, sptCurrent, Globals.INTERVAL_PT_HR,
                                                             isAuto, sptId, funcOnChange, CLASS_Select_PT);
                    string mptValue = HtmlHelper.GenSelectOption(mptMax, mptMin, mptCurrent, Globals.INTERVAL_PT_HR, false,
                                                             mptId, "", CLASS_Select_PT);
                    string sAutoValue = HtmlHelper.GenSelectOption(sautoMax, sautoMin, sautoCurrent, Globals.INTERVAL_PT_HR,
                                                               !isAuto, sAutoId, "", CLASS_Select_PT);
                    string isAutoValue = HtmlHelper.GenInput(Globals.InputType.CheckBox, isAutoId, "", "", false, isAuto,
                                                         funcChkSAutoClick);
                    if (totalBettype == 0)
                    {
                        sptValue += HtmlHelper.GenChangePTButtonsHR(sptId, isAuto, true);
                        mptValue += HtmlHelper.GenChangePTButtonsHR(mptId, false);
                        sAutoValue += HtmlHelper.GenChangePTButtonsHR(sAutoId, !isAuto, true);
                        copyFromBettype = GetBettypeName(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }

                    TemplateHandler.Assign(ref valueSpt, TAG_Value, sptValue);
                    dataSpt.Append(valueSpt);
                    TemplateHandler.Assign(ref valueMpt, TAG_Value, mptValue);
                    dataMPT.Append(valueMpt);
                    TemplateHandler.Assign(ref valueSAutoPt, TAG_Value, sAutoValue);
                    dataSAutoPT.Append(valueSAutoPt);
                    TemplateHandler.Assign(ref valueIsAutoPt, TAG_Value, isAutoValue);
                    dataIsSAutoPT.Append(valueIsAutoPt);

                    totalBettype++;
                    if (totalBettype == lsBettype.Count)
                    {
                        if (totalBettype == 1)
                        {
                            TemplateHandler.RemoveBlock(ref tplPTData, TAG_CopyPT);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplPTData, TAG_CHK_Copy,
                                                   HtmlHelper.GenInput(Globals.InputType.CheckBox, copyFromId, "", "", "",
                                                                   false, false, funcChkCopyClick));
                            TemplateHandler.Assign(ref tplPTData, TAG_LBL_CopyFrom,
                                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyFrom"), copyFromBettype));
                            TemplateHandler.Assign(ref tplPTData, TAG_Copy_Colspan, lsBettype.Count);
                        }
                    }
                }
                TemplateHandler.Assign(ref tplPTData, TAG_PT_HEADER_Value, dataHeader.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superpt_value, dataSpt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_masterpt_value, dataMPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superautopt_value, dataSAutoPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_chksauto_value, dataIsSAutoPT.ToString());

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPtType = new List<string> { SPT, MPT, SPT + Delimiter + Auto, SPT + Delimiter + STR_IsAuto };
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenJSArrayPT(lsPtType);
                js += string.Format(Globals.JS_VariableFormat, JS_Var_IsUseAuto, (isUseAuto) ? "1" : "0");
                js += string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js += AssignJSPTData(lsPTData);
                js = string.Format(Globals.JS_Block, js);

                TemplateHandler.Assign(ref tplPTData, TAG_PT_JS, js);

                isCopyPreset = (intTotalSPTZero == lsBettype.Count && !isUseAuto) ? true : false;
                //isDisabledCopyPreset = (intTotalUsedAuto == lsBettype.Count) ? true : false;
                if (isInsert)
                {
                    TemplateHandler.RemoveBlock(ref tplSetting, TAG_copy_preset);
                }
                else
                {
                    //TemplateHandler
                    string strpttype = "";
                    if (hrType == HRType.TOTE)
                        strpttype = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTote") + " - Win";
                    else
                        strpttype = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOpenTote") + " - Win";

                    TemplateHandler.Assign(ref tplSetting, TAG_CHK_CopyPreset,
                                           HtmlHelper.GenCheckBox(
                                               GetCopyPresetBettype(hrType) + Delimiter + IS_COPY_PRESET,
                                               isDisabledCopyPreset, isCopyPreset, ""));
                    TemplateHandler.Assign(ref tplSetting, TAG_LBL_CopyPreset,
                                           string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyPreset"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super"), strpttype));
                }
                TemplateHandler.Assign(ref tplSetting, TAG_PT_Data, tplPTData);
                //return tplPTData;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        //agent
        /// <summary>
        /// Edit Agent PT
        /// </summary>
        /// <returns></returns>
        private static void GenHTMLAgentPT(ref string tplSetting, Dictionary<string, double> datapt, int sporttype,
                                           List<int> lsBettype, HRType hrType, bool isInsert, bool enableAutoMPT)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_HR_PTDATA);
                string tplPTValue = WebUtils.LoadTemplate(TPL_HR_PT_Value);
                //Remove unuse row
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_superpt);
                if (!Permissions.IsEnabledAutoMaster(enableAutoMPT))
                    TemplateHandler.RemoveBlock(ref tplPTData, TAG_superautopt);

                //Assign Label

                TemplateHandler.Assign(ref tplPTData, TAG_LBL_MasterPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPresetPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_AgentPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperAutoPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAutoPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));

                var dataHeader = new StringBuilder();
                var dataAPT = new StringBuilder();
                var dataMPT = new StringBuilder();
                var dataMAutoPT = new StringBuilder();
                var dataIsMAutoPT = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);
                string funcChkMAutoClick = string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);

                double mptMax = maxPTHR, aptMax = maxPTHR, mautoptMax = maxPTHR;
                double aptMin = minPT, mptMin = minPT, mautoptMin = minPT;
                double aptCurrent = minPT, mptCurrent = minPT, mautoptCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype + Delimiter + GetHRTypeString(hrType);
                string arrBettypeData = "";
                bool isCopyPreset = false;
                bool isDisabledCopyPreset = false;
                int intTotalMPTZero = 0;
                int intTotalUsedAuto = 0;

                bool isUseAuto = false;
                var lsPtData = new List<PositionTakingDetail>();
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueMpt = tplPTValue;
                    string valueApt = tplPTValue;
                    string valueMAutoPt = tplPTValue;
                    string valueIsMAutoPt = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, GetBettypeName(bettype));
                    dataHeader.Append(valueHeader);

                    bool isAuto = Permissions.IsEnabledAutoMaster(enableAutoMPT)
                                      ? CastAide.AsBoolean(
                                          datapt[STR_IsAuto + Delimiter + prefix + MPT + Delimiter + bettype])
                                      : false;
                    //CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + bettype]);
                    if (isAuto)
                        intTotalUsedAuto++;

                    mptMax = Math.Min(
                        CastAide.AsDouble(datapt[MAXPT + Delimiter + prefix + MaPT + Delimiter + bettype]), maxPTHR);
                    mptMin = minPT;
                    mptCurrent = CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype]);

                    //auto
                    if (Permissions.IsEnabledAutoMaster(enableAutoMPT))
                    {
                        mautoptMax =
                            CastAide.AsDouble(
                                datapt[Auto + Delimiter + prefix + MPT + Delimiter + bettype + Delimiter + MAXPT]);
                        mautoptMin = 0;
                        mautoptCurrent = CastAide.AsDouble(datapt[Auto + Delimiter + prefix + MPT + Delimiter + bettype]);
                    }

                    if (mptCurrent == 0)
                        intTotalMPTZero++;

                    aptMax = CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype + Delimiter + MAXPT]);
                    aptMax = Math.Min(aptMax, mptMax);
                    lsPtData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + APT + Delimiter + MAXPT,
                                                 aptMax));
                    aptMax = Math.Min(aptMax, mptMax - mptCurrent);
                    aptMin = CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype + Delimiter + MINPT]);
                    aptCurrent = datapt[prefix + APT + Delimiter + bettype];
                    //Math.Min(CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype]), aptMax);

                    lsPtData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MAXPT,
                                                 mptMax));
                    lsPtData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + APT + Delimiter + MINPT,
                                                 aptMin));

                    if (bettype == bettypeMemberPTMaxHR)
                        lsPtData.Add(new PositionTakingDetail(JS_Var_mmPTMax, mptMax));

                    isUseAuto = (isUseAuto == false) ? isAuto : isUseAuto;

                    string aptId = sporttype + Delimiter + bettype + Delimiter + APT;
                    string mptId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string mAutoId = sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + Auto;
                    string isAutoId = sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + STR_IsAuto;
                    lsPtData.Add(new PositionTakingDetail(isAutoId, (isAuto) ? 1 : 0));

                    string aptValue = HtmlHelper.GenSelectOption(aptMax, aptMin, aptCurrent, Globals.INTERVAL_PT_HR, false,
                                                             aptId, "", CLASS_Select_PT);
                    string mptValue = HtmlHelper.GenSelectOption(mptMax - aptMin, mptMin, mptCurrent, Globals.INTERVAL_PT_HR,
                                                             isAuto, mptId, funcOnChange, CLASS_Select_PT);
                    string mAutoValue = HtmlHelper.GenSelectOption(mautoptMax, mautoptMin, mautoptCurrent,
                                                               Globals.INTERVAL_PT_HR, !isAuto, mAutoId, "",
                                                               CLASS_Select_PT);
                    string isAutoValue = HtmlHelper.GenInput(Globals.InputType.CheckBox, isAutoId, "", "", false, isAuto,
                                                         funcChkMAutoClick);
                    if (totalBettype == 0)
                    {
                        aptValue += HtmlHelper.GenChangePTButtonsHR(aptId, false);
                        mptValue += HtmlHelper.GenChangePTButtonsHR(mptId, isAuto, true);
                        mAutoValue += HtmlHelper.GenChangePTButtonsHR(mAutoId, !isAuto, true);
                        copyFromBettype = GetBettypeName(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }

                    TemplateHandler.Assign(ref valueMpt, TAG_Value, mptValue);
                    dataMPT.Append(valueMpt);
                    TemplateHandler.Assign(ref valueApt, TAG_Value, aptValue);
                    dataAPT.Append(valueApt);
                    TemplateHandler.Assign(ref valueMAutoPt, TAG_Value, mAutoValue);
                    dataMAutoPT.Append(valueMAutoPt);
                    TemplateHandler.Assign(ref valueIsMAutoPt, TAG_Value, isAutoValue);
                    dataIsMAutoPT.Append(valueIsMAutoPt);

                    totalBettype++;
                    if (totalBettype == lsBettype.Count)
                    {
                        if (totalBettype == 1)
                        {
                            TemplateHandler.RemoveBlock(ref tplPTData, TAG_CopyPT);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplPTData, TAG_CHK_Copy,
                                                   HtmlHelper.GenInput(Globals.InputType.CheckBox, copyFromId, "", "", "",
                                                                   false, false, funcChkCopyClick));
                            TemplateHandler.Assign(ref tplPTData, TAG_LBL_CopyFrom,
                                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyFrom"), copyFromBettype));
                            TemplateHandler.Assign(ref tplPTData, TAG_Copy_Colspan, lsBettype.Count);
                        }
                    }
                }
                TemplateHandler.Assign(ref tplPTData, TAG_PT_HEADER_Value, dataHeader.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_agentpt_value, dataAPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_masterpt_value, dataMPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superautopt_value, dataMAutoPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_chksauto_value, dataIsMAutoPT.ToString());

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPtType = new List<string> { APT, MPT, MPT + Delimiter + Auto, MPT + Delimiter + STR_IsAuto };
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenJSArrayPT(lsPtType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_Var_IsUseAuto, (isUseAuto) ? "1" : "0");
                js += string.Format(Globals.JS_VariableFormat, JS_Var_IsAutoMaster,
                                    Permissions.IsEnabledAutoMaster(enableAutoMPT) ? "true" : "false");
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js += AssignJSPTData(lsPtData);
                js = string.Format(Globals.JS_Block, js);

                TemplateHandler.Assign(ref tplPTData, TAG_PT_JS, js);

                isCopyPreset = (intTotalMPTZero == lsBettype.Count && !isUseAuto) ? true : false;
                if (isInsert)
                {
                    TemplateHandler.RemoveBlock(ref tplSetting, TAG_copy_preset);
                }
                else
                {
                    string strpttype = "";
                    if (hrType == HRType.TOTE)
                        strpttype = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTote") + " - Win";
                    else
                        strpttype = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOpenTote") + " - Win";
                    TemplateHandler.Assign(ref tplSetting, TAG_CHK_CopyPreset,
                                           HtmlHelper.GenCheckBox(
                                               GetCopyPresetBettype(hrType) + Delimiter + IS_COPY_PRESET,
                                               isDisabledCopyPreset, isCopyPreset, ""));
                    TemplateHandler.Assign(ref tplSetting, TAG_LBL_CopyPreset,
                                           string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyPreset"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"), strpttype));
                }
                TemplateHandler.Assign(ref tplSetting, TAG_PT_Data, tplPTData);
                //return tplPTData;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        //member
        /// <summary>
        /// This IBC Customer wasn't synchronized to Casino System
        /// </summary>
        /// <returns></returns>
        private static string GenHTMLMemberPT(Dictionary<string, double> datapt, Globals.PTType editType, int sporttype,
                                              List<int> lsBettype, HRType hrType, bool enableAutoMPT)
        {
            try
            {
                switch (editType)
                {
                    case Globals.PTType.SuperOnMember:
                        return GenHTMLSuperOnMember(datapt, sporttype, lsBettype, hrType);
                    case Globals.PTType.MasterOnMember:
                        return GenHTMLMasterOnMember(datapt, sporttype, lsBettype, hrType, enableAutoMPT);
                    default:
                        return GenHTMLAgentOnMember(datapt, sporttype, lsBettype, hrType);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Edit super PT on Member
        /// </summary>
        /// <param name="tplPT"></param>
        /// <param name="_cust"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        private static string GenHTMLSuperOnMember(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype,
                                                   HRType hrType)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_HR_PTDATA);
                string tplPTValue = WebUtils.LoadTemplate(TPL_HR_PT_Value);
                //Assign Label
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_AgentPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super"));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_MasterPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperAutoPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAuto"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));

                var dataHeader = new StringBuilder();
                var dataSpt = new StringBuilder();
                var dataApt = new StringBuilder();
                var dataMpt = new StringBuilder();
                var dataSAutoPt = new StringBuilder();
                var dataIsSAutoPt = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);
                string funcChkSAutoClick = ""; // string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);

                double sptMax = maxPTHR, mptMax = maxPTHR, sautoMax = maxPTHR;
                double sptMin = minPT, mptMin = minPT, sautoMin = minPT;
                double sptCurrent = minPT, mptCurrent = minPT, sautoCurrent = minPT, aptCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype + Delimiter + GetHRTypeString(hrType);
                string arrBettypeData = "";
                var lsPtData = new List<PositionTakingDetail>();
                bool enabledCopy = true;
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueSPT = tplPTValue;
                    string valueMPT = tplPTValue;
                    string valueAPT = tplPTValue;
                    string valueSAutoPT = tplPTValue;
                    string valueIsAutoPT = tplPTValue;
                    bool isAutoMem =
                        CastAide.AsBoolean(
                            datapt[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + MEM + Delimiter + bettype]);
                    bool isAutoMaster =
                        CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + bettype]);
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, GetBettypeName(bettype));
                    dataHeader.Append(valueHeader);

                    sptMax = Math.Min(CastAide.AsDouble(datapt[MAXPT + prefix + "pos" + Delimiter + bettype]), maxPTHR);
                    sptMin = Math.Max(CastAide.AsDouble(datapt[MINPT + prefix + "pos" + Delimiter + bettype]), minPT);
                    sptCurrent = isAutoMaster
                                     ? datapt[prefix + SPT + Delimiter + bettype]
                                     : datapt[prefix + SPT + Delimiter + bettype];

                    //mptMax = Math.Min(CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype + Delimiter + MAXPT]), maxPTHR);
                    //assign max MPT to javascript
                    //lsPTData.Add(new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MAXPT, mptMax));
                    //mptMax = (isAuto) ? mptMax : (Math.Min(mptMax, sptMax - sptCurrent));
                    //mptMin = CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype + Delimiter + MINPT]);

                    sautoMax = CastAide.AsDouble(datapt[MAXPT + Auto + prefix + "pos" + Delimiter + bettype]);
                    sautoMin = 0;
                    sautoCurrent = CastAide.AsDouble(datapt[Auto + Delimiter + prefix + SPT + Delimiter + bettype]);

                    mptCurrent = datapt[prefix + MPT + Delimiter + bettype];
                    aptCurrent = datapt[prefix + APT + Delimiter + bettype];

                    lsPtData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + MAXPT,
                                                 sptMax));
                    lsPtData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MINPT,
                                                 mptMin));
                    lsPtData.Add(
                        new PositionTakingDetail(
                            sporttype + Delimiter + bettype + Delimiter + STR_IsAuto + Delimiter + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"),
                            isAutoMaster.ToString().ToLower()));

                    string sptId = sporttype + Delimiter + bettype + Delimiter + SPT;
                    string mptId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string aptId = sporttype + Delimiter + bettype + Delimiter + APT;
                    string sAutoId = sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + Auto;
                    string isAutoId = sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + STR_IsAuto;
                    string sptValue = isAutoMaster
                                          ? HtmlHelper.GenLabel(sptId, sptCurrent.ToString(), "")
                                          : HtmlHelper.GenSelectOption(sptMax, sptMin, sptCurrent, Globals.INTERVAL_PT_HR,
                                                                   isAutoMem, sptId, funcOnChange, CLASS_Select_PT);
                    string mptValue = HtmlHelper.GenLabel(mptId, mptCurrent.ToString(), "");
                    string aptValue = HtmlHelper.GenLabel(aptId, aptCurrent.ToString(), "");
                    string sAutoValue = HtmlHelper.GenSelectOption(sautoMax, sautoMin, sautoCurrent, Globals.INTERVAL_PT_HR,
                                                               !isAutoMem, sAutoId, "", CLASS_Select_PT);

                    if (totalBettype == 0)
                    {
                        sptValue += isAutoMaster ? string.Empty : HtmlHelper.GenChangePTButtonsHR(sptId, isAutoMem, true);
                        sAutoValue += HtmlHelper.GenChangePTButtonsHR(sAutoId, !isAutoMem, true);
                        copyFromBettype = GetBettypeName(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                        enabledCopy = isAutoMaster ? (isAutoMem ? true : false) : true;
                        if (bettype == BETTYPE_COPY_ALL)
                            EnabledCopyAll = isAutoMaster ? (isAutoMem ? true : false) : true;

                        funcChkSAutoClick = string.Format(FUNC_ChkAutoForCopyBettype, PARAMS_ChkAuto);
                    }
                    else
                    {
                        funcChkSAutoClick = string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);
                    }
                    string isAutoValue = HtmlHelper.GenInput(Globals.InputType.CheckBox, isAutoId, "", "", false, isAutoMem,
                                                         funcChkSAutoClick);

                    TemplateHandler.Assign(ref valueSPT, TAG_Value, sptValue);
                    dataSpt.Append(valueSPT);
                    TemplateHandler.Assign(ref valueMPT, TAG_Value, mptValue);
                    dataMpt.Append(valueMPT);
                    TemplateHandler.Assign(ref valueAPT, TAG_Value, aptValue);
                    dataApt.Append(valueAPT);
                    TemplateHandler.Assign(ref valueSAutoPT, TAG_Value, sAutoValue);
                    dataSAutoPt.Append(valueSAutoPT);
                    TemplateHandler.Assign(ref valueIsAutoPT, TAG_Value, isAutoValue);
                    dataIsSAutoPt.Append(valueIsAutoPT);

                    totalBettype++;
                    if (totalBettype == lsBettype.Count)
                    {
                        if (totalBettype == 1)
                        {
                            TemplateHandler.RemoveBlock(ref tplPTData, TAG_CopyPT);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplPTData, TAG_CHK_Copy,
                                                   HtmlHelper.GenInput(Globals.InputType.CheckBox, copyFromId, "", "", "",
                                                                   !enabledCopy, false, funcChkCopyClick));
                            TemplateHandler.Assign(ref tplPTData, TAG_LBL_CopyFrom,
                                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyFrom"), copyFromBettype));
                            TemplateHandler.Assign(ref tplPTData, TAG_Copy_Colspan, lsBettype.Count);
                        }
                    }
                }
                TemplateHandler.Assign(ref tplPTData, TAG_PT_HEADER_Value, dataHeader.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superpt_value, dataSpt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_masterpt_value, dataMpt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_agentpt_value, dataApt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superautopt_value, dataSAutoPt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_chksauto_value, dataIsSAutoPt.ToString());

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPtType = new List<string> { SPT, SPT + Delimiter + Auto, SPT + Delimiter + STR_IsAuto };
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenJSArrayPT(lsPtType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js += AssignJSPTData(lsPtData);
                js = string.Format(Globals.JS_Block, js);

                TemplateHandler.Assign(ref tplPTData, TAG_PT_JS, js);
                return tplPTData;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Master PT on member
        /// </summary>
        /// <param name="tplPT"></param>
        /// <param name="_cust"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        public static string GenHTMLMasterOnMember(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype,
                                                   HRType hrType, bool enableAutoMPT)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_HR_PTDATA);
                string tplPTValue = WebUtils.LoadTemplate(TPL_HR_PT_Value);
                //Remove unuse row
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_superpt);
                if (!Permissions.IsEnabledAutoMaster(enableAutoMPT))
                    TemplateHandler.RemoveBlock(ref tplPTData, TAG_superautopt);

                //Assign Label

                TemplateHandler.Assign(ref tplPTData, TAG_LBL_MasterPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_AgentPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperAutoPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAuto"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));

                var dataHeader = new StringBuilder();
                var dataApt = new StringBuilder();
                var dataMpt = new StringBuilder();
                var dataMAutoPt = new StringBuilder();
                var dataIsMAutoPt = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);
                string funcChkMAutoClick = ""; //string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);

                double mptMax = maxPTHR, aptMax = maxPTHR, mautoptMax = maxPTHR;
                double aptMin = minPT, mptMin = minPT, mautoptMin = minPT;
                double aptCurrent = minPT, mptCurrent = minPT, mautoptCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype + Delimiter + GetHRTypeString(hrType);
                string arrBettypeData = "";
                var lsPtData = new List<PositionTakingDetail>();
                bool enabledCopy = true;
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";

                    //Header
                    string valueHeader = tplPTValue;
                    string valueMpt = tplPTValue;
                    string valueApt = tplPTValue;
                    string valueMAutoPt = tplPTValue;
                    string valueIsMAutoPt = tplPTValue;
                    bool isAutoMem = Permissions.IsEnabledAutoMaster(enableAutoMPT)
                                         ? CastAide.AsBoolean(
                                             datapt[
                                                 STR_IsAuto + Delimiter + prefix + MPT + Delimiter + MEM + Delimiter +
                                                 bettype])
                                         : false;
                    //CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + bettype]);
                    bool isAutoAgent = Permissions.IsEnabledAutoMaster(enableAutoMPT)
                                           ? CastAide.AsBoolean(
                                               datapt[STR_IsAuto + Delimiter + prefix + MPT + Delimiter + bettype])
                                           : false;
                    //CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + bettype]);
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, GetBettypeName(bettype));
                    dataHeader.Append(valueHeader);

                    mptMax = CastAide.AsDouble(datapt[MAXPT + prefix + "pos" + Delimiter + bettype]);
                    mptMax = Math.Max(0, mptMax);
                    mptMin = CastAide.AsDouble(datapt[MINPT + prefix + "pos" + Delimiter + bettype]);
                    ;
                    mptMin = Math.Max(0, mptMin);
                    mptCurrent = isAutoAgent
                                     ? CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype])
                                     : CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype]);
                    mptCurrent = Math.Max(mptCurrent, 0);
                    if (Permissions.IsEnabledAutoMaster(enableAutoMPT))
                    {
                        mautoptMax =
                            CastAide.AsDouble(
                                datapt[MAXPT + Auto + Delimiter + prefix + MPT + Delimiter + MEM + Delimiter + bettype]);
                        mautoptMin = 0;
                        mautoptCurrent =
                            CastAide.AsDouble(
                                datapt[Auto + Delimiter + prefix + MPT + Delimiter + MEM + Delimiter + bettype]);
                    }

                    aptCurrent = CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype]); //s4;
                    lsPtData.Add(
                        new PositionTakingDetail(
                            sporttype + Delimiter + bettype + Delimiter + STR_IsAuto + Delimiter + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"),
                            isAutoAgent.ToString().ToLower()));

                    string aptId = sporttype + Delimiter + bettype + Delimiter + APT;
                    string mptId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string mAutoId = sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + Auto;
                    string isAutoId = sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + STR_IsAuto;
                    //string _APTValue = Helper.GenSelectOption(aptMax, aptMin, aptCurrent, Globals.INTERVAL_PT_HR, false, _APTId, funcOnChange, CLASS_Select_PT);

                    string aptValue = HtmlHelper.GenLabel(aptId, aptCurrent.ToString(), "");
                    string mptValue = isAutoAgent
                                          ? HtmlHelper.GenLabel(mptId, mptCurrent.ToString(), "")
                                          : HtmlHelper.GenSelectOption(mptMax, mptMin, mptCurrent, Globals.INTERVAL_PT_HR,
                                                                   isAutoMem, mptId, funcOnChange, CLASS_Select_PT);
                    string mAutoValue = HtmlHelper.GenSelectOption(mautoptMax, mautoptMin, mautoptCurrent,
                                                               Globals.INTERVAL_PT_HR, !isAutoMem, mAutoId, "",
                                                               CLASS_Select_PT);

                    if (totalBettype == 0)
                    {
                        //_APTValue += Helper.GenChangePTButtonsHR(_APTId, false);
                        mptValue += isAutoAgent ? string.Empty : HtmlHelper.GenChangePTButtonsHR(mptId, isAutoMem, true);
                        mAutoValue += HtmlHelper.GenChangePTButtonsHR(mAutoId, !isAutoMem, true);

                        copyFromBettype = GetBettypeName(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                        enabledCopy = isAutoAgent ? (isAutoMem ? true : false) : true;
                        if (bettype == BETTYPE_COPY_ALL)
                            EnabledCopyAll = isAutoAgent ? (isAutoMem ? true : false) : true;

                        funcChkMAutoClick = string.Format(FUNC_ChkAutoForCopyBettype, PARAMS_ChkAuto);
                    }
                    else
                    {
                        funcChkMAutoClick = string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);
                    }
                    string isAutoValue = HtmlHelper.GenInput(Globals.InputType.CheckBox, isAutoId, "", "", false, isAutoMem,
                                                         funcChkMAutoClick);

                    TemplateHandler.Assign(ref valueMpt, TAG_Value, mptValue);
                    dataMpt.Append(valueMpt);
                    TemplateHandler.Assign(ref valueApt, TAG_Value, aptValue);
                    dataApt.Append(valueApt);
                    TemplateHandler.Assign(ref valueMAutoPt, TAG_Value, mAutoValue);
                    dataMAutoPt.Append(valueMAutoPt);
                    TemplateHandler.Assign(ref valueIsMAutoPt, TAG_Value, isAutoValue);
                    dataIsMAutoPt.Append(valueIsMAutoPt);
                    totalBettype++;
                    if (totalBettype == lsBettype.Count)
                    {
                        if (totalBettype == 1)
                        {
                            TemplateHandler.RemoveBlock(ref tplPTData, TAG_CopyPT);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplPTData, TAG_CHK_Copy,
                                                   HtmlHelper.GenInput(Globals.InputType.CheckBox, copyFromId, "", "", "",
                                                                   !enabledCopy, false, funcChkCopyClick));
                            TemplateHandler.Assign(ref tplPTData, TAG_LBL_CopyFrom,
                                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyFrom"), copyFromBettype));
                            TemplateHandler.Assign(ref tplPTData, TAG_Copy_Colspan, lsBettype.Count);
                        }
                    }
                }
                TemplateHandler.Assign(ref tplPTData, TAG_PT_HEADER_Value, dataHeader.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_agentpt_value, dataApt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_masterpt_value, dataMpt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superautopt_value, dataMAutoPt.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_chksauto_value, dataIsMAutoPt.ToString());
                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPtType = new List<string> { APT, MPT, MPT + Delimiter + Auto, MPT + Delimiter + STR_IsAuto };
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenJSArrayPT(lsPtType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js += string.Format(Globals.JS_VariableFormat, JS_Var_IsAutoMaster,
                                    Permissions.IsEnabledAutoMaster(enableAutoMPT) ? "true" : "false");
                js += AssignJSPTData(lsPtData);
                js = string.Format(Globals.JS_Block, js);

                TemplateHandler.Assign(ref tplPTData, TAG_PT_JS, js);
                return tplPTData;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="datapt"></param>
        /// <param name="sporttype"></param>
        /// <param name="lsBettype"></param>
        /// <param name="hrType"></param>
        /// <returns></returns>
        private static string GenHTMLAgentOnMember(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype,
                                                   HRType hrType)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_HR_PTDATA);
                string tplPTValue = WebUtils.LoadTemplate(TPL_HR_PT_Value);
                //Remove unuse row
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_superpt);
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_superautopt);
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_masterpt);
                EnabledCopyAll = true;
                //Assign Label

                TemplateHandler.Assign(ref tplPTData, TAG_LBL_AgentPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_MasterPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));

                var dataHeader = new StringBuilder();
                var dataApt = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);
                string funcChkSAutoClick = string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);

                double aptMin = minPT;
                double aptCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype + Delimiter + GetHRTypeString(hrType);
                string arrBettypeData = "";
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueMPT = tplPTValue;
                    string valueAPT = tplPTValue;

                    TemplateHandler.Assign(ref valueHeader, TAG_Value, GetBettypeName(bettype));
                    dataHeader.Append(valueHeader);

                    double aptMax = Math.Min(maxPTHR,
                                             CastAide.AsDouble(datapt[MAXPT + prefix + "pos" + Delimiter + bettype]));
                    aptMin = CastAide.AsDouble(datapt[MINPT + prefix + "pos" + Delimiter + bettype]);
                    ;
                    aptCurrent = CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype]);

                    string aptId = sporttype + Delimiter + bettype + Delimiter + APT;

                    string aptValue = HtmlHelper.GenSelectOption(aptMax, aptMin, aptCurrent, Globals.INTERVAL_PT_HR, false,
                                                             aptId, funcOnChange, CLASS_Select_PT);
                    if (totalBettype == 0)
                    {
                        aptValue += HtmlHelper.GenChangePTButtonsHR(aptId, false);
                        copyFromBettype = GetBettypeName(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }

                    TemplateHandler.Assign(ref valueAPT, TAG_Value, aptValue);
                    dataApt.Append(valueAPT);

                    totalBettype++;
                    if (totalBettype == lsBettype.Count)
                    {
                        if (totalBettype == 1)
                        {
                            TemplateHandler.RemoveBlock(ref tplPTData, TAG_CopyPT);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplPTData, TAG_CHK_Copy,
                                                   HtmlHelper.GenInput(Globals.InputType.CheckBox, copyFromId, "", "", "",
                                                                   false, false, funcChkCopyClick));
                            TemplateHandler.Assign(ref tplPTData, TAG_LBL_CopyFrom,
                                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyFrom"), copyFromBettype));
                            TemplateHandler.Assign(ref tplPTData, TAG_Copy_Colspan, lsBettype.Count);
                        }
                    }
                }
                TemplateHandler.Assign(ref tplPTData, TAG_PT_HEADER_Value, dataHeader.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_agentpt_value, dataApt.ToString());

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPtType = new List<string> { APT, MPT };
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenJSArrayPT(lsPtType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js = string.Format(Globals.JS_Block, js);

                TemplateHandler.Assign(ref tplPTData, TAG_PT_JS, js);
                return tplPTData;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        private static void AssignCommonHeader(ref string tplHTML)
        {
            try
            {
                TemplateHandler.Assign(ref tplHTML, "lblCustomerInfomation", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCustomerInfomation"));
                TemplateHandler.Assign(ref tplHTML, "lblBetSetting", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));
                TemplateHandler.Assign(ref tplHTML, "lblPositionTaking", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        private static string GenMinMaxPT(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype,
                                          Globals.AccountType accountType, HRType hrType)
        {
            try
            {
                if (lsBettype.Count < 1)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplMinMaxPT = WebUtils.LoadTemplate(TPL_HR_MINMAX_PT);
                string tplPTValue = WebUtils.LoadTemplate(TPL_HR_PT_Value);
                //Remove unuse row
                if (accountType == Globals.AccountType.Super)
                    TemplateHandler.RemoveBlock(ref tplMinMaxPT, TAG_maxpt);
                //Assign Label
                TemplateHandler.Assign(ref tplMinMaxPT, TAG_LBL_MaxPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPositionTaking"));
                TemplateHandler.Assign(ref tplMinMaxPT, TAG_LBL_MinPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMinPositionTaking"));

                var dataHeader = new StringBuilder();
                var dataMaxPT = new StringBuilder();
                var dataMinPT = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);

                double maxPTMax = maxPTHR, minPTMax = minPT;
                double maxPTMin = minPT, minPTMin = minPT;
                double currentPTMax = minPT, currentPTMin = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype + Delimiter + GetHRTypeString(hrType);
                string arrBettypeData = "";
                string PTType = "";
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        PTType = SPT;
                        break;

                    case Globals.AccountType.Master:
                        PTType = MPT;
                        break;

                    case Globals.AccountType.Agent:
                        PTType = APT;
                        break;
                }
                var lsPtData = new List<PositionTakingDetail>();
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueMaxPt = tplPTValue;
                    string valueMpt = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, GetBettypeName(bettype));
                    dataHeader.Append(valueHeader);

                    maxPTMax = maxPTHR;
                    if (DataUtils.IsExistKey(datapt,
                                             PREFIX_SPORT + sporttype + PTType + Delimiter + bettype + Delimiter + "max"))
                        currentPTMax =
                            datapt[PREFIX_SPORT + sporttype + PTType + Delimiter + bettype + Delimiter + "max"];
                    currentPTMax = (accountType == Globals.AccountType.Super) ? maxPTHR : currentPTMax; //s4;

                    maxPTMin = Math.Min(currentPTMax, datapt[PREFIX_SPORT + sporttype + PTType + Delimiter + bettype]);
                    currentPTMin = datapt[PREFIX_SPORT + sporttype + PTType + Delimiter + bettype + Delimiter + "min"];

                    lsPtData.Add(
                        new PositionTakingDetail(
                            sporttype + Delimiter + bettype + Delimiter + MINPT + Delimiter + MAXPT, maxPTMin));
                    minPTMax = currentPTMin;
                    //SPT
                    string maxPtId = sporttype + Delimiter + bettype + Delimiter + "max";
                    string minPtId = sporttype + Delimiter + bettype + Delimiter + "min";
                    string maxPtValue = HtmlHelper.GenSelectOption(maxPTMax, minPTMax, currentPTMax, Globals.INTERVAL_PT_HR,
                                                               false, maxPtId, funcOnChange, CLASS_Select_PT);
                    string minPtValue = HtmlHelper.GenSelectOption(maxPTMin, minPTMin, currentPTMin, Globals.INTERVAL_PT_HR,
                                                               false, minPtId, funcOnChange, CLASS_Select_PT);
                    if (totalBettype == 0)
                    {
                        maxPtValue += HtmlHelper.GenChangePTButtonsHR(maxPtId, false);
                        minPtValue += HtmlHelper.GenChangePTButtonsHR(minPtId, false);
                        copyFromBettype = GetBettypeName(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }

                    TemplateHandler.Assign(ref valueMaxPt, TAG_Value, maxPtValue);
                    dataMaxPT.Append(valueMaxPt);
                    TemplateHandler.Assign(ref valueMpt, TAG_Value, minPtValue);
                    dataMinPT.Append(valueMpt);

                    totalBettype++;
                    if (totalBettype == lsBettype.Count)
                    {
                        if (totalBettype == 1)
                        {
                            TemplateHandler.RemoveBlock(ref tplMinMaxPT, TAG_CopyPT);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref tplMinMaxPT, TAG_CHK_Copy,
                                                   HtmlHelper.GenInput(Globals.InputType.CheckBox, copyFromId, "", "", "",
                                                                   false, false, funcChkCopyClick));
                            TemplateHandler.Assign(ref tplMinMaxPT, TAG_LBL_CopyFrom,
                                                   string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyFrom"), copyFromBettype));
                            TemplateHandler.Assign(ref tplMinMaxPT, TAG_Copy_Colspan, lsBettype.Count);
                        }
                    }
                }
                TemplateHandler.Assign(ref tplMinMaxPT, TAG_PT_HEADER_Value, dataHeader.ToString());
                TemplateHandler.Assign(ref tplMinMaxPT, TAG_maxpt_value, dataMaxPT.ToString());
                TemplateHandler.Assign(ref tplMinMaxPT, TAG_minpt_value, dataMinPT.ToString());

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPtType = new List<string> { MAXPT, MINPT };
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenJSArrayPT(lsPtType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                //js += string.Format(Globals.JS_VariableFormat, JS_Var_disableMinPTHR, (WebSettings.DisableMinPTHR) ? "true" : "false");
                js += AssignJSPTData(lsPtData);
                js = string.Format(Globals.JS_Block, js);

                TemplateHandler.Assign(ref tplMinMaxPT, TAG_PT_JS, js);
                return tplMinMaxPT;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        //End Member
        private static void AssignCommission(ref string tplSetting, DbResult data, Globals.PTType ptType)
        {
            try
            {
                if (data == null)
                {
                    TemplateHandler.Assign(ref tplSetting, TAG_Commission,
                                           AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error));
                    return;
                }
                if (data.ResultSet.Tables.Count < 1)
                {
                    TemplateHandler.Assign(ref tplSetting, TAG_Commission,
                                           AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error));
                    return;
                }
                if (data.ResultSet.Tables[0].Rows.Count < 1)
                {
                    TemplateHandler.Assign(ref tplSetting, TAG_Commission,
                                           AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error));
                    return;
                }
                DataRow row = data.ResultSet.Tables[0].Rows[0];
                string tplCommission = "";

                string commId = "151" + Delimiter; // +TAG_Commission;
                if (ptType == Globals.PTType.Default || ptType == Globals.PTType.AgentOnMember)
                {
                    int colspan = 0;
                    tplCommission = WebUtils.LoadTemplate(TPL_HR_COMM);
                    TemplateHandler.Assign(ref tplCommission, TAG_LBL_Commission, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission"));

                    if (AllowSaTote(custDetail.Currency.ToString()))
                    {
                        double commToTeMax = 0.0;
                        double commTote = 0.0;
                        commToTeMax = CastAide.AsDouble(row["v151discount_31_max"]);
                        commTote = CastAide.AsDouble(row["v151discount_31"]);
                        //Math.Min(CastAide.AsDouble(row["v151discount_31"]), commToTeMax);
                        TemplateHandler.Assign(ref tplCommission, TAG_LBL_Tote, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblToteComm"));
                        TemplateHandler.Assign(ref tplCommission, TAG_Value_Tote,
                                               HtmlHelper.GenSelectOption(commToTeMax, 0, commTote, COMM_HR_INTERVAL, false,
                                                                      commId + "31" + Delimiter + TAG_Commission, "",
                                                                      "select-group"));
                        colspan += 2;
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref tplCommission, "tote");
                    }

                    if (WebSettings.DisableHRPT_OpenTote)
                    {
                        TemplateHandler.RemoveBlock(ref tplCommission, "fixed");
                    }
                    else
                    {
                        double commFixedMax = 0.0;
                        double commFixed = 0.0;
                        commFixedMax = CastAide.AsDouble(row["v151discount_41_max"]);
                        commFixed = CastAide.AsDouble(row["v151discount_41"]);
                        TemplateHandler.Assign(ref tplCommission, TAG_LBL_Fixed, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOpenToteComm"));
                        TemplateHandler.Assign(ref tplCommission, TAG_Value_Fixed,
                                               HtmlHelper.GenSelectOption(commFixedMax, 0, commFixed, COMM_HR_INTERVAL,
                                                                      false, commId + "41" + Delimiter + TAG_Commission,
                                                                      "", "select-group"));
                        colspan += 2;
                    }
                    TemplateHandler.Assign(ref tplCommission, "{colspan}", colspan.ToString());
                }
                else if (ptType == Globals.PTType.SuperOnMember)
                {
                    tplCommission = WebUtils.LoadTemplate(TPL_HR_COMM_MEMBER);
                    TemplateHandler.Assign(ref tplCommission, TAG_LBL_Commission, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission"));
                    double mcommTote = CastAide.AsDouble(row["v151discount_31_Mag"]);
                    double acommTote = CastAide.AsDouble(row["v151discount_31_Ag"]);
                    double playercommTote = CastAide.AsDouble(row["v151discount_31"]);
                    ;
                    double mcommFixed = CastAide.AsDouble(row["v151discount_41_Mag"]);
                    double acommFixed = CastAide.AsDouble(row["v151discount_41_Ag"]);
                    double playercommFixed = CastAide.AsDouble(row["v151discount_41"]);

                    //New HR Commission
                    string tplPTData = WebUtils.LoadTemplate(TPL_HR_COMM_MEMBER_DATA);
                    string tplPTValue = WebUtils.LoadTemplate(TPL_HR_COMM_MEMBER_VALUE);

                    var dataHeader = new StringBuilder();
                    var dataTote = new StringBuilder();
                    var dataFixed = new StringBuilder();

                    string valueHeader = tplPTValue;
                    string valueBetType = tplPTValue;

                    valueHeader = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, "{value}",
                                           HtmlHelper.GenLabel("", String.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommFor"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")), ""));
                    dataHeader.Append(valueHeader);

                    valueHeader = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, "{value}",
                                           HtmlHelper.GenLabel("", String.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommFor"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")), ""));
                    dataHeader.Append(valueHeader);

                    valueHeader = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, "{value}",
                                           HtmlHelper.GenLabel("", String.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommFor"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Member")), ""));
                    dataHeader.Append(valueHeader);

                    // Tote
                    ////Master
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}", HtmlHelper.GenLabel("", mcommTote.ToString(), ""));
                    dataTote.Append(valueBetType);
                    ////Agent
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}", HtmlHelper.GenLabel("", acommTote.ToString(), ""));
                    dataTote.Append(valueBetType);
                    ////Agent
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}",
                                           HtmlHelper.GenLabel("", playercommTote.ToString(), ""));
                    dataTote.Append(valueBetType);

                    // Fixed
                    ////Master
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}", HtmlHelper.GenLabel("", mcommFixed.ToString(), ""));
                    dataFixed.Append(valueBetType);
                    ////Agent
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}", HtmlHelper.GenLabel("", acommFixed.ToString(), ""));
                    dataFixed.Append(valueBetType);
                    ////Agent
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}",
                                           HtmlHelper.GenLabel("", playercommFixed.ToString(), ""));
                    dataFixed.Append(valueBetType);

                    TemplateHandler.Assign(ref tplPTData, "{header_value}", dataHeader.ToString());
                    TemplateHandler.Assign(ref tplPTData, "{tote_value}", dataTote.ToString());
                    TemplateHandler.Assign(ref tplPTData, "{fixed_value}", dataFixed.ToString());

                    TemplateHandler.Assign(ref tplPTData, "{lblTote}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTote"));
                    TemplateHandler.Assign(ref tplPTData, "{lblFixed}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOpenTote"));

                    if (WebSettings.DisableHRPT_OpenTote)
                    {
                        TemplateHandler.RemoveBlock(ref tplPTData, "Fixed");
                    }
                    if (!AllowSaTote(custDetail.Currency.ToString()))
                    {
                        TemplateHandler.RemoveBlock(ref tplPTData, "Tote");
                    }

                    TemplateHandler.Assign(ref tplCommission, TAG_Commission, tplPTData);
                    //End New Commission
                }
                else
                {
                    tplCommission = WebUtils.LoadTemplate(TPL_HR_COMM_MEMBER);
                    TemplateHandler.Assign(ref tplCommission, TAG_LBL_Commission, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission"));

                    double acommTote = CastAide.AsDouble(row["v151discount_31_ag"]);
                    double playercommTote = CastAide.AsDouble(row["v151discount_31"]);
                    double acommFixed = CastAide.AsDouble(row["v151discount_41_ag"]);
                    double playercommFixed = CastAide.AsDouble(row["v151discount_41"]);
                    ;

                    //New HR Commission
                    string tplPTData = WebUtils.LoadTemplate(TPL_HR_COMM_MEMBER_DATA);
                    string tplPTValue = WebUtils.LoadTemplate(TPL_HR_COMM_MEMBER_VALUE);

                    var dataHeader = new StringBuilder();
                    var dataTote = new StringBuilder();
                    var dataFixed = new StringBuilder();

                    string valueHeader = tplPTValue;
                    string valueBetType = tplPTValue;

                    valueHeader = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, "{value}",
                                           HtmlHelper.GenLabel("", String.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommFor"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")), ""));
                    dataHeader.Append(valueHeader);

                    valueHeader = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, "{value}",
                                           HtmlHelper.GenLabel("", String.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCommFor"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Member")), ""));
                    dataHeader.Append(valueHeader);

                    // Tote
                    ////Agent
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}", HtmlHelper.GenLabel("", acommTote.ToString(), ""));
                    dataTote.Append(valueBetType);
                    ////Agent
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}",
                                           HtmlHelper.GenLabel("", playercommTote.ToString(), ""));
                    dataTote.Append(valueBetType);

                    // Fixed
                    ////Agent
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}", HtmlHelper.GenLabel("", acommFixed.ToString(), ""));
                    dataFixed.Append(valueBetType);
                    ////Agent
                    valueBetType = tplPTValue;
                    TemplateHandler.Assign(ref valueBetType, "{value}",
                                           HtmlHelper.GenLabel("", playercommFixed.ToString(), ""));
                    dataFixed.Append(valueBetType);

                    TemplateHandler.Assign(ref tplPTData, "{header_value}", dataHeader.ToString());
                    TemplateHandler.Assign(ref tplPTData, "{tote_value}", dataTote.ToString());
                    TemplateHandler.Assign(ref tplPTData, "{fixed_value}", dataFixed.ToString());

                    TemplateHandler.Assign(ref tplPTData, "{lblTote}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTote"));
                    TemplateHandler.Assign(ref tplPTData, "{lblFixed}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOpenTote"));

                    if (WebSettings.DisableHRPT_OpenTote)
                    {
                        TemplateHandler.RemoveBlock(ref tplPTData, "Fixed");
                    }
                    if (!AllowSaTote(custDetail.Currency.ToString()))
                    {
                        TemplateHandler.RemoveBlock(ref tplPTData, "Tote");
                    }
                    TemplateHandler.Assign(ref tplCommission, TAG_Commission, tplPTData);
                    //End New Commission
                }

                TemplateHandler.Assign(ref tplSetting, TAG_Commission, tplCommission);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="listbetsetting"></param>
        /// <param name="sporttype"></param>
        /// <param name="roleid"></param>
        /// <param name="pttype"></param>
        /// <returns></returns>
        private static string GenHtmlBetSetting(Dictionary<string, string> listbetsetting, int sporttype, int roleid,
                                                Globals.PTType pttype, int bettype)
        {
            string betsetting = "";
            try
            {
                if (pttype == Globals.PTType.Default || pttype == Globals.PTType.AgentOnMember)
                {
                    betsetting = WebUtils.LoadTemplate(TPL_HR_BetSetting);
                    TemplateHandler.Assign(ref betsetting, "{minbetvalue}", listbetsetting[MINBET + "_" + bettype]);
                    TemplateHandler.Assign(ref betsetting, "{maxbetvalue}", listbetsetting[MAXBET + "_" + bettype]);
                    TemplateHandler.Assign(ref betsetting, "{maxpermatchvalue}",
                                           listbetsetting[MaxPerMatch + "_" + bettype]);

                    try
                    {
                        TemplateHandler.Assign(ref betsetting, "{minbet-min}",
                                               listbetsetting[MINBET + "_min" + "_" + bettype]);
                        TemplateHandler.Assign(ref betsetting, "{maxbet-max}",
                                               listbetsetting[MAXBET + "_max" + "_" + bettype]);
                        TemplateHandler.Assign(ref betsetting, "{maxpermatch-max}",
                                               listbetsetting[MaxPerMatch + "_max" + "_" + bettype]);
                        try
                        {
                            TemplateHandler.Assign(ref betsetting, "{maxpermatch-min}",
                                                   listbetsetting[MaxPerMatch + "_min" + "_" + bettype]);
                        }
                        catch
                        {
                        }

                        if (listbetsetting[MINBET + "_min" + "_" + bettype] == "0")
                        {
                            TemplateHandler.Assign(ref betsetting, "{hide1}", "hide");
                        }
                        if (listbetsetting[MAXBET + "_max" + "_" + bettype] == "0")
                        {
                            TemplateHandler.Assign(ref betsetting, "{hide2}", "hide");
                        }
                        if (listbetsetting[MaxPerMatch + "_max" + "_" + bettype] == "0")
                        {
                            TemplateHandler.Assign(ref betsetting, "{hide3}", "hide");
                        }
                    }
                    catch
                    {
                    }

                    TemplateHandler.Assign(ref betsetting, "{hide1}", roleid == 4 ? "hide" : "");
                    TemplateHandler.Assign(ref betsetting, "{hide2}", roleid == 4 ? "hide" : "");
                    TemplateHandler.Assign(ref betsetting, "{hide3}", roleid == 4 ? "hide" : "");
                    TemplateHandler.Assign(ref betsetting, "{bettype}", bettype.ToString());
                    return betsetting;
                }

                betsetting = WebUtils.LoadTemplate(TPL_HR_SUPERMASTER_BetSetting);
                TemplateHandler.Assign(ref betsetting, "{minbetvalue}", listbetsetting[MINBET + "_" + bettype]);
                TemplateHandler.Assign(ref betsetting, "{maxbetvalue}", listbetsetting[MAXBET + "_" + bettype]);
                TemplateHandler.Assign(ref betsetting, "{maxpermatchvalue}", listbetsetting[MaxPerMatch + "_" + bettype]);
                TemplateHandler.Assign(ref betsetting, "{bettype}", bettype.ToString());
                return betsetting;
                //throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }
    }
}