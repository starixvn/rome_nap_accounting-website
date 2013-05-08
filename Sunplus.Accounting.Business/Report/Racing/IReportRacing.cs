using System.Collections.Generic;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Report.Racebook;
using Sunplus.Accounting.Core.Common;
using System;

namespace Sunplus.Accounting.Business.Report.Racing
{

    public interface IReportRacing
    {
        RacingWinlossBO GetWinlossRacingReport(Enumerations.ReportType reportType, string winlossDate);

        RacingWinlossDetailsBO GetWinlossRacingReportDetails(string winlossDate, string poolCurrency, string partnerId);

        RacingWinlossBetlistBO GetWinlossRacingBetlist(string winlossdate, string leagueid, string raceNo, string currency);

        RacingWinlossBO GetWinlossRacingReportNew(Enumerations.ReportType reportType, string winlossDate);

        RacingWinlossDetailsBO GetWinlossRacingReportDetailsNew(string winlossDate, string poolCurrency, string partnerId);

        RacingWinlossBetlistBO GetWinlossRacingBetlistNew(string winlossdate, string leagueid, string raceNo, string currency);

        void ExportToExcelWinLossRacingReport(Enumerations.ReportType reportType, string winlossDate);

        void ExportToExcelWinLossRacingReportDetail(string winlossDate, string poolCurrency, string partnerId, string meetingName, Enumerations.ReportType reportType);

        RacingWinlossGroupDTO GetRacingWinlossGroup(string fromdate, string todate);

        RacingWinlossGroupDayBO GetRacingWinlossGroupForTote(string fromdate, string todate);

        void ExportToExcelWinLossRacingGroup(string fromDate, string toDate);

        bool SendMailWinLossRacingGroup(string fromDate, string toDate);

        SendMailResult SendMailWinLossRacingGroup(string fromDate, string toDate, List<MailFormat> mails, string Subject, string Content);

        Mails GetDefaultToEmailAddress(int horseId);

        void ExportToExcelWinLossRacingGroupForTote(string fromDate, string toDate);

        bool SendMailWinLossRacingGroupForTote(string fromDate, string toDate);

        SendMailResult SendMailWinLossRacingGroupForTote(string fromDate, string toDate, List<MailFormat> mails, string Subject, string Content);

        RacingDailyBySiteBO WinlostRacingLicensee_SiteSel();

        RacingDailyBySiteBO WinlostRacingLicensee_Details(string winlossdate, string site);

        RacingDailyBySiteBO RacingWinLossByLicenseeHavingPt(string winlossdate, string site);

        RacingDailyBySiteBetlistBO WinlostRacingLicensee_BetList_Details(string leagueid, string raceNo, string currency, DateTime winlostdate, string site);

        void ExportToExcelWinlossRacingLicenseeDetails(string winlossDate, string site, string reportTitle);

        void ExportToExcelWinlossRacingLicenseeDetailsPt(string winlossDate, string site, string reportTitle);

        RacingDailyWinLossDTO SelWinlostHR_Details(string wldate);

        void ExportToExcelRacingDailyReportForTote(string reportTitle, string winlostDate);
    }
}
