<%@ WebHandler Language="C#" Class="DirectBalanceDownLine" %>

using System;
using System.Web;
using Sunplus.Accounting.Core.Helper;

public class DirectBalanceDownLine : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        int pcustid = DNAide.Core.CastAide.AsInt32(context.Request["pcustid"], 0);
        string user = DNAide.Core.CastAide.AsString(context.Request["user"], "");
        string puser = DNAide.Core.CastAide.AsString(context.Request["puser"], "");
        int roleid = DNAide.Core.CastAide.AsInt32(context.Request["roleid"]);
        int custid = DNAide.Core.CastAide.AsInt32(context.Request["custid"]);
        IServiceDirectBalance direct = new ServiceDirectBalance();
        string strData = direct.GetDirectBalance_Downline(custid, pcustid, roleid, user);
        string strHeader = direct.getHeaderDownLine(roleid, user, custid.ToString(), puser, pcustid);
        
        System.Collections.Hashtable hash = new System.Collections.Hashtable();
        hash.Add("header", strHeader);
        hash.Add("data", strData);
        
        context.Response.Write(JsonUtil.ToJson(hash));
    }
 
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}