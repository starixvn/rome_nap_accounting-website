<%@ WebHandler Language="C#" Class="UpdateFI" %>
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
using Sunplus.Accounting.Core.Web;

public class JsonDataHR
{
    public string errorCode;
    public string errorMsg;
}
public class UpdateFI : BaseHandler
{
   

    public override void ProcessRequest (HttpContext context)
    {
        base.ProcessRequest(context);
        context.Response.ContentType = "text/plain";
        
        Hashtable param = new Hashtable();
        IBCUtils.IbcError error = IBCUtils.IbcError.SuccessResult;
        string errmsg = "";
        JsonDataHR Error = new JsonDataHR();
        if (context.Session["AccId"] == null)
        {
            Error.errorCode = "500";
            Error.errorMsg =AlertMessage.GetMsg("Session timeout", AlertMessage.MsgType.Error);;
            context.Response.Write(JsonMapper.ToJson(Error));
            return;
        }
        else if (Permissions.CheckPermission(AppSession.AccId, Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
        {
            Error.errorCode = "500";
            Error.errorMsg = AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error); ;
            context.Response.Write(JsonMapper.ToJson(Error));
            return;
        }
        List<int> bettype = new List<int>(new int[] { 31,32,41,42 });
        List<int> bettypeCopyPreset = new List<int>(new int[] { 31,41 });
        int custId = DNAide.Core.CastAide.AsInt32(context.Request[Globals.HIDDEN_CustId]);
        int roleId = DNAide.Core.CastAide.AsInt32(context.Request[Globals.HIDDEN_Role]);
        Globals.AccountType accountType = (Globals.AccountType)roleId;
        param.Add(Globals.HIDDEN_CustId, custId);
        param.Add(Globals.HIDDEN_Role, roleId);
        if (context.Request["isdisablesa"] != null)
        {
            param.Add("isdisablesa", context.Request["isdisablesa"]);
        }
        switch (accountType)
        {
            case Globals.AccountType.Super:
                HRPositionTakingManager.SuperAddParamsPT(context.Request, 151, bettype, ref param);
                break;
            case Globals.AccountType.Master:
                HRPositionTakingManager.MasterAddParamsPT(context.Request, 151, bettype, ref param);
                HRPositionTakingManager.AddParamsCopyPreset(context.Request, bettypeCopyPreset, ref param);
                break;
            case Globals.AccountType.Agent:
                HRPositionTakingManager.AgentAddParamsPT(context.Request, 151, bettype, ref param);
                HRPositionTakingManager.AddParamsCopyPreset(context.Request, bettypeCopyPreset, ref param);
                break;
            case Globals.AccountType.Member:
                HRPositionTakingManager.MemberAddParamsPT(context.Request, 151, bettype, ref param);
                break;
        }
        HRPositionTakingManager.AddParamsMinMaxBet(context.Request,151, ref param);
        HRPositionTakingManager.AddParamsDisabled(context.Request, ref param);
        HRPositionTakingManager.AddParamsComm(context.Request, 151, ref param);
        
        param.Add("userid", AppSession.AccId);
        error = HRPositionTakingManager.UpdatePT(accountType, param);

        if (error.ErrorCode == 0)
        {
            error.ErrorMsg = "Updated Successfully";
            error.ErrorMsg= AlertMessage.GetMsg(error.ErrorMsg,AlertMessage.MsgType.Success);
        }
        //else
        //{
        //    context.Response.Write(error.ErrorMsg);
        //}

        
        Error.errorCode = error.ErrorCode.ToString();
        Error.errorMsg = error.ErrorMsg.ToString();

        context.Response.Write( JsonMapper.ToJson(Error));
    }

    protected override void RedirectError()
    {
        var error = new JsonDataHR
                        {
                            errorCode = "-100",
                            errorMsg =
                                AlertMessage.GetMsg(Resources.ErrorMsg.error_InvalidRequest, AlertMessage.MsgType.Error)
                        };
        _context.Response.Write(JsonMapper.ToJson(error));
        return;

    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}
