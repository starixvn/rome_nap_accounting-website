<%@ WebHandler Language="C#" Class="UpdateCSSetting" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using CasinoLibrary;
using CasinoLibrary.SBAgent;
using System.Data;

using BLL;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Core.LoggingLib;



public class UpdateCSSetting : BaseHandler
{
    public override void ProcessRequest(HttpContext context)
    {
        base.ProcessRequest(context);
        context.Response.ContentType = "text/html";

        context.Response.Cache.SetNoStore();
        string errmsg = "";
        //context.Session.Clear();
        if (AppSession.AccId == null || AppSession.AccId == 0)
        {
            //context.Response.Write("<script>parent.scroll('0', '0'); parent.ShowMessage('You session has been timeout. Please login again.');</script>");
            AuthManager.DoLogout();
            return;
        }
        else if (Permissions.CheckPermission(AppSession.AccId, Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
        {
            context.Response.Write("<script>parent.scroll('0', '0'); parent.ShowMessage('" + AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error) + "');parent.afterUpdate(false,2000);</script>");
            return;
        }
        else
            SynCasino(context);
    }

    protected override int CustId
    {
        get
        {
            var customerInfo = (CSCustomerDetail) (HttpContext.Current.Session[Globals.SESSION_CUSTOMER]);
            return customerInfo.IBCCustID;
        }
    }
    
    public string RenderTable(DataTable dt)
    {
        string strRet = "<table border=1>";
        strRet += "<tr>";
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            strRet += "<td>" + dt.Columns[i].ColumnName + "</td>";
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strRet += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                strRet += "<td>" + dt.Rows[i][j].ToString() + "</td>";
            }
            strRet += "</tr>";
        }

        strRet += "</tr>";
        strRet += "</table><br />";

        return strRet;
    }

    public void SynCasino(HttpContext context)
    {
        bool status = false;
        int custid = 0;
        string roleid;
        CSCustomerDetail custInfo = null;
        HttpRequest r = null;
        string strReturn = "<script>parent.ShowMessage('msg')</script>"; // Return error script
        Customer csCustomer = new CasinoLibrary.SBAgent.Customer();
        bool isEnableCasino = true;
        
        try
        {
            custInfo = (CSCustomerDetail)(context.Session[Globals.SESSION_CUSTOMER]);
            bool bInserting = !custInfo.CasinoSync;
            
            r = context.Request;
            custid = custInfo.IBCCustID;
            roleid = custInfo.RoleId.ToString();

            bool bPTByGameType = Convert.ToBoolean(r[Globals.HIDDEN_PTByGameType]);

            bool isS = roleid == Globals.SuperLevel.ToString();
            bool isM = roleid == Globals.MasterLevel.ToString();
            bool isA = roleid == Globals.AgentLevel.ToString();
            bool isC = roleid == Globals.MemberLevel.ToString();
            
            
            if (isS)
            {
                roleid = Globals.Super;
            }
            else if (isM)
            {
                bPTByGameType = custInfo.PTByGameType;
                roleid = Globals.Master;
            }
            else if (isA)
            {
                bPTByGameType = custInfo.PTByGameType;
                roleid = Globals.Agent;
            }

            // Format gameKind1[gameType1(stake1, stake2)-gameType2(stake3, stake4))];gameKind2...        

            string strCS = r["hidCSData"]; // Ex: "8[1(24, 25, 29, 30)-2(1, 2, 3, 4)];10[1(29, 30)-2(3, 4)];"
            if (string.IsNullOrEmpty(strCS)) return;
            string[] gameKinds = strCS.Split(new string[] { Globals.SEP_GAMEKIND }, StringSplitOptions.RemoveEmptyEntries); // Ex: 10[1(29, 30)-2(3, 4)]
            int kc = gameKinds.Length; //

            if (kc == 0)
            {
                context.Response.Write("BAD POST DUE TO UNKNOWN DATA");
                return;
            }

            bool bGameKindGroup = r[GetGameKindId(gameKinds[0]) + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CHKGROUP] != null;

            // Upd cust setting max win, max lose
            decimal maxWin = 0;
            decimal maxLose = 0;

            int intMaxTransfer = 0;
            if (!int.TryParse("0" + r[Globals.TXT_MAXTRANSTERCREDIT], out intMaxTransfer))
            {
                context.Response.Write(@"<script>parent.ShowMessage('" + AlertMessage.GetMsg(Resources.Language.maxTransferInvalid, AlertMessage.MsgType.Error) + "'); parent.document.getElementById('" + Globals.TXT_MAXTRANSTERCREDIT + "').focus(); </script>");
                return;
            }

            bool isMaxTransfer = true;

            if (isS)
            {

                if (intMaxTransfer < 0 || intMaxTransfer < (int)(custInfo.MinTransferCreditLimit))
                {
                    context.Response.Write(@"<script>parent.ShowMessage('" + AlertMessage.GetMsg(Resources.Language.transferCreditMsg, AlertMessage.MsgType.Error) + "'); parent.document.getElementById('" + Globals.TXT_MAXTRANSTERCREDIT + "').focus(); </script>");
                    return;                    
                }
                
            }
            else
            {
                if (intMaxTransfer > (int)(custInfo.MaxTransferCreditLimit) || intMaxTransfer < (int)(custInfo.MinTransferCreditLimit)) // Show error
                {
                    context.Response.Write(@"<script>parent.ShowMessage('" + AlertMessage.GetMsg(Resources.Language.transferCreditMsg, AlertMessage.MsgType.Error) + "'); parent.document.getElementById('" + Globals.TXT_MAXTRANSTERCREDIT + "').focus(); </script>");
                    return;
                }
            }                       

            bool bAutoReset = false;
            bAutoReset = r["cbxAutoReset"] != null;
            bool isDisableCasino = r["chkDisabledCasino"] == null;
            isEnableCasino = !isDisableCasino || !custInfo.CasinoSync;
            
            csCustomer.SiteName = new Sunplus.Accounting.DataAccess.AdoNet.CustomerDAO().GetRNGSiteNameByCustId(custid);
            
            //var custSetting = csCustomer.GetCustSetting(custInfo.IBCCustID);            
            if (bInserting)
            {
                CSCustomerDetail parentCustInfo = null;
                Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider pCustInfo = new Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider();
                if (custInfo.RoleId.ToString() == "3") {
                    parentCustInfo = pCustInfo.GetCustInfoIBC(custInfo.SIBCCustID);
                }
                    
                else if (custInfo.RoleId.ToString() == "2") {
                    parentCustInfo = pCustInfo.GetCustInfoIBC(custInfo.MIBCCustID);
                }

                else if (custInfo.RoleId.ToString() == "1")
                {
                    parentCustInfo = pCustInfo.GetCustInfoIBC(custInfo.AIBCCustID);
                }
                                    
                if (parentCustInfo!=null)
                    isEnableCasino = !parentCustInfo.CasinoDisabled;
            }
            
            if (!bInserting)
            {                                         
               status = csCustomer.UpdateMaxTransferMisc(
                     custid,
                     isEnableCasino,
                     (!isEnableCasino)?true:DNAide.Core.CastAide.AsBoolean(r[Globals.HIDDEN_UPDDOWNLLINE]),
                     isMaxTransfer,
                     intMaxTransfer, // max transfer credit
                     bAutoReset,
                     bPTByGameType,
                    //!bGameKindGroup,                             
                     r.UserHostAddress,
                     AppSession.CustID,
                     AppSession.Username,
                     3);
                if (!status)
                {
                    context.Response.Write("<script>parent.ShowMessage('" + AlertMessage.GetMsg(Resources.Language.updCustSettingFailed, AlertMessage.MsgType.Error) + "', 0); parent.ResetBtnState(); </script>");
                    return;
                }
                
            }
            else
            {

                if (isS)
                {
                    bPTByGameType = !bGameKindGroup;
                }
                
                status = csCustomer.InsertCustSetting(
                     Globals.CASINO_GROUP1,
                     custid,
                     custInfo.UserName,
                     custInfo.UserName2,
                     custInfo.FirstName,
                     custInfo.LastName,
                     custInfo.UserPwd,
                     custInfo.Phone,
                     custInfo.MobilePhone,
                     custInfo.Currency,
                     custInfo.Fax,
                     custInfo.Closed,// closed
                     custInfo.Suspend,// suspended
                     false,// enablePT
                     maxWin,
                     maxLose,
                     isMaxTransfer,
                     intMaxTransfer, // max transfer credit
                     custInfo.SIBCCustID, // sid
                     custInfo.MIBCCustID, // mid
                     custInfo.AIBCCustID, // aid
                     bPTByGameType,
                     custInfo.RoleId.ToString(),
                     DateTime.Now,
                     r.ServerVariables["REMOTE_ADDR"],
                     AppSession.AccId, AppSession.Username, 3, bAutoReset
                     );

                if (!status)
                {
                    context.Response.Write("<script>parent.ShowMessage('" + AlertMessage.GetMsg(Resources.Language.updCustSettingFailed, AlertMessage.MsgType.Error) + "', 0); parent.ResetBtnState(); </script>");
                    return;
                }

                csCustomer.UpdateCustEnablePT(custid, true);
            }
            status = CasinoProvider.Instance.UpdateCSStatusWithoutSync(custid, true, !isEnableCasino,
                                                                           (!isEnableCasino) ? true : DNAide.Core.CastAide.AsBoolean(r[Globals.HIDDEN_UPDDOWNLLINE]));
            if (!status)
            {
                context.Response.Write("<script>parent.ShowMessage('" + AlertMessage.GetMsg("Error enable Casino from IBC", AlertMessage.MsgType.Error) + "', 0); parent.ResetBtnState(); </script>");
                return;
            }
            // Update cust game kind and game type
            DataTable dtGameKinds = GameKindsDataTable();
            DataTable dtGameTypes = GameTypesDataTable();
            DataTable dtGameTypesStake = GameTypesStakesDataTable();

            DataTable dtGameKindsReturn = null;
            DataTable dtGameTypesReturn = null;

            for (int i = 0; i < kc; i++)
            {
                int gameKind = GetGameKindId(gameKinds[i]);
                bool isAuto = r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.strCHK + Globals.Super + Globals.strAutoPT] != null;
                // Update cust game kind
                dtGameKinds.Rows.Add(new object[] { 
                    custid,
                    gameKind,
                    0,
                    (!(isS||isM)||isAuto?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strPresetPT]),
                    (!(isM||isA)?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.strPresetPT]),
                    (!(isA||isC)?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Agent + Globals.strPresetPT]),
                    isAuto?"1":"0",
                    (!isAuto?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strAutoPT]),
                    (!(isS||isM||isA)?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + roleid + Globals.strMinPT]),
                    0,
                    !isS?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.Comm],
                    !isM?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.Comm],
                    !isA?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Agent + Globals.Comm],
                    !isC?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + 0 + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Member + Globals.Comm],
                    custInfo.RoleId,
                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                    AppSession.AccId, AppSession.Username, 3
                    
                });


                if (bGameKindGroup)
                {
                    // Loop and update game types
                    string[] gameTypes = GetGameTypesId(gameKinds[i]);
                    int tc = gameTypes.Length;
                    for (int j = 0; j < tc; j++)
                    { // Update game type
                        string stakeList = string.Empty;
                        bool p2p = !gameTypes[j].Contains(Globals.OPEN_STAKELIST_MARK);
                        if (!p2p)
                        {
                            stakeList = GetStakeLevelList(gameTypes[j]);
                        }
                        int gameTypeId = GetGameTypeId(gameTypes[j]);
                        isAuto = r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.strCHK + Globals.Super + Globals.strAutoPT] != null;

                        dtGameTypesStake.Rows.Add(new object[] { 
                        custid,
                        gameTypeId,
                        GetSelectedStakeLevels(stakeList, gameKind, gameTypeId),
                        custInfo.RoleId,
                        HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                        AppSession.AccId, AppSession.Username, 3
                    });
                    }
                }
                else
                { // 
                    // Loop and update game types
                    string[] gameTypes = GetGameTypesId(gameKinds[i]);
                    int tc = gameTypes.Length;
                    for (int j = 0; j < tc; j++)
                    { // Update game type
                        string stakeList = string.Empty;
                        bool p2p = !gameTypes[j].Contains(Globals.OPEN_STAKELIST_MARK);
                        if (!p2p)
                        {
                            stakeList = GetStakeLevelList(gameTypes[j]);
                        }
                        int gameTypeId = GetGameTypeId(gameTypes[j]);
                        isAuto = r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.strCHK + Globals.Super + Globals.strAutoPT] != null;

                        dtGameTypes.Rows.Add(new object[] { 
                        custid,
                        gameTypeId,
                        0,
                        (!(isS||isM)||isAuto?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strPresetPT]),
                        (!(isM||isA)?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.strPresetPT]),
                        (!(isA||isC)?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Agent + Globals.strPresetPT]),
                        isAuto?"1":"0",
                        (!isAuto?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.strAutoPT]),
                        (!(isS||isM||isA)?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + roleid + Globals.strMinPT]),
                        0,
                        !isS?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Super + Globals.Comm],
                        !isM?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Master + Globals.Comm],
                        !isA?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Agent + Globals.Comm],
                        !isC?"0":r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.Member + Globals.Comm],
                        GetSelectedStakeLevels(stakeList, gameKind, gameTypeId),
                        custInfo.RoleId,
                        HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                        AppSession.AccId, AppSession.Username, 3
                    });
                    }
                }
            }            

            if (dtGameKinds.Rows.Count > 0)
            {
                dtGameKindsReturn = csCustomer.UpdateCustGameKind(dtGameKinds);
            }
            if (bGameKindGroup)
            {
                dtGameTypesReturn = csCustomer.UpdateCustGameTypeStakeLevel(dtGameTypesStake);
            }
            else
            {
                dtGameTypesReturn = csCustomer.UpdateCustGameType(dtGameTypes);
            }

            string errMsg = string.Empty;
            for (int i = 0; i < dtGameKindsReturn.Rows.Count; i++)
            {
                status = false;
                errMsg += dtGameKindsReturn.Rows[i][2].ToString() + "<br />&nbsp;&nbsp;";
            }

            for (int i = 0; i < dtGameTypesReturn.Rows.Count; i++)
            {
                status = false;
                errMsg += dtGameTypesReturn.Rows[i][2].ToString() + "<br />&nbsp;&nbsp;";
            }

            if (!status && !custInfo.CasinoSync) // Synchronize case
            {
                status = CasinoProvider.Instance.UpdateCSStatusWithoutSync(custid, false, true,
                                                                           (!isEnableCasino) ? true : DNAide.Core.CastAide.AsBoolean(r[Globals.HIDDEN_UPDDOWNLLINE]));
                status = csCustomer.RemoveAccount(custInfo.UserName, r.UserHostAddress, AppSession.AccId, AppSession.Username, 3);
            }
            
            

            if (errMsg != string.Empty)
            {
                context.Response.Write(@"<script>parent.ShowMessage('" + AlertMessage.GetMsg(errMsg, AlertMessage.MsgType.Error) + "');</script>");
                return;
            }            
        }
        catch (Exception ex)
        {
            
            LogExtensions.ExceptionHandler(ex);
            context.Response.Write(strReturn.Replace("msg", ex.Message.Replace("'", "\\'").Replace("\n", "")));

            if (!custInfo.CasinoSync)
            {
                status = CasinoProvider.Instance.UpdateCSStatusWithoutSync(custid, false, true,
                                                                          (!isEnableCasino) ? true : DNAide.Core.CastAide.AsBoolean(r[Globals.HIDDEN_UPDDOWNLLINE]));
                status = csCustomer.RemoveAccount(custInfo.UserName, r.UserHostAddress, AppSession.AccId, AppSession.Username, 3);
            }
            return;
        }

        // Return result
        context.Response.Write(@"<script>parent.showAlertMessage('err','Update Casino Setting Successfully',0);parent.afterUpdate(true, 2000);</script>");                                        
                                           

        // Clear session variable
        // context.Session[Globals.SESSION_CUSTOMER] = null;
    }


    public DataTable GameKindsDataTable()
    {
        DataTable dtGameKinds = new DataTable();
        dtGameKinds.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameKind"),
            new DataColumn("HPositionTaking"),
            new DataColumn("SPositionTaking"),
            new DataColumn("MPositionTaking"),
            new DataColumn("APositionTaking"),
            new DataColumn("IsAutoPositionTaking"),
            new DataColumn("AutoPositionTaking"),
            new DataColumn("MinPositionTaking"),
            new DataColumn("MaxPositionTaking"),
            new DataColumn("SMAgentRollingRate"),
            new DataColumn("MAgentRollingRate"),
            new DataColumn("AgentRollingRate"),
            new DataColumn("MemberRollingRate"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

        return dtGameKinds;
    }

    public DataTable GameTypesDataTable()
    {
        DataTable dtGameTypes = new DataTable();
        dtGameTypes.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameTypeID"),
            new DataColumn("HPositionTaking"),
            new DataColumn("SPositionTaking"),
            new DataColumn("MPositionTaking"),
            new DataColumn("APositionTaking"),
            new DataColumn("IsAutoPositionTaking"),
            new DataColumn("AutoPositionTaking"),
            new DataColumn("MinPositionTaking"),
            new DataColumn("MaxPositionTaking"),
            new DataColumn("SMAgentRollingRate"),
            new DataColumn("MAgentRollingRate"),
            new DataColumn("AgentRollingRate"),
            new DataColumn("MemberRollingRate"),
            new DataColumn("StakeLevelList"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

        return dtGameTypes;
    }

    public DataTable GameTypesStakesDataTable()
    {
        DataTable dtGameTypesStake = new DataTable();
        dtGameTypesStake.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameTypeID"),
            new DataColumn("StakeLevelList"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

        return dtGameTypesStake;
    }

    public string GetSelectedStakeLevels(string str, int gameKind, int gameTypeId)
    {
        HttpRequest r = HttpContext.Current.Request;
        string[] stakeLevels = str.Split(new string[] { Globals.SEP_STAKE }, StringSplitOptions.RemoveEmptyEntries);
        string selectedStakeLevels = string.Empty;

        for (int i = 0; i < stakeLevels.Length; i++)
        {
            if (r[gameKind + Globals.SEP_GAMEKIND_GAMETYPE + gameTypeId + Globals.SEP_GAMEKIND_GAMETYPE + Globals.CHK_STAKE + Globals.SEP_GAMEKIND_GAMETYPE + stakeLevels[i]] != null)
                selectedStakeLevels += stakeLevels[i] + ",";
        }

        if (selectedStakeLevels.EndsWith(","))
        {
            selectedStakeLevels = selectedStakeLevels.Remove(selectedStakeLevels.LastIndexOf(","), 1);
        }

        return selectedStakeLevels;
    }

    public string GetStakeLevelList(string str)
    {
        return str.Substring(str.IndexOf(Globals.OPEN_STAKELIST_MARK) + 1, str.IndexOf(Globals.CLOSE_STAKELIST_MARK) - str.IndexOf(Globals.OPEN_STAKELIST_MARK) - 1);
    }

    public int GetGameTypeId(string str)
    {
        if (str.Contains(Globals.OPEN_STAKELIST_MARK))
            return Convert.ToInt32(str.Substring(0, str.IndexOf(Globals.OPEN_STAKELIST_MARK)));
        else
            return Convert.ToInt32(str);
    }

    public int GetGameKindId(string str)
    {
        return Convert.ToInt32(str.Substring(0, str.IndexOf(Globals.OPEN_GAMETYPELIST_MARK)));
    }

    public string[] GetGameTypesId(string str)
    {
        return str.Substring(str.IndexOf(Globals.OPEN_GAMETYPELIST_MARK) + 1, str.IndexOf(Globals.CLOSE_GAMETYPELIST_MARK) - str.IndexOf(Globals.OPEN_GAMETYPELIST_MARK) - 1).Split(new string[] { Globals.SEP_GAMETYPE }, StringSplitOptions.RemoveEmptyEntries);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
