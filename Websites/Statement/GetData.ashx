<%@ WebHandler Language="C#" Class="GetData" %>

using DNAide.Core;
using Fanex.Tarus.Business.Implement;
using Sunplus.Accounting.Business.Statement;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using System.Web;

public class GetData : BaseHandler
{
    protected override void RedirectError()
    {
        _context.Response.Write(AlertMessage.GetMsg(Resources.ErrorMsg.error_InvalidRequest, AlertMessage.MsgType.Error));
    }

    protected override string Username
    {
        get
        {
            return CastAide.AsString(System.Web.HttpUtility.UrlDecode(_context.Request["custname"]));
        }
    }

    public override void ProcessRequest(HttpContext context)
    {
        base.ProcessRequest(context);
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        int reportNo = 1;
        string strData = string.Empty;
        reportNo = CastAide.AsInt32(context.Request["repno"], 1);
        var isReward = CastAide.AsBoolean(context.Request["isReward"]);
        if (reportNo != 7 && (reportNo < 1 || reportNo > 5 || reportNo == 2)) //combine reportNo 1&2
            reportNo = 1;

        var helper = new Helper();
        if (helper.CheckIsInternal(Username))
        {
            context.Response.Write("<div class=\"alertboxerr\"><div>This function supports to view statement of Real Accounts only!</div></div>");
            return;
        }

        switch (reportNo)
        {
            case 1:
                strData = StatementHTML.GetMemberStatement(CastAide.AsString(context.Request["fdate"]), CastAide.AsString(context.Request["tdate"]), isReward);
                AppSession.CustUName = Username;
                context.Response.Write(strData);
                break;

            case 3:
                strData = StatementHTML.GetDetailStatement(CastAide.AsString(context.Request["fdate"]), CastAide.AsString(context.Request["tdate"]), isReward);
                context.Response.Write(strData);
                break;

            case 4:
                strData = StatementHTML.GetAuditStatement(isReward);
                context.Response.Write(strData);
                break;

            case 5:
                strData = StatementHTML.GetCompanyStatement(
                    CastAide.AsString(context.Request["fdate"]),
                    CastAide.AsString(context.Request["tdate"]));
                context.Response.Write(strData);
                break;

            case 7: 
                strData = StatementHTML.GetWhiteLabelTxn(
                    CastAide.AsString(context.Request["custname"]),
                    CastAide.AsString(context.Request["fdate"]),
                    CastAide.AsString(context.Request["tdate"]));
                context.Response.Write(strData);
                break;

        }
    }

    public override bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
