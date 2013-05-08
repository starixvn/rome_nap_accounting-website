using System;
using DNAide.Core;

public partial class LiveCasino_PTSetting_SuperEditOnMember : System.Web.UI.Page
{
    protected int CustId, UplineId;
    protected void Page_Load(object sender, EventArgs e)
    {
        CustId = CastAide.AsInt32(Request["custId"]);
        UplineId = CastAide.AsInt32(Request["superId"]);
    }
}