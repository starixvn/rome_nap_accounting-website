using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Report.Racebook;

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [ServiceContract]
    public interface IReportRacing
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingWinlossBO GetWinlossRacingReport(string date, int reporttype);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingWinlossDetailsBO GetWinlossRacingReportDetails(string winlossDate, string poolCurrency, string partnerId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingWinlossDetailsBO GetWinlossRacingReportDetailsNew(string winlossDate, string poolCurrency, string partnerId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void WinLossRacingReportExportToExcel(string winlossDate, int reportType);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void WinLossRacingReportDetailExportToExcel(string winlossDate, string poolCurrency, string partnerId, string meetingName, int reportType);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingWinlossBetlistBO GetWinlossRacingBetlist(string winlossdate, string leagueid, string raceNo, string currency);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingWinlossBetlistBO GetWinlossRacingBetlistNew(string winlossdate, string leagueid, string raceNo, string currency);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingWinlossGroupDTO GetRacingWinlossGroup(string fromdate, string todate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingWinlossGroupDayBO GetRacingWinlossGroupForTote(string fromdate, string todate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void ExportToExcelWinLossRacingGroup(string fromDate, string toDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void ExportToExcelWinLossRacingGroupForTote(string fromDate, string toDate);

        [OperationContract(Name = "SendMailWinLossRacingGroup")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        bool SendMailWinLossRacingGroup(string fromDate, string toDate);

        [OperationContract(Name = "SendMailWinLossRacingGroup2")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        SendMailResult SendMailWinLossRacingGroup(string fromDate, string toDate, List<MailFormat> mails, string Subject, string Content);

        [OperationContract(Name = "SendMailWinLossRacingGroupForTote")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        bool SendMailWinLossRacingGroupForTote(string fromDate, string toDate);

        [OperationContract(Name = "SendMailWinLossRacingGroupForTote2")]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        SendMailResult SendMailWinLossRacingGroupForTote(string fromDate, string toDate, List<MailFormat> mails, string Subject, string Content);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Mails GetDefaultToEmailAddress(string horseId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingDailyBySiteBO WinlostRacingLicensee_SiteSel();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingDailyBySiteBO WinlostRacingLicensee_Details(string winlossdate, string site);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingDailyBySiteBO RacingWinLossByLicenseeHavingPt(string winlossdate, string site);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingDailyBySiteBetlistBO WinlostRacingLicensee_BetList_Details(string meetingid, string raceNo, string currency,
                                                                         string winlossdate, string site);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        RacingDailyWinLossDTO SelWinlostHR_Details(string wldate);
    }
}