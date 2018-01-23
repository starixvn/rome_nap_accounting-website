<%@ WebHandler Language="C#" Class="SubHeader" %>

using System;
using System.Web;
using DNAide.Web;
using DNAide.Utils;

public class SubHeader : IHttpHandler 
{
    
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.ContentType = "text/xml";
        string type = context.Request["type"];
        string tpl = "";
        switch (type)
        { 
            case "2a":
                tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/OPtionViewReport2a-oldV2-1.htm");                
                break;
        }
        context.Response.Write(tpl);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
