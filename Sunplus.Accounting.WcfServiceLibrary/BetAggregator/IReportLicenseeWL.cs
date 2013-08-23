using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.BetAggregator;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Accounting.WcfServiceLibrary.BetAggregator
{
    [ServiceContract]
    public interface IReportLicenseeWL
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<Item> SelLicensees(bool IsAll);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Report_LicenseeWLBA_DTO SelCurrency(string siteId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Report_LicenseeWLBA_DTO GetDatetimeOption(string dateOption);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Report_LicenseeWLBA_DTO SelReportByLicensee(int LicenseeID, int Currency, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Report_LicenseeWLBA_DTO SelReportByLicenseeDetail_Ag(int LicenseeID, int Currency, int baseCurrency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Report_LicenseeWLBA_DTO SelReportByLicenseeDetail(int LicenseeID, int Currency, int baseCurrency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<BetListDetail> BetlistDepositReport(int custId, string fdate, string tdate, bool showsb, bool showng, int showba, bool showrb, bool showlc, int drawFilter);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        LicenseeWinLossCommDTO LicenseeWinLossByCommLevel(int licenseeId, int currencyId, bool isExcludeTest,
                                                          string fromDate, string toDate, int drawFilter);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        LicenseesWLBA SelReportByLicensees(int licenseeId, int currency, int baseCurrency, string fromDate, string toDate, int reportType, int drawfilter, int excludeTest, int pageIndex, int pageSize, int fromCache);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Report_LicenseeWLBA_DTO SelWinLossReportByLicenseeDetailAgent(int licenseeId, int currencyId,
                                                                             int baseCurrencyId, string fromDate,
                                                                             string toDate, int drawfilter,
                                                                             int excludeTest, int pageIndex,
                                                                             int pageSize, int fromCache);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Report_LicenseeWLBA_DTO SelWinLossReportByLicenseeDetail(int licenseeId, int currencyId, int baseCurrencyId,
                                                                 int custId, string fromDate, string toDate,
                                                                 int drawfilter, int excludeTest, int reportType,
                                                                 int pageIndex, int pageSize, int fromCache);
    }
}