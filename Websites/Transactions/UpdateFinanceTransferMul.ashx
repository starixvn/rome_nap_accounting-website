<%@ WebHandler Language="C#" Class="UpdateFinanceTransferMul" %>

using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.SessionState;
using DNAide.Web;
using DNAide.Data;
using DNAide.Core;
using DNAide.Utils;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;



public class UpdateFinanceTransferMul : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        try
        {
            //Get Data here

            if (!Permissions.GetHostPermission<Enumerations.Functions.FinancialManagement>(Enumerations.Functions.FinancialManagement.AutomateFinanceTransfer))
            {
                context.Response.Write(AlertMessage.GetMsg("You are not authorized on this function.", AlertMessage.MsgType.Error));
                return;
            }
            
            IUserService userService = CommonBootStrapper.Locator.GetInstance<IUserService>();   
            
            if (CastAide.AsString(context.Request["bolUnLoadPage"]) == "0")
            {
                int Index = CastAide.AsInt32(context.Request["Index"]);
                var updateError = new System.Collections.Hashtable();                
                var dbr = userService.UpdFinanceTransferMul(AppSession.AccId, context.Request["Filename"], CastAide.AsInt32(context.Request["Filesize"], 0), context.Request.UserHostAddress, ref updateError);
                if (dbr == null | CastAide.AsInt32(updateError["errcode"]) != 0)
                {
                    context.Response.Write(AlertMessage.GetMsg(updateError["errmsg"].ToString(), AlertMessage.MsgType.Error));
                    return;
                }

                userService.DelDataTmpExcel(AppSession.AccId);//Delete temp data in table Acc_CustFinanceTransfer

                DataTable dtbFail = dbr.Tables[0];
                DataTable dtbSucc = dbr.Tables[1];
                //Render html table success and fail
                string strFail = dtbFail.Rows.Count > 0 ? AlertMessage.GetMsg("Failed transactions", AlertMessage.MsgType.Error) + GenHTML(dtbFail, "add-customer-bggray", true) + "<br />" : string.Empty;
                string strSucc = dtbSucc.Rows.Count > 0 ?AlertMessage.GetMsg("Successful transactions", AlertMessage.MsgType.Success) + GenHTML(dtbSucc, string.Empty, false) : string.Empty;
                
                //context.Response.Write(AlertMessage.GetMsg("Updated successfully", AlertMessage.MsgType.Success));
                context.Response.Write(strFail + strSucc);
            }
            else
                userService.DelDataTmpExcel(AppSession.AccId);
        }
        catch (Exception ex)
        {
            context.Response.Write(AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error));
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }      
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    string GenHTML(DataTable dtb, string rowStyle, bool hasReason)
    {
        string strResult = WebUtils.LoadTemplate("Transactions/AutomateTransfer.htm");
        if (!hasReason)
        {
            TemplateHandler.ReplaceBlock(ref strResult, "hreason", string.Empty);
            TemplateHandler.ReplaceBlock(ref strResult, "creason", string.Empty);
        }
        
        string strContent = TemplateHandler.CopyBlock(strResult, "content");
        TemplateHandler.Assign(ref strContent, "rowstyle", rowStyle);        

        StringBuilder strBuild = new StringBuilder();
        //Generate data
        if (dtb.Rows.Count > 0)
        {
            foreach (DataRow row in dtb.Rows)
            {
                string strTmp = strContent;
                TemplateHandler.Assign(ref strTmp, "no", row["Seq"]);
                TemplateHandler.Assign(ref strTmp, "fromcust", row["FromUserName"]);
                TemplateHandler.Assign(ref strTmp, "tocust", row["ToUserName"]);
                TemplateHandler.Assign(ref strTmp, "amount", NumberFormat.DecFormat(row["amount"], 2));
                TemplateHandler.Assign(ref strTmp, "actualrate", SplitString(row["ExchangeRate"].ToString()));
                TemplateHandler.Assign(ref strTmp, "description", row["Description"]);
                if (hasReason)
                    TemplateHandler.Assign(ref strTmp, "reason", CastAide.AsString(row["reason"], string.Empty));
                //TemplateHandler.Assign(ref strTmp, "description", row["Description"]);
                strBuild.Append(strTmp);
            }
        }

        TemplateHandler.ReplaceBlock(ref strResult, "content", strBuild.ToString());
        return strResult;
    }

    private string SplitString(string value)
    {
        var result = string.Empty;
        var arr = value.Split(new char[] { '.' });
        result = arr[0];
        if (arr.Length > 1)
        {
            result += ".";
            var decimalvalue = arr[1];
            for (int i = 0; i < decimalvalue.Length && i <= 3; i++)
            {
                result += decimalvalue[i];
            }
        }

        return result;
    }


}