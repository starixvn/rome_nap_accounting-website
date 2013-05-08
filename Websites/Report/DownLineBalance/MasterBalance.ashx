<%@ WebHandler Language="C#" Class="MasterBalance" %>

using System;
using System.Web;

public class MasterBalance : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        
        string db = (DNAide.Core.CastAide.AsString(context.Request["IsRep"], "0").Equals("1")) ? "rep" : "main";
        bool IsYesterday = DNAide.Core.CastAide.AsString(context.Request["IsYesterDay"], "0").Equals("1");
        string username = context.Request["username"];
        string firstletter = context.Request["letter"]; 
        string currency = context.Request["currency"];
        int roleid = (string.IsNullOrEmpty(context.Request["roleid"])) ? 3 : DNAide.Core.CastAide.AsInt32(context.Request["roleid"]);
        int custid = (string.IsNullOrEmpty(context.Request["custid"])) ? 0 : DNAide.Core.CastAide.AsInt32(context.Request["custid"]);
        
        

        IServiceDownLineBalance balance = new ServiceDownLineBalance();
        context.Response.Write(balance.MasterBalance(db, roleid, custid, IsYesterday, username, firstletter, currency));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}