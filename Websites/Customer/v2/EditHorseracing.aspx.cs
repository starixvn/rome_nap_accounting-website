using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using IBCUtils;

using Resources;
using DNAide.Data;



public partial class Customer_V2_EditHorseracing : PageBase
{

    #region Declare variable

    public StringBuilder content = new StringBuilder();
    protected string ControlHandler = "UpdateCustomerHR.ashx";
    private int custId = 0;
    protected string fromSet = "";
    protected string msg = "";
    private int result = 0;
    private int roleId = 0;
    protected string type = "";
    protected string username = "";

    #endregion

    /*@phuong.nguyen
     * Render edit horseracing form
     * */

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (!string.IsNullOrEmpty(Request.QueryString["CustId"]) && !string.IsNullOrEmpty(Request.QueryString["roleid"]))
        {
            custId = Convert.ToInt32(Request.QueryString["CustId"]);
            roleId = Convert.ToInt32(Request.QueryString["roleid"]);
        }
        else if (!string.IsNullOrEmpty(Request.Form["CustId"]) && !string.IsNullOrEmpty(Request.Form["roleid"]))
        {
            custId = Convert.ToInt32(Request.Form["CustId"]);
            roleId = Convert.ToInt32(Request.Form["roleid"]);
        }
        else
        {
            custId = Convert.ToInt32(Session["CustId"]);
            roleId = Convert.ToInt32(Session["roleId"]);
        }
        if (!string.IsNullOrEmpty(Request.QueryString["result"]))
            result = Convert.ToInt32(Request.QueryString["result"]);
        if (!WebSettings.disabledHR && custId != 0)
            InitHRInfo(custId);
    }

    #region Init HR Info

    private void InitHRInfo(int intCustId)
    {
        try
        {
            #region IBCUtils Lib
            /*
            DataResult dsHRInfo = CustomerManager.GetAllHRSetting(intCustId, roleId);
            if (dsHRInfo.ResultsSet.Tables[0].Rows.Count == 0)
            {
                return;
            }
            DataRow drAcc = dsHRInfo.ResultsSet.Tables[0].Rows[0];
            DataRow drHRSetting = dsHRInfo.ResultsSet.Tables[1].Rows[0];
            */
            #endregion

            #region DNAide Lib
            DbResult dsHRInfo = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerManager.GetAllHRSetting(intCustId, roleId);
            if (dsHRInfo.ResultSet.Tables[0].Rows.Count == 0)
            {
                return;
            }
            DataRow drAcc = dsHRInfo.ResultSet.Tables[0].Rows[0];
            DataRow drHRSetting = dsHRInfo.ResultSet.Tables[1].Rows[0];
            #endregion


            fromSet = Request.QueryString["set"];
            custId = Convert.ToInt32(drAcc["custid"]);
            string firstname = Convert.IsDBNull(drAcc["firstname"]) ? "" : drAcc["firstname"].ToString();
            string lastname = Convert.IsDBNull(drAcc["lastname"]) ? "" : drAcc["lastname"].ToString();
            string phone = drAcc["phone"] + "";
            string mobile = drAcc["mobilephone"] + "";
            string fax = drAcc["fax"] + "";
            username = CastAide.AsString(drAcc["username"], string.Empty);
            int _ParentId = 0;
            if (roleId != 4)
                _ParentId = Convert.ToInt32(drAcc["parentid"]);
            //Horseraing information
            bool _ParentDisabledHR = false;
            if (0 != _ParentId)
                _ParentDisabledHR = Convert.IsDBNull(drAcc["DisableHorseracingParent"])
                                        ? false
                                        : Convert.ToBoolean(drAcc["DisableHorseracingParent"]);
            bool _DisabledHR = Convert.IsDBNull(drAcc["DisableHorseracing"])
                                   ? false
                                   : Convert.ToBoolean(drAcc["DisableHorseracing"]);
            bool _ADisabledHR = false;
            if (roleId == 1)
            {
                _ADisabledHR = false;
            }
            else if (_DisabledHR)
            {
                _ADisabledHR = true;
            }

            else
            {

                _ADisabledHR = Convert.IsDBNull(drAcc["DisableDownlinesHR"])
                                            ? false
                                            : Convert.ToBoolean(drAcc["DisableDownlinesHR"]);
            }
            //Horseracing Bet Setting Info
            double _HRMinBet = FormatHandler.GetDouble(drHRSetting["minbet"]);
            double _HRMaxBet = FormatHandler.GetDouble(drHRSetting["maxbet"]);
            double _HRMinBet_Min = 0.00;
            double _HRMaxBet_Max = 0.00;
            double _HRMaxPerMactch_Max = 0.00;
            double _OtherComm_Max = 0.00;
            double _UKComm_Max = 0.00;
            if (roleId != 4)
            {
                _HRMinBet_Min = FormatHandler.GetDouble(drHRSetting["MinBet_Min"]);
                _HRMaxBet_Max = FormatHandler.GetDouble(drHRSetting["MaxBet_Max"]);
                _HRMaxPerMactch_Max = CastAide.AsDouble(drHRSetting["MaxperMatch_Max"]);
                _OtherComm_Max = FormatHandler.GetDouble(drHRSetting["OtherComm_Max"]);
                _UKComm_Max = FormatHandler.GetDouble(drHRSetting["UKComm_Max"]);
            }
            double _HRMaxPerMatch = FormatHandler.GetDouble(drHRSetting["maxpermatch"]);
            //Horseracing Commission Info
            double _OtherComm = FormatHandler.GetDouble(drHRSetting["OtherComm"]);
            double _UKComm = FormatHandler.GetDouble(drHRSetting["UKComm"]);
            content =
                LoadHRTemplate(username, firstname, lastname, phone, mobile, fax, custId, roleId, _ParentId,
                               _ParentDisabledHR, _DisabledHR, _ADisabledHR, _HRMinBet, _HRMaxBet, _HRMinBet_Min, _HRMaxBet_Max,
                               _HRMaxPerMactch_Max, _HRMaxPerMatch, _OtherComm, _UKComm,
                               _OtherComm_Max, _UKComm_Max);

        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }

    #endregion

    #region VariableScript

    private string VariableScript()
    {
        string script = "";
        script += "<script language='javascript'>";
        script += " var lblPasswordRequire='" + Language.lblPasswordRequire + "';";
        script += " var alertPasswordEmpty='" + Language.alertPasswordEmpty + "';";
        script += " var alertUserName='" + Language.alertUserName + "';";
        script += " var alertCompare3='" + Language.alertMaxLessMin + "'; ";
        script += " var alertMinbet='" + Language.alertIncorrectMinBet + "'; ";
        script += " var alertMaxbet='" + Language.alertIncorrectMaxBet + "';";
        script += " var alertMaxpermatch='" + Language.alertIncorrectMaxPerMatch + "';";
        script += " var alertCompare='" + Language.alertMaxLessMaxPer + "'; ";
        script += " var alertIncorrectCredit='" + Language.alertIncorrectCredit + "';";
        script += " var alertMinLessCredit='" + Language.alertMinLessCredit + "';";
        script += " var alertSite='" + Language.alertSite + "';";
        script += " var alertCurrency='" + Language.alertCurrency + "';";
        script += " var alertMinbetNegative='" + Language.lblalertMinBetNegative + "';";
        script += " var alertMaxbetNegative='" + Language.lblalertMaxBetNegative + "';";
        script += " var alertMaxperMatchNegative='" + Language.lblalertMaxPerMatchNegative + "';";
        script += " var alertminbetC='" + Language.lblalertMinbetCompare + "';";
        script += " var alertMaxbetC='" + Language.lblalerMaxbetCompare + "';";
        script += " var alertMaxpermatchC='" + Language.lblalertMaxPerMatchCompare + "';";
        script += " var alertMaxpermatchCMin='" + Language.lblalertMaxPerMatchCompareMin + "';";
        script += " var alertMaxpermatchCMax='" + Language.lblalertMaxPerMatchCompareMax + "';";
        script += " var alertDisableHR=\"" + Language.lblAlertDisableHR + "\";";
        script += " var alertDisableHRMember=\"" + Language.lblAlertDisableHRMember + "\";";
        script += " var alertEnableHR=\"" + Language.lblAlertEnableHR + "\";";
        script += " var alertEnableSubLevelHR=\"" + Language.lblAlertEnableHRDownline + "\";";
        string lblCreditInvalid = "";
        lblCreditInvalid = Language.lblCreditInvalid.Replace("{0}", Language.lblMemberMaxCredit);
        script += " var alertCreditInvalid='" + lblCreditInvalid + "';";
        script += "</script>";
        return script;
    }

    #endregion

    #region Load HR template setting form

    private StringBuilder LoadHRTemplate(string _Username, string _FirstName, string _LastName, string _Phone,
                                         string _MobilePhone, string _Fax, int _CustId, int _RoldeId, int _ParentId,
                                         bool _ParentDisabledHR, bool _DisabledHR, bool _ADisabledHR, double _HRMinBet, double _HRMaxBet,
                                         double _HRMinBet_Min, double _HRMaxBet_Max, double _HRMaxPerMatch_Max,
                                         double _HRMaxPerMatch, double _OtherComm, double _UKComm, double _OtherComm_Max,
                                         double _UKComm_Max)
    {
        StringBuilder retValue = new StringBuilder();
        try
        {
            string tplHR = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/HorseracingInfo.htm");
            string strEditType = "";
            string strHRCommParent = "";
            string strHRComm = "";
            string strlblChk = "";
            switch (_RoldeId)
            {
                case 1:
                    strlblChk = "Member";
                    strEditType = Language.EditMember;
                    strHRComm = Language.CommissionForMember;
                    strHRCommParent = Language.CommissionForAgent;
                    TemplateHandler.RemoveBlock(ref tplHR, "unlimited-disable-downline");
                    TemplateHandler.RemoveBlock(ref tplHR, "comm-not-member");
                    break;
                case 2:
                    strlblChk = "Agent";
                    strEditType = Language.EditAgent;

                    TemplateHandler.RemoveBlock(ref tplHR, "limited-disable-downline");
                    TemplateHandler.RemoveBlock(ref tplHR, "comm-member");
                    break;
                case 3:
                    strlblChk = "Master";
                    strEditType = Language.EditMaster;
                    TemplateHandler.RemoveBlock(ref tplHR, "limited-disable-downline");
                    TemplateHandler.RemoveBlock(ref tplHR, "comm-member");
                    break;
                default:
                    strlblChk = "Super";
                    strEditType = strEditType = Language.EditSuper;
                    TemplateHandler.RemoveBlock(ref tplHR, "limited-disable-downline");
                    TemplateHandler.RemoveBlock(ref tplHR, "comm-member");
                    break;
            }
            TemplateHandler.Assign(ref tplHR, "{VariableScript}", VariableScript());
            TemplateHandler.Assign(ref tplHR, "{add-edit-agent}",
                                      strEditType + " " + _Username + " " + Language.lblHRSettingTitle);
            TemplateHandler.Assign(ref tplHR, "{username}", _Username);
            TemplateHandler.Assign(ref tplHR, "{lblUsername}", Language.lblUsername);
            TemplateHandler.Assign(ref tplHR, "{lblFirstname}", Language.lblFirstname);
            TemplateHandler.Assign(ref tplHR, "{firstname}", _FirstName);
            TemplateHandler.Assign(ref tplHR, "{lblLastname}", Language.lblLastname);
            TemplateHandler.Assign(ref tplHR, "{lastname}", _LastName);
            TemplateHandler.Assign(ref tplHR, "{lblPhone}", Language.lblPhone);
            TemplateHandler.Assign(ref tplHR, "{phone}", _Phone);
            TemplateHandler.Assign(ref tplHR, "{lblMobilePhone}", Language.lblMobilePhone);
            TemplateHandler.Assign(ref tplHR, "{mobile}", _MobilePhone);
            TemplateHandler.Assign(ref tplHR, "{lblFax}", Language.lblFax);
            TemplateHandler.Assign(ref tplHR, "{fax}", FormatHandler.FormatNumber1(_Fax));
            TemplateHandler.Assign(ref tplHR, "{status}", _DisabledHR ? "" : "checked='true'");
            TemplateHandler.Assign(ref tplHR, "{statusDL}", _ADisabledHR ? "" : "checked='true'");
            TemplateHandler.Assign(ref tplHR, "{lblCommHR}", Language.lblCommHR);
            TemplateHandler.Assign(ref tplHR, "{lblUKMeeting}", Language.lblUKMeeting);
            TemplateHandler.Assign(ref tplHR, "{lblOthersMeeting}", Language.lblOthersMeeting);
            TemplateHandler.Assign(ref tplHR, "{lblBetSetting}", Language.lblBetSetting);
            TemplateHandler.Assign(ref tplHR, "{lblUpdate}", Language.lblUpdate);
            TemplateHandler.Assign(ref tplHR, "{lblDisabledHR}", Language.lblDisabledHR);
            TemplateHandler.Assign(ref tplHR, "{lblProcessing}", Language.lblProcessing);
            
            TemplateHandler.Assign(ref tplHR, "{lblIsUpdDownlineHR}", Language.lblIsUpdDownlineHR);
            TemplateHandler.Assign(ref tplHR, "{lblHRComm}", strHRComm);
            TemplateHandler.Assign(ref tplHR, "{lblHRCommParent}", strHRCommParent);
            TemplateHandler.Assign(ref tplHR, "{UKCommParent}", _UKComm_Max.ToString());
            TemplateHandler.Assign(ref tplHR, "{OthersCommParent}", _OtherComm_Max.ToString());
            TemplateHandler.Assign(ref tplHR, "{oldHRStatus}", (_DisabledHR == true) ? "1" : "0");
            TemplateHandler.Assign(ref tplHR, "{oldHRStatusDL}", (_ADisabledHR == true) ? "1" : "0");
            
            
            TemplateHandler.Assign(ref tplHR, "{lblagenttype}", strlblChk);
            TemplateHandler.Assign(ref tplHR, "{lblagenttypeDL}", strlblChk + "'s downlines");

            TemplateHandler.Assign(ref tplHR, "{role-id}", _RoldeId.ToString());
            TemplateHandler.Assign(ref tplHR, "{cust-id}", _CustId.ToString());

            string disabledHR = "";
            if (4 == _RoldeId || !_ParentDisabledHR)
            {
                disabledHR = string.Empty;
            }
            else
            {
                disabledHR = "disabled='disabled'";
            }
            if (_RoldeId == 1)
            {
                TemplateHandler.RemoveBlock(ref tplHR, "chkdownline");
            }
            TemplateHandler.Assign(ref tplHR, "{disabledChangeHR}", disabledHR);
            TemplateHandler.Assign(ref tplHR, "{listUKMeetingValue}", CustomerUtils.GetListGroup((_RoldeId == 4)? WebSettings.UkMeetingMax : _UKComm_Max, _UKComm, 25));
                //25
            TemplateHandler.Assign(ref tplHR, "{listOthersMeetingValue}",
                                      CustomerUtils.GetListGroup((_RoldeId == 4) ? WebSettings.OtherMeetingMax : _OtherComm_Max, _OtherComm, 25));
            string scriptMsg = "";
            if (result == 1)
                scriptMsg = "<script type='text/javascript'>alert_error('" + Language.lblUpdatedHRSuccessfull +
                            "','error151');</script>";
            TemplateHandler.Assign(ref tplHR, "{ScriptMsg}", scriptMsg);
            string tplHRBetSetting = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/BetSetting.htm");
            tplHRBetSetting =
                AddBetSetting(tplHRBetSetting, "151", _RoldeId, _HRMinBet, _HRMaxBet, _HRMaxPerMatch, _HRMinBet_Min,
                              _HRMaxBet_Max, _HRMaxPerMatch_Max);
            TemplateHandler.Assign(ref tplHR, "{BetSetting}", tplHRBetSetting);

            retValue = new StringBuilder(tplHR);
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return retValue;
    }

    #endregion

    #region temp Add Bet Setting 

    private string AddBetSetting(string tpl, string sporttype, int roleId, double minbet, double maxbet,
                                 double maxpermatch, double minbet_min, double maxbet_max, double maxpermatch_max)
    {
        try
        {
            TemplateHandler.Assign(ref tpl, "{sporttype}", sporttype);
            TemplateHandler.Assign(ref tpl, "{minbet-text}", Language.lblMinBet);
            TemplateHandler.Assign(ref tpl, "{maxbet-text}", Language.lblMaxBet);
            TemplateHandler.Assign(ref tpl, "{maxpermatch-text}", Language.lblMaxPerMatch);
            TemplateHandler.Assign(ref tpl, "{bet-text}", Language.lblBetSetting);
            TemplateHandler.Assign(ref tpl, "{minbet" + sporttype + "-val}", FormatHandler.FormatNumber1(minbet));
            TemplateHandler.Assign(ref tpl, "{maxbet" + sporttype + "-val}", FormatHandler.FormatNumber1(maxbet));
            TemplateHandler.Assign(ref tpl, "{maxpermatch" + sporttype + "-val}",
                                      FormatHandler.FormatNumber1(maxpermatch));
            TemplateHandler.Assign(ref tpl, "{mbMin}", (roleId == 4 || minbet_min <= 0) ? "hide" : "minmaxbet");
            TemplateHandler.Assign(ref tpl, "{mbMax}", (roleId == 4 || maxbet_max <= 0) ? "hide" : "minmaxbet");
            TemplateHandler.Assign(ref tpl, "{mpmMax}", (roleId == 4 || maxpermatch_max <= 0) ? "hide" : "minmaxbet");
            TemplateHandler.Assign(ref tpl, "{minbet-min-" + sporttype + "}", FormatHandler.FormatNumber1(minbet_min));
            TemplateHandler.Assign(ref tpl, "{maxbet-max-" + sporttype + "}", FormatHandler.FormatNumber1(maxbet_max));
            TemplateHandler.Assign(ref tpl, "{mpmMin}", "hide");
            TemplateHandler.Assign(ref tpl, "{maxpermatch-max-" + sporttype + "}",
                                      FormatHandler.FormatNumber1(maxpermatch_max));
            return tpl;
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }

    #endregion
}
