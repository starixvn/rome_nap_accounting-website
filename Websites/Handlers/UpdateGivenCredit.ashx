<%@ WebHandler Language="C#" Class="UpdateGivenCredit" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess;
using IBCUtils.DAL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.Web;
using System.Text;
using IBCUtils;


public class UpdateGivenCredit :BaseHandler {
    
    public override void ProcessRequest (HttpContext context) {
        base.ProcessRequest(context);
        
        try
        {       
            
            context.Response.ContentType = "text/html";
            string sAmount = context.Request["amount"] as string ?? string.Empty;
            if (string.IsNullOrEmpty(sAmount)) throw new AccException("Invalid amount!");
            decimal amount = Convert.ToDecimal(sAmount);
            int _accid = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (_accid == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);
            int _custid = CustId;
            if (_custid == -1) throw new AccException(IbcError.USERNAME_UNAVAILABLE_MSG);

            var custDetail = IBCProvider.IBCCustomer.GetBasicCustInfoIBC(_custid);
            if (custDetail.IBCCustID == 0)
            {
                context.Response.Write("<script type='text/javascript'>alert('Invalid Customer!!');</script>");
                return;
            }
            //else
            //{
            //    if (custDetail.RoleId == 4 && AppSession.SYSTEM.ToLower() != Enumerations.AccountingSystem.Accounting.ToString().ToLower())
            //    {
            //        context.Response.Write("<script type='text/javascript'>alert('" + Resources.ErrorMsg._500 +
            //                               "');</script>");
            //        return;
            //    }
            //}

            bool isallowupdategivencredit = Sunplus.Accounting.Business.Security.Permissions.GetHostPermission(Enumerations.Functions.Customer.Credit);
            if (isallowupdategivencredit == true)
            {
                Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Credit.CustomerUpdateCredit(_accid, _custid, amount);
                StringBuilder s = new StringBuilder();
                s.Append("parent.document.getElementById('spanGivenCredit').innerHTML ='" + string.Format("{0:n}", amount) + "';");
                s.Append("parent.ChangeGivenCredit();");
                context.Response.Write("<script type='text/javascript'>" + s.ToString() + "</script>");
            }

        }        
        catch (AccException ex)
        {
            context.Response.Write("<script type='text/javascript'>alert('" + ex.Message.ToString() + "');</script>");  
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
