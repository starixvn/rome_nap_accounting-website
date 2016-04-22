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
                voidbridgeResult = "<table class=\"fn-rs casino popup\"><tbody><tr><td class=\"row-noinfo\">There is no information available</td></tr></tbody></table>";
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            voidbridgeResult = ex.Message;
        }

        this.lResult.Text = voidbridgeResult;
    }
}
