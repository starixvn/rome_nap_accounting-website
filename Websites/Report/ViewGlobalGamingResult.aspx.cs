using System;
using Fanex.Result.Core.GlobalGaming;
using Fanex.Result.Core.GlobalGaming.DTO;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class Report_ViewGloablGamingResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string globalGamingResult;

        GlobalGamingResultApi globalGamingResultApi = new GlobalGamingResultApi();

        try
        {
            globalGamingResult = globalGamingResultApi.GetResultUrl(new GlobalGamingDTO
            {
                CustId = Request.QueryString["custId"],
                GameId = Request.QueryString["gameId"]
            });

            if (string.IsNullOrEmpty(globalGamingResult))
            {
                globalGamingResult = "<table><tbody><tr><td>There is no information available</td></tr></tbody></table>";
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            globalGamingResult = "<table><tbody><tr><td style=\"color: Red;font-weight: bold;\">" + ex.Message + "</td></tr></tbody></table>";
        }

        this.lResult.Text = globalGamingResult;
    }
}
