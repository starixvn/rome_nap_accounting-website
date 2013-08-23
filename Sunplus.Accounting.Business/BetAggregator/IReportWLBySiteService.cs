using System.Collections.Generic;
using System.ServiceModel;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Entities.BetAggregator;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Accounting.Business.BetAggregator
{
    [ServiceContract]
    public interface IReportWLBABySiteService
    {
        List<Item> SelLicensees(bool IsAll);

        Report_LicenseeWLBA_DTO SelCurrency(string siteId);

        Report_LicenseeWLBA_DTO GetDatetimeOption(string dateOption);

        Report_LicenseeWLBA_DTO SelReportByLicensee(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int fromCache);

        LicenseesWLBA SelReportByLicensees(int licenseeId, int currency, int baseCurrency, string fromDate,
                                           string toDate, int reportType, int drawfilter, int excludeTest, int pageIndex,
                                           int pageSize, int fromCache);

        Report_LicenseeWLBA_DTO SelReportByLicenseeDetail_Ag(int LicenseeID, int Currency, int BaseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int fromCache);

        Report_LicenseeWLBA_DTO SelWinLossReportByLicenseeDetailAgent(int licenseeId, int currency, int baseCurrency, string fromDate, string toDate, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        Report_LicenseeWLBA_DTO SelReportByLicenseeDetail(int LicenseeID, int Currency, int BaseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int BAFilter, int pageIndex, int pageSize, int fromCache);

        Report_LicenseeWLBA_DTO SelWinLossReportByLicenseeDetail(int licenseeId, int currency, int baseCurrency, int custId, string fromDate, string toDate, int drawfilter, int excludeTest, int reportType, int pageIndex, int pageSize, int fromCache);

        ExcelFile ExportLicenseeReport(string site, string fdate, string tdate, ref bool hasData, string reportType, string level, string currency, string baseCurrency);

        void Export2ExcelReportByLicensee(string reportTitle, int licenseeId, int currency, int BaseCurrency, string fromDate,
                                          string toDate, int showSb, int showRb, int showNg, int reportType, int drawFilter, int excludeTest,
                                          int pageIndex, int pageSize, int reportSelected, int custId);

        void ExportExcelBAWinLossReportByLicensee(int licenseeId, int currency, int baseCurrency, string fromDate,
                                                  string toDate, int reportType, int drawfilter, int excludeTest,
                                                  string title, int reportSelected);

        void ExportExcelBAWinLoss12BetDetail(int licenseeId, int currency, int baseCurrency, string fromDate,
                                             string toDate, int drawfilter, int excludeTest, string title,
                                             int reportSeleted);

        void ExportExcelBAWinLossDetail(int licenseeId, int currency, int baseCurrency, string fromDate, int custId,
                                             string toDate, int drawfilter, int excludeTest, string title,
                                             int reportSeleted, int reportType);

        string RenderHTML(System.Collections.Hashtable input);

        string RenderHTMLV2(System.Collections.Hashtable input);

        List<BetListDetail> SelBetlistDepositReport(int custId, string fdate, string tdate, bool showsb, bool showng, bool showrb, int showba, bool showlc, int drawFilter);

        List<Sunplus.Accounting.Entities.BetAggregator.BALicWinlossDaily> GetBALicWinlossDaily(System.DateTime wldate);

        List<Sunplus.Accounting.Entities.BetAggregator.BALicWinlossDaily> GetBALicWinlossMonthToDate(System.DateTime wldate);

        List<Sunplus.Accounting.Entities.BetAggregator.BALicWinlossDaily> GetBALicWinlossYearToDate(System.DateTime wldate);

        void Export2ExcelReportByCommissionLevel(string reportTitle, int licenseeId, int currency, string fromDate, string toDate, int drawFilter, bool excludeTest);

        LicenseeWinLossCommDTO LicenseeWinLossByCommLevel(int licenseeId, int currencyId, bool isExcludeTest,
                                                          string fromDate, string toDate, int drawFilter);

    }
}