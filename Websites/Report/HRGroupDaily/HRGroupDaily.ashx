<%@ WebHandler Language="C#" Class="HRGroupDaily" %>

using System;
using System.Web;
using System.Web.SessionState;

public class HRGroupDaily : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string type = context.Request["type"];
        IServiceHRGroupDaily service = new ServiceHRGroupDaily();
        string fromdate = context.Request["fdate"];
        string todate = context.Request["tdate"];
        bool IsSendMail = context.Request["IsSendMail"] == "1";
        switch (type)
        {
            case "tote":
                context.Response.Write(service.HRGroupDaily_ToteOnly(fromdate, todate, IsSendMail));
                break;
            default:
                context.Response.Write(service.HRGroupDaily(fromdate, todate, IsSendMail));
                break;
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}