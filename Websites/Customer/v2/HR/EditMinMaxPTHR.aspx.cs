using System;
using DNAide.Core;
using Resources;
using Sunplus.Accounting.Business.PositionTaking;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;

public partial class Customer_EditMinMaxPTHR : RequiredAuthorizationPage
{

    public string htmlContent = "";
    public string strtitle = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        int custid = 0;
        int roleid = 0;
        int errorCode = -1;
        int uplineId = CastAide.AsInt32(Request.QueryString["uplineid"]);
        errorCode = CastAide.AsInt32(Request.QueryString["errorcode"], -1);
        custid = CustId;
        roleid = CastAide.AsInt32(Request.QueryString["roleid"], 4);
        if (custid == 0)
        {
            htmlContent = AlertMessage.GetMsg(Language.thereisnodata, AlertMessage.MsgType.Error);
        }
        if (roleid == 4)
        {
            Page.Title = "Update Min PT HR";
            strtitle = "Update Min PT HR";
        }
        else
        {
            Page.Title = " Update Min/Max PT HR";
            strtitle = " Update Min/Max PT HR";
        }
        htmlContent  = HRPositionTakingManager.GenHTMLMinMax(custid, uplineId, roleid, Globals.PTType.MinMaxPT, errorCode);   
    }
   
}

