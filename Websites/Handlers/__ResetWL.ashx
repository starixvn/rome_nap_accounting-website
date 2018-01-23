<%@ WebHandler Language="C#" Class="ResetMaxTransfer" %>

using System;
using System.Web;
using System.Web.SessionState;

using BLL;
using DNAide.Web;
using DNAide.Core;
using System.Threading;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;

using Microsoft.Practices.EnterpriseLibrary.Logging;

public class ResetMaxTransfer : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/plain";
        context.Response.Cache.SetNoStore();

        bool status = false;
        
        try
        {
            var custId = int.Parse(context.Request["custid"]);
            CasinoLibrary.SBAgent.Customer csCustomer = new CasinoLibrary.SBAgent.Customer();
            csCustomer.SiteName = new Sunplus.Accounting.DataAccess.AdoNet.CustomerDAO().GetRNGSiteNameByCustId(custId);
            status = csCustomer.ResetMaxTransfer(custId, context.Request.UserHostAddress, AppSession.AccId, AppSession.Username, 3);
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            context.Response.Write(ex.Message.Replace("'", "\\'"));
            return;
        }
        
        context.Response.Write(status?"Ok":"Failed");
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
