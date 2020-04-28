using System;
using Fanex.Result.Core.GTCasino;

public partial class ViewGTCasinoResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string gtCasinoResult;
        var gtCasinoResultService = new GTCasinoResultBuilder();

        gtCasinoResult = gtCasinoResultService.Render(Request.QueryString["transDesc"], Request.QueryString["matchCode"], Request.QueryString["betTypeId"]);
        this.lResult.Text = gtCasinoResult;
    }
}