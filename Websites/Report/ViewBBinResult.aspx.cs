using System;
using Fanex.Result.Core.BBin;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class Report_ViewLiveCasinoResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string bbinResult;
        var service = new BBinResultBuilder();

        try
        {
            bbinResult = service.Render(Request.QueryString["transDesc"], Request.QueryString["betcheck"]);
            if (string.IsNullOrEmpty(bbinResult))
            {
                bbinResult = "<table class=\"fn-rs casino popup\"><tbody><tr><td class=\"row-noinfo\">There is no information available</td></tr></tbody></table>";
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            bbinResult = ex.Message;
        }

        this.lResult.Text = bbinResult;
    }
}
