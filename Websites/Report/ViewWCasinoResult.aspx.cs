using System;
using Fanex.Result.Core.WCasino;

public partial class Report_ViewLiveCasinoResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var service = new WCasinoResultBuilder("/Content/Fanex.Result/WM/images/");
        var result = service.Render(Request.QueryString["transDesc"], Request.QueryString["matchCode"], Convert.ToInt32(Request.QueryString["betTypeId"]));

        this.lResult.Text = result;
    }
}
