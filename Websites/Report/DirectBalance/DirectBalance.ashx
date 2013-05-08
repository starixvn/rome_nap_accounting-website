<%@ WebHandler Language="C#" Class="DirectBalance" %>

using System;
using System.Web;

public class DirectBalance : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string type = DNAide.Core.CastAide.AsString(context.Request["display"]);
        IServiceDirectBalance direct = new ServiceDirectBalance();
        string strData = direct.GetDirectBalance(type);
        context.Response.Write(strData);
    }
 
    public bool IsReusable {        
        get { 
            return false;
            
        }
    }

}