using System;
using DNAide.Core;
using Fanex.Tarus.BetList.Business.Results;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class Report_ViewVirtualRacingResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            int matchId = CastAide.AsInt32(Request.QueryString["matchid"], 0);
            int sportType = CastAide.AsInt32(Request.QueryString["sportType"], 0);
            DateTime eventDate = CastAide.AsDateTime(Request.QueryString["eventdate"], DateTime.MinValue);

            lResult.Text = new VS2RacingResultService().GetResult(matchId, sportType, eventDate);
        }
        catch (Exception exception)
        {
            LogExtensions.ExceptionHandler(exception);
            lResult.Text = exception.Message;
        }
    }
}