<%@ WebHandler Language="C#" Class="AddTransaction" %>

using System;
using System.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;


public class AddTransaction : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) 
    {
        
        
        context.Response.ContentType = "text/html";
        context.Response.Write("<script type=\"text/javascript\">");        
        try
        {
            if (context.Session["AccId"] == null) return;
            int custId = Convert.ToInt32(context.Request.QueryString["CustID"]);
            DateTime transDate = Convert.ToDateTime(context.Request["txtDate"]);
            double amount = Convert.ToDouble(context.Request["txtamount"]);
            string transDesc = context.Request["txtDesc"];
            string lang="E";
            int creator =DNAide.Core.CastAide.AsInt32(context.Session["AccId"]);
            bool isInternal = Convert.ToBoolean(context.Request.QueryString["IsInternal"]);
            string[] arrIns ={ "", "", "" };
            arrIns= AccountTransaction.BettransInsTransaction(custId, transDate, amount, transDesc, creator,lang, context.Request.UserHostAddress);            
            if (arrIns[1] == "0")
            {
                if (isInternal)
                {
                    context.Response.Write("parent.location.href='../CustomerStatement/Index?CustID=" + custId + "&CustName=" + context.Request.QueryString["CustName"] + "';"); 
                }
                else
                {
                    context.Response.Write("parent.location.href='../Statement/CustomerStatement.aspx?repno=3&CustName=" + context.Request.QueryString["CustName"] + "';");
                } 
            }
            else
            {
                context.Response.Write("parent.alert_error('"+ AlertMessage.GetMsg(arrIns[2], AlertMessage.MsgType.Error)+"','errMsg');");
            }
        }
            
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) + "','errMsg');");
        }
        context.Response.Write("</script>");        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
