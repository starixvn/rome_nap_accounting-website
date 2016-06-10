<%@ WebHandler Language="C#" Class="CheckUserName" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils.DAL;
using DNAide.Data;


public class CheckUserName : IHttpHandler, IRequiresSessionState
{
    private string siteName = string.Empty;
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string username = context.Request["user"] as string ?? string.Empty;
        context.Response.Write("<html><head></head><body>");
        context.Response.Write("<script type=\"text/javascript\">");
        try
        {
            string strType = context.Request["listType"] as string ?? string.Empty;
            var ds = SelectCustomer.CustSelByUsername(username, AppSession.SYSTEM);
            if (ds != null)
            {
                if (ds.Tables.Count> 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var row = ds.Tables[0].Rows[0];
                    int roleId = 0;
                    int.TryParse(ds.Tables[0].Rows[0]["roleid"].ToString(), out roleId);
                    bool isDeposit = Sunplus.Accounting.Core.Configurations.WebSettings.IsApplySplitingDepositeCustomer ?  IsDeposit(username) : false;
                    context.Response.Write("var siteName = '" + siteName + "';");
                    //bool isDeposit = IsDeposit(username);
                    if ((string.Compare(strType, "member", true) == 0 && roleId != 2) || (string.Compare(strType, "member", true) == 0 && isDeposit))
                    {
                        context.Response.Write(" parent.parent.document.body.rows = '55,*';parent.showAlertMessage('errMsg','This Agent does not exist.',1);");
                    }
                    else if ((string.Compare(strType, "agent", true) == 0 && roleId != 3) || (string.Compare(strType, "agent", true) == 0 && isDeposit))
                    {
                        context.Response.Write("parent.parent.document.body.rows = '55,*';parent.showAlertMessage('errMsg','This Master does not exist.',1);");
                    }
                    else
                    {
                        context.Response.Write("parent.document.getElementById('frmSearchCopyAddNewSuper').style.display='block';parent.OpenFormAdd(" + ds.Tables[0].Rows[0]["custid"] + "," + ds.Tables[0].Rows[0]["roleid"] + ",'" + siteName + "');");
                    }
                }
                else
                {
                    context.Response.Write("parent.parent.document.body.rows = '55,*';parent.showAlertMessage('errMsg','This Username does not exist.',1);");
                }
            }
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        context.Response.Write("</script>");
        context.Response.Write("</body></html>");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private bool IsDeposit(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName)) return false;
        var customerService = new Sunplus.Accounting.Business.Customer.CustomerService();
        var ds = customerService.GetCustomerInfo(userName, AppSession.SYSTEM);
        siteName = ds.ResultSet.Tables[0].Rows[0]["site"].ToString();
        if (ds == null || ds.ResultSet.Tables.Count == 0) return false;
        if (ds.ResultSet.Tables[0].Rows.Count == 0) return false;
        System.Data.DataRow row1 = ds.ResultSet.Tables[0].Rows[0];
        return DNAide.Core.CastAide.AsBoolean(row1["IsDeposit"]);
    }

}
