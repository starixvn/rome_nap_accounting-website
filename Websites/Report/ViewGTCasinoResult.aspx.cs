using System;
using Fanex.Result.Core.GTCasino;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class ViewGTCasinoResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string gtCasinoResult;
        var gtCasinoResultService = new GTCasinoResultBuilder();

        try
        {
            gtCasinoResult = gtCasinoResultService.Render(Request.QueryString["transDesc"], Request.QueryString["matchCode"], Request.QueryString["betTypeId"]);
            if (string.IsNullOrEmpty(gtCasinoResult))
            {
                gtCasinoResult = "<table class=\"fn-rs casino popup\"><tbody><tr><td class=\"row-noinfo\">There is no information available</td></tr></tbody></table>";
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            gtCasinoResult = ex.Message;
        }

        this.lResult.Text = gtCasinoResult;
    }
}