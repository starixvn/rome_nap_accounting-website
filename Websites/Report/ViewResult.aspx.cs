using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Web;
using DNAide.Core;
using DNAide.Data;
using System.Data;
using Sunplus.Accounting.Business.BetList;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;


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
