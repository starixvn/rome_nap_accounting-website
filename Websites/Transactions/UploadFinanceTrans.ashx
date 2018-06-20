<%@ WebHandler Language="C#" Class="UploadFinanceTrans" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Data;
using System.Web.SessionState;
using DNAide.Web;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Security;
using Fanex.Tarus.Excels.AutomateTransfer;
using Sunplus.Accounting.Core.Web;
using DNAide.Core;
using DNAide.Utils;
using System.Text;
using Sunplus.Accounting.Core.Common;
using System.Linq;

public class UploadFinanceTrans : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";

        if (!Permissions.GetHostPermission(Enumerations.Functions.FinancialManagement.AutomateFinanceTransfer))
        {
            context.Response.Write(BuildResult(Resources.ErrorMsg._500, string.Empty));
            return;
        }

        try
        {
            var uploadFile = context.Request.Files[0];

            if (uploadFile.ContentLength > 1024000)
            {
                context.Response.Write(BuildResult("Your uploaded file's size can't be larger than 1 MB", string.Empty));
                return;
            }

            string ext = string.Empty;
            if (!AccCommon.IsValidExtension(uploadFile.FileName, ref ext))
            {
                context.Response.Write(BuildResult("Sorry, You choose invalid file extension. Please choose an Excel file (.xls)", string.Empty));
                return;
            }

            IAutomateTransferExcel automateTransferExcel = new AutomateTransferExcel(uploadFile.InputStream);
            var result = automateTransferExcel.ValidateTemplateFile();
            
            if (!result.IsSuccessful)
            {
                context.Response.Write(BuildResult(result.Message, string.Empty));
                return;
            }

            DataTable dtb = automateTransferExcel.ExcelToDataTable();

            if (dtb.Rows.Count <= 0)
            {
                context.Response.Write(BuildResult("Data is empty.", string.Empty));
                return;
            }

            var htmlData = GenHtml(dtb);
            var fileSize = "<input type='hidden' id='FileSize' value='" + context.Request.ContentLength + "'/>";
            context.Response.Write(BuildResult(string.Empty, htmlData + fileSize));
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    string GenHtml(DataTable dtb)
    {
        DataTable dtbFail = null;
        DataTable dtbSuccess = null;
        var dataset = ValidateExcelData(dtb);

        if (dataset.Tables.Count > 1)
        {
            dtbFail = dataset.Tables[1];
            dtbSuccess = dataset.Tables[0];
        }

        var strFail = string.Empty;
        var strSuccess = string.Empty;

        if (dtbFail != null && dtbFail.Rows.Count > 0)
        {
            strFail = AlertMessage.GetMsg("Failed transactions, please check!", AlertMessage.MsgType.Error) + GenHTMLByTable(dtbFail, "add-customer-bggray", true) + "<br />";
        }

        if (dtbSuccess != null && dtbSuccess.Rows.Count > 0)
        {
            string msgMultiply = GetMultipliedCurrency(dtbSuccess);
            string strMsg = "<b>Please be aware that there is at least one transaction with exchange rate having 'Multiplied by' number: <br/></b>";

            strSuccess = AlertMessage.GetMsg("Valid transactions", AlertMessage.MsgType.Success) +
                (msgMultiply.Length > 0 ? AlertMessage.GetMsg(strMsg + msgMultiply, AlertMessage.MsgType.Warning) : string.Empty) +
                GenHTMLByTable(dtbSuccess, string.Empty, false);
        }

        return strFail + strSuccess;
    }

    private DataSet ValidateExcelData(DataTable data)
    {
        IUserService userService = CommonBootStrapper.Locator.GetInstance<IUserService>();

        // Delete old temporary data.
        var userId = AppSession.AccId;
        var isSuccess = userService.DelDataTmpExcel(userId);

        if (isSuccess)
        {
            var inputParams = ConvertToXmlParams(data);
            var insertError = new Hashtable();

            return userService.InsDataTmpExcel(userId, inputParams, ref insertError);
        }

        return new DataSet();
    }

    private string ConvertToXmlParams(DataTable data)
    {
        var xmlBuilder = new StringBuilder();
        xmlBuilder.Append("<ListTransfer>");

        foreach (DataRow drow in data.Rows)
        {
            xmlBuilder
                .Append("<Transfer Seq=\"")
                .Append(drow[0])
                .Append("\" FromCust=\"")
                .Append(HttpUtility.HtmlEncode(drow[1].ToString().Trim()))
                .Append("\" ToCust=\"")
                .Append(HttpUtility.HtmlEncode(drow[2].ToString().Trim()))
                .Append("\" Amount=\"")
                .Append(CastAide.AsDouble(drow[3]))
                .Append("\" ExRate=\"")
                .Append(CastAide.AsDouble(drow[4]))
                .Append("\" Description=\"")
                .Append(HttpUtility.HtmlEncode(drow[5].ToString().Trim()))
                .Append("\"></Transfer>");
        }

        xmlBuilder.Append("</ListTransfer>");

        return xmlBuilder.ToString();
    }

    string GetMultipliedCurrency(DataTable dt)
    {
        var result = new List<string>();
        var currencies = dt.AsEnumerable()
                .Where(s => CastAide.AsDecimal(s["MultipliedBy"]) > 1)
                .Select(s => s["currencyname"])
                .Distinct();

        foreach (var currency in currencies)
        {
            var row = dt.AsEnumerable()
                    .Where(s => CastAide.AsString(s["currencyname"]) == CastAide.AsString(currency))
                    .FirstOrDefault();

            var currencyName = row["CurrencyName"];
            var multipliedBy = row["MultipliedBy"];

            result.Add("<span class=\"currencyList\"></span><b>- Currency " + currencyName + " - Multiplied by " + multipliedBy + "</b>");
        }

        return string.Join("<br>", result);
    }

    string GenHTMLByTable(DataTable dtb, string rowStyle, bool hasReason)
    {
        var strResult = WebUtils.LoadTemplate("Transactions/AutomateTransfer.htm");

        if (!hasReason)
        {
            TemplateHandler.Assign(ref strResult, "HeaderReason", "Remark");
        }
        else
        {
            TemplateHandler.Assign(ref strResult, "HeaderReason", "Reason");
        }

        var strContent = TemplateHandler.CopyBlock(strResult, "content");
        var strBuild = new StringBuilder();

        // Generate data
        if (dtb.Rows.Count > 0)
        {
            foreach (DataRow row in dtb.Rows)
            {
                string strTmp = strContent;
                TemplateHandler.Assign(ref strTmp, "no", row["Seq"]);
                TemplateHandler.Assign(ref strTmp, "fromcust", HttpUtility.HtmlDecode(row["FromCust"].ToString()));
                TemplateHandler.Assign(ref strTmp, "tocust", HttpUtility.HtmlDecode(row["tocust"].ToString()));
                double Num;
                TemplateHandler.Assign(ref strTmp, "amount", double.TryParse(row["amount"].ToString(), out Num) ? NumberFormat.DecFormat(row["amount"], 2) : row["amount"].ToString());
                TemplateHandler.Assign(ref strTmp, "actualrate", SplitString(row["ExRate"].ToString()));
                StringWriter des = new StringWriter();
                HttpUtility.HtmlDecode(row["Description"].ToString(), des);
                TemplateHandler.Assign(ref strTmp, "description", des.ToString());

                if (hasReason)
                {
                    TemplateHandler.Assign(ref strTmp, "reason", CastAide.AsString(row["reason"], string.Empty));
                    TemplateHandler.Assign(ref strTmp, "rowstyle", rowStyle);
                }
                else
                {
                    rowStyle = string.Empty;
                    var remark = string.Empty;

                    int fromCurrency = CastAide.AsInt32(row["FromCurrency"]);
                    int toCurrency = CastAide.AsInt32(row["ToCurrency"]);
                    decimal exrate = CastAide.AsDecimal(row["exrate"]);
                    string strCurrentExRate = SplitString(CastAide.AsString(row["CurrentExRate"]));
                    string strExRate = SplitString(CastAide.AsString(row["ExRate"]));

                    if (fromCurrency == toCurrency && CastAide.AsDecimal(strExRate) != 1)
                    {
                        remark = "<b>Same currency, but actual rate is different 1.</b>";
                        rowStyle = "add-customer-bggray";
                    }
                    else if (fromCurrency == 2 || toCurrency == 2)
                    {
                        if (!string.IsNullOrEmpty(row["CurrentExRate"].ToString()) && strCurrentExRate != strExRate)
                        {
                            decimal minActualRate = (-1) * CastAide.AsDecimal(row["MinActualRate"]);
                            decimal maxActualRate = CastAide.AsDecimal(row["MaxActualRate"]);
                            decimal min = CastAide.AsDecimal(strCurrentExRate) * (1 + minActualRate);
                            decimal max = CastAide.AsDecimal(strCurrentExRate) * (1 + maxActualRate);

                            if (Convert.IsDBNull(row["MinActualRate"]) || !Convert.IsDBNull(row["MinActualRate"]) && exrate >= min && exrate <= max)
                            {
                                remark = "<b>Different system actual rate (" + strCurrentExRate + ")</b>";
                            }
                            else
                            {
                                remark = "<b>Exceed limit range (" + SplitString(min.ToString()) + " - " + SplitString(max.ToString()) + ")</b>";
                            }

                            rowStyle = "add-customer-bggray";
                        }
                    }

                    TemplateHandler.Assign(ref strTmp, "rowstyle", rowStyle);
                    TemplateHandler.Assign(ref strTmp, "reason", remark);
                }

                strBuild.Append(strTmp);
            }
        }

        TemplateHandler.ReplaceBlock(ref strResult, "content", strBuild.ToString());
        return strResult;
    }

    private string SplitString(string value)
    {
        var result = new StringBuilder();
        var arr = value.Split(new char[] { '.' });
        result.Append(arr[0]);

        if (arr.Length > 1)
        {
            result.Append(".");
            var decimalvalue = arr[1];

            for (int i = 0; i < decimalvalue.Length && i <= 3; i++)
            {
                result.Append(decimalvalue[i]);
            }
        }

        return result.ToString();
    }
}
