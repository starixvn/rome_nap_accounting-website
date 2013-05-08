<%@ WebHandler Language="C#" Class="SuperBalance" %>

using System;
using System.Web;

public class SuperBalance : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        
        string db = (DNAide.Core.CastAide.AsString(context.Request["IsRep"], "0").Equals("1")) ? "rep" : "main";
        bool IsYesterday = DNAide.Core.CastAide.AsString(context.Request["IsYesterDay"], "0").Equals("1");
        string username = context.Request["username"];
        string firstletter = context.Request["letter"];
        string currency = context.Request["currency"]; 
        

        IServiceDownLineBalance balance = new ServiceDownLineBalance();
        context.Response.Write(balance.SuperBalance(db, IsYesterday, username, firstletter, currency));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}