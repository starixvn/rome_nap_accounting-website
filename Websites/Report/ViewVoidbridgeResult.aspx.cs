using System;
using Fanex.Result.Core.Voidbridge;
using Fanex.Tarus.Infrastructure.Extensions;

public partial class Report_ViewVoidbridgeResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string voidbridgeResult;

        var voidbridge = new VoidbridgeApiImp();

        try
        {
            voidbridgeResult = voidbridge.GetBetHistoryUrl(Request.QueryString["custId"], Request.QueryString["betCheck"]);
            if (string.IsNullOrEmpty(voidbridgeResult))
            {
                voidbridgeResult = "<table><tbody><tr><td>There is no information available</td></tr></tbody></table>";
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            voidbridgeResult = "<table><tbody><tr><td style=\"color: Red;font-weight: bold;\">" + ex.Message + "</td></tr></tbody></table>";
        }

        this.lResult.Text = voidbridgeResult;
    }
}
