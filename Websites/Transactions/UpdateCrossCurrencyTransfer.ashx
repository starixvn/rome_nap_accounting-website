<%@ WebHandler Language="C#" Class="UpdateCrossCurrencyTransfer" %>

using System;
using System.Web;
using System.Data;
using System.IO;
using DNAide.Web;
using System.Collections.Generic;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business;
using System.Text;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Accounting.Business.Customer;
using DNAide.Core;

public class UpdateCrossCurrencyTransfer : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        IUserService userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
        if (string.IsNullOrEmpty(context.Request.QueryString["delTemp"]) || context.Request.QueryString["delTemp"].Trim() == "true")
        {
            userService.DelDataTmpExcel(AppSession.AccId);
            return;
        }

        string fileName = !string.IsNullOrEmpty(context.Request.QueryString["filename"]) ? context.Request.QueryString["filename"] : string.Empty;
        string fileSize = !string.IsNullOrEmpty(context.Request.QueryString["filesize"]) ? context.Request.QueryString["filesize"] : string.Empty;
        string forexName = !string.IsNullOrEmpty(context.Request.QueryString["forexname"]) ? context.Request.QueryString["forexname"] : string.Empty;

        if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(fileSize) || string.IsNullOrEmpty(forexName))
        {
            context.Response.Write(BuildResult("Invalid information", string.Empty));
            return;
        }
        DataSet dbr = null;
        try
        {
            var errorInfo = new System.Collections.Hashtable();
            dbr = userService.UpdFinanceCrossTransferMul(AppSession.AccId, fileName, CastAide.AsInt32(fileSize, 0), context.Request.UserHostAddress, forexName, ref errorInfo);

            if (dbr == null | CastAide.AsInt32(errorInfo["errcode"]) != 0)
            {
                context.Response.Write(BuildResult(errorInfo["errmsg"].ToString(), string.Empty));
                return;
            }

            userService.DelDataTmpExcel(AppSession.AccId);
            context.Response.Write(BuildResult(string.Empty, RenderContent(dbr.Tables[0], dbr.Tables[1])));
        }
        catch (Exception ex)
        {
            context.Response.Write(BuildResult(ex.Message, string.Empty));
        }
    }

    private string BuildResult(string msg, string data)
    {
        return msg + "|" + data;
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    private string RenderContent(DataTable dtbError, DataTable dtbSuccess)
    {
        string strTemplateErrorPart = string.Empty;
        string strTemplateSuccessPart = string.Empty;

        //------------------------------------
        if (dtbError.Rows.Count > 0)
        {
            RenderErrorPart(dtbError, ref strTemplateErrorPart);
        }
        RenderSuccessPart(dtbError.Rows.Count, dtbSuccess, ref strTemplateSuccessPart);

        return ReplaceToken(strTemplateErrorPart) + ReplaceToken(strTemplateSuccessPart);
    }

    private void RenderErrorPart(DataTable dataTable, ref string strTemplate)
    {
        strTemplate += "<div id='divErrorMsgPart' class='alertboxerr'><div>Failed transactions, please check!</div></div>";
        strTemplate += WebUtils.LoadTemplate("Transactions/CrossCurrencyTransfer.htm");
        TemplateHandler.RemoveBlock(ref strTemplate, "successpart");
        TemplateHandler.RemoveBlock(ref strTemplate, "reasonheader");
        TemplateHandler.RemoveBlock(ref strTemplate, "reason");

        string strContent = TemplateHandler.CopyBlock(strTemplate, "content");

        string strConentTemp = string.Empty;

        string fromcust = string.Empty;
        string tocust = string.Empty;
        double fromamount;
        double toamount;
        string remark = string.Empty;

        StringBuilder stb = new StringBuilder();

        foreach (DataRow row in dataTable.Rows)
        {
            strConentTemp = strContent;

            fromcust = row.IsNull(0) ? string.Empty : row[0].ToString();
            tocust = row.IsNull(1) ? string.Empty : row[1].ToString();

            fromamount = row.IsNull(2) ? 0 : DNAide.Core.CastAide.AsDouble(row[2].ToString().Trim());
            toamount = row.IsNull(3) ? 0 : DNAide.Core.CastAide.AsDouble(row[3].ToString().Trim());

            remark = row.IsNull(4) ? string.Empty : row[4].ToString();

            TemplateHandler.Assign(ref strConentTemp, "fromcust", fromcust);
            TemplateHandler.Assign(ref strConentTemp, "tocust", tocust);

            TemplateHandler.Assign(ref strConentTemp, "fromamount", IBCUtils.FormatHandler.FormatNumber2(fromamount));
            TemplateHandler.Assign(ref strConentTemp, "toamount", IBCUtils.FormatHandler.FormatNumber2(toamount));

            TemplateHandler.Assign(ref strConentTemp, "remark", remark);

            TemplateHandler.Assign(ref strConentTemp, "rowstyle", "class='add-customer-bggray'");
            stb.Append(strConentTemp);
        }

        TemplateHandler.ReplaceBlock(ref strTemplate, "content", stb.ToString());
    }

    private void RenderSuccessPart(int errorCount, DataTable dataTable, ref string strTemplate)
    {
        strTemplate += (errorCount <= 0 ? string.Empty : "<br/>")
            + "<div id='divSuccessMsgPart' class='alertboxscc'><div>Successful transactions</div></div>";
        strTemplate += WebUtils.LoadTemplate("Transactions/CrossCurrencyTransfer.htm");
        TemplateHandler.RemoveBlock(ref strTemplate, "errorpart");

        string strContent = TemplateHandler.CopyBlock(strTemplate, "content");
        string strConentTemp = string.Empty;

        string fromcust = string.Empty;
        string tocust = string.Empty;
        double fromamount;
        double toamount;
        string remark = string.Empty;

        StringBuilder stb = new StringBuilder();

        foreach (DataRow row in dataTable.Rows)
        {
            strConentTemp = strContent;

            fromcust = row.IsNull(0) ? string.Empty : row[0].ToString();
            tocust = row.IsNull(1) ? string.Empty : row[1].ToString();

            fromamount = row.IsNull(2) ? 0 : DNAide.Core.CastAide.AsDouble(row[2].ToString().Trim());
            toamount = row.IsNull(3) ? 0 : DNAide.Core.CastAide.AsDouble(row[3].ToString().Trim());

            remark = row.IsNull(4) ? string.Empty : row[4].ToString();

            TemplateHandler.Assign(ref strConentTemp, "fromcust", fromcust);
            TemplateHandler.Assign(ref strConentTemp, "tocust", tocust);

            TemplateHandler.Assign(ref strConentTemp, "fromamount", IBCUtils.FormatHandler.FormatNumber2(fromamount));
            TemplateHandler.Assign(ref strConentTemp, "toamount", IBCUtils.FormatHandler.FormatNumber2(toamount));

            TemplateHandler.Assign(ref strConentTemp, "remark", remark);

            TemplateHandler.Assign(ref strConentTemp, "rowstyle", string.Empty);
            stb.Append(strConentTemp);
        }
        TemplateHandler.ReplaceBlock(ref strTemplate, "content", stb.ToString());
    }
    
    private string ReplaceToken(string strTemplate)
    {
        strTemplate = strTemplate.Replace("<!--errorpart-->", string.Empty);
        strTemplate = strTemplate.Replace("<!--/errorpart-->", string.Empty);

        strTemplate = strTemplate.Replace("<!--successpart-->", string.Empty);
        strTemplate = strTemplate.Replace("<!--/successpart-->", string.Empty);

        strTemplate = strTemplate.Replace("<!--reasonheader-->", string.Empty);
        strTemplate = strTemplate.Replace("<!--/reasonheader-->", string.Empty);
        strTemplate = strTemplate.Replace("<!--content-->", string.Empty);
        strTemplate = strTemplate.Replace("<!--/content-->", string.Empty);
        strTemplate = strTemplate.Replace("<!--reason-->", string.Empty);
        strTemplate = strTemplate.Replace("<!--/reason-->", string.Empty);
        return strTemplate;
    }
}