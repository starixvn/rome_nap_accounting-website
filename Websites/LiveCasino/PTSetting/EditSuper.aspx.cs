using System;
using DNAide.Core;

public partial class LiveCasino_PTSetting_EditSuper : System.Web.UI.Page
{
    protected int CustId, ExchangeId, IsAddNew;
    protected void Page_Load(object sender, EventArgs e)
    {
        CustId = CastAide.AsInt32(Request["custId"]);
        ExchangeId = CastAide.AsInt32(Request["exchangeId"]);
        IsAddNew = Request["roleId"] == null ? 1 : 0;
    }
}