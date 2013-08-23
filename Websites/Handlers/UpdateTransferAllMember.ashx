<%@ WebHandler Language="C#" Class="UpdateTransferAllMember" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.DAL;
using IBCUtils.Web;
using DNAide.Data;


public class UpdateTransferAllMember : BaseHandler
{
    public override void ProcessRequest(HttpContext context)
    {
        base.ProcessRequest(context);
        context.Response.ContentType = "text/html";
        try
        {
            int custID = CastAide.AsInt32(context.Session["AccId"], -1);
            if (custID == -1) throw new AccException(IBCUtils.IbcError.ACCESS_DENIED_MSG);
            string agentName = Username;
            agentName = agentName.Trim();

            string type = CastAide.AsString(context.Request["type"]);
            var result = new Sunplus.Accounting.Entities.Result();
            if (type == "members")
            {
                result = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Transfer.TransferAllMember(custID, agentName, context.Request.UserHostAddress);
            }
            else
            {
                result = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Transfer.TransferAllDownlines(custID, agentName, context.Request.UserHostAddress);
            }
            if (result.ErrorCode > 0)
            {
                string errMsg = result.ErrorMessage.Replace("downlines", type);
                context.Response.Write("0|" + AlertMessage.GetMsg(errMsg, AlertMessage.MsgType.Error));
            }
            else
            {
                context.Response.Write("1|" + AlertMessage.GetMsg("Transfer successfully!", AlertMessage.MsgType.Success));
            }
        }
        catch (AccException ex)
        {
            context.Response.Write("0|" + ex.Message.Replace("'", string.Empty));
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            context.Response.Write("0|" + ex.Message.Replace("'", string.Empty));
        }
    }

    protected override void RedirectError()
    {
        _context.Response.Write("0|" + AlertMessage.GetMsg(Resources.ErrorMsg.error_InvalidUser, AlertMessage.MsgType.Error));
        return;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
