<%@ WebHandler Language="C#" Class="UploadFinanceTrans" %>

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Web;
using System.IO;
using System.Data;
using System.Web.SessionState;
using DNAide.Web;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using DNAide.Data;
using DNAide.Core;
using DNAide.Utils;
using System.Text;
using Sunplus.Accounting.Core.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Linq;

public class UploadFinanceTrans : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";

        if (!Permissions.GetHostPermission<Enumerations.Functions.FinancialManagement>(Enumerations.Functions.FinancialManagement.AutomateFinanceTransfer))
        {
            context.Response.Write(BuildResult(Resources.ErrorMsg._500, string.Empty));
            return;
        }

        string htmlData = "", strLocation = "";
        string FileSize = "<input type='hidden' id='FileSize' value='" + context.Request.ContentLength + "'/>";
        try
        {
            if (context.Request.Files[0].ContentLength < 1024000)//Maximum length file is 1MB
            {
                string strFileName = AppSession.Username;
                string newfilename = context.Request.Files[0].FileName;
                string ext = string.Empty;
                if (!string.IsNullOrEmpty(newfilename))
                {
                    if (AccCommon.IsValidExtension(newfilename, ref ext))
                    {
                        //strLocation = context.Server.MapPath("../Transactions") + "\\" + strFileName + ".xls";

                        //FileInfo file = new FileInfo(strLocation);
                        //if (file.Exists) file.Delete();

                        DataTable dtb = GetDataFromExcelNPoi(context.Request.Files[0].InputStream);
                        if (dtb.Columns[4].ColumnName.ToLower() != "actualrate")
                        {
                            context.Response.Write(BuildResult("You're using wrong template file. Please double check and try again. Thanks", string.Empty));
                            return;
                        }

                        if (dtb.Rows.Count <= 0)
                        {
                            context.Response.Write(BuildResult("Data is empty", string.Empty));
                            return;
                        }
                        htmlData = GenHtml(dtb);
                        context.Response.Write(BuildResult(string.Empty, htmlData + FileSize));
                    }
                    else
                    {
                        context.Response.Write(BuildResult("Sorry, You choose invalid file extension. Please choose an Excel file (.xls)", string.Empty));
                        return;
                    }
                }
            }
            else
            {
                context.Response.Write(BuildResult("Your uploaded file's size can't be larger than 1 MB", string.Empty));
                return;
            }
        }
        catch (Exception ex)
        {
            if (strLocation != "")
            {
                FileInfo file = new FileInfo(strLocation);
                if (file.Exists) file.Delete();
            }
            context.Response.Write(BuildResult(ex.Message, string.Empty));
        }
    }


    public DataTable GetDataFromExcelNPoi(Stream stream)
    {
        var dt = new DataTable();
        string message = string.Empty;
        try
        {
            HSSFWorkbook hssfworkbook;
           
            hssfworkbook = new HSSFWorkbook(stream);
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            dt.Columns.Add("No");
            dt.Columns.Add("From Cust");
            dt.Columns.Add("To Cust");
            dt.Columns.Add("Amount");
            dt.Columns.Add("ActualRate");
            dt.Columns.Add("Description");
            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();
                //// Current Row Index

                int count = 0;
                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);

                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.NUMERIC:
                                dr[i + 1] = cell.NumericCellValue;
                                break;
                            default:
                                try
                                {
                                    if (string.IsNullOrEmpty(cell.StringCellValue))
                                    {
                                        count++;
                                        break;
                                    }

                                    dr[i + 1] = cell.StringCellValue;
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    LogExtensions.ExceptionHandler(ex);
                                    throw new Exception("Input invalid at row " + (row.RowNum + 1) + " cell " + (i + 1) + "!");
                                }
                                break;
                                
                        }
                        
                    }
                    else
                    {
                        count++;
                    }

                }
                if (count != row.LastCellNum && row.RowNum > 0)
                {
                    dr[0] = row.RowNum + 1;
                    dt.Rows.Add(dr);
                }

            }

        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            throw new Exception(ex.Message);
        }
        return dt;
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
        string strResult = WebUtils.LoadTemplate("Transactions/AutomateTransfer.htm");
        string strContent = TemplateHandler.CopyBlock(strResult, "content");

        //Delete old temporary data
        bool isSuccess = false;
        IUserService userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
        isSuccess = userService.DelDataTmpExcel(AppSession.AccId);

        //Get Date
        DataSet rs = null;
        DataTable dtbFail = new DataTable();
        dtbFail.Columns.Add("FromCust");
        dtbFail.Columns.Add("ToCust");
        dtbFail.Columns.Add("Amount");
        dtbFail.Columns.Add("ExRate");
        dtbFail.Columns.Add("Description");

        DataTable dtbSuccess = new DataTable(); //rs.ResultSet.Tables[0];//Valid transaction  
        dtbSuccess.Columns.Add("FromCust");
        dtbSuccess.Columns.Add("ToCust");
        dtbSuccess.Columns.Add("Amount");
        dtbSuccess.Columns.Add("ExRate");
        dtbSuccess.Columns.Add("Description");

        StringBuilder strBuild = new StringBuilder();
        //Generate data
        StringBuilder arrFromCust = new StringBuilder();
        StringBuilder arrToCust = new StringBuilder();
        StringBuilder arrAmount = new StringBuilder();
        StringBuilder arrExRate = new StringBuilder();
        StringBuilder arrDescription = new StringBuilder();

        if (isSuccess)
        {
            string xmlstr = string.Empty;
            xmlstr = "<ListTransfer>";
            foreach (DataRow row in dtb.Rows)
            {
                var no = CastAide.AsInt32(row[0]);
                var fromCust =HttpUtility.HtmlEncode(row[1].ToString().Trim());
                var toCust = HttpUtility.HtmlEncode(row[2].ToString().Trim());
                var amount = CastAide.AsDouble(row[3]);
                var exrate = CastAide.AsDouble(row[4]);
                var des = HttpUtility.HtmlEncode(row[5].ToString().Trim());

                xmlstr += "<Transfer Seq=\"" + no + "\" FromCust=\"" + fromCust +
                        "\" ToCust=\"" + toCust + "\" Amount=\"" + amount +
                        "\" ExRate=\"" + exrate + "\" Description=\"" + des + "\"></Transfer>";
            }
            xmlstr += "</ListTransfer>";
           
            var insertError = new System.Collections.Hashtable();
            rs = userService.InsDataTmpExcel(AppSession.AccId, xmlstr, ref insertError);
            if (rs != null && rs.Tables.Count > 1)
            {
                dtbFail = rs.Tables[1];
                dtbSuccess = rs.Tables[0];
            }
        }
        string strFail = string.Empty;
        string strSuccess = string.Empty;
        if (dtbFail.Rows.Count > 0)
            strFail = AlertMessage.GetMsg("Failed transactions, please check!", AlertMessage.MsgType.Error) + GenHTMLByTable(dtbFail, "add-customer-bggray", true) + "<br />";
        if (dtbSuccess.Rows.Count > 0)
        {
            string msgMultiply = GetMultipliedCurrency(dtbSuccess);
            string strMsg = "<b>Please be aware that there is at least one transaction with exchange rate having 'Multiplied by' number: <br/></b>";
            strSuccess = AlertMessage.GetMsg("Valid transactions", AlertMessage.MsgType.Success) +
                (msgMultiply.Length > 0 ? AlertMessage.GetMsg(strMsg + msgMultiply, AlertMessage.MsgType.Warning) : string.Empty) +
                GenHTMLByTable(dtbSuccess, string.Empty, false);
        }

        return strFail + strSuccess;
    }
    
    string GetMultipliedCurrency(DataTable dt)
    {
        List<string> result = new List<string>();
        var Currencies = dt.AsEnumerable().Where(s => CastAide.AsDecimal(s["MultipliedBy"]) > 1).Select(s => s["currencyname"]).ToList().Distinct();
        foreach (var currency in Currencies)
        {
            var row = dt.AsEnumerable().Where(s => CastAide.AsString(s["currencyname"]) == CastAide.AsString(currency)).ToList().FirstOrDefault();            
            var currencyName = row["CurrencyName"];
            var multipliedBy = row["MultipliedBy"];
            result.Add("<span class=\"currencyList\"></span><b>- Currency " + currencyName + " - Multiplied by " + multipliedBy + "</b>");
        }
        return string.Join("<br>" , result);
    }

    string GenHTMLByTable(DataTable dtb, string rowStyle, bool hasReason)
    {
        string strResult = WebUtils.LoadTemplate("Transactions/AutomateTransfer.htm");

        if (!hasReason)
        {
            TemplateHandler.Assign(ref strResult, "HeaderReason", "Remark");
        }
        else
        {
            TemplateHandler.Assign(ref strResult, "HeaderReason", "Reason");
        }
        string strContent = TemplateHandler.CopyBlock(strResult, "content");
        StringBuilder strBuild = new StringBuilder();
        //Generate data
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
                        remark = "<b>Same currency, but actual rate different 1.</b>";
                        rowStyle = "add-customer-bggray";
                    }
                    else if(fromCurrency == 2 || toCurrency == 2)
                    {                        
                        if (!string.IsNullOrEmpty(row["CurrentExRate"].ToString()) && strCurrentExRate != strExRate )
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
                    
                    if (CastAide.AsDecimal(row["MultipliedBy"]) > 1)
                    {
                        rowStyle = "add-customer-bggray";            
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
