using System;
using Fanex.Result.Core.VGaming;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class Report_ViewVGamingResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string result;
        var service = new VGamingResult();

        try
        {
            var sportType = int.Parse(Request.QueryString["SportType"]);

            result = service.GetResult(
                Request.QueryString["custId"],
                Request.QueryString["GameRoundId"],
                Request.QueryString["GameId"],
                Request.QueryString["SiteId"],
                sportType,
                Request.QueryString["CurrencyName"]);
            if (string.IsNullOrEmpty(result))
            {
                result = "<div class=\"alertboxerr\"><div>There is no information available</div></div>";
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            result = "<div class=\"alertboxerr\"><div>" + ex.Message + "</div></div>";
        }

        this.lResult.Text = result;
    }
}
