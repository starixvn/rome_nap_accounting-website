<%@ WebHandler Language="C#" Class="UploadCrossCurrency" %>

using System;
using System.Web;
using System.Data;
using System.IO;
using DNAide.Web;
using System.Collections.Generic;

using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business;
using System.Text;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Accounting.Business.Customer;
using Fanex.Tarus.Excels.CrossCurrencyTransfer;

public class UploadCrossCurrency : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    private List<StringBuilder> arrFromCust = new List<StringBuilder>();
    private List<StringBuilder> arrToCust = new List<StringBuilder>();
    private List<StringBuilder> arrFromAmt = new List<StringBuilder>();
    private List<StringBuilder> arrToAmt = new List<StringBuilder>();
    private List<StringBuilder> arrDescription = new List<StringBuilder>();

    private int maxLength = 3000;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        if (!Permissions.GetHostPermission<Enumerations.Functions.FinancialManagement>(Enumerations.Functions.FinancialManagement.AutomateFinanceTransfer))
        {
            context.Response.Write(BuildResult(Resources.ErrorMsg._500, string.Empty));
            return;
        }

        try
        {
            if (context.Request.Files[0].ContentLength > 1024000)
            {
                context.Response.Write(BuildResult("Your uploaded file's size can't be larger than 1 MB", string.Empty));
                return;
            }

            string ext = string.Empty;
            if (!AccCommon.IsValidExtension(context.Request.Files[0].FileName, ref ext))
            {
                context.Response.Write(BuildResult("Sorry, You choose invalid file extension. Please choose an Excel file (.xls)", string.Empty));
                return;
            }

            var strLocation = context.Server.MapPath("../Transactions") + "\\" + AppSession.Username + ".xls";
            FileInfo file = new FileInfo(strLocation);
            if (file.Exists) file.Delete();

            context.Request.Files[0].SaveAs(strLocation);

            ICrossCurrencyTransferExcel crossCurrencyTransferExcel = new CrossCurrencyTransferExcel();
            DataTable dataTable = crossCurrencyTransferExcel.ExcelToDataTable(strLocation);

            if (dataTable == null)
            {
                context.Response.Write(BuildResult("You're using wrong template file. Please double check and try again. Thanks", string.Empty));
                return;
            };

            file = new FileInfo(strLocation);
            if (file.Exists) file.Delete();

            DeleteEmptyRow(dataTable);
            if (dataTable.Rows.Count == 0)
            {
                context.Response.Write(BuildResult("Data is emtpy", string.Empty));
                return;
            }

            string hdFileInfo = @"<input type='hidden' id='hdFCTFileInfo' filename='" + context.Request.Files[0].FileName
                + "' filesize='" + context.Request.Files[0].ContentLength.ToString() + "' />";
            context.Response.Write(BuildResult(string.Empty, RenderContent(dataTable) + hdFileInfo
                + "|" + GetAllForexAccount()));
        }
        catch (Exception exception)
        {
            context.Response.Write(BuildResult(exception.Message, string.Empty));
        }
    }

    public DataTable GetDataFromExcel_AutoCrossCurTransfer(string ExcelPath)
    {
        try
        {
            DataTable dtb = new DataTable();
            DataColumn col = new DataColumn();
            col.ColumnName = "From Cust";
            col.DataType = typeof(string);
            dtb.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "To Cust";
            col.DataType = typeof(string);
            dtb.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "From Amount";
            col.DataType = typeof(double);
            dtb.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "To Amount";
            col.DataType = typeof(double);
            dtb.Columns.Add(col);

            col = new DataColumn();
            col.ColumnName = "Remark";
            col.DataType = typeof(string);
            dtb.Columns.Add(col);

            ExcelUtil.Spreadsheet.ExcelFile excelFile = new ExcelUtil.Spreadsheet.ExcelFile();
            excelFile.LoadXls(ExcelPath);
            var workSheet = excelFile.Worksheets[0];

            if (workSheet.Rows.Count <= 0) return dtb;

            for (int i = 1; i < workSheet.Rows.Count; ++i)
            {
                var row = dtb.NewRow();
                row[0] = DNAide.Core.CastAide.AsString(workSheet.Rows[i].Cells[0].Value);
                row[1] = DNAide.Core.CastAide.AsString(workSheet.Rows[i].Cells[1].Value);
                row[2] = DNAide.Core.CastAide.AsDouble(workSheet.Rows[i].Cells[2].Value);
                row[3] = DNAide.Core.CastAide.AsDouble(workSheet.Rows[i].Cells[3].Value);
                row[4] = DNAide.Core.CastAide.AsString(workSheet.Rows[i].Cells[4].Value);
                dtb.Rows.Add(row);
            }
            return dtb;
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            throw new Exception("Please choose a Microsoft Excel file (.xls)");
        }
        return null;
    }

    private void DeleteEmptyRow(DataTable dataTable)
    {
        List<int> listEmptyRowsIndex = new List<int>();
        for (int i = 0; i < dataTable.Rows.Count; ++i)
        {
            int count = 0;
            for (int columnIndex = 0; columnIndex < 2; ++columnIndex)
            {
                if (dataTable.Rows[i].IsNull(columnIndex) || string.IsNullOrEmpty(dataTable.Rows[i][columnIndex].ToString().Trim()))
                {
                    ++count;
                }
            }
            if (count == 2) listEmptyRowsIndex.Add(i);
        }
        foreach (var index in listEmptyRowsIndex)
        {
            if (index >= dataTable.Rows.Count) continue;
            if (
                (dataTable.Rows[index].IsNull(0) && dataTable.Rows[index].IsNull(1))
                ||
                (string.IsNullOrEmpty(dataTable.Rows[index][0].ToString().Trim()) && string.IsNullOrEmpty(dataTable.Rows[index][1].ToString().Trim()))
                )
            {
                dataTable.Rows.RemoveAt(index);
            }
        }
        if (listEmptyRowsIndex.Count <= 0) return;
        DeleteEmptyRow(dataTable);
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

    private string RenderContent(DataTable dataTable)
    {
        string strTemplateErrorPart = string.Empty;
        string strTemplateSuccessPart = string.Empty;

        //Delete old temporary data
        bool isSuccess = false;
        IUserService userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
        isSuccess = userService.DelDataTmpExcel(AppSession.AccId);

        //------------------------------------
        List<int> listValidRow = new List<int>();
        RenderErrorPart(dataTable, ref strTemplateErrorPart, ref listValidRow);
        RenderSuccessPart(dataTable, ref strTemplateSuccessPart, ref listValidRow);

        for (int i = 0; i < arrFromCust.Count; ++i)
        {
            var rs = userService.InsDataTmpExcelFinanceCrossTransfer(AppSession.AccId, arrFromCust[i].ToString().Trim()
            , arrToCust[i].ToString().Trim(), arrFromAmt[i].ToString(), arrToAmt[i].ToString(), arrDescription[i].ToString());

            if (rs.ErrorCode > 0)
                isSuccess = false;
        }

        string hiddens = string.Empty;
        if (!isSuccess)
        {
            hiddens = "<input type='hidden' id='upHidden' haveerror='true' havesuccess='false' />";
        }
        else
        {
            hiddens = "<input type='hidden' id='upHidden' haveerror='" +
               (listValidRow.Count == dataTable.Rows.Count ? "false" : "true")
               + "' havesuccess='" +
               (listValidRow.Count <= 0 ? "false" : "true")
               + "' />";
        }
        return ReplaceToken(strTemplateErrorPart) + ReplaceToken(strTemplateSuccessPart) + hiddens;
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

    private void RenderErrorPart(DataTable dataTable, ref string strTemplate, ref List<int> listValidRow)
    {
        strTemplate += "<div id='divErrorMsgPart' class='alertboxerr'><div>Failed transactions, please check!</div></div>";
        strTemplate += WebUtils.LoadTemplate("Transactions/CrossCurrencyTransfer.htm");
        TemplateHandler.RemoveBlock(ref strTemplate, "successpart");

        string strContent = TemplateHandler.CopyBlock(strTemplate, "content");

        string strConentTemp = string.Empty;

        string fromcust = string.Empty;
        string tocust = string.Empty;
        double fromamount;
        double toamount;
        string remark = string.Empty;

        StringBuilder stb = new StringBuilder();

        List<CustomerExRate> listCust = new List<CustomerExRate>();

        int rowIndex = -1;
        foreach (DataRow row in dataTable.Rows)
        {
            ++rowIndex;
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

            CustomerExRate fromCustomerExRate = null;
            CustomerExRate toCustomerExRate = null;

            if (!UserNameIsValid(fromcust, ref fromCustomerExRate, ref listCust) || !UserNameIsValid(tocust, ref toCustomerExRate, ref listCust))
            {
                TemplateHandler.Assign(ref strConentTemp, "reason", "From Cust or To Cust does not exist");
            }
            else if (fromamount.Equals(0) || toamount.Equals(0) || (fromamount * toamount) < 0)
            {
                TemplateHandler.Assign(ref strConentTemp, "reason", "Invalid From Amount or To Amount");
            }
            else if (fromCustomerExRate != null && toCustomerExRate != null && fromCustomerExRate.currency == toCustomerExRate.currency)
            {
                TemplateHandler.Assign(ref strConentTemp, "reason", "From Cust and To Cust must belong to differrent currencies");
            }
            else if (fromCustomerExRate != null && toCustomerExRate != null
                && (fromCustomerExRate.actualrate == 0 || toCustomerExRate.actualrate == 0))
            {
                TemplateHandler.Assign(ref strConentTemp, "reason", "Invalid Exchange Rate");
            }
            else if (remark.Length > 200)
            {
                TemplateHandler.Assign(ref strConentTemp, "reason", "Max length of Remark must be less than 200 characters");
            }
            else
            {
                if (!isReachMax())
                {
                    if (arrFromCust.Count <= 0)
                    {
                        arrFromCust.Add(new StringBuilder());
                        arrToCust.Add(new StringBuilder());
                        arrFromAmt.Add(new StringBuilder());
                        arrToAmt.Add(new StringBuilder());
                        arrDescription.Add(new StringBuilder());

                        arrFromCust[0].Append(fromcust + "");
                        arrToCust[0].Append(tocust + "");

                        arrFromAmt[0].Append(fromamount + "");
                        arrToAmt[0].Append(toamount + "");

                        arrDescription[0].Append(remark + "");
                    }
                    else
                    {
                        int currentIndex = arrFromCust.Count - 1;
                        arrFromCust[currentIndex].Append(fromcust + "");
                        arrToCust[currentIndex].Append(tocust + "");

                        arrFromAmt[currentIndex].Append(fromamount + "");
                        arrToAmt[currentIndex].Append(toamount + "");

                        arrDescription[currentIndex].Append(remark + "");
                    }
                }
                else
                {
                    arrFromCust.Add(new StringBuilder());
                    arrToCust.Add(new StringBuilder());
                    arrFromAmt.Add(new StringBuilder());
                    arrToAmt.Add(new StringBuilder());
                    arrDescription.Add(new StringBuilder());

                    int currentIndex = arrFromCust.Count - 1;
                    arrFromCust[currentIndex].Append(fromcust + "");
                    arrToCust[currentIndex].Append(tocust + "");

                    arrFromAmt[currentIndex].Append(fromamount + "");
                    arrToAmt[currentIndex].Append(toamount + "");

                    arrDescription[currentIndex].Append(remark + "");
                }
                listValidRow.Add(rowIndex);
                continue;
            }

            TemplateHandler.Assign(ref strConentTemp, "rowstyle", "class='add-customer-bggray'");
            stb.Append(strConentTemp);
        }

        TemplateHandler.ReplaceBlock(ref strTemplate, "content", stb.ToString());
    }

    private bool isReachMax()
    {
        if (arrFromCust.Count <= 0 && arrToCust.Count <= 0 && arrFromAmt.Count <= 0 && arrToAmt.Count <= 0 && arrDescription.Count <= 0)
            return false;
        int currentIndex = arrFromCust.Count - 1;
        int currentMaxLength = arrFromCust[currentIndex].Length;
        if (currentMaxLength < arrToCust[currentIndex].Length) currentMaxLength = arrToCust[currentIndex].Length;
        if (currentMaxLength < arrFromAmt[currentIndex].Length) currentMaxLength = arrFromAmt[currentIndex].Length;
        if (currentMaxLength < arrToAmt[currentIndex].Length) currentMaxLength = arrToAmt[currentIndex].Length;
        if (currentMaxLength < arrDescription[currentIndex].Length) currentMaxLength = arrDescription[currentIndex].Length;
        return currentMaxLength > maxLength;
    }

    private void RenderSuccessPart(DataTable dataTable, ref string strTemplate, ref List<int> listValidRow)
    {
        strTemplate += (listValidRow.Count == dataTable.Rows.Count ? string.Empty : "<br/>")
            + "<div id='divSuccessMsgPart' class='alertboxscc'><div>Valid transactions</div></div>";
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

        int rowIndex = -1;
        foreach (DataRow row in dataTable.Rows)
        {
            ++rowIndex;
            if (!listValidRow.Contains(rowIndex)) continue;

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

    private bool UserNameIsValid(string username, ref CustomerExRate cust, ref List<CustomerExRate> listCust)
    {
        if (ExistedCustInList(username, ref listCust, ref cust))
        {
            return true;
        }
        var customerManager = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Customer.ICustomerManager>();
        var dbResult = customerManager.SelectCustomerExRate(username.Trim());
        cust = dbResult;
        if (cust != null)
        {
            listCust.Add(cust);
        }
        return dbResult == null ? false : true;
    }

    private bool ExistedCustInList(string username, ref List<CustomerExRate> listCust, ref CustomerExRate cust)
    {
        foreach (CustomerExRate _cust in listCust)
        {
            if (_cust.username.ToLower() == username.ToLower())
            {
                cust = _cust;
                return true;
            }
        }
        return false;
    }

    private string GetAllForexAccount()
    {
        var customerManager = CommonBootStrapper.Locator.GetInstance<ICustomerManager>();
        var listForexAcc = customerManager.SelectAllForexAccount();
        if (listForexAcc == null) return string.Empty;
        List<string> lstForexAccName = new List<string>();
        foreach (Sunplus.Accounting.Entities.Customer.ForexAccount forexAcc in listForexAcc)
        {
            lstForexAccName.Add(forexAcc.username);
        }
        return string.Join(",", lstForexAccName.ToArray());
    }
}

 