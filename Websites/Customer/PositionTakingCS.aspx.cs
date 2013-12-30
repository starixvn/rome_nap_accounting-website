using System;
using System.Web.UI;

using BLL;
using DNAide.Core;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Resources;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Entities.PositionTaking.Racing;
using Sunplus.Accounting.Business.Customer;
using Fanex.Tarus.DataAccess.Implement.Customer;
using System.Linq;


public partial class PositionTakingCS : RequiredAuthorizationPage
{

    //protected string PageTitle = Resources.Language.lblUpdatePTCasino;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetNoStore();

        try
        {
            if (CastAide.AsInt32(Request["custid"], 0) != 0)
            {
                Globals.PTType ptType = (Globals.PTType)CastAide.AsInt32(Request["pttype"], 0);
                CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
                CSCustomerDetail _c = CasinoManager.GetCustInfoIBC(int.Parse(Request["custid"]));

                if (CastAide.AsInt32(Request["FlagUnSyn"], 0) == 1)
                    _c.CasinoSync = false;

                //Not available user
                if (_c.NoIBCData)
                {
                    ctrHTML.Text = Language.thereisnodata;
                    return;
                }

                #region //Get upline information
                int uplineId = 0;
                string urlAgentList = "";
                string strUrl = "parent.location.href";
                string strQuery = "strSearch=search&txtSearch=" + _c.UserName;
                bool isAllowHR = _c.CasinoSync;
                switch ((Globals.AccountType)_c.RoleId)
                {
                    case Globals.AccountType.Super:
                        //urlAgentList = "<script>DelayReloadPage('SuperAgentList.aspx?" + strQuery + "&hr=" + (!WebSettings.disabledHR) + "', 2000);</script>";
                        urlAgentList = "SuperAgentList.aspx?" + strQuery;
                        uplineId = 0;
                        break;
                    case Globals.AccountType.Master:

                        uplineId = _c.SIBCCustID;

                        if (uplineId < 100)
                        {
                            //urlAgentList = "<script type='text/javascript'>DelayReloadPage('MasterAgentList.aspx?bycom=1" + strQuery + "&hr=" + (!WebSettings.disabledHR) + "',2000,'" + strUrl + "');</script>";
                            urlAgentList = "MasterAgentList.aspx?bycom=1&" + strQuery;
                        }
                        else
                        {

                            //urlAgentList = "<script type='text/javascript'>DelayReloadPage('MasterAgentList.aspx?superId=" + uplineId + "" + strQuery + "&hr=" + (isAllowHR && !WebSettings.disabledHR).ToString() + "',2000,'" + strUrl + "');</script>";
                            urlAgentList = "MasterAgentList.aspx?superId=" + uplineId + "&" + strQuery;
                        }

                        break;
                    case Globals.AccountType.Agent:
                        uplineId = _c.MIBCCustID;

                        //urlAgentList = "<script type='text/javascript'> DelayReloadPage('AgentList.aspx?masterId=" + uplineId + "&hr=" + (isAllowHR && !WebSettings.disabledHR).ToString() + "',2000,'" + strUrl + "'); </script>";
                        urlAgentList = "AgentList.aspx?masterId=" + uplineId;

                        break;
                    case Globals.AccountType.Member:
                        uplineId = _c.AIBCCustID;

                        //urlAgentList = "<script type='text/javascript'>DelayReloadPage('MemberList.aspx?agentId=" + uplineId + "&hr=" + (isAllowHR && !WebSettings.disabledHR).ToString()+ "',2000);</script>";
                        urlAgentList = "MemberList.aspx?agentId=" + uplineId;
                        break;
                }

                var _cUpline = new CSCustomerDetail();

                #endregion
                #region set RNG casino site
                _cust.SiteName = _c.SiteName;
                #endregion

                #region check sync data on casino system
                try
                {
                    if (_c.RoleId < (int)CustInfo.Roles.Super)
                    {
                        _cUpline = CasinoManager.GetCustInfoIBC(uplineId);
                        //                        var uplineDetail = IBCProvider.IBCCustomer.GetBasicCustInfoIBC(CastAide.AsInt32(uplineId));
                        //casinoLibray.CheckCustSetting(userId)
                        bool syncInfo = _cust.CheckCustSetting(_c.IBCCustID);
                        if (syncInfo && !_c.CasinoSync)
                        {
                            _c.CasinoSync = true;
                            var isUpdIbc = CasinoProvider.Instance.UpdateCSStatusAfterSync(_c.IBCCustID, _c.CasinoSync, _cUpline.CasinoDisabled, AppSession.AccId, BaseManager.CurrentUserIp);
                        }
                        else if (!syncInfo && _c.CasinoSync)
                            _c.CasinoSync = false;
                    }
                }
                catch
                {
                }

                #endregion

                if (0 == uplineId) //super user
                {
                    if (_c.CasinoSync) //this customer have already been sync to Casino 
                        CasinoManager.GetCustSettingCS(ref _c); //get more information from Casino
                    
                    bool _IsLicensee = IsLicensee(_c.SiteName);
                    var group = _IsLicensee ? Globals.CasinoGroup.Group2 : Globals.CasinoGroup.Group1;
                    ctrHTML.Text = CasinoManager.GenCasinoPTHTML(_c, group, ptType);
                }
                else //other user
                {
                    if (_cUpline.NoIBCData)
                    {
                        ctrHTML.Text = Language.thereisnodata;
                        return;
                    }

                    if (!_cUpline.CasinoSync) //Upline user have not been sync to Casino yet
                    {
                        bool isInsert = CastAide.AsBoolean(Request["insert"], false);
                        if (isInsert == true)
                        {
                            Response.Redirect(urlAgentList);
                        }
                        else
                        {
                            ctrHTML.Text = Resources.Language.lblUplineHaveNotSyncToCasino;//Language.thereisnodata;
                        }
                        return;
                    }

                    CasinoManager.GetCustSettingCS(ref _cUpline);
                    _c.UpperCustID = uplineId;
                    if (_c.CasinoSync) //this customer have already been sync to Casino 
                    {
                        CasinoManager.GetCustSettingCS(ref _c); //get more information from Casino
                    }

                    _c.PTByGameTypeUpline = _cUpline.PTByGameType;
                    _c.PTByGameType = _cUpline.PTByGameType;
                    
                    _c.PTByGameTypeUpline = _cUpline.PTByGameType;
                    _c.PTByGameType = _cUpline.PTByGameType;
                    bool _IsLicensee = IsLicensee(_c.SiteName);
                    var group = _IsLicensee ? Globals.CasinoGroup.Group2 : Globals.CasinoGroup.Group1;
                    ctrHTML.Text = CasinoManager.GenCasinoPTHTML(_c, group, ptType);
                }

            }
            else
                ctrHTML.Text = Language.thereisnodata;
            AddJsVariable("fullSettlementMsg", Casino.fullSettlement);
        }
        catch (Exception ex)
        {

            ctrHTML.Text = AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
        }

    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
        WriteJsVariables(writer);
    }

    private readonly System.Text.StringBuilder _jsVariables = new System.Text.StringBuilder(5000);
    public void AddJsRawVariable(string strName, string value)
    {
        _jsVariables.Append(strName + "=" + value.ToString() + ";\r\n");
    }

    public void AddJsVariable(string strName, string value)
    {
        _jsVariables.Append(strName + "='" + value.ToString().Replace("'", "\\'") + "';\r\n");
    }

    private void WriteJsVariables(HtmlTextWriter writer)
    {
        if (_jsVariables.Length == 0) return;
        // write javascript variables
        writer.WriteLine("<script type=\"text/javascript\">\r\n");
        writer.WriteLine(_jsVariables.ToString());
        writer.WriteLine("</script>\r\n");
    }

    bool IsLicensee(string siteName)
    {

        var licenseeAccess = new DepositCustomerAccess();
        var dtLicensee = licenseeAccess.SelAllLicensee();
        bool result = dtLicensee.Select().Where(s => CastAide.AsString(s["LicenseeName"]) == siteName).Count() > 0;
        return result;
    }
}
