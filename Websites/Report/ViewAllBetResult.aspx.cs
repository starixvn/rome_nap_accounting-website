using System;
using Fanex.Result.Core.AllBetCasino;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class Report_ViewAllBetResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string allbetResult;
        IBuilderService allbetResultService = new BuilderService();

        try
        {
            allbetResult = allbetResultService.Render(Request.QueryString["transId"]);
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
