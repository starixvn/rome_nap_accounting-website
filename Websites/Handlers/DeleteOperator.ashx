<%@ WebHandler Language="C#" Class="DeleteOperator" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;


public class DeleteOperator : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        if (context.Session["AccId"] == null)
        {
        }
        else
        {
            context.Response.Write("<script>");
            try
            {
                Hashtable resultUpdated = new Hashtable();
                int custId = Convert.ToInt32(context.Request.QueryString["operatorId"]); //int  id of current Operator                
                int userId = Convert.ToInt32(context.Session["AccId"], System.Globalization.CultureInfo.CurrentUICulture); //int id of current Login Admin
                resultUpdated = UpdateCustomer.CustomerDelOperatorRpt(custId, "E", userId);
                if (Convert.ToString(resultUpdated["errCode"]) == "0")
                {
                    context.Response.Write("parent.location.href='../Customer/OperatorList.aspx';" + Environment.NewLine);
                }
                else
                {
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(resultUpdated["errMsg"].ToString(), AlertMessage.MsgType.Error) + "','errMsg');");
                }

            }
            catch
            {
                context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(Resources.Language.thereisnodata, AlertMessage.MsgType.Error) + "','errMsg');");
            }
            finally
            {
                context.Response.Write("</script>");
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
