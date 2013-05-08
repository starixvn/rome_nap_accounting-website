using System;
using Sunplus.Accounting.Business.PositionTaking;
using Sunplus.Accounting.Core.Common;
using Resources;
using DNAide.Core;
using Sunplus.Accounting.Core.Helper;

public partial class Customer_V2_MinMaxFinancePT : RequiredAuthorizationPage
{

    public string strtitle = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        int custid = 0;
        int roleid = 0;
        int errorCode = -1;
        int uplineId = CastAide.AsInt32(Request.QueryString["uplineid"]);
        errorCode = CastAide.AsInt32(Request.QueryString["errorcode"], -1);
        custid = CustId;
        roleid =CastAide.AsInt32(Request.QueryString["roleid"],4);
        if (roleid == 4)
        {
            Page.Title = "Update Min PT Finance";
            strtitle = "Update Min PT Finance";
        }
        else
        {
            Page.Title = " Update Min/Max PT Finance";
            strtitle = " Update Min/Max PT Finance";
        }
        if(custid == 0)
        {
            htmlContent.Text = AlertMessage.GetMsg(Language.thereisnodata, AlertMessage.MsgType.Error);
        }
        htmlContent.Text = PositionTakingManager.GenHTMLMinMax(custid, uplineId, roleid, Globals.PTType.MinMaxPT, errorCode);
    }
   
}
