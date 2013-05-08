<%@ WebHandler Language="C#" Class="NonePTLastestBet" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Business.Report.NonePT;

public class NonePTLastestBet : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        IServiceNonePT nonePt = new ServiceNonePT();
        string type = context.Request["type"];
        switch (type)
        {
            case "getAllBettype":
                context.Response.Write(nonePt.getAllBettypes());                
                break;
            case "getAllCurrencies":
                context.Response.Write(nonePt.getAllCurrencies());                
                break;
            case "getData":
                string username = CastAide.AsString(context.Request["username"]);
                string bettype = CastAide.AsString(context.Request["bettype"]);
                string fromstake = CastAide.AsString(context.Request["fromstake"],"0");
                string tostake = CastAide.AsString(context.Request["tostake"], "0");
                string pagesize = CastAide.AsString(context.Request["pagesize"], "100");
                context.Response.Write(nonePt.getNonePTLastestBet(username, "", bettype, fromstake, tostake, "0", pagesize));
                break;
        }
        //context.Response.Write(nonePt.getNonePTLastestBet("", ""));
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}