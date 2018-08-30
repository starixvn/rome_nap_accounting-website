using System;
using Fanex.Result.Core.PokerGames;
using Fanex.Result.Core.PokerGames.Models;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class Report_ViewPokerGamesResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string pokerGamesResult;
        var pokerGamesResultService = new PokerGamesResult();

        try
        {
            pokerGamesResult = pokerGamesResultService.GetResult(
                Request.QueryString["custId"],
                Request.QueryString["gameTypeId"],
                Request.QueryString["transId3rd"],
                ViewerLevel.Company);
            if (string.IsNullOrEmpty(pokerGamesResult))
            {
                pokerGamesResult = "<div class=\"alertboxerr\"><div>There is no information available</div></div>";
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            pokerGamesResult = "<div class=\"alertboxerr\"><div>" + ex.Message + "</div></div>";
        }

        this.lResult.Text = pokerGamesResult;
    }
}
