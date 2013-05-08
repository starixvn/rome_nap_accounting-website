<%@ WebHandler Language="C#" Class="InvalidExRate" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Business.UI;
using Sunplus.Core.LoggingLib;
using IBCUtils.DAL;
using System.Text;


public class InvalidExRate : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        if (CastAide.AsString(context.Request["params"]) != "")
        {
            string[] param = context.Request.Params["params"].Split(";".ToCharArray());
            StringBuilder result = new StringBuilder();
            try
            {
                result.Append("1$$$");
                result.Append(GenerateHtml.GetContentInvalidExRate(CastAide.AsDateTime(param[0]), CastAide.AsDateTime(param[1])));
            }
            catch (Exception ex)
            {
                GotoErroPage(context);
                context.Response.End();
                //result.Append(ex.ToString() + "$$$");
            }
            context.Response.Write(result.ToString());
        }
        else
        {
            GotoErroPage(context);
        }

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}
