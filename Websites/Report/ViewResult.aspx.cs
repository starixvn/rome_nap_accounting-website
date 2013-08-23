using System;
using DNAide.Core;
using Sunplus.Accounting.Business.BetList;

public partial class Report_ViewResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int matchID = CastAide.AsInt32(Request.QueryString["matchid"], 0);
            int bettype = CastAide.AsInt32(Request.QueryString["bettype"], 0);
            string betId = CastAide.AsString(Request.QueryString["betid"]);
            BetListMain betlist = new BetListMain();
            lResult.Text = betlist.LoadCalHTML(matchID, bettype, betId);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            lResult.Text = ex.Message;
        }
    }
}