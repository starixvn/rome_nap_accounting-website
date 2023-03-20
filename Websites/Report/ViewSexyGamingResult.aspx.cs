using System;
using Fanex.Result.Core.SexyGaming;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class Report_ViewLiveCasinoResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string allbetResult;
        var service = new SexyGamingResultBuilder();

        try
        {
            var bettypeid = Int32.Parse(Request.QueryString["betTypeId"]);
            allbetResult = service.Render(Request.QueryString["transDesc"], Request.QueryString["matchCode"], bettypeid);
            if (string.IsNullOrEmpty(allbetResult))
            {
                allbetResult = "<table class=\"fn-rs casino popup\"><tbody><tr><td class=\"row-noinfo\">There is no information available</td></tr></tbody></table>";
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            allbetResult = ex.Message;
        }

        this.lResult.Text = allbetResult;
    }
}
