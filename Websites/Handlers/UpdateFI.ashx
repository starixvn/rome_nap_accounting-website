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

class JsonDataFI
{
    public string errorCode;
    public string errorMsg;
}

public class UpdateFI : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context)
    {
        base.ProcessRequest(context);
        Hashtable param = new Hashtable();
        IBCUtils.IbcError error = IBCUtils.IbcError.SuccessResult;
        string errmsg = "";
        JsonDataFI Error = new JsonDataFI();
        if (context.Session["AccId"] == null)
        {
            error.ErrorMsg = "Session timeout";
            error.ErrorMsg=AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error);
           
            Error.errorCode = "500";
            Error.errorMsg = error.ErrorMsg.ToString();

            context.Response.Write(JsonMapper.ToJson(Error));
            return;
        }
        else if (Permissions.CheckPermission(AppSession.AccId, Enumerations.Functions.Customer.OtherCust, ref errmsg) != 0)
        {
            error.ErrorMsg = errmsg;
            error.ErrorMsg =AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error);

            Error.errorCode = "500";
            Error.errorMsg = error.ErrorMsg.ToString();

            context.Response.Write(JsonMapper.ToJson(Error));
            
            return;
        }
        List<int> bettype = new List<int>(new int[] { 2, 3 });
        int custId = DNAide.Core.CastAide.AsInt32(context.Request[Globals.HIDDEN_CustId]);
        int roleId = DNAide.Core.CastAide.AsInt32(context.Request[Globals.HIDDEN_Role]);
        Globals.AccountType accountType = (Globals.AccountType)roleId;
        param.Add(Globals.HIDDEN_CustId, custId);
        param.Add(Globals.HIDDEN_Role, roleId);

        switch (accountType)
        {
            case Globals.AccountType.Super:
                PositionTakingManager.SuperAddParamsPT(context.Request, 201, bettype, ref param);
                break;
            case Globals.AccountType.Master:
                PositionTakingManager.MasterAddParamsPT(context.Request, 201, bettype, ref param);
                break;
            case Globals.AccountType.Agent:
                PositionTakingManager.AgentAddParamsPT(context.Request, 201, bettype, ref param);
                break;
            case Globals.AccountType.Member:
                PositionTakingManager.MemberAddParamsPT(context.Request, 201, bettype, ref param);
                break;
        }
        PositionTakingManager.AddParamsMinMaxBet(context.Request,201, ref param);
        PositionTakingManager.AddParamsDisabled(context.Request, ref param);
        PositionTakingManager.AddParamsComm(context.Request, 201, ref param);
        
        param.Add("userid", AppSession.AccId);
        error = PositionTakingManager.UpdatePT(accountType, param);

        if (error.ErrorCode == 0)
        {
            error.ErrorMsg = "Updated Successfully";
            error.ErrorMsg = AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Success);
        }
       
      
        Error.errorCode = error.ErrorCode.ToString();
        Error.errorMsg = error.ErrorMsg.ToString();

        context.Response.Write(JsonMapper.ToJson(Error));
    }

    protected override void RedirectError()
    {
        var error = new JsonDataFI
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
