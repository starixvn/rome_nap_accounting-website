using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.Entities;

//using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for FinanceManager
/// </summary>

namespace Sunplus.Accounting.Business.PositionTaking
{
    public class PositionTakingManager
    {
        #region CONST variables

        //path to template
        public static readonly string TPL_FINANCE_SETTING = "Customer/v2/FinanceSetting.htm";

        public static readonly string TPL_FINANCE_PT = "Customer/v2/FinancePTData.htm";
        public static readonly string TPL_FINANCE_PT_SUPER = "Customer/v2/FinancePTDataSuper.htm";
        public static readonly string TPL_FINANCE_PT_HEADER = "Customer/v2/FinancePTHeader.htm";
        public static readonly string TPL_FINANCE_CustInfo = "Customer/v2/FinanceInfo.htm";
        public static readonly string TPL_FINANCE_BetSetting = "Customer/v2/FinanceBetSetting.htm";
        public static readonly string TPL_FINANCE_PT_Value = "Customer/v2/FinancePTValue.htm";
        public static readonly string TPL_FINANCE_COMM = "Customer/v2/FinanceCommission.htm";

        public static readonly string TPL_FINANCE_SUPERMASTER_BetSetting =
            "Customer/v2/FinanceBetSettingSuperMasterPT.htm";

        public static readonly string TPL_FINANCE_MINMAX_SETTING = "Customer/v2/FinanceMinMaxSetting.htm";
        public static readonly string TPL_FINANCE_MINMAX_PT = "Customer/v2/FinanceMinMaxPT.htm";

        //Tag name on template
        public static readonly string TAG_maxpt = "maxpt";

        public static readonly string TAG_minpt = "minpt";
        public static readonly string TAG_maxpt_value = "maxpt_value";
        public static readonly string TAG_minpt_value = "minpt_value";

        public static readonly string TAG_Commission = "commission";
        public static readonly string TAG_superpt = "superpt";
        public static readonly string TAG_superpt_value = "superpt_value";
        public static readonly string TAG_superautopt = "superautopt";
        public static readonly string TAG_superautopt_value = "superautopt_value";
        public static readonly string TAG_chksauto_value = "chksauto";
        public static readonly string TAG_masterpt = "masterpt";
        public static readonly string TAG_masterpt_value = "masterpt_value";
        public static readonly string TAG_agentpt = "agentpt";
        public static readonly string TAG_agentpt_value = "agentpt_value";
        public static readonly string TAG_PT_Data = "pt_data";
        public static readonly string TAG_PT_HEADER = "header";
        public static readonly string TAG_PT_HEADER_Value = "header_value";
        public static readonly string TAG_CopyPT = "copypt";
        public static readonly string TAG_CHK_Copy = "chkCopy";
        public static readonly string TAG_Copy_Colspan = "copy-colspan";
        public static readonly string TAG_Value = "value";
        public static readonly string TAG_LBL_CopyFrom = "lblCopyFrom";
        public static readonly string TAG_LBL_MaxPT = "lblMaxPT";
        public static readonly string TAG_LBL_MinPT = "lblMinPT";
        public static readonly string TAG_LBL_SuperPT = "lblSuperPT";

        public static readonly string TAG_LBL_SuperAutoPT = "lblSuperAutoPT";
        public static readonly string TAG_LBL_MasterPT = "lblMasterPT";
        public static readonly string TAG_LBL_AgentPT = "lblAgentPT";
        public static readonly string TAG_LBL_Commission = "lblCommission";
        public static readonly string TAG_LBL_CopyPT = "copypt";
        public static readonly string TAG_PT_JS = "js";
        public static string DISCOUNT = "discount";

        //Style
        public static string CLASS_Select_PT = "pt";

        ///
        //javascript variable
        private static string JS_arrPTType = "arrPTType";

        private static string JS_arrBetType = "arrBettype";
        private static string JS_CustId = "custid";
        private static string JS_RoleId = "roleid";
        private static string JS_UplineId = "uplineid";
        private static string JS_hPTData = "hPTData";
        private static string JS_AssignHashtableData = "{0}.put('{1}', {2});";
        private static string JS_Var_IsUseAuto = "isUseAuto";
        private static string JS_Var_IsAutoMaster = "isAutoMaster";
        private static string JS_Var_msPTMax = "msPTMax";
        private static string JS_Var_mmPTMax = "mmPTMax";
        private static string JS_Var_disableMinPTFI = "disableMinPTFI";

        //END
        //javascript function
        public static readonly string FUNC_OnChangePT = "OnChangePT({0})";

        public static readonly string PARAMS_OnChangePT = "this";

        public static readonly string FUNC_ChkCopyFrom = "ChkCopyFrom({0})";
        public static readonly string PARAMS_ChkCopyFrom = "this";

        public static readonly string FUNC_ChkAuto = "CheckAuto({0})";
        public static readonly string PARAMS_ChkAuto = "this";
        //END

        //END

        private static int bettypeMemberPTMaxFI = 3;

        private static double s4 = 0.9,
                              s3 = 0.9,
                              s1 = 0.9,
                              m4 = 0.9,
                              m3 = 0.9,
                              m2 = 0.9,
                              m1 = 0.9,
                              a4 = 0.9,
                              a2 = 0.9,
                              a1 = 0.9;

        private static double u1 = 0.9, u3 = 0.9, n4, n3, n2, l4 = 0.9, l3 = 0.9, l2 = 0.9;

        private static double s4Max = 0.9,
                              s3Max = 0.9,
                              s1Max = 0.9,
                              m3Max = 0.9,
                              m2Max = 0.9,
                              m1Max = 0.9,
                              a2Max = 0.9,
                              a1Max = 0.9;

        private static double u1Max = 0.9,
                              u3Max = 0.9,
                              n4Max = 0.9,
                              n3Max = 0.9,
                              n2Max = 0.9,
                              l4Max = 0.9,
                              l3Max = 0.9,
                              l2Max = 0.9;

        private static double m3MaxAuto = 0.9, n3MaxAuto = 0.9;

        private static double s4Min, s3Min, s1Min, m3Min, m2Min, m1Min, a2Min, a1Min;
        private static double u1Min, u3Min, n4Min, n3Min, n2Min, l4Min, l3Min, l2Min;
        private static double m3MinAuto, n3MinAuto;
        private static double maxPTFI = 0.5, minPT, maxAutoPT = 0.5;

        //Commission
        private static double COMM_FI_MAX = 0.01;

        private static int COMM_FI_INTERVAL = 5;

        public static readonly string Delimiter = "_";
        public static readonly string PREFIX_SPORT = "v";
        public static readonly string PREFIX_CHK_SAUTO = "chksauto";
        public static readonly string PT = "positiontaking";

        public static readonly string SPT = "spositiontaking",
                                      MPT = "mpositiontaking",
                                      APT = "apositiontaking",
                                      Auto = "auto",
                                      STR_IsAuto = "isauto",
                                      MINPT = "min",
                                      MAXPT = "max",
                                      MaPT = "mapositiontaking",
                                      SMPT = "smpositiontaking",
                                      MEM = "mem";

        public static readonly string MINBET = "minbet", MAXBET = "maxbet", MaxPerMatch = "maxpermatch";

        public static readonly string ISUPLINEDISABLED = "uplinedisablefi",
                                      ISDISABLED = "disablefinancial",
                                      ISDOWNLINEDISABLED = "downlinedisablefi";

        public static readonly string ISUPLINEDISABLED_Update = "uplinedisablefi",
                                      ISDISABLED_Update = "disablefinancial",
                                      ISDOWNLINEDISABLED_Update = "disabledownlinefi";

        #endregion CONST variables

        #region Get Data

        /// <summary>
        /// Get PT data for edit
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="accountType"></param>
        /// <param name="uplineId"></param>
        /// <returns></returns>
        public static DbResult GetPT(int custId, Globals.AccountType accountType, int uplineId)
        {
            try
            {
                return IBCProvider.PositionTaking.GetPT(custId, accountType, uplineId);
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
                return IBCProvider.PositionTaking.GetMinMaxPT(custId, accountType);
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
                return IBCProvider.PositionTaking.GetPTMasterEditMember(custId, uplineId);
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
                return IBCProvider.PositionTaking.GetPTSuperEditMember(custId, uplineId);
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
                return IBCProvider.PositionTaking.UpdatePT(accountType, param);
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
                return IBCProvider.PositionTaking.UpdateMinMaxPT(accountType, param);
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
                return IBCProvider.PositionTaking.UpdatePTMasterOnMember(param);
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
                return IBCProvider.PositionTaking.UpdatePTSuperOnMember(param);
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
        /// <param name="_cust"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        public static string GenHTML(int custId, int roleId, int uplineId, Globals.PTType ptType, bool isInsert,
                                     int errorCode)
        {
            try
            {
                int sporttype = 201; //hardcode for FI
                bool enableAutoMPT = true;
                var custDetail = new CustomerDetail();
                custDetail.CustID = custId;
                custDetail.RoleId = roleId;
                custDetail.UplineId = uplineId;
                var betSetting = new Dictionary<string, string>();
                var custComm = new Dictionary<string, string>();
                var custPT = new Dictionary<string, double>();

                string tplSetting = WebUtils.LoadTemplate(TPL_FINANCE_SETTING);
                if (AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL))
                {
                    TemplateHandler.RemoveBlock(ref tplSetting, "HideUpdate");
                }
                var accountType = (Globals.AccountType)(roleId);
                DbResult data = null;
                switch (ptType)
                {
                    case Globals.PTType.Default:
                        data = GetPT(custId, accountType, uplineId);
                        TemplateHandler.Assign(ref tplSetting, "{FuncOnClick}", "UpdatePT();");
                        break;

                    case Globals.PTType.MasterOnMember:
                        data = GetPTMasterEditMember(custId, uplineId);
                        TemplateHandler.Assign(ref tplSetting, "{FuncOnClick}", "UpdatePTFIMasterOnMember();");
                        break;

                    case Globals.PTType.SuperOnMember:
                        data = GetPTSuperEditMember(custId, uplineId);
                        TemplateHandler.Assign(ref tplSetting, "{FuncOnClick}", "UpdatePTFISuperOnMember();");
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
                    enableAutoMPT = !CastAide.AsBoolean(dt.Rows[0]["DisableAutoMPT"]);

                //Get data from database
                //dt = //GenSampleDataTablePT();
                //assign data to object

                var listbettype = new List<int>();

                listbettype = GetBetTypeList(dt, Convert.ToChar(Delimiter));
                listbettype.Sort();
                listbettype.Reverse();
                AssignData(sporttype, ref custDetail, ref betSetting, ref custComm, ref custPT, dt);
                //Assign cust info

                TemplateHandler.Assign(ref tplSetting, "{lblCustInfo}", GetRoleName(roleId) + ": " + custDetail.UserName);
                TemplateHandler.Assign(ref tplSetting, "{custinfo}", AssignCustInfo(custDetail, roleId, ptType));
                if (isInsert)
                {
                    TemplateHandler.RemoveBlock(ref tplSetting, "chkdownline");
                }

                //Assign Commission
                TemplateHandler.Assign(ref tplSetting, "{lblCommission}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission"));
                AssignCommission(ref tplSetting, data, ptType);

                // Bet Setting
                TemplateHandler.Assign(ref tplSetting, "{lblBetSetting}", "Bet Setting");
                TemplateHandler.Assign(ref tplSetting, "{BetSetting}",
                                       GenHTMLBetSetting(betSetting, sporttype, roleId, ptType));

                //Assign PT
                TemplateHandler.Assign(ref tplSetting, "{lblPositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        TemplateHandler.Assign(ref tplSetting, TAG_PT_Data,
                                               GenHTMLSuperPT(custPT, sporttype, listbettype));
                        break;

                    case Globals.AccountType.Master:
                        TemplateHandler.Assign(ref tplSetting, TAG_PT_Data,
                                               GenHTMLMasterPT(custPT, sporttype, listbettype));
                        break;

                    case Globals.AccountType.Agent:
                        TemplateHandler.Assign(ref tplSetting, TAG_PT_Data,
                                               GenHTMLAgentPT(custPT, sporttype, listbettype, enableAutoMPT));
                        break;

                    case Globals.AccountType.Member:
                        TemplateHandler.Assign(ref tplSetting, TAG_PT_Data,
                                               GenHTMLMemberPT(custPT, ptType, sporttype, listbettype, enableAutoMPT));
                        break;
                }

                //assign common header
                //AssignCommonHeader(ref htmlContent);
                //assign javascript here
                string js = AssignJavaScript(ref tplSetting, custDetail);
                js += string.Format(Globals.JS_VariableFormat, "insert", (isInsert) ? "true" : "false");
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
                int sporttype = 201; //hardcode for FI
                var custDetail = new CustomerDetail();
                custDetail.CustID = custId;
                custDetail.RoleId = roleId;
                custDetail.UplineId = uplineId;
                var betSetting = new Dictionary<string, string>();
                var custComm = new Dictionary<string, string>();
                var custPT = new Dictionary<string, double>();
                var accountType = (Globals.AccountType)(roleId);
                string tplSetting = WebUtils.LoadTemplate(TPL_FINANCE_MINMAX_SETTING);
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
                var listbettype = new List<int>();
                listbettype = GetBetTypeList(dt, Convert.ToChar(Delimiter));
                AssignData(sporttype, ref custDetail, ref betSetting, ref custComm, ref custPT, dt);
                //Assign cust info

                TemplateHandler.Assign(ref tplSetting, "{lblCustInfo}", GetRoleName(roleId) + ": " + custDetail.UserName);
                TemplateHandler.Assign(ref tplSetting, "{custinfo}", AssignCustInfo(custDetail, roleId, ptType));

                TemplateHandler.RemoveBlock(ref tplSetting, "UpdatePT");

                //Assign PT
                TemplateHandler.Assign(ref tplSetting, "{lblPositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));

                TemplateHandler.Assign(ref tplSetting, TAG_PT_Data,
                                       GenMinMaxPT(custPT, sporttype, listbettype, accountType));

                //assign common header
                //AssignCommonHeader(ref htmlContent);
                //assign javascript here
                string js = AssignJavaScript(ref tplSetting, custDetail);
                //js += string.Format(Globals.JS_VariableFormat, "insert", (isInsert) ? "true" : "false");
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

        private static string GenHTMLSuperPT(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype)
        {
            try
            {
                if (lsBettype.Count < 1)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_FINANCE_PT_SUPER);
                string tplPTValue = WebUtils.LoadTemplate(TPL_FINANCE_PT_Value);
                //Remove unuse row
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_superautopt);
                //Assign Label
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperPT, string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_MasterPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_AgentPT, string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPT"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));

                var dataHeader = new StringBuilder();
                var dataSPT = new StringBuilder();
                var dataMPT = new StringBuilder();
                var dataAPT = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);

                double sptMax = maxPTFI, mptMax = maxPTFI, aptMax = maxPTFI;
                double sptMin = minPT, mptMin = minPT, aptMin = minPT;
                double sptCurrent = minPT, mptCurrent = minPT, aptCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype;
                string arrBettypeData = "";

                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueSPT = tplPTValue;
                    string valueMPT = tplPTValue;
                    string valueAPT = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, CacheRef.GetBettype(bettype));
                    dataHeader.Append(valueHeader);
                    s4 = maxPTFI;
                    l4 = maxPTFI;
                    n4 = datapt[PREFIX_SPORT + sporttype + SPT + Delimiter + bettype + Delimiter + "min"];
                    ;

                    l4 = datapt[PREFIX_SPORT + sporttype + SPT + Delimiter + bettype + Delimiter + "max"];

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
                    string _SPTId = sporttype + Delimiter + bettype + Delimiter + SPT;
                    string _MPTId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string _APTId = sporttype + Delimiter + bettype + Delimiter + APT;
                    string _SPTValue = HtmlHelper.GenSelectOption(sptMax, sptMin, sptCurrent, Globals.INTERVAL_PT, false,
                                                              _SPTId, funcOnChange, CLASS_Select_PT);
                    string _MPTValue = HtmlHelper.GenSelectOption(mptMax, mptMin, mptCurrent, Globals.INTERVAL_PT, false,
                                                              _MPTId, funcOnChange, CLASS_Select_PT);
                    string _APTValue = HtmlHelper.GenSelectOption(aptMax, aptMin, aptCurrent, Globals.INTERVAL_PT, false,
                                                              _APTId, funcOnChange, CLASS_Select_PT);
                    if (totalBettype == 0)
                    {
                        _SPTValue += HtmlHelper.GenChangePTButtons(_SPTId, false);
                        _MPTValue += HtmlHelper.GenChangePTButtons(_MPTId, false);
                        _APTValue += HtmlHelper.GenChangePTButtons(_APTId, false);
                        copyFromBettype = CacheRef.GetBettype(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }

                    TemplateHandler.Assign(ref valueSPT, TAG_Value, _SPTValue);
                    dataSPT.Append(valueSPT);
                    TemplateHandler.Assign(ref valueMPT, TAG_Value, _MPTValue);
                    dataMPT.Append(valueMPT);
                    TemplateHandler.Assign(ref valueAPT, TAG_Value, _APTValue);
                    dataAPT.Append(valueAPT);

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
                TemplateHandler.Assign(ref tplPTData, TAG_superpt_value, dataSPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_masterpt_value, dataMPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_agentpt_value, dataAPT.ToString());

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                arrPTType = "'" + SPT + "','" + MPT + "','" + APT + "'";
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
        private static string GenHTMLMasterPT(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_FINANCE_PT);
                string tplPTValue = WebUtils.LoadTemplate(TPL_FINANCE_PT_Value);
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
                var dataSPT = new StringBuilder();
                var dataMPT = new StringBuilder();
                var dataSAutoPT = new StringBuilder();
                var dataIsSAutoPT = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);
                string funcChkSAutoClick = string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);

                double sptMax = maxPTFI, mptMax = maxPTFI, sautoMax = maxPTFI;
                double sptMin = minPT, mptMin = minPT, sautoMin = minPT;
                double sptCurrent = minPT, mptCurrent = minPT, sautoCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype;
                string arrBettypeData = "";
                var lsPTData = new List<PositionTakingDetail>();
                string js = "";
                bool isUseAuto = false;
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueSPT = tplPTValue;
                    string valueMPT = tplPTValue;
                    string valueSAutoPT = tplPTValue;
                    string valueIsAutoPT = tplPTValue;
                    bool isAuto = CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + bettype]);
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, CacheRef.GetBettype(bettype));
                    dataHeader.Append(valueHeader);

                    sptMax = Math.Min(
                        CastAide.AsDouble(datapt[prefix + SPT + Delimiter + bettype + Delimiter + MAXPT]), maxPTFI);
                    sptMin = 0;
                    sptCurrent = datapt[prefix + SPT + Delimiter + bettype];

                    mptMax = Math.Min(
                        CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype + Delimiter + MAXPT]), maxPTFI);
                    //assign max MPT to javascript
                    mptMax = Math.Min(mptMax, sptMax);
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
                    //lsPTData.Add(new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MAXPT, mptMax));
                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MINPT,
                                                 mptMin));

                    if (bettype == bettypeMemberPTMaxFI)
                        lsPTData.Add(new PositionTakingDetail(JS_Var_msPTMax, sptMax));

                    isUseAuto = (isUseAuto == false) ? isAuto : isUseAuto;

                    string _SPTId = sporttype + Delimiter + bettype + Delimiter + SPT;
                    string _MPTId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string _SAutoId = sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + Auto;
                    string _IsAutoId = sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + STR_IsAuto;
                    lsPTData.Add(new PositionTakingDetail(_IsAutoId, (isAuto) ? 1 : 0));
                    string _SPTValue = HtmlHelper.GenSelectOption(sptMax - mptMin, sptMin, sptCurrent, Globals.INTERVAL_PT,
                                                              isAuto, _SPTId, funcOnChange, CLASS_Select_PT);
                    string _MPTValue = HtmlHelper.GenSelectOption(mptMax, mptMin, mptCurrent, Globals.INTERVAL_PT, false,
                                                              _MPTId, "", CLASS_Select_PT);
                    string _SAutoValue = HtmlHelper.GenSelectOption(sautoMax, sautoMin, sautoCurrent, Globals.INTERVAL_PT,
                                                                !isAuto, _SAutoId, "", CLASS_Select_PT);
                    string _IsAutoValue = HtmlHelper.GenInput(Globals.InputType.CheckBox, _IsAutoId, "", "", false, isAuto,
                                                          funcChkSAutoClick);
                    if (totalBettype == 0)
                    {
                        _SPTValue += HtmlHelper.GenChangePTButtons(_SPTId, isAuto, true);
                        _MPTValue += HtmlHelper.GenChangePTButtons(_MPTId, false);
                        _SAutoValue += HtmlHelper.GenChangePTButtons(_SAutoId, !isAuto, true);
                        copyFromBettype = CacheRef.GetBettype(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }

                    TemplateHandler.Assign(ref valueSPT, TAG_Value, _SPTValue);
                    dataSPT.Append(valueSPT);
                    TemplateHandler.Assign(ref valueMPT, TAG_Value, _MPTValue);
                    dataMPT.Append(valueMPT);
                    TemplateHandler.Assign(ref valueSAutoPT, TAG_Value, _SAutoValue);
                    dataSAutoPT.Append(valueSAutoPT);
                    TemplateHandler.Assign(ref valueIsAutoPT, TAG_Value, _IsAutoValue);
                    dataIsSAutoPT.Append(valueIsAutoPT);

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
                TemplateHandler.Assign(ref tplPTData, TAG_superpt_value, dataSPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_masterpt_value, dataMPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superautopt_value, dataSAutoPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_chksauto_value, dataIsSAutoPT.ToString());

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPTType = new List<string>();
                lsPTType.Add(SPT);
                lsPTType.Add(MPT);
                lsPTType.Add(SPT + Delimiter + Auto);
                lsPTType.Add(SPT + Delimiter + STR_IsAuto);
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenArrayPTType(lsPTType);
                js += string.Format(Globals.JS_VariableFormat, JS_Var_IsUseAuto, (isUseAuto) ? "1" : "0");
                js += string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js += AssignJSPTData(lsPTData);
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

        //agent
        /// <summary>
        /// Edit Agent PT
        /// </summary>
        /// <param name="_cust"></param>
        /// <returns></returns>
        private static string GenHTMLAgentPT(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype,
                                             bool enableAutoMPT)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_FINANCE_PT);
                string tplPTValue = WebUtils.LoadTemplate(TPL_FINANCE_PT_Value);
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

                double mptMax = maxPTFI, aptMax = maxPTFI, mautoMax = maxPTFI;
                double aptMin = minPT, mptMin = minPT, mautoMin = minPT;
                double aptCurrent = minPT, mptCurrent = minPT, mautoCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype;
                string arrBettypeData = "";
                var lsPTData = new List<PositionTakingDetail>();
                bool isUseAuto = false;
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueMPT = tplPTValue;
                    string valueAPT = tplPTValue;
                    string valueMAutoPT = tplPTValue;
                    string valueIsMAutoPT = tplPTValue;

                    TemplateHandler.Assign(ref valueHeader, TAG_Value, CacheRef.GetBettype(bettype));
                    dataHeader.Append(valueHeader);

                    bool isAuto = Permissions.IsEnabledAutoMaster(enableAutoMPT)
                                      ? CastAide.AsBoolean(
                                          datapt[STR_IsAuto + Delimiter + prefix + MPT + Delimiter + bettype])
                                      : false;
                    //CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + bettype]);

                    mptMax =
                        Math.Min(
                            CastAide.AsDouble(
                                datapt[Auto + Delimiter + prefix + MPT + Delimiter + bettype + Delimiter + MAXPT]),
                            maxPTFI);
                    mptMin = minPT;
                    mptCurrent = CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype]);

                    //auto
                    if (Permissions.IsEnabledAutoMaster(enableAutoMPT))
                    {
                        mautoMax =
                            CastAide.AsDouble(
                                datapt[Auto + Delimiter + prefix + MPT + Delimiter + bettype + Delimiter + MAXPT]);
                        mautoMin = 0;
                        mautoCurrent = CastAide.AsDouble(datapt[Auto + Delimiter + prefix + MPT + Delimiter + bettype]);
                    }

                    aptMax = CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype + Delimiter + MAXPT]);
                    aptMax = Math.Min(mptMax, aptMax);
                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + APT + Delimiter + MAXPT,
                                                 aptMax));
                    aptMax = Math.Min(aptMax, mptMax - mptCurrent);
                    aptMin = CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype + Delimiter + MINPT]);
                    aptCurrent = CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype]);
                    //Math.Min(CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype]), aptMax);

                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MAXPT,
                                                 mptMax));
                    //lsPTData.Add(new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + APT + Delimiter + MAXPT, aptMax));
                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + APT + Delimiter + MINPT,
                                                 aptMin));

                    if (bettype == bettypeMemberPTMaxFI)
                        lsPTData.Add(new PositionTakingDetail(JS_Var_mmPTMax, mptMax));

                    isUseAuto = (isUseAuto == false) ? isAuto : isUseAuto;

                    string _APTId = sporttype + Delimiter + bettype + Delimiter + APT;
                    string _MPTId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string _MAutoId = sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + Auto;
                    string _IsAutoId = sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + STR_IsAuto;
                    lsPTData.Add(new PositionTakingDetail(_IsAutoId, (isAuto) ? 1 : 0));
                    string _APTValue = HtmlHelper.GenSelectOption(aptMax, aptMin, aptCurrent, Globals.INTERVAL_PT, false,
                                                              _APTId, "", CLASS_Select_PT);
                    string _MPTValue = HtmlHelper.GenSelectOption(mptMax - aptMin, mptMin, mptCurrent, Globals.INTERVAL_PT,
                                                              isAuto, _MPTId, funcOnChange, CLASS_Select_PT);
                    string _MAutoValue = HtmlHelper.GenSelectOption(mautoMax, mautoMin, mautoCurrent, Globals.INTERVAL_PT,
                                                                !isAuto, _MAutoId, "", CLASS_Select_PT);
                    string _IsAutoValue = HtmlHelper.GenInput(Globals.InputType.CheckBox, _IsAutoId, "", "", false, isAuto,
                                                          funcChkMAutoClick);
                    if (totalBettype == 0)
                    {
                        _APTValue += HtmlHelper.GenChangePTButtons(_APTId, false);
                        _MPTValue += HtmlHelper.GenChangePTButtons(_MPTId, isAuto, true);
                        _MAutoValue += HtmlHelper.GenChangePTButtons(_MAutoId, !isAuto, true);
                        copyFromBettype = CacheRef.GetBettype(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }

                    TemplateHandler.Assign(ref valueMPT, TAG_Value, _MPTValue);
                    dataMPT.Append(valueMPT);
                    TemplateHandler.Assign(ref valueAPT, TAG_Value, _APTValue);
                    dataAPT.Append(valueAPT);
                    TemplateHandler.Assign(ref valueMAutoPT, TAG_Value, _MAutoValue);
                    dataMAutoPT.Append(valueMAutoPT);
                    TemplateHandler.Assign(ref valueIsMAutoPT, TAG_Value, _IsAutoValue);
                    dataIsMAutoPT.Append(valueIsMAutoPT);

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
                var lsPTType = new List<string>();
                lsPTType.Add(APT);
                lsPTType.Add(MPT);
                lsPTType.Add(MPT + Delimiter + Auto);
                lsPTType.Add(MPT + Delimiter + STR_IsAuto);
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenArrayPTType(lsPTType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_Var_IsUseAuto, (isUseAuto) ? "1" : "0");
                js += string.Format(Globals.JS_VariableFormat, JS_Var_IsAutoMaster,
                                    Permissions.IsEnabledAutoMaster(true) ? "true" : "false");
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js += AssignJSPTData(lsPTData);
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

        //member
        /// <summary>
        /// This IBC Customer wasn't synchronized to Casino System
        /// </summary>
        /// <param name="_cust"></param>
        /// <returns></returns>
        /// <summary>
        /// This IBC Customer wasn't synchronized to Casino System
        /// </summary>
        /// <param name="_cust"></param>
        /// <returns></returns>
        private static string GenHTMLMemberPT(Dictionary<string, double> datapt, Globals.PTType editType, int sporttype,
                                              List<int> lsBettype, bool enableAutoMPT)
        {
            try
            {
                switch (editType)
                {
                    case Globals.PTType.SuperOnMember:
                        return GenHTMLSuperOnMember(datapt, sporttype, lsBettype);
                    case Globals.PTType.MasterOnMember:
                        return GenHTMLMasterOnMember(datapt, sporttype, lsBettype, enableAutoMPT);
                    default:
                        return GenHTMLAgentOnMember(datapt, sporttype, lsBettype);
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
        private static string GenHTMLSuperOnMember(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_FINANCE_PT);
                string tplPTValue = WebUtils.LoadTemplate(TPL_FINANCE_PT_Value);
                //Assign Label
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_AgentPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super"));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_MasterPT, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_SuperAutoPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAuto"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super")));

                var dataHeader = new StringBuilder();
                var dataSPT = new StringBuilder();
                var dataAPT = new StringBuilder();
                var dataMPT = new StringBuilder();
                var dataSAutoPT = new StringBuilder();
                var dataIsSAutoPT = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);
                string funcChkSAutoClick = string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);

                double sptMax = maxPTFI, mptMax = maxPTFI, sautoMax = maxPTFI;
                double sptMin = minPT, mptMin = minPT, sautoMin = minPT;
                double sptCurrent = minPT, mptCurrent = minPT, sautoCurrent = minPT, aptCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype;
                string arrBettypeData = "";
                bool enabledCopy = true;
                var lsPTData = new List<PositionTakingDetail>();
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
                    bool isAuto_Master =
                        CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + bettype]);
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, CacheRef.GetBettype(bettype));
                    dataHeader.Append(valueHeader);

                    sptMax = Math.Min(CastAide.AsDouble(datapt[MAXPT + prefix + "pos" + Delimiter + bettype]), maxPTFI);
                    sptMin = Math.Max(CastAide.AsDouble(datapt[MINPT + prefix + "pos" + Delimiter + bettype]), minPT);
                    sptCurrent = isAuto_Master
                                     ? datapt[prefix + SPT + Delimiter + bettype]
                                     : datapt[prefix + SPT + Delimiter + bettype];

                    //mptMax = Math.Min(CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype + Delimiter + MAXPT]), maxPTFI);
                    //assign max MPT to javascript
                    //lsPTData.Add(new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MAXPT, mptMax));
                    //mptMax = (isAuto) ? mptMax : (Math.Min(mptMax, sptMax - sptCurrent));
                    //mptMin = CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype + Delimiter + MINPT]);

                    sautoMax = CastAide.AsDouble(datapt[MAXPT + Auto + prefix + "pos" + Delimiter + bettype]);
                    sautoMin = 0;
                    sautoCurrent = CastAide.AsDouble(datapt[Auto + Delimiter + prefix + SPT + Delimiter + bettype]);

                    mptCurrent = datapt[prefix + MPT + Delimiter + bettype];
                    aptCurrent = datapt[prefix + APT + Delimiter + bettype];

                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + MAXPT,
                                                 sptMax));
                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MINPT,
                                                 mptMin));
                    lsPTData.Add(
                        new PositionTakingDetail(
                            sporttype + Delimiter + bettype + Delimiter + STR_IsAuto + Delimiter + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"),
                            isAuto_Master.ToString().ToLower()));

                    string _SPTId = sporttype + Delimiter + bettype + Delimiter + SPT;
                    string _MPTId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string _APTId = sporttype + Delimiter + bettype + Delimiter + APT;
                    string _SAutoId = sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + Auto;
                    string _IsAutoId = sporttype + Delimiter + bettype + Delimiter + SPT + Delimiter + STR_IsAuto;
                    string _SPTValue = isAuto_Master
                                           ? HtmlHelper.GenLabel(_SPTId, sptCurrent.ToString(), "")
                                           : HtmlHelper.GenSelectOption(sptMax, sptMin, sptCurrent, Globals.INTERVAL_PT,
                                                                    isAutoMem, _SPTId, funcOnChange, CLASS_Select_PT);
                    string _MPTValue = HtmlHelper.GenLabel(_MPTId, mptCurrent.ToString(), "");
                    string _APTValue = HtmlHelper.GenLabel(_APTId, aptCurrent.ToString(), "");
                    string _SAutoValue = HtmlHelper.GenSelectOption(sautoMax, sautoMin, sautoCurrent, Globals.INTERVAL_PT,
                                                                !isAutoMem, _SAutoId, "", CLASS_Select_PT);
                    string _IsAutoValue = HtmlHelper.GenInput(Globals.InputType.CheckBox, _IsAutoId, "", "", false,
                                                          isAutoMem, funcChkSAutoClick);
                    if (totalBettype == 0)
                    {
                        _SPTValue += isAuto_Master ? string.Empty : HtmlHelper.GenChangePTButtons(_SPTId, isAutoMem, true);
                        _SAutoValue += HtmlHelper.GenChangePTButtons(_SAutoId, !isAutoMem, true);
                        copyFromBettype = CacheRef.GetBettype(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                        enabledCopy = isAuto_Master ? (isAutoMem ? true : false) : true;
                    }

                    TemplateHandler.Assign(ref valueSPT, TAG_Value, _SPTValue);
                    dataSPT.Append(valueSPT);
                    TemplateHandler.Assign(ref valueMPT, TAG_Value, _MPTValue);
                    dataMPT.Append(valueMPT);
                    TemplateHandler.Assign(ref valueAPT, TAG_Value, _APTValue);
                    dataAPT.Append(valueAPT);
                    TemplateHandler.Assign(ref valueSAutoPT, TAG_Value, _SAutoValue);
                    dataSAutoPT.Append(valueSAutoPT);
                    TemplateHandler.Assign(ref valueIsAutoPT, TAG_Value, _IsAutoValue);
                    dataIsSAutoPT.Append(valueIsAutoPT);

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
                TemplateHandler.Assign(ref tplPTData, TAG_superpt_value, dataSPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_masterpt_value, dataMPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_agentpt_value, dataAPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superautopt_value, dataSAutoPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_chksauto_value, dataIsSAutoPT.ToString());

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPTType = new List<string>();
                lsPTType.Add(SPT);
                lsPTType.Add(SPT + Delimiter + Auto);
                lsPTType.Add(SPT + Delimiter + STR_IsAuto);
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenArrayPTType(lsPTType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js += AssignJSPTData(lsPTData);
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
                                                   bool enableAutoMPT)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_FINANCE_PT);
                string tplPTValue = WebUtils.LoadTemplate(TPL_FINANCE_PT_Value);
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

                double mptMax = maxPTFI, aptMax = maxPTFI, mautoptMax = maxPTFI;
                double aptMin = minPT, mptMin = minPT, mautoptMin = minPT;
                double aptCurrent = minPT, mptCurrent = minPT, mautoptCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype;
                string arrBettypeData = "";
                bool enabledCopy = true;
                var lsPTData = new List<PositionTakingDetail>();
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueMPT = tplPTValue;
                    string valueAPT = tplPTValue;
                    string valueMAutoPT = tplPTValue;
                    string valueIsMAutoPT = tplPTValue;

                    bool isAutoMem = Permissions.IsEnabledAutoMaster(enableAutoMPT)
                                         ? CastAide.AsBoolean(
                                             datapt[
                                                 STR_IsAuto + Delimiter + prefix + MPT + Delimiter + MEM + Delimiter +
                                                 bettype])
                                         : false;
                    //CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + MPT + Delimiter + bettype]);
                    bool isAuto_Agent = Permissions.IsEnabledAutoMaster(enableAutoMPT)
                                            ? CastAide.AsBoolean(
                                                datapt[STR_IsAuto + Delimiter + prefix + MPT + Delimiter + bettype])
                                            : false;
                    //CastAide.AsBoolean(datapt[STR_IsAuto + Delimiter + prefix + MPT + Delimiter + bettype]);
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, CacheRef.GetBettype(bettype));
                    dataHeader.Append(valueHeader);

                    mptMax = Math.Min(CastAide.AsDouble(datapt[MAXPT + prefix + "pos" + Delimiter + bettype]), maxPTFI);
                    mptMin = Math.Min(CastAide.AsDouble(datapt[MINPT + prefix + "pos" + Delimiter + bettype]), maxPTFI);
                    mptCurrent = isAuto_Agent
                                     ? CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype])
                                     : CastAide.AsDouble(datapt[prefix + MPT + Delimiter + bettype]);

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

                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + MAXPT,
                                                 mptMax));
                    lsPTData.Add(
                        new PositionTakingDetail(sporttype + Delimiter + bettype + Delimiter + APT + Delimiter + MINPT,
                                                 aptMin));
                    lsPTData.Add(
                        new PositionTakingDetail(
                            sporttype + Delimiter + bettype + Delimiter + STR_IsAuto + Delimiter + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"),
                            isAuto_Agent.ToString().ToLower()));

                    string _APTId = sporttype + Delimiter + bettype + Delimiter + APT;
                    string _MPTId = sporttype + Delimiter + bettype + Delimiter + MPT;
                    string _MAutoId = sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + Auto;
                    string _IsAutoId = sporttype + Delimiter + bettype + Delimiter + MPT + Delimiter + STR_IsAuto;
                    //string _APTValue = Helper.GenSelectOption(aptMax, aptMin, aptCurrent, Globals.INTERVAL_PT, false, _APTId, funcOnChange, CLASS_Select_PT);

                    string _APTValue = HtmlHelper.GenLabel(_APTId, aptCurrent.ToString(), "");
                    string _MPTValue = isAuto_Agent
                                           ? HtmlHelper.GenLabel(_MPTId, mptCurrent.ToString(), "")
                                           : HtmlHelper.GenSelectOption(mptMax, mptMin, mptCurrent, Globals.INTERVAL_PT,
                                                                    isAutoMem, _MPTId, funcOnChange, CLASS_Select_PT);
                    string _MAutoValue = HtmlHelper.GenSelectOption(mautoptMax, mautoptMin, mautoptCurrent,
                                                                Globals.INTERVAL_PT, !isAutoMem, _MAutoId, "",
                                                                CLASS_Select_PT);
                    string _IsAutoValue = HtmlHelper.GenInput(Globals.InputType.CheckBox, _IsAutoId, "", "", false,
                                                          isAutoMem, funcChkMAutoClick);
                    if (totalBettype == 0)
                    {
                        //_APTValue += Helper.GenChangePTButtons(_APTId, false);
                        _MPTValue += isAuto_Agent ? string.Empty : HtmlHelper.GenChangePTButtons(_MPTId, isAutoMem, true);
                        _MAutoValue += HtmlHelper.GenChangePTButtons(_MAutoId, !isAutoMem, true);
                        copyFromBettype = CacheRef.GetBettype(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                        enabledCopy = isAuto_Agent ? (isAutoMem ? true : false) : true;
                    }

                    TemplateHandler.Assign(ref valueMPT, TAG_Value, _MPTValue);
                    dataMPT.Append(valueMPT);
                    TemplateHandler.Assign(ref valueAPT, TAG_Value, _APTValue);
                    dataAPT.Append(valueAPT);
                    TemplateHandler.Assign(ref valueMAutoPT, TAG_Value, _MAutoValue);
                    dataMAutoPT.Append(valueMAutoPT);
                    TemplateHandler.Assign(ref valueIsMAutoPT, TAG_Value, _IsAutoValue);
                    dataIsMAutoPT.Append(valueIsMAutoPT);

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
                TemplateHandler.Assign(ref tplPTData, TAG_agentpt_value, dataAPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_masterpt_value, dataMPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_superautopt_value, dataMAutoPT.ToString());
                TemplateHandler.Assign(ref tplPTData, TAG_chksauto_value, dataIsMAutoPT.ToString());
                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPTType = new List<string>();
                lsPTType.Add(APT);
                lsPTType.Add(MPT);
                lsPTType.Add(MPT + Delimiter + Auto);
                lsPTType.Add(MPT + Delimiter + STR_IsAuto);
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenArrayPTType(lsPTType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js += string.Format(Globals.JS_VariableFormat, JS_Var_IsAutoMaster,
                                    Permissions.IsEnabledAutoMaster(true) ? "true" : "false");
                js += AssignJSPTData(lsPTData);
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
        /// Edit Agent PT on Member
        /// </summary>
        /// <param name="tplPT"></param>
        /// <param name="_cust"></param>
        /// <param name="csGroup"></param>
        /// <returns></returns>
        private static string GenHTMLAgentOnMember(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype)
        {
            try
            {
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplPTData = WebUtils.LoadTemplate(TPL_FINANCE_PT);
                string tplPTValue = WebUtils.LoadTemplate(TPL_FINANCE_PT_Value);
                //Remove unuse row
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_superpt);
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_superautopt);
                TemplateHandler.RemoveBlock(ref tplPTData, TAG_masterpt);

                //Assign Label

                TemplateHandler.Assign(ref tplPTData, TAG_LBL_AgentPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent")));
                TemplateHandler.Assign(ref tplPTData, TAG_LBL_MasterPT,
                                       string.Format(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPositionTakingFormat"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master")));

                var dataHeader = new StringBuilder();
                var dataAPT = new StringBuilder();
                var dataMPT = new StringBuilder();
                int totalBettype = 0;
                string funcOnChange = string.Format(FUNC_OnChangePT, PARAMS_OnChangePT);
                string copyFromBettype = "";
                string copyFromId = "";
                string funcChkCopyClick = string.Format(FUNC_ChkCopyFrom, PARAMS_ChkCopyFrom);
                string funcChkSAutoClick = string.Format(FUNC_ChkAuto, PARAMS_ChkAuto);

                double aptMax = maxPTFI;
                double aptMin = minPT;
                double aptCurrent = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype;
                string arrBettypeData = "";
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueMPT = tplPTValue;
                    string valueAPT = tplPTValue;

                    TemplateHandler.Assign(ref valueHeader, TAG_Value, CacheRef.GetBettype(bettype));
                    dataHeader.Append(valueHeader);

                    //a2 = CastAide.AsDouble(datapt[MAXPT + prefix + "pos" + Delimiter + bettype]);
                    a1 = CastAide.AsDouble(datapt[prefix + APT + Delimiter + bettype]);
                    ;
                    l2 = CastAide.AsDouble(datapt[MAXPT + prefix + "pos" + Delimiter + bettype]);
                    n2 = CastAide.AsDouble(datapt[MINPT + prefix + "pos" + Delimiter + bettype]);

                    aptMax = Math.Min(maxPTFI, l2);
                    aptMin = n2;
                    aptCurrent = a1; //Math.Min(a1, aptMax);

                    string _APTId = sporttype + Delimiter + bettype + Delimiter + APT;

                    string _APTValue = HtmlHelper.GenSelectOption(aptMax, aptMin, aptCurrent, Globals.INTERVAL_PT, false,
                                                              _APTId, funcOnChange, CLASS_Select_PT);
                    if (totalBettype == 0)
                    {
                        _APTValue += HtmlHelper.GenChangePTButtons(_APTId, false);
                        copyFromBettype = CacheRef.GetBettype(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }

                    TemplateHandler.Assign(ref valueAPT, TAG_Value, _APTValue);
                    dataAPT.Append(valueAPT);

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

                //assign javascript
                arrBettypeData = (!string.IsNullOrEmpty(arrBettypeData))
                                     ? arrBettypeData.Substring(0, arrBettypeData.Length - 1)
                                     : "";
                var lsPTType = new List<string>();
                lsPTType.Add(APT);
                lsPTType.Add(MPT);
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenArrayPTType(lsPTType);
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

        private static string GenMinMaxPT(Dictionary<string, double> datapt, int sporttype, List<int> lsBettype,
                                          Globals.AccountType accountType)
        {
            try
            {
                if (lsBettype.Count < 1)
                    return AlertMessage.GetMsg(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"), AlertMessage.MsgType.Error);
                string prefix = (sporttype > 1) ? PREFIX_SPORT + sporttype : "";
                string tplMinMaxPT = WebUtils.LoadTemplate(TPL_FINANCE_MINMAX_PT);
                string tplPTValue = WebUtils.LoadTemplate(TPL_FINANCE_PT_Value);
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

                double maxPTMax = maxPTFI, minPTMax = minPT;
                double maxPTMin = minPT, minPTMin = minPT;
                double currentPTMax = minPT, currentPTMin = minPT;
                string arrPTType = "";
                string arrBettypeName = JS_arrBetType + sporttype;
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
                var lsPTData = new List<PositionTakingDetail>();
                foreach (int bettype in lsBettype)
                {
                    arrBettypeData += bettype + ",";
                    //Header
                    string valueHeader = tplPTValue;
                    string valueMaxPT = tplPTValue;
                    string valueMPT = tplPTValue;
                    TemplateHandler.Assign(ref valueHeader, TAG_Value, CacheRef.GetBettype(bettype));
                    dataHeader.Append(valueHeader);

                    maxPTMax = maxPTFI;
                    if (DataUtils.IsExistKey(datapt,
                                             PREFIX_SPORT + sporttype + PTType + Delimiter + bettype + Delimiter + "max"))
                        currentPTMax =
                            datapt[PREFIX_SPORT + sporttype + PTType + Delimiter + bettype + Delimiter + "max"];
                    currentPTMax = (accountType == Globals.AccountType.Super) ? maxPTFI : currentPTMax; //s4;

                    maxPTMin = Math.Min(currentPTMax, datapt[PREFIX_SPORT + sporttype + PTType + Delimiter + bettype]);
                    currentPTMin = datapt[PREFIX_SPORT + sporttype + PTType + Delimiter + bettype + Delimiter + "min"];
                    currentPTMin = Math.Min(currentPTMin, maxPTMin);
                    lsPTData.Add(
                        new PositionTakingDetail(
                            sporttype + Delimiter + bettype + Delimiter + MINPT + Delimiter + MAXPT, maxPTMin));
                    minPTMax = currentPTMin;
                    //SPT
                    string _MaxPTId = sporttype + Delimiter + bettype + Delimiter + "max";
                    string _MinPTId = sporttype + Delimiter + bettype + Delimiter + "min";
                    string _MaxPTValue = HtmlHelper.GenSelectOption(maxPTMax, minPTMax, currentPTMax, Globals.INTERVAL_PT,
                                                                false, _MaxPTId, funcOnChange, CLASS_Select_PT);
                    string _MinPTValue = HtmlHelper.GenSelectOption(maxPTMin, minPTMin, currentPTMin, Globals.INTERVAL_PT,
                                                                WebSettings.DisableMinPTFI, _MinPTId, funcOnChange,
                                                                CLASS_Select_PT);
                    if (totalBettype == 0)
                    {
                        _MaxPTValue += HtmlHelper.GenChangePTButtons(_MaxPTId, false);
                        _MinPTValue += HtmlHelper.GenChangePTButtons(_MinPTId, WebSettings.DisableMinPTFI);
                        copyFromBettype = CacheRef.GetBettype(bettype);
                        copyFromId = sporttype + Delimiter + TAG_CHK_Copy + Delimiter + bettype; // 201_2_ChkCopy;
                    }

                    TemplateHandler.Assign(ref valueMaxPT, TAG_Value, _MaxPTValue);
                    dataMaxPT.Append(valueMaxPT);
                    TemplateHandler.Assign(ref valueMPT, TAG_Value, _MinPTValue);
                    dataMinPT.Append(valueMPT);

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
                                                                   ((accountType == Globals.AccountType.Super) &&
                                                                    WebSettings.DisableMinPTFI), false, funcChkCopyClick));
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
                var lsPTType = new List<string>();
                lsPTType.Add(MAXPT);
                lsPTType.Add(MINPT);
                arrBettypeData = string.Format(Globals.JS_arrayFormat, arrBettypeData);
                arrPTType = GenArrayPTType(lsPTType);
                string js = string.Format(Globals.JS_VariableFormat, arrBettypeName, arrBettypeData);
                js += string.Format(Globals.JS_VariableFormat, JS_arrPTType, arrPTType);
                js += string.Format(Globals.JS_VariableFormat, JS_Var_disableMinPTFI,
                                    (WebSettings.DisableMinPTFI) ? "true" : "false");
                js += AssignJSPTData(lsPTData);
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
                string tplCommission = WebUtils.LoadTemplate(TPL_FINANCE_COMM);

                double commFIMax = 0.0;
                double commFI = 0.0;

                double commFIMin = 0;
                string commId = "201" + Delimiter + TAG_Commission;
                if (ptType == Globals.PTType.Default || ptType == Globals.PTType.AgentOnMember)
                {
                    TemplateHandler.Assign(ref tplCommission, TAG_LBL_Commission, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission"));

                    commFIMax = CastAide.AsDouble(row["v201discount_max"]);
                    commFI = CastAide.AsDouble(row["v201discount"]);
                    //Math.Min(CastAide.AsDouble(row["v201discount"]), commFIMax);
                    TemplateHandler.Assign(ref tplCommission, TAG_Commission,
                                           HtmlHelper.GenSelectOption(commFIMax, 0, commFI, COMM_FI_INTERVAL, false, commId,
                                                                  ""));
                }
                else if (ptType == Globals.PTType.MasterOnMember)
                {
                    string commmission = "";
                    string strcommission = "";
                    string _strcommission = "";
                    _strcommission = TemplateHandler.CopyBlock(tplCommission, "UpdatePT");
                    //Agent commision
                    strcommission = (string)_strcommission.Clone();
                    commFI = CastAide.AsDouble(row["v201discount_ag"]);
                    TemplateHandler.Assign(ref strcommission, TAG_LBL_Commission, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission") + " for Agent");
                    TemplateHandler.Assign(ref strcommission, TAG_Commission,
                                           HtmlHelper.GenLabel(commId + "_ag", commFI.ToString(), ""));
                    commmission += strcommission;

                    //Member commission
                    strcommission = (string)_strcommission.Clone();
                    commFI = CastAide.AsDouble(row["v201discount"]);
                    TemplateHandler.Assign(ref strcommission, TAG_LBL_Commission, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission") + " for Member");
                    TemplateHandler.Assign(ref strcommission, TAG_Commission,
                                           HtmlHelper.GenLabel(commId + "_me", commFI.ToString(), ""));
                    commmission += strcommission;

                    TemplateHandler.ReplaceBlock(ref tplCommission, "UpdatePT", commmission);
                }
                else
                {
                    string commmission = "";
                    string strcommission = "";
                    string _strcommission = "";
                    _strcommission = TemplateHandler.CopyBlock(tplCommission, "UpdatePT");

                    //Master commision
                    strcommission = (string)_strcommission.Clone();
                    commFI = CastAide.AsDouble(row["v201discount_Mag"]);
                    TemplateHandler.Assign(ref strcommission, TAG_LBL_Commission, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission") + " for Master");
                    TemplateHandler.Assign(ref strcommission, TAG_Commission,
                                           HtmlHelper.GenLabel(commId + "_mag", commFI.ToString(), ""));
                    commmission += strcommission;

                    //Agent commision
                    strcommission = (string)_strcommission.Clone();
                    commFI = CastAide.AsDouble(row["v201discount_ag"]);
                    TemplateHandler.Assign(ref strcommission, TAG_LBL_Commission, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission") + " for Agent");
                    TemplateHandler.Assign(ref strcommission, TAG_Commission,
                                           HtmlHelper.GenLabel(commId + "_ag", commFI.ToString(), ""));
                    commmission += strcommission;

                    //Member commission
                    strcommission = (string)_strcommission.Clone();
                    commFI = CastAide.AsDouble(row["v201discount"]);
                    TemplateHandler.Assign(ref strcommission, TAG_LBL_Commission, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission") + " for Member");
                    TemplateHandler.Assign(ref strcommission, TAG_Commission,
                                           HtmlHelper.GenLabel(commId + "_me", commFI.ToString(), ""));
                    commmission += strcommission;

                    TemplateHandler.ReplaceBlock(ref tplCommission, "UpdatePT", commmission);
                }

                TemplateHandler.Assign(ref tplSetting, TAG_Commission, tplCommission);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Common header: Customer Information,
        /// </summary>
        /// <param name="tplCSPT"></param>
        ///
        /// <summary>
        /// Add pt value to javascript variable hPTData
        /// </summary>
        /// <returns></returns>
        private static string AssignJSPTData(List<PositionTakingDetail> lsPTData)
        {
            try
            {
                string retValue = "";
                string hashtableName = JS_hPTData;
                foreach (PositionTakingDetail ptDetail in lsPTData)
                {
                    retValue += string.Format(JS_AssignHashtableData, hashtableName, ptDetail.PTName, ptDetail.PTValue);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        private static string GenArrayPTType(List<string> lsPT)
        {
            try
            {
                string retValue = "";

                if (lsPT == null || lsPT.Count == 0)
                    return string.Empty;
                foreach (string ptType in lsPT)
                {
                    retValue += "'" + ptType + "'" + ",";
                }
                retValue = retValue.Substring(0, retValue.Length - 1);
                retValue = string.Format(Globals.JS_arrayFormat, retValue);

                return retValue;
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

        private static void AssignJavaScript(ref string tplPT, List<string> lsData)
        {
            string jsData = "<script type='text/javascript'>";
            if (lsData != null)
            {
                foreach (string sData in lsData)
                {
                    jsData += sData;
                }
            }
            jsData += "var ID_CHECKBOX_DISABLEDCASINO = '" + Globals.ID_CHECKBOX_DISABLEDCASINO + "';\r\n";
            jsData += "var strHidUpdDownline = '" + Globals.HIDDEN_UPDDOWNLLINE + "';";
            jsData += "var strConfirmDisableCS = '" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "strConfirmDisableCS").Replace("'", "\\'") + "';\r\n";
            jsData += "var strConfirmEnableCS = '" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "strConfirmEnableCS").Replace("'", "\\'") + "';\r\n";

            //Assign js betsetting
            jsData += " var alertMinbet='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertIncorrectMinBet") + "';\r\n";
            jsData += " var alertMaxbet='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertIncorrectMaxBet") + "';\r\n";
            jsData += " var alertMaxpermatch='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertIncorrectMaxPerMatch") + "';\r\n";
            jsData += " var alertCompare3='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertMaxLessMin") + "';\r\n";
            jsData += " var alertCompare='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertMaxLessMaxPer") + "';\r\n";
            jsData += " var alertminbetC='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalertMinbetCompare") + "';\r\n";
            jsData += " var alertMaxbetC='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalerMaxbetCompare") + "';\r\n";
            jsData += " var alertMaxpermatchC='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalertMaxPerMatchCompare") + "';\r\n";
            jsData += " var alertMaxpermatchCMin='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalertMaxPerMatchCompareMin") + "';\r\n";
            jsData += " var alertMaxpermatchCMax='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalertMaxPerMatchCompareMax") + "';\r\n";

            jsData += "</script>";

            TemplateHandler.Assign(ref tplPT, "{jsdata}", jsData);
        }

        private static string AssignJavaScript(ref string tplPT, CustomerDetail cust)
        {
            string jsData = "";
            jsData += "var ID_CHECKBOX_DISABLEDCASINO = '" + Globals.ID_CHECKBOX_DISABLEDCASINO + "';\r\n";
            jsData += "var strHidUpdDownline = '" + Globals.HIDDEN_UPDDOWNLLINE + "';";
            jsData += "var strConfirmDisableCS = '" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "strConfirmDisableCS").Replace("'", "\\'") + "';\r\n";
            jsData += "var strConfirmEnableCS = '" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "strConfirmEnableCS").Replace("'", "\\'") + "';\r\n";

            //Assign js betsetting
            jsData += " var alertMinbet='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertIncorrectMinBet") + "';\r\n";
            jsData += " var alertMaxbet='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertIncorrectMaxBet") + "';\r\n";
            jsData += " var alertMaxpermatch='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertIncorrectMaxPerMatch") + "';\r\n";
            jsData += " var alertCompare3='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertMaxLessMin") + "';\r\n";
            jsData += " var alertCompare='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "alertMaxLessMaxPer") + "';\r\n";
            jsData += " var alertminbetC='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalertMinbetCompare") + "';\r\n";
            jsData += " var alertMaxbetC='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalerMaxbetCompare") + "';\r\n";
            jsData += " var alertMaxpermatchC='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalertMaxPerMatchCompare") + "';\r\n";
            jsData += " var alertMaxpermatchCMin='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalertMaxPerMatchCompareMin") + "';\r\n";
            jsData += " var alertMaxpermatchCMax='" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblalertMaxPerMatchCompareMax") + "';\r\n";
            jsData += string.Format(Globals.JS_VariableFormat, JS_CustId, cust.CustID);
            jsData += string.Format(Globals.JS_VariableFormat, JS_RoleId, cust.RoleId);
            jsData += string.Format(Globals.JS_VariableFormat, JS_UplineId, cust.UplineId);

            return jsData;
        }

        private static string AssignCustInfo(CustomerDetail custdetail, int roleid, Globals.PTType pttype)
        {
            string fiinfo = "";
            fiinfo = WebUtils.LoadTemplate(TPL_FINANCE_CustInfo);
            TemplateHandler.Assign(ref fiinfo, "{lblFirstname}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFirstname"));
            TemplateHandler.Assign(ref fiinfo, "{firstname}", custdetail.FirstName);
            TemplateHandler.Assign(ref fiinfo, "{lblLastname}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblLastname"));
            TemplateHandler.Assign(ref fiinfo, "{lastname}", custdetail.LastName);
            TemplateHandler.Assign(ref fiinfo, "{lblPhone}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPhone"));
            TemplateHandler.Assign(ref fiinfo, "{phone}", custdetail.Phone);
            TemplateHandler.Assign(ref fiinfo, "{lblMobilePhone}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMobilePhone"));
            TemplateHandler.Assign(ref fiinfo, "{mobile}", custdetail.MobilePhone);
            TemplateHandler.Assign(ref fiinfo, "{lblFax}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFax"));
            TemplateHandler.Assign(ref fiinfo, "{fax}", custdetail.Fax);

            if (pttype == Globals.PTType.Default)
            {
                TemplateHandler.RemoveBlock(ref fiinfo, "MinMaxPT");
                TemplateHandler.Assign(ref fiinfo, "{lblDisabledFI}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblEnabledFI"));
                TemplateHandler.Assign(ref fiinfo, "{lbltype}", GetRoleName(roleid));
                TemplateHandler.Assign(ref fiinfo, "{lbltypeDL}", GetRoleName(roleid) + "'s downlines");
                if (roleid == 4)
                {
                    TemplateHandler.Assign(ref fiinfo, "{disabledChangeFI}", "");
                }
                else
                {
                    TemplateHandler.Assign(ref fiinfo, "{disabledChangeFI}",
                                           custdetail.IsDisabledFIUL ? " disabled='disabled' " : " ");
                }
                TemplateHandler.Assign(ref fiinfo, "{status}", custdetail.IsDisabledFI ? "" : "checked='checked'");
                TemplateHandler.Assign(ref fiinfo, "{statusDL}",
                                       (custdetail.IsDisabledFI || custdetail.IsDisabledFIDL) ? "" : "checked='checked'");
                TemplateHandler.Assign(ref fiinfo, "{isDisabledFI}", custdetail.IsDisabledFI ? "yes" : "no");
                TemplateHandler.Assign(ref fiinfo, "{isDisabledFIDL}",
                                       (custdetail.IsDisabledFI || custdetail.IsDisabledFIDL) ? "yes" : "no");
                if (roleid == 1)
                {
                    TemplateHandler.RemoveBlock(ref fiinfo, "chkdownline");
                }
            }
            else
            {
                TemplateHandler.RemoveBlock(ref fiinfo, "UpdatePT");
            }
            string hiddenInfo = HtmlHelper.GenHidden(Globals.HIDDEN_CustId, CastAide.AsString(custdetail.CustID));
            hiddenInfo += HtmlHelper.GenHidden(Globals.HIDDEN_Role, CastAide.AsString(custdetail.RoleId));
            TemplateHandler.Assign(ref fiinfo, "hiddenInfo", hiddenInfo);
            return fiinfo;
        }

        private static string GenHTMLBetSetting(Dictionary<string, string> listbetsetting, int sporttype, int roleid,
                                                Globals.PTType pttype)
        {
            string betsetting = "";
            try
            {
                if (pttype == Globals.PTType.Default || pttype == Globals.PTType.AgentOnMember)
                {
                    betsetting = WebUtils.LoadTemplate(TPL_FINANCE_BetSetting);
                    TemplateHandler.Assign(ref betsetting, "{minbetvalue}", listbetsetting[MINBET + "v" + sporttype]);
                    TemplateHandler.Assign(ref betsetting, "{maxbetvalue}", listbetsetting[MAXBET + "v" + sporttype]);
                    TemplateHandler.Assign(ref betsetting, "{maxpermatchvalue}",
                                           listbetsetting[MaxPerMatch + "v" + sporttype]);

                    try
                    {
                        TemplateHandler.Assign(ref betsetting, "{minbet-min}",
                                               listbetsetting[MINBET + "v" + sporttype.ToString() + "_max"]);
                        TemplateHandler.Assign(ref betsetting, "{maxbet-max}",
                                               listbetsetting[MAXBET + "v" + sporttype + "_min"]);
                        TemplateHandler.Assign(ref betsetting, "{maxpermatch-min}",
                                               listbetsetting[MaxPerMatch + "v" + sporttype + "_min"]);
                        TemplateHandler.Assign(ref betsetting, "{maxpermatch-max}",
                                               listbetsetting[MaxPerMatch + "v" + sporttype + "_max"]);

                        if (listbetsetting[MINBET + "v" + sporttype.ToString() + "_max"] == "0")
                        {
                            TemplateHandler.Assign(ref betsetting, "{hide1}", "hide");
                        }
                        if (listbetsetting[MAXBET + "v" + sporttype + "_min"] == "0")
                        {
                            TemplateHandler.Assign(ref betsetting, "{hide2}", "hide");
                        }
                        if (listbetsetting[MaxPerMatch + "v" + sporttype + "_max"] == "0")
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

                    return betsetting;
                }
                else
                {
                    betsetting = WebUtils.LoadTemplate(TPL_FINANCE_SUPERMASTER_BetSetting);
                    TemplateHandler.Assign(ref betsetting, "{minbetvalue}", listbetsetting[MINBET + "v" + sporttype]);
                    TemplateHandler.Assign(ref betsetting, "{maxbetvalue}", listbetsetting[MAXBET + "v" + sporttype]);
                    TemplateHandler.Assign(ref betsetting, "{maxpermatchvalue}",
                                           listbetsetting[MaxPerMatch + "v" + sporttype]);
                    return betsetting;
                }
                //throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        public static DbResult GetSuperPT(int custId)
        {
            try
            {
                return null; //IBCProvider.PositionTaking.GetSuperPT(custId);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        public static void SuperAddParamsPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsFI)
        {
            try
            {
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (r[prefix + SPT + "_" + t] != null)
                        paramsFI.Add(prefix + SPT + "_" + t, r[prefix + SPT + "_" + t]);
                    if (r[prefix + MPT + "_" + t] != null)
                        paramsFI.Add(prefix + MPT + "_" + t, r[prefix + MPT + "_" + t]);
                    if (r[prefix + APT + "_" + t] != null)
                        paramsFI.Add(prefix + APT + "_" + t, r[prefix + APT + "_" + t]);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void MasterAddParamsPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsFI)
        {
            try
            {
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                string isauto = "";
                foreach (int t in bettype)
                {
                    if (r[prefix + SPT + Delimiter + t] != null)
                        paramsFI.Add(prefix + SPT + Delimiter + t, r[prefix + SPT + Delimiter + t]);
                    if (r[prefix + MPT + Delimiter + t] != null)
                        paramsFI.Add(prefix + MPT + Delimiter + t, r[prefix + MPT + Delimiter + t]);
                    if (r[Auto + Delimiter + prefix + SPT + Delimiter + t] != null)
                        paramsFI.Add(Auto + Delimiter + prefix + SPT + Delimiter + t,
                                     r[Auto + Delimiter + prefix + SPT + Delimiter + t]);
                    isauto = STR_IsAuto + Delimiter + prefix + SPT + Delimiter + t;
                    if (r[isauto] != null)
                    {
                        paramsFI.Add(isauto, r[isauto]);
                        if (r[isauto] == "true")
                        {
                            paramsFI[prefix + SPT + Delimiter + t] = 0;
                        }
                        else
                        {
                            paramsFI[Auto + Delimiter + prefix + SPT + Delimiter + t] = 0;
                        }
                    }
                }
                if (r["newspositiontaking"] != null) paramsFI.Add("newspositiontaking", r["newspositiontaking"]);
                if (r["updateallmembers"] != null)
                    paramsFI.Add("updateallmembers", CastAide.AsBoolean(CastAide.AsInt16(r["updateallmembers"])));
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AgentAddParamsPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsFI)
        {
            try
            {
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (r[prefix + APT + "_" + t] != null)
                        paramsFI.Add(prefix + APT + "_" + t, r[prefix + APT + "_" + t]);
                    if (r[prefix + MPT + "_" + t] != null)
                        paramsFI.Add(prefix + MPT + "_" + t, r[prefix + MPT + "_" + t]);
                    if (r[Auto + "_" + prefix + MPT + "_" + t] != null)
                    {
                        paramsFI.Add(Auto + "_" + prefix + MPT + "_" + t, r[Auto + "_" + prefix + MPT + "_" + t]);
                    }
                    if (r[STR_IsAuto + "_" + prefix + MPT + "_" + t] != null)
                    {
                        paramsFI.Add(STR_IsAuto + "_" + prefix + MPT + "_" + t,
                                     r[STR_IsAuto + "_" + prefix + MPT + "_" + t]);
                        if (r[STR_IsAuto + "_" + prefix + MPT + "_" + t] == "true")
                        {
                            paramsFI[prefix + MPT + "_" + t] = 0;
                        }
                        else
                        {
                            paramsFI[Auto + "_" + prefix + MPT + "_" + t] = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void MemberAddParamsPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsFI)
        {
            try
            {
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (r[prefix + APT + "_" + t] != null)
                        paramsFI.Add(prefix + APT + "_" + t, r[prefix + APT + "_" + t]);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsMinMaxBet(HttpRequest r, int sporttype, ref Hashtable paramsFI)
        {
            try
            {
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                if (r[MINBET + prefix] != null) paramsFI.Add(MINBET + prefix, r[MINBET + prefix]);
                if (r[MAXBET + prefix] != null) paramsFI.Add(MAXBET + prefix, r[MAXBET + prefix]);
                if (r[MaxPerMatch + prefix] != null) paramsFI.Add(MaxPerMatch + prefix, r[MaxPerMatch + prefix]);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsDisabled(HttpRequest r, ref Hashtable paramsFI)
        {
            try
            {
                if (r[ISDISABLED] != null) paramsFI.Add(ISDISABLED_Update, r[ISDISABLED]);
                if (r[ISDOWNLINEDISABLED] != null) paramsFI.Add(ISDOWNLINEDISABLED_Update, r[ISDOWNLINEDISABLED]);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsComm(HttpRequest r, int sporttype, ref Hashtable paramsFI)
        {
            try
            {
                string comm = (sporttype > 1) ? "v" + sporttype + DISCOUNT : "";
                if (r[comm] != null) paramsFI.Add(comm, r[comm]);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsMinMaxPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsFI)
        {
            try
            {
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (r[prefix + MINPT + Delimiter + t] != null)
                        paramsFI.Add(MINPT + prefix + PT + Delimiter + t, r[prefix + MINPT + Delimiter + t]);
                    if (r[prefix + MAXPT + Delimiter + t] != null)
                        paramsFI.Add(MAXPT + prefix + PT + Delimiter + t, r[prefix + MAXPT + Delimiter + t]);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsPTMasterOnMember(HttpRequest r, int sporttype, List<int> bettype,
                                                     ref Hashtable paramsFI)
        {
            try
            {
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                string isauto = "";
                foreach (int t in bettype)
                {
                    if (r[prefix + MPT + Delimiter + t] != null)
                        paramsFI.Add(prefix + MPT + Delimiter + t, r[prefix + MPT + Delimiter + t]);

                    if (r[prefix + MPT + Delimiter + Auto + Delimiter + t] != null)
                        paramsFI.Add(Auto + Delimiter + prefix + MPT + Delimiter + "mem" + Delimiter + t,
                                     r[prefix + MPT + Delimiter + Auto + Delimiter + t]);
                    isauto = STR_IsAuto + Delimiter + prefix + MPT + Delimiter + t;

                    if (r[isauto] != null)
                    {
                        paramsFI.Add(STR_IsAuto + Delimiter + prefix + MPT + Delimiter + MEM + Delimiter + t, r[isauto]);
                        if (r[isauto] == "true")
                        {
                            paramsFI[prefix + MPT + Delimiter + t] = 0;
                        }
                        else
                        {
                            paramsFI[Auto + Delimiter + prefix + MPT + Delimiter + "mem" + Delimiter + t] = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsPTSuperOnMember(HttpRequest r, int sporttype, List<int> bettype,
                                                    ref Hashtable paramsFI)
        {
            try
            {
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (r[prefix + SPT + Delimiter + t] != null)
                        paramsFI.Add(prefix + SPT + Delimiter + t, r[prefix + SPT + Delimiter + t]);
                    if (r[prefix + SPT + Delimiter + Auto + Delimiter + t] != null)
                        paramsFI.Add(Auto + Delimiter + prefix + SPT + Delimiter + t,
                                     r[prefix + SPT + Delimiter + Auto + Delimiter + t]);
                    if (r[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + t] != null)
                        paramsFI.Add(STR_IsAuto + Delimiter + prefix + SPT + Delimiter + t,
                                     r[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + t]);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static List<int> GetBetTypeList(DataTable dt, char delimiter)
        {
            string value;
            int ivalue;
            int positon;
            var listbettype = new List<int>();
            foreach (DataColumn dtc in dt.Columns)
            {
                positon = dtc.ColumnName.LastIndexOf(delimiter);
                if (positon > 0)
                {
                    value = dtc.ColumnName.Substring(positon + 1, dtc.ColumnName.Length - positon - 1);
                    ivalue = CastAide.AsInt16(value, 0);
                    if (ivalue > 0)
                    {
                        if (!listbettype.Contains(ivalue))
                        {
                            listbettype.Add(ivalue);
                        }
                    }
                }
            }
            return listbettype;
        }

        public static Dictionary<string, double> GetValueByBetType(DataTable dt, char delimiter, int bettype)
        {
            string value;
            int ivalue;
            int positon;
            var dvalue = new Dictionary<string, double>();
            foreach (DataColumn dtc in dt.Columns)
            {
                positon = dtc.ColumnName.LastIndexOf(delimiter);
                if (positon > 0)
                {
                    value = dtc.ColumnName.Substring(positon + 1, dtc.ColumnName.Length - positon - 1);
                    ivalue = CastAide.AsInt16(value, 0);
                    if (ivalue == bettype)
                    {
                        dvalue.Add(dtc.ColumnName, CastAide.AsDouble(dt.Rows[0][dtc.ColumnName], 0));
                    }
                    else
                    {
                        if (ivalue == 0)
                        {
                            if (value.Equals("min", StringComparison.CurrentCultureIgnoreCase) ||
                                value.Equals("max", StringComparison.CurrentCultureIgnoreCase))
                            {
                                value = dtc.ColumnName.Substring(positon - 1, 1);
                                ivalue = CastAide.AsInt16(value, -1);
                                if (ivalue == bettype)
                                {
                                    if (dtc.ColumnName.Substring(0, positon).IndexOf(delimiter) > 0)
                                    {
                                        dvalue.Add(dtc.ColumnName, CastAide.AsDouble(dt.Rows[0][dtc.ColumnName], 0));
                                    }
                                }
                            }
                        }
                    }
                }
                //if (dtc.ColumnName.IndexOf(
            }
            return dvalue;
        }

        public static string GetRoleName(int RoleID)
        {
            switch (RoleID)
            {
                case 1:
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Member");
                case 2:
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent");
                case 3:
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master");
                case 4:
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Super");
            }
            return "";
        }

        public static void AssignData(int sporttype, ref CustomerDetail custDetail,
                                      ref Dictionary<string, string> betsetting, ref Dictionary<string, string> custComm,
                                      ref Dictionary<string, double> custPT, DataTable dt)
        {
            var bettype = new List<int>();
            string prefix = "";
            prefix = (sporttype > 1) ? "v" + sporttype : "";

            bettype = GetBetTypeList(dt, '_');
            //Assign PT Data
            var _custPT = new Dictionary<string, double>();
            foreach (int i in bettype)
            {
                _custPT.Clear();
                _custPT = GetValueByBetType(dt, '_', i);

                foreach (string key in _custPT.Keys)
                {
                    custPT.Add(key, _custPT[key]);
                }
            }

            //Assign Cust Detail
            custDetail.UserName = dt.Rows[0]["username"].ToString();
            custDetail.FirstName = dt.Rows[0]["firstname"].ToString();
            custDetail.LastName = dt.Rows[0]["lastname"].ToString();
            custDetail.Phone = dt.Rows[0]["phone"].ToString();
            custDetail.MobilePhone = dt.Rows[0]["mobilephone"].ToString();
            custDetail.Fax = dt.Rows[0]["Fax"].ToString();

            try
            {
                custDetail.IsDisabledFI = CastAide.AsBoolean(dt.Rows[0][ISDISABLED], false);
                custDetail.IsDisabledFIDL = CastAide.AsBoolean(dt.Rows[0][ISDOWNLINEDISABLED], false);
            }
            catch (Exception ex)
            {
            }
            try
            {
                custDetail.IsDisabledFIUL = CastAide.AsBoolean(dt.Rows[0][ISUPLINEDISABLED], false);

                betsetting.Add(MINBET + prefix + "_max",
                               FormatHandler.FormatNumber1(CastAide.AsDouble(dt.Rows[0][MINBET + prefix + "_max"])));
                betsetting.Add(MAXBET + prefix + "_min",
                               FormatHandler.FormatNumber1(CastAide.AsDouble(dt.Rows[0][MAXBET + prefix + "_min"])));
                betsetting.Add(MaxPerMatch + prefix + "_min",
                               FormatHandler.FormatNumber1(CastAide.AsDouble(dt.Rows[0][MaxPerMatch + prefix + "_min"])));
                betsetting.Add(MaxPerMatch + prefix + "_max",
                               FormatHandler.FormatNumber1(CastAide.AsDouble(dt.Rows[0][MaxPerMatch + prefix + "_max"])));
            }
            catch (Exception ex)
            {
            }

            //Assign Cust BetSetting
            try
            {
                betsetting.Add(MINBET + prefix,
                               FormatHandler.FormatNumber1(CastAide.AsDouble(dt.Rows[0][MINBET + prefix])));
                betsetting.Add(MAXBET + prefix,
                               FormatHandler.FormatNumber1(CastAide.AsDouble(dt.Rows[0][MAXBET + prefix])));
                betsetting.Add(MaxPerMatch + prefix,
                               FormatHandler.FormatNumber1(CastAide.AsDouble(dt.Rows[0][MaxPerMatch + prefix])));
            }
            catch (Exception ex)
            {
            }

            //Assign Cust Comm
        }

        public static DataTable GenSampleDataTablePT()
        {
            var dt = new DataTable();
            var dtc1 = new DataColumn("v201spositiontaking_2", Type.GetType("System.Double"));
            var dtc2 = new DataColumn("v201mpositiontaking_2", Type.GetType("System.Double"));
            var dtc3 = new DataColumn("v201apositiontaking_2", Type.GetType("System.Double"));
            var dtc4 = new DataColumn("v201spositiontaking_3", Type.GetType("System.Double"));
            var dtc5 = new DataColumn("v201mpositiontaking_3", Type.GetType("System.Double"));
            var dtc6 = new DataColumn("v201apositiontaking_3", Type.GetType("System.Double"));

            var dtc7 = new DataColumn(MINBET + "v201", Type.GetType("System.Double"));
            var dtc8 = new DataColumn(MAXBET + "v201", Type.GetType("System.Double"));
            var dtc9 = new DataColumn(MaxPerMatch + "v201", Type.GetType("System.Double"));
            var dtc10 = new DataColumn(ISUPLINEDISABLED, Type.GetType("System.Boolean"));
            var dtc11 = new DataColumn(ISDISABLED, Type.GetType("System.Boolean"));
            var dtc12 = new DataColumn(ISDOWNLINEDISABLED, Type.GetType("System.Boolean"));

            var dtc13 = new DataColumn("firstname", Type.GetType("System.String"));
            var dtc14 = new DataColumn("lastname", Type.GetType("System.String"));
            var dtc15 = new DataColumn("phone", Type.GetType("System.String"));
            var dtc16 = new DataColumn("mobilephone", Type.GetType("System.String"));
            var dtc17 = new DataColumn("Fax", Type.GetType("System.String"));
            var dtc18 = new DataColumn("username", Type.GetType("System.String"));

            var dtc19 = new DataColumn(MINBET + "v201_max", Type.GetType("System.Double"));
            var dtc20 = new DataColumn(MAXBET + "v201_min", Type.GetType("System.Double"));
            var dtc21 = new DataColumn(MaxPerMatch + "v201_min", Type.GetType("System.Double"));
            var dtc22 = new DataColumn(MaxPerMatch + "v201_max", Type.GetType("System.Double"));

            dt.Columns.Add(dtc1);
            dt.Columns.Add(dtc2);
            dt.Columns.Add(dtc3);
            dt.Columns.Add(dtc4);
            dt.Columns.Add(dtc5);
            dt.Columns.Add(dtc6);
            dt.Columns.Add(dtc7);
            dt.Columns.Add(dtc8);
            dt.Columns.Add(dtc9);
            dt.Columns.Add(dtc10);
            dt.Columns.Add(dtc11);
            dt.Columns.Add(dtc12);
            dt.Columns.Add(dtc13);
            dt.Columns.Add(dtc14);
            dt.Columns.Add(dtc15);
            dt.Columns.Add(dtc16);
            dt.Columns.Add(dtc17);
            dt.Columns.Add(dtc18);
            dt.Columns.Add(dtc19);
            dt.Columns.Add(dtc20);
            dt.Columns.Add(dtc21);
            dt.Columns.Add(dtc22);

            DataRow row;
            row = dt.NewRow();
            row["v201spositiontaking_2"] = "0.9";
            row["v201mpositiontaking_2"] = "0.9";
            row["v201apositiontaking_2"] = "0.9";
            row["v201spositiontaking_3"] = "0.85";
            row["v201mpositiontaking_3"] = "0.85";
            row["v201apositiontaking_3"] = "0.85";

            row[MINBET + "v201"] = "0";
            row[MAXBET + "v201"] = "0";
            row[MaxPerMatch + "v201"] = "0";
            row[MINBET + "v201_max"] = "12";
            row[MAXBET + "v201_min"] = "60";
            row[MaxPerMatch + "v201_min"] = "12";
            row[MaxPerMatch + "v201_max"] = "1000";

            row[ISUPLINEDISABLED] = "false";
            row[ISDISABLED] = "true";
            row[ISDOWNLINEDISABLED] = "true";

            row["username"] = "phuongnguyen";
            row["firstname"] = "Phuong";
            row["lastname"] = "nguyen";
            row["phone"] = "123456789";
            row["mobilephone"] = "123456789";
            row["Fax"] = "123456789";

            dt.Rows.Add(row);
            return dt;
        }
    }
}