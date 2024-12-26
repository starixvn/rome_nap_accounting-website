<%@ WebHandler Language="C#" Class="AddWhiteLabelTransaction" %>

using System;
using System.Web;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using System.Web.SessionState;
using Fanex.Data;
using Fanex.CustomerSetting.Module.Core.Models;
using System.Collections;


public class AddWhiteLabelTransaction : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        context.Response.Write("<script type=\"text/javascript\">");
        try
        {
            if (context.Session["AccId"] == null) return;
            int custId = Convert.ToInt32(context.Request.QueryString["CustID"]);
            DateTime transDate = Convert.ToDateTime(context.Request["txtWhiteLabelDate"]);
            double amount = Convert.ToDouble(context.Request["txtWhiteLabelAmount"]);
            string transDesc = context.Request["txtWhiteLabelDesc"];
            int creator = DNAide.Core.CastAide.AsInt32(context.Session["AccId"]);
            var parameters = new Hashtable() {
                { "custid", custId },
                { "amount", amount },
                { "winlostdate", transDate },
                { "transdesc", transDesc },
                { "creator", creator },
                { "sitedbgroupid", ObjectDbExtensions.GetSiteDBConfigByCustId(custId).SiteDBGroupID }
            };

            var result = Acc_WhiteLabel_TransactionIns(parameters, custId);

            if (result.IsSuccess)
            {
                context.Response.Write("parent.location.href='../Statement/CustomerStatement.aspx?repno=7&CustName=" + context.Request.QueryString["CustName"] + "&isWhiteLabel=true';");
            }
            else
            {
                context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(result.ErrorMessage, AlertMessage.MsgType.Error) + "','errMsgWL');");
            }
        }

        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) + "','errMsg');");
        }

        context.Response.Write("</script>");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private Result Acc_WhiteLabel_TransactionIns(Hashtable parameters, int custId)
    {
        using (var objDb = new ObjectDb("Acc_WhiteLabel_TransactionIns"))
        {
            objDb.ExecuteNonQueryScaleoutDB(parameters, custId);

            var outputs = objDb.GetOuputParams();
            return new Result(Convert.ToInt32(outputs["errcode"]), outputs["errmsg"].ToString());
        }
    }

}
