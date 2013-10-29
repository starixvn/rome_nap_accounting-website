using System.Collections.Generic;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.BetAggregator;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities.BetAggregator;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Accounting.WcfServiceLibrary.BetAggregator
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReportLicenseeWL : IReportLicenseeWL
    {
        public List<Item> SelLicensees(bool IsAll)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return licenseeList.SelLicensees(IsAll);
        }

        public Report_LicenseeWLBA_DTO SelCurrency(string siteId)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return licenseeList.SelCurrency(siteId);
        }

        public Report_LicenseeWLBA_DTO GetDatetimeOption(string dateOption)
        {
            var deposit = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return deposit.GetDatetimeOption(dateOption);
        }

        public Report_LicenseeWLBA_DTO SelReportByLicensee(int LicenseeID, int Currency, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return licenseeList.SelReportByLicensee(LicenseeID, Currency, baseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG,
                                                        ReportType, drawfilter, excludeTest, 1, pageIndex, pageSize, fromCache);
        }

        public Report_LicenseeWLBA_DTO SelReportByLicenseeDetail_Ag(int LicenseeID, int Currency, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return licenseeList.SelReportByLicenseeDetail_Ag(LicenseeID, Currency, baseCurrency, FromDate, ToDate, showSB, ShowRB, ShowNG,
                                                        ReportType, drawfilter, excludeTest, 1, pageIndex, pageSize, fromCache);
        }

        public Report_LicenseeWLBA_DTO SelReportByLicenseeDetail(int LicenseeID, int Currency, int baseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return licenseeList.SelReportByLicenseeDetail(LicenseeID, Currency, baseCurrency, CustId, FromDate, ToDate, showSB, ShowRB, ShowNG,
                                                        ReportType, drawfilter, excludeTest, 1, pageIndex, pageSize, fromCache);
        }

        public List<BetListDetail> BetlistDepositReport(int custId, string fdate, string tdate, bool showsb, bool showng, int showba, bool showrb, bool showlc, int drawFilter)
        {
            var betlist = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return betlist.SelBetlistDepositReport(custId, fdate, tdate, showsb, showng, showrb, showba, showlc, drawFilter);
        }

        public LicenseeWinLossCommDTO LicenseeWinLossByCommLevel(int licenseeId, int currencyId, bool isExcludeTest, string fromDate, string toDate, int drawFilter)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return licenseeList.LicenseeWinLossByCommLevel(licenseeId, currencyId, isExcludeTest, fromDate, toDate, drawFilter);
        }

        public LicenseesWLBA SelReportByLicensees(int licenseeId, int currency, int baseCurrency, string fromDate, string toDate, int reportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return licenseeList.SelReportByLicensees(licenseeId, currency, baseCurrency, fromDate, toDate, reportType,
                                                     drawfilter, excludeTest,
                                                     pageIndex, pageSize, fromCache);
        }

        public Report_LicenseeWLBA_DTO SelWinLossReportByLicenseeDetailAgent(int licenseeId, int currencyId, int baseCurrencyId, string fromDate, string toDate, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return licenseeList.SelWinLossReportByLicenseeDetailAgent(licenseeId, currencyId, baseCurrencyId, fromDate,
                                                                      toDate, drawfilter, excludeTest, pageIndex,
                                                                      pageSize, fromCache);
        }

        public Report_LicenseeWLBA_DTO SelWinLossReportByLicenseeDetail(int licenseeId, int currencyId, int baseCurrencyId, int custId, string fromDate, string toDate, int drawfilter, int excludeTest, int reportType, int pageIndex, int pageSize, int fromCache)
        {
            var licenseeList = CommonBootStrapper.Locator.GetInstance<IReportWLBABySiteService>();
            return licenseeList.SelWinLossReportByLicenseeDetail(licenseeId, currencyId, baseCurrencyId, custId,
                                                                 fromDate, toDate, drawfilter, excludeTest, reportType,
                                                                 pageIndex, pageSize, fromCache);
        }
    }
}