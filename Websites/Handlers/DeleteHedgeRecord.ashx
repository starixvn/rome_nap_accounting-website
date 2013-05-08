<%@ WebHandler Language="C#" Class="DeleteHedgeRecord" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;


public class DeleteHedgeRecord : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) 
    {
        context.Response.ContentType = "text/xml";
        {                
            try
            {
                if (context.Session["AccId"] == null)
                {
                    context.Session.Clear();
                    context.Response.Write("no|" + IBCUtils.IbcError.SESSION_EXPIRED_MSG);
                    context.Response.End();
                }
                else
                {
                    long TransID =  DNAide.Core.CastAide.AsInt64(context.Request["TransID"]);
                    int Creator =  DNAide.Core.CastAide.AsInt32(context.Session["AccId"]);
                    Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccountReport.Acc_HedgeDel(TransID, Creator);
                    context.Response.Write("ok|");
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                context.Response.Write("no|" + StringUtils.ParseToJSMsg(ex.Message));
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
