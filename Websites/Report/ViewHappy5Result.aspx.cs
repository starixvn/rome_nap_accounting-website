using System;
using DNAide.Core;
using Fanex.Tarus.BetList.Business.Results;
using Sunplus.Accounting.Core.Extensions;

public partial class Report_ViewHappy5Result : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int matchId = CastAide.AsInt32(Request.QueryString["matchid"], 0);

            lResult.Text = new Happy5PopupResultService().GetResult(matchId);
        }
        catch (Exception exception)
        {
            LogExtensions.ExceptionHandler(exception);
            lResult.Text = exception.Message;
        }
    }
}