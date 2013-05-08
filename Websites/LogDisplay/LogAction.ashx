<%@ WebHandler Language="C#" Class="LogAction" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using System.Web.UI.WebControls;
public class LogAction : BaseHandler
{
    
    public override void ProcessRequest (HttpContext context) {
        base.ProcessRequest(context);
        string type = CastAide.AsString(context.Request["type"]);
        string result = string.Empty;
        if (type.Trim().ToLower() == "logaction")
        {
            string txtFilter = CastAide.AsString(context.Request["filter"]);
            string lang = CastAide.AsString(context.Request["lang"]);
            string actionType = CastAide.AsString(context.Request["actiontype"]);
            result = Sunplus.Accounting.Business.LogDisplay.LogAction.GetActionFilterContent(txtFilter, lang, actionType);
            context.Response.Write(result);
        }
        else if (type.Trim().ToLower() == "logactionupdate")
        {
            string actionID = CastAide.AsString(context.Request["actionid"]);
            string txtEN = CastAide.AsString(context.Request["txtEN"]);
            string txtCH = CastAide.AsString(context.Request["txtCH"]);
            string txtCS = CastAide.AsString(context.Request["txtCS"]);
            string txtJP = CastAide.AsString(context.Request["txtJP"]);
            string txtTH = CastAide.AsString(context.Request["txtTH"]);
            string txtKO = CastAide.AsString(context.Request["txtKO"]);
            string txtIT = CastAide.AsString(context.Request["txtIT"]);
            string isInternal = CastAide.AsString(context.Request["internal"]);
            string txtSPName = CastAide.AsString(context.Request["txtSPName"]);
            string actionType = CastAide.AsString(context.Request["actionType"]);
            result = Sunplus.Accounting.Business.LogDisplay.LogAction.EditLogAction(actionID, actionType, txtEN, txtCH, txtCS, txtJP, txtTH, txtKO, txtIT, isInternal, txtSPName);
            context.Response.Write(result);
        }
        else if (type.Trim().ToLower() == "logactiontype")
        {
            result = Sunplus.Accounting.Business.LogDisplay.LogAction.GetActionTypeContent();
            context.Response.Write(result);
        }
        else if (type.Trim().ToLower() == "logactiontypeupdate")
        {
             string actionTypeID = CastAide.AsString(context.Request["actiontypeid"]);
            string txtEN = CastAide.AsString(context.Request["txtEN"]);
            string txtCH = CastAide.AsString(context.Request["txtCH"]);
            string txtCS = CastAide.AsString(context.Request["txtCS"]);
            string txtJP = CastAide.AsString(context.Request["txtJP"]);
            string txtTH = CastAide.AsString(context.Request["txtTH"]);
            string txtKO = CastAide.AsString(context.Request["txtKO"]);
            string txtIT = CastAide.AsString(context.Request["txtIT"]);
            result = Sunplus.Accounting.Business.LogDisplay.LogAction.EditLogActionType(actionTypeID, txtEN, txtCH, txtCS, txtJP, txtTH, txtKO, txtIT);
            context.Response.Write(result);
        }
    }

    protected override bool HasPermission()
    {
        return Sunplus.Accounting.Core.Common.Functions.Customer.AllowViewLog && base.HasPermission();
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}
