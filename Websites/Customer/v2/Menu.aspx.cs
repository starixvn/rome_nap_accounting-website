using System;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Bingo.Core;

public partial class Others_Menu : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string tplMenu = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/v2/MenuUpdatePT.htm");
        string defaultUrl = "UpdateFinancePT.aspx" + Request.UrlReferrer.Query;
        
        Boolean AllowedFi, AllowedCS, AllowedBI, AllowedPoker, allowEditLiveCs, allowVirtualSports;
        string[] arrCustID = Request.UrlReferrer.Query.Split('&');
        string custID = arrCustID[0].ToString();
        custID = custID.Substring(8, custID.Length - 8);
        var HRURL = string.Empty;
        string PokerURL = "../PT/Poker/";
        DbResult db = CustomerManager.CheckAllowedCurrencyAll(CastAide.AsInt32(custID));
        AllowedFi = CastAide.AsBoolean(db.Outputs["allowedfi"]);
        AllowedCS = CastAide.AsBoolean(db.Outputs["allowedcs"]);
        AllowedBI = CastAide.AsBoolean(db.Outputs["allowedbi"]);
        AllowedPoker = CastAide.AsBoolean(PokerSection.Instance[PokerSection.PokerCollections.CustomerSetting]["visibleP2PSetting"].Value);
        allowEditLiveCs = CastAide.AsBoolean(db.Outputs["allowedlivecs"]);
        allowVirtualSports = WebSettings.EnableVirtualSports ? CastAide.AsBoolean(db.Outputs["allowedvs"]) : false;

        int roleid = CastAide.AsInt32(arrCustID[2].Split('=')[1]);
        string bingopath = "../PT/Bingo/";
        var liveCasinoPath = "../../LiveCasino/PTSetting/";
        var virtualSportsPath = string.Empty;
        var uplineId = CastAide.AsInt32(arrCustID[3].Split('=')[1]);
        
        if (roleid == 1)
        {
            bingopath += "MemberUpdate.aspx?custId=" + custID.ToString();
            HRURL = string.Format("../../EditMemberPTRacing/Index?custid={0}&uplineid={1}", custID, uplineId);
            PokerURL += "MemberUpdate.aspx?custId=" + custID.ToString() + "&roleId=" + roleid.ToString();
            liveCasinoPath += "EditMember.aspx?custId=" + custID + "&uplineId=" + uplineId;
            virtualSportsPath = "../../VirtualSportMemberPT/EditMember?custId=" + custID + "&uplineId=" + uplineId;
        }
        else if (roleid == 2)
        {
            bingopath += "AgentUpdate.aspx?custId=" + custID.ToString();
            HRURL = string.Format("../../EditAgentPTRacing/Index?custid={0}&uplineid={1}", custID, uplineId);
            PokerURL += "AgentUpdate.aspx?custId=" + custID.ToString() + "&roleId=" + roleid.ToString();
            liveCasinoPath += "EditAgent.aspx?custId=" + custID + "&uplineId=" + uplineId;
            virtualSportsPath = "../../VirtualSportAgentPT/EditAgent?custId=" + custID + "&uplineId=" + uplineId;
        }
        else if (roleid == 3)
        {
            bingopath += "MasterUpdate.aspx?custId=" + custID.ToString();
            HRURL = string.Format("../../EditMasterPTRacing/Index?custid={0}&uplineid={1}", custID, uplineId);
            PokerURL += "MasterUpdate.aspx?custId=" + custID.ToString() + "&roleId=" + roleid.ToString();
            liveCasinoPath += "EditMaster.aspx?custId=" + custID + "&uplineId=" + uplineId;
            virtualSportsPath = "../../VirtualSportMasterPT/EditMaster?custId=" + custID + "&uplineId=" + uplineId;
        }
        else
        {
            var exChangeId = CastAide.AsInt32(arrCustID[4].Split('=')[1]);
            bingopath += "SuperUpdate.aspx?custId=" + custID.ToString();
            HRURL = string.Format("../../EditSuperPTRacing/Index?custid={0}", custID);
            PokerURL += "SuperUpdate.aspx?custId=" + custID.ToString() + "&roleId=" + roleid.ToString();
            liveCasinoPath += "EditSuper.aspx?custId=" + custID + "&exchangeId=" + exChangeId;
            virtualSportsPath = "../../VirtualSportSuperPT/EditSuper?custId=" + custID;
        }
        if (WebSettings.disabledFI || !AllowedFi || AppSession.CurrentSystemNAME.Equals(Enumerations.AccountingSystem.EXTERNAL))
        {
            TemplateHandler.RemoveBlock(ref tplMenu, "Finance");
            defaultUrl = HRURL;
        }
        if (WebSettings.disabledHR)
        {
            TemplateHandler.RemoveBlock(ref tplMenu, "HorceRacing");
            if ((WebSettings.disabledFI || !AllowedFi) && !WebSettings.disableCS)
                defaultUrl = "../PositionTakingCS.aspx" + Request.UrlReferrer.Query;
        }
        if (WebSettings.disableCS || !AllowedCS)
            TemplateHandler.RemoveBlock(ref tplMenu, "Casino");
        if (!BingoSettings.BingoGame.Enabled || !AllowedBI)
        {
            TemplateHandler.RemoveBlock(ref tplMenu, "Bingo");
        }
        if (!AllowedPoker)
        {
            TemplateHandler.RemoveBlock(ref tplMenu, "Poker");
        }
        if(!WebSettings.EnableLiveCasino || !allowEditLiveCs)
            TemplateHandler.RemoveBlock(ref tplMenu, "LiveCasino");
        if(!allowVirtualSports)
            TemplateHandler.RemoveBlock(ref tplMenu, "VirtualSports");

        TemplateHandler.Assign(ref tplMenu, "{strURL}", HRURL); // Link Default
        TemplateHandler.Assign(ref tplMenu, "{urlFinance}", "UpdateFinancePT.aspx" + Request.UrlReferrer.Query);
        TemplateHandler.Assign(ref tplMenu, "{urlHorse}", HRURL);
        TemplateHandler.Assign(ref tplMenu, "{urlCasino}", "../PositionTakingCS.aspx" + Request.UrlReferrer.Query);
        TemplateHandler.Assign(ref tplMenu, "{urlBingo}", bingopath);
        TemplateHandler.Assign(ref tplMenu, "{urlPoker}", PokerURL);
        TemplateHandler.Assign(ref tplMenu, "{urlLiveCasino}", liveCasinoPath);
        TemplateHandler.Assign(ref tplMenu, "{urlVirtualSports}", virtualSportsPath);

        this.sMenu.Text = tplMenu;
    }
}
