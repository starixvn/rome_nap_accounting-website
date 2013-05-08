using System;
using Sunplus.Accounting.Business.PositionTaking;
using Sunplus.Accounting.Core.Common;
using Resources;
using DNAide.Core;

public partial class Customer_V2_EditSuperOnMemberFI : RequiredAuthorizationPage
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
            htmlContent.Text = Language.thereisnodata;
        }

        htmlContent.Text = PositionTakingManager.GenHTML(custid, roleid, uplineId, Globals.PTType.SuperOnMember, isInsertSuper, errorCode);
    }
   
}
