using System;
using Fanex.Result.Core.GTCasino;

public partial class ViewGTCasinoResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string gtCasinoResult;
        var gtCasinoResultService = new GTCasinoResultBuilder();

        gtCasinoResult = gtCasinoResultService.Render(Request.QueryString["transDesc"], ParseTranId3rd(Request.QueryString["matchCode"]), Request.QueryString["betTypeId"]);
        this.lResult.Text = gtCasinoResult;
    }

    private static string ParseTranId3rd(string transId3rd)
    {
        const string transId3rdSeparator = "|$|";
        if (transId3rd == null || !transId3rd.Contains(transId3rdSeparator))
        {
            return transId3rd;
        }

        var transIdParts = transId3rd.Split(new string[] { transId3rdSeparator }, StringSplitOptions.RemoveEmptyEntries);

        return transIdParts[transIdParts.Length - 1];
    }
}