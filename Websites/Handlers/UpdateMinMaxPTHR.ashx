<%@ WebHandler Language="C#" Class="UpdateMinMaxPTHR" %>
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

public class JsonDataMinMaxPTHR
{
    public string errorCode;
    public string errorMsg;
}

public class UpdateMinMaxPTHR : BaseHandler 
{
    
    public override void ProcessRequest (HttpContext context)
    {
        base.ProcessRequest(context);
        
        context.Response.ContentType = "text/plain";
        Hashtable param = new Hashtable();
        ErrorDetail error = new ErrorDetail();
        string errmsg = "";
        JsonDataMinMaxPTHR Error = new JsonDataMinMaxPTHR();
        if (context.Session["AccId"] == null)
        {
         
            Error.errorCode = "-1";
            Error.errorMsg =AlertMessage.GetMsg("Session timeout", AlertMessage.MsgType.Error);
            context.Response.Write(JsonMapper.ToJson(Error));
            return;
        }
        else if (Permissions.CheckPermission(AppSession.AccId, Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
        {
            Error.errorCode = "-1";
            Error.errorMsg = AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error);
            context.Response.Write(JsonMapper.ToJson(Error));
            return;
        }
        List<int> bettype = new List<int>(new int[] { 31, 32, 41, 42 });
        int custId = CustId;
        int roleId = DNAide.Core.CastAide.AsInt32(context.Request[Globals.HIDDEN_Role]);
        Globals.AccountType accountType = (Globals.AccountType)roleId;
        param.Add(Globals.HIDDEN_CustId, custId);
        HRPositionTakingManager.AddParamsMinMaxPT(context.Request,151, bettype, ref param);
        param.Add("userid", AppSession.AccId);
        error = HRPositionTakingManager.UpdateMinMaxPT(accountType, param);
        error.ErrorMessage = IBCUtils.Encryption.RemoveSpecialChars(error.ErrorMessage);
        if (error.ErrorCode == 0)
        {
            error.ErrorMessage ="Updated Successfully";
            //error.ErrorMessage = AlertMessage.GetMsg(error.ErrorMessage, AlertMessage.MsgType.Success);
        }
        else
        {
           // error.ErrorMessage = AlertMessage.GetMsg(, AlertMessage.MsgType.Error);
        }

      
        Error.errorCode = error.ErrorCode.ToString();
        Error.errorMsg = error.ErrorMessage.ToString();

        context.Response.Write(JsonMapper.ToJson(Error));
        
    }

    protected override void RedirectError()
    {
        JsonDataMinMaxPTHR error = new JsonDataMinMaxPTHR();
        error.errorCode = "-100";
        error.errorMsg = AlertMessage.GetMsg(Resources.ErrorMsg.error_InvalidRequest, AlertMessage.MsgType.Error);
        _context.Response.Write(JsonMapper.ToJson(error));
        return;
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}
