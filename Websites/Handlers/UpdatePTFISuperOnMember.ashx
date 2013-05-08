<%@ WebHandler Language="C#" Class="UpdatePTFISuperOnMember" %>
using System.Collections;
using System.Collections.Generic;
using System.Web.SessionState;
using System;
using System.Web;
using DNAide.Core;
using DNAide.JSon;
using Sunplus.Accounting.Business.PositionTaking;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;

public class JsonDataFISuperOnMember
{
    public string errorCode;
    public string errorMsg;
}
public class UpdatePTFISuperOnMember : BaseHandler
{
    protected override void RedirectError()
    {
        JsonDataFISuperOnMember error = new JsonDataFISuperOnMember();
        error.errorCode = "-100";
        error.errorMsg = AlertMessage.GetMsg(Resources.ErrorMsg.error_InvalidRequest, AlertMessage.MsgType.Error);

        _context.Response.Write(JsonMapper.ToJson(error));
    }
    
    
    
    public override void ProcessRequest (HttpContext context)
    {
        base.ProcessRequest(context);
        context.Response.ContentType = "text/plain";
        Hashtable param = new Hashtable();
        IBCUtils.IbcError error = IBCUtils.IbcError.SuccessResult;
        string errmsg = "";
        JsonDataFISuperOnMember Error = new JsonDataFISuperOnMember();
        if (context.Session["AccId"] == null)
        {
            error.ErrorMsg = "Session timeout";
            error.ErrorMsg =AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error);
           
            Error.errorCode = "1";
            Error.errorMsg = error.ErrorMsg.ToString();

            context.Response.Write(JsonMapper.ToJson(Error));
            return;
        }
        else if (Permissions.CheckPermission(AppSession.AccId, Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
        {
            error.ErrorMsg = errmsg;
            error.ErrorMsg =AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error);
            Error.errorCode = "1";
            Error.errorMsg = error.ErrorMsg.ToString();

            context.Response.Write(JsonMapper.ToJson(Error));
            return;
        }
        List<int> bettype = new List<int>(new int[] { 2, 3 });
        int custId = DNAide.Core.CastAide.AsInt32(context.Request[Globals.HIDDEN_CustId]);
        int roleId = DNAide.Core.CastAide.AsInt32(context.Request[Globals.HIDDEN_Role]);
        Globals.AccountType accountType = (Globals.AccountType)roleId;
        param.Add(Globals.HIDDEN_CustId, custId);
        PositionTakingManager.AddParamsPTSuperOnMember(context.Request, 201, bettype, ref param);
        
        param.Add("userid", AppSession.AccId);
        error = PositionTakingManager.UpdatePTSuperOnMember(param);
        if (error.ErrorCode == 0)
        {
            error.ErrorMsg = "Updated Successfully";
            error.ErrorMsg =AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Success);
        }
        else
        {
            error.ErrorMsg = AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error);
            
        }

       
        Error.errorCode = error.ErrorCode.ToString();
        Error.errorMsg = error.ErrorMsg.ToString();

        context.Response.Write(JsonMapper.ToJson(Error));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
