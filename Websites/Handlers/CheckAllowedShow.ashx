<%@ WebHandler Language="C#" Class="CheckAllowedShow" %>

using System.Web;

public class CheckAllowedShow : BaseHandler
{
    public override void ProcessRequest(HttpContext context)
    {
        var result = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CustomerManager.CheckAllowedCurrencyAll(DNAide.Core.CastAide.AsInt32(context.Request.QueryString["custid"]));
        context.Response.Write(DNAide.Core.CastAide.AsBoolean(result.Outputs["allowedng2"]) ? 1 : 0);
    }

    public new bool IsReusable
    {
        get
        {
            return false;
        }
    }

}