using System;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;

public partial class Report_Deposit_Deposit : PageBase
{
    public string strfdate = string.Empty, strtdate = string.Empty, strSite = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["IsExport"] != null && Request["IsExport"] == "1")
        {
            var deposit = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            var reportTitle = Request["reportTitle"];
            var licensseId = CastAide.AsInt32(Request["licenseeId"]);
            var currency = CastAide.AsInt32(Request["currency"]);
            var fromDate = Request["fromDate"];
            var toDate = Request["toDate"];
            var showSb = CastAide.AsInt32(Request["showSb"]);
            var showRb = CastAide.AsInt32(Request["showRb"]);
            var showNg = CastAide.AsInt32(Request["showNg"]);
            var showBa = CastAide.AsInt32(Request["showBa"]);
            var showLc = CastAide.AsInt32(Request["showLc"]);
            var reportType = CastAide.AsInt32(Request["reportType"]);
            var drawFilter = CastAide.AsInt32(Request["drawFilter"]);
            var excludeTest = CastAide.AsInt32(Request["excludeTest"]);

            var pageIndex = CastAide.AsInt32(Request["pageIndex"]);
            var pageSize = CastAide.AsInt32(Request["pageSize"]);
            var fromCache = CastAide.AsInt32(Request["fromCache"]);
            var reportSelected = CastAide.AsInt32(Request["reportSelected"]);
            var custId = CastAide.AsInt32(Request["custid"]);
            var baseCurrency = CastAide.AsInt32(Request["baseCurrency"]);
            var reportLevel = CastAide.AsInt32(Request["ReportLevel"]);

            switch (reportLevel)
            {
                case 0:
                    deposit.ExportCurrencyReport(reportTitle, licensseId,
                        currency, baseCurrency,
                        fromDate, toDate,
                        showSb, showRb,
                        showNg, showBa,
                        showLc, reportType,
                        drawFilter, excludeTest,
                        pageIndex, pageSize,
                        reportSelected, custId);
                    break;
                case 1:
                    deposit.ExportSuperReport(reportTitle, licensseId,
                        currency, baseCurrency,
                        fromDate, toDate,
                        showSb, showRb,
                        showNg, showBa,
                        showLc, reportType,
                        drawFilter, excludeTest,
                        pageIndex, pageSize,
                        reportSelected, custId);
                    break;
                case 2:
                    deposit.ExportMasterReport(reportTitle, licensseId,
                        currency, baseCurrency,
                        fromDate, toDate,
                        showSb, showRb,
                        showNg, showBa,
                        showLc, reportType,
                        drawFilter, excludeTest,
                        pageIndex, pageSize,
                        reportSelected, custId);
                    break;
                case 3:
                    deposit.ExportAgentReport(reportTitle, licensseId,
                        currency, baseCurrency,
                        fromDate, toDate,
                        showSb, showRb,
                        showNg, showBa,
                        showLc, reportType,
                        drawFilter, excludeTest,
                        pageIndex, pageSize,
                        reportSelected, custId);
                    break;
                case 4:
                    deposit.ExportMemberReport(reportTitle, licensseId,
                        currency, baseCurrency,
                        fromDate, toDate,
                        showSb, showRb,
                        showNg, showBa,
                        showLc, reportType,
                        drawFilter, excludeTest,
                        pageIndex, pageSize,
                        reportSelected, custId);
                    break;
            }

        }
        else
        {
            strfdate = DateTime.Today.ToShortDateString();
            strtdate = DateTime.Today.ToShortDateString();
        }

    }

}
