using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExcelUtil.Spreadsheet;
using BLL.Report;
using BLL;
using Sunplus.Accounting.Business.ExportStatement;

public partial class Report_ExportExcelFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ExcelFile excelFile = new ExcelFile();

        string type = Request["type"];
        IServiceOpenToteSettlement serviceOpenToteSettlement;
        IServiceExportStatement serviceExportStatement;
        IServiceHRGroupDaily serviceHRGroupDaily;
        string filename = "";
        bool IsTest = false;
        switch (type)
        {
            case "ExportToExcel_OpenTote":
                serviceOpenToteSettlement = new ServiceOpenToteSettlement();
                IsTest = Request["IsTest"] == "1";
                excelFile = serviceOpenToteSettlement.Export2ExcelWinlostHR_OpenTote(Request["winlostdate"], Request["db"], IsTest);
                filename = " Horse-Racing-OpenTote-Daily-Report-" + Request["winlostdate"];
                break;
            case "ExportToExcel_OpenToteDetail":
                serviceOpenToteSettlement = new ServiceOpenToteSettlement();
                IsTest = Request["IsTest"] == "1";
                excelFile = serviceOpenToteSettlement.ExportToExcel_OpenToteDetail(Request["winlostdate"], Request["partnertype"], Request["poolcurrency"], Request["db"], IsTest);
                filename = " Horse-Racing-OpenTote-Detail-Daily-Report-" + Request["winlostdate"];
                break;
            case "ExportStatement":
                serviceExportStatement = new ServiceExportStatement();
                var userList = Request["userslistvalid"];
                userList = userList.Replace("&amp;", "&");
                excelFile = serviceExportStatement.Export2Excel(userList.Trim(), Request["fdate"], Request["tdate"]);
                filename = "Export-Multi-Statement-" + Request["fdate"] + "-to-" + Request["tdate"];
                break;
            case "ExportHRGroupDaily":
                serviceHRGroupDaily = new ServiceHRGroupDaily();
                excelFile = serviceHRGroupDaily.ExportExcelHRGroupDaily(Request["fdate"], Request["tdate"]);
                filename = "Horse-Racing-Group-Daily-" + Request["fdate"] + "-to-" + Request["tdate"];
                break;
            case "ExportHRGroupDaily_ToteOnly":
                serviceHRGroupDaily = new ServiceHRGroupDaily();
                excelFile = serviceHRGroupDaily.ExportExcelHRGroupDaily_ToteOnly(Request["fdate"], Request["tdate"]);
                filename = "Horse-Racing-Group-Daily-ToteOnly-" + Request["fdate"] + "-to-" + Request["tdate"];
                break;
            case "NonePTMemSummary":
                IServiceNonePTMemSummary nonePTMemSummary = new ServiceNonePTMemSummary();
                excelFile = nonePTMemSummary.ExportToExcel_NonePTMemSummary(Request["username"], Request["pageindex"]);
                filename = "NonePT-Members-Summary-" + DateTime.Today.ToShortDateString();
                break;
        }

        Response.ContentType = "Application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".xls");
        excelFile.SaveXls(Response.OutputStream);


    }
}
