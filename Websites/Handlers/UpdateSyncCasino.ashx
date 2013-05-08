<%@ WebHandler Language="C#" Class="UpdateSyncCasino" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using IBCUtils.DAL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.Web;
using DNAide.Data;
using System.Text;
using IBCUtils;


public class UpdateSyncCasino : IHttpHandler, IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        try 
        {
            context.Response.ContentType = "text/html";
            int _accid = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (_accid == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);
            int _custid = Convert.ToInt32(context.Request["CustId"] as object ?? -1);
            if (_custid == -1) throw new AccException(IbcError.USERNAME_UNAVAILABLE_MSG);
            DbResult dbr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.CasinoSynchronise.Acc_CustomerSyncUpd(_accid, _custid);
            StringBuilder s = new StringBuilder();
            //s.Append("parent.document.getElementById('spanGivenCredit').innerHTML ='" + string.Format("{0:n}", amount) + "';");
            //s.Append("parent.ChangeGivenCredit();");
            //s.Append("parent.opener.DelayReloadPage();");

            if (dbr.Outputs["errcode"].ToString() != "0")
                context.Response.Write(dbr.Outputs["errmsg"]);
            else
                context.Response.Write("succeed");            
        }
        catch(AccException ex)
        {
            
            context.Response.Write(ex.Message.ToString());
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }            
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
