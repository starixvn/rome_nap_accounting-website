<%@ WebHandler Language="C#" Class="InsertOperator" %>

using System;
using System.Web;
using System.Collections;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using System.Web.SessionState;


public class InsertOperator : BaseHandler {
    
    public override void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";        
        if (context.Session["AccId"] == null)
        {
        }
        else
        {
            try
            {
                context.Response.Write("<html><head></head><body>");
                context.Response.Write("<script type='text/javascript'>\n");
                Hashtable resultUpdated = new Hashtable();
                Hashtable hash = new Hashtable();
                hash.Add("custid", 0); //int OUTPUT
                hash.Add("firstname", context.Request["txtFirstname"]); //nvarchar
                hash.Add("lastname", context.Request["txtLastname"]); //nvarchar
                hash.Add("address", context.Request["txtAddress"]); //nvarchar
                hash.Add("postal", context.Request["txtPCode"]); //nvarchar
                hash.Add("city", context.Request["txtCity"]); //nvarchar
                hash.Add("state", context.Request["txtState"]); //nvarchar
                hash.Add("country", context.Request["listCountry"]); //nvarchar
                hash.Add("phone", context.Request["txtPhone"]); //nvarchar
                hash.Add("mobilephone", context.Request["txtMobile"]); //nvarchar
                hash.Add("fax", context.Request["txtFax"]); //nvarchar
                hash.Add("email", context.Request["txtEmail"]); //nvarchar
                hash.Add("creator", context.Session["AccId"]); //int
                hash.Add("username", context.Request["txtUsername"].Trim()); //nvarchar
                hash.Add("userpwd", IBCUtils.Encryption.CfsEnCode(Fanex.Tarus.Security.Services.Encryptor.Decrypt(context.Request["txtPwd"].Trim(), context.Request["code"]))); //nvarchar
                hash.Add("roleid", context.Request["listRole"]); //int
                hash.Add("returnerrcode",1); //int
                hash.Add("errcode", 0); //int
                hash.Add("errmsg", ""); //int
                resultUpdated = UpdateCustomer.CustomerInsOperatorRpt(hash);
                if (Convert.ToString(resultUpdated["errCode"]) == "0")
                {
                    context.Response.Write("parent.location.href='../Customer/OperatorList.aspx';");
                }
                else
                {
                    string msg=resultUpdated["errMsg"] as string ?? string.Empty;
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(msg, AlertMessage.MsgType.Error) + "','errMsg');");                    
                }
            }
            catch
            {
               
                GotoErroPage(context);
            }
            finally
            {
                context.Response.Write("</script>");
                context.Response.Write("</body></html>");
            }
        }     
    }
 
    public override bool IsReusable {
        get {
            return false;
        }
    }

}
