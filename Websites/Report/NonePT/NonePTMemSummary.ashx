<%@ WebHandler Language="C#" Class="NonePTMemSummary" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Business.Report.NonePT;

public class NonePTMemSummary : IHttpHandler, IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        IServiceNonePT nonePT = new ServiceNonePT();
        string username = DNAide.Core.CastAide.AsString(context.Request["username"], string.Empty);
        string currpage = DNAide.Core.CastAide.AsString(context.Request["pageindex"], "1");
        context.Response.Write(nonePT.getNonePTMembers(username, currpage));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}