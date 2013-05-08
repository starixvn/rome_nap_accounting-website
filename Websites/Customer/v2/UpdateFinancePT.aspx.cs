using System;
using Sunplus.Accounting.Business.PositionTaking;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Resources;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;

public partial class Customer_V2_UpdateFinancePT : RequiredAuthorizationPage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        int custid = 0;
        int roleid = 0;
        int errorCode = -1;
        int uplineId = CastAide.AsInt32(Request.QueryString["uplineid"]);
        errorCode = CastAide.AsInt32(Request.QueryString["errorcode"], -1);
        bool isInsertSuper = DNAide.Core.CastAide.AsBoolean(Request.QueryString["insert"], false);
        custid = CastAide.AsInt32(Request.QueryString["custid"]);
        roleid =CastAide.AsInt32(Request.QueryString["roleid"],4);
        if(custid == 0)
        {
            htmlContent.Text = AlertMessage.GetMsg(Language.thereisnodata, AlertMessage.MsgType.Error);
        }
        htmlContent.Text = PositionTakingManager.GenHTML(custid, roleid, uplineId, Globals.PTType.Default, isInsertSuper, errorCode);
    }


   
}
