using System;
using Fanex.Tarus.BetList.Business.Results;
using Sunplus.Accounting.Core.Extensions;

public partial class Report_ViewResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var transdesc = Request.QueryString["TransDesc"];
            this.lResult.Text = new LeapGamingResultService().GetResult(transdesc);
        }
        catch (Exception exception)
        {
            LogExtensions.ExceptionHandler(exception);
            lResult.Text = exception.Message;
        }
    }
}