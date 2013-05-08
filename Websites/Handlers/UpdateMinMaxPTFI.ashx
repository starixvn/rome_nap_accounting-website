<%@ WebHandler Language="C#" Class="UpdateMinMaxPTFI" %>
using System.Collections;
using System.Collections.Generic;
using System.Web.SessionState;
using System;
using System.Web;
using DNAide.Core;
using DNAide.JSon;
using Sunplus.Accounting.Business.PositionTaking;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;

public class JsonDataMinMaxPTFI
{
    public string errorCode;
    public string errorMsg;
}

public class UpdateMinMaxPTFI : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context)
    {
        base.ProcessRequest(context);
        
        context.Response.ContentType = "text/html";       
        
        Hashtable param = new Hashtable();
        ErrorDetail error = new ErrorDetail();
        string errmsg = "";
        JsonDataMinMaxPTFI Error = new JsonDataMinMaxPTFI();
        if (context.Session["AccId"] == null)
        {
            error.ErrorMessage = "Session timeout";
            error.ErrorMessage =AlertMessage.GetMsg(error.ErrorMessage, AlertMessage.MsgType.Error);
          
            Error.errorCode = "1";
            Error.errorMsg = error.ErrorMessage.ToString();

            context.Response.Write(JsonMapper.ToJson(Error));
            
            return;
        }
        else if (Permissions.CheckPermission(AppSession.AccId, Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
        {
            error.ErrorMessage = errmsg;
            error.ErrorMessage =AlertMessage.GetMsg(error.ErrorMessage, AlertMessage.MsgType.Error);
            Error.errorCode = "1";
            Error.errorMsg = error.ErrorMessage.ToString();

            context.Response.Write(JsonMapper.ToJson(Error));
            
            
            return;
        }
        List<int> bettype = new List<int>(new int[] { 2, 3 });
        int custId = CustId;
        int roleId = DNAide.Core.CastAide.AsInt32(context.Request[Globals.HIDDEN_Role]);
        Globals.AccountType accountType = (Globals.AccountType)roleId;
        param.Add(Globals.HIDDEN_CustId, custId);
        PositionTakingManager.AddParamsMinMaxPT(context.Request, 201, bettype, ref param);
        param.Add("userid", AppSession.AccId);
        error = PositionTakingManager.UpdateMinMaxPT(accountType, param);
        IBCUtils.Encryption.RemoveSpecialChars(error.ErrorMessage);
        if (error.ErrorCode == 0)
        {
            error.ErrorMessage = "Updated Successfully";
            error.ErrorMessage=AlertMessage.GetMsg(error.ErrorMessage, AlertMessage.MsgType.Success);
        }
        else
        {
            error.ErrorMessage = AlertMessage.GetMsg(error.ErrorMessage, AlertMessage.MsgType.Error);
        }
       
        Error.errorCode = error.ErrorCode.ToString();
        Error.errorMsg = error.ErrorMessage.ToString();

        context.Response.Write(JsonMapper.ToJson(Error));
    }

    protected override void RedirectError()
    {
        JsonDataMinMaxPTFI Error = new JsonDataMinMaxPTFI();
        Error.errorCode = "-100";
        Error.errorMsg = AlertMessage.GetMsg(Resources.ErrorMsg.error_InvalidRequest, AlertMessage.MsgType.Error);

        _context.Response.Write(JsonMapper.ToJson(Error));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
