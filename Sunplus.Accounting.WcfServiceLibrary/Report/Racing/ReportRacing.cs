using System.Collections.Generic;
using System.ServiceModel.Activation;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Report.Racebook;

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReportRacing : IReportRacing
    {
        public RacingWinlossBO GetWinlossRacingReport(string date, int reporttype)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            Enumerations.ReportType _reporttype;
            switch (reporttype)
            {
                case 1:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Daily_Report_For_UK;
                    break;

                case 2:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Daily_Report_For_AU;
                    break;

                case 3:
                    _reporttype = Enumerations.ReportType.Greyhound_Daily_Report_For_UK;
                    break;

                case 4:
                    _reporttype = Enumerations.ReportType.Greyhound_Daily_Report_For_AU;
                    break;

                case 5:
                    _reporttype = Enumerations.ReportType.Harness_Daily_Report_For_AU;
                    break;

                case 6:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Report_for_SA_Tote_NEW;
                    return racingwinloss.GetWinlossRacingReportNew(_reporttype, date);
                //break;
                default:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Daily_Report_For_UK;
                    break;
            }
            return racingwinloss.GetWinlossRacingReport(_reporttype, date);
        }

        public RacingWinlossDetailsBO GetWinlossRacingReportDetails(string winlossDate, string poolCurrency, string partnerId)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racingwinloss.GetWinlossRacingReportDetails(winlossDate, poolCurrency, partnerId);
        }

        public RacingWinlossDetailsBO GetWinlossRacingReportDetailsNew(string winlossDate, string poolCurrency, string partnerId)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racingwinloss.GetWinlossRacingReportDetailsNew(winlossDate, poolCurrency, partnerId);
        }

        #region Export 2 Excel

        public void WinLossRacingReportExportToExcel(string winlossDate, int reportType)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();

            #region get report type

            Enumerations.ReportType _reporttype;
            switch (reportType)
            {
                case 1:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Daily_Report_For_UK;
                    break;

                case 2:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Daily_Report_For_AU;
                    break;

                case 3:
                    _reporttype = Enumerations.ReportType.Greyhound_Daily_Report_For_UK;
                    break;

                case 4:
                    _reporttype = Enumerations.ReportType.Greyhound_Daily_Report_For_AU;
                    break;

                case 5:
                    _reporttype = Enumerations.ReportType.Harness_Daily_Report_For_AU;
                    break;

                case 6:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Report_for_SA_Tote_NEW;
                    break;

                default:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Daily_Report_For_UK;
                    break;
            }

            #endregion get report type

            racingwinloss.ExportToExcelWinLossRacingReport(_reporttype, winlossDate);
        }

        public void WinLossRacingReportDetailExportToExcel(string winlossDate, string poolCurrency, string partnerId, string meetingName, int reportType)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();

            #region get report type

            Enumerations.ReportType _reporttype;
            switch (reportType)
            {
                case 101:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Daily_Report_Detail_For_UK;
                    break;

                case 102:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Daily_Report_Detail_For_AU;
                    break;

                case 103:
                    _reporttype = Enumerations.ReportType.Greyhound_Daily_Report_Detail_For_UK;
                    break;

                case 104:
                    _reporttype = Enumerations.ReportType.Greyhound_Daily_Report_Detail_For_AU;
                    break;

                case 105:
                    _reporttype = Enumerations.ReportType.Harness_Daily_Report_Detail_For_AU;
                    break;

                case 106:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Report_Detail_for_SA_Tote_NEW;
                    break;

                default:
                    _reporttype = Enumerations.ReportType.Horse_Racing_Daily_Report_Detail_For_UK;
                    break;
            }

            #endregion get report type

            racingwinloss.ExportToExcelWinLossRacingReportDetail(winlossDate, poolCurrency, partnerId, meetingName, _reporttype);
        }

        #endregion Export 2 Excel

        public RacingWinlossBetlistBO GetWinlossRacingBetlist(string winlossdate, string leagueid, string raceNo, string currency)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racingwinloss.GetWinlossRacingBetlist(winlossdate, leagueid, raceNo, currency);
        }

        public RacingWinlossBetlistBO GetWinlossRacingBetlistNew(string winlossdate, string leagueid, string raceNo, string currency)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racingwinloss.GetWinlossRacingBetlistNew(winlossdate, leagueid, raceNo, currency);
        }

        public RacingWinlossGroupDTO GetRacingWinlossGroup(string fromdate, string todate)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racingwinloss.GetRacingWinlossGroup(fromdate, todate);
        }

        public RacingWinlossGroupDayBO GetRacingWinlossGroupForTote(string fromdate, string todate)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racingwinloss.GetRacingWinlossGroupForTote(fromdate, todate);
        }

        public void ExportToExcelWinLossRacingGroup(string fromDate, string toDate)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            racing.ExportToExcelWinLossRacingGroup(fromDate, toDate);
        }

        public void ExportToExcelWinLossRacingGroupForTote(string fromDate, string toDate)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            racing.ExportToExcelWinLossRacingGroupForTote(fromDate, toDate);
        }

        public bool SendMailWinLossRacingGroup(string fromDate, string toDate)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racing.SendMailWinLossRacingGroup(fromDate, toDate);
        }

        public SendMailResult SendMailWinLossRacingGroup(string fromDate, string toDate, List<MailFormat> mails, string Subject, string Content)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racing.SendMailWinLossRacingGroup(fromDate, toDate, mails, Subject, Content);
        }

        public bool SendMailWinLossRacingGroupForTote(string fromDate, string toDate)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racing.SendMailWinLossRacingGroupForTote(fromDate, toDate);
        }

        public SendMailResult SendMailWinLossRacingGroupForTote(string fromDate, string toDate, List<MailFormat> mails, string Subject, string Content)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racing.SendMailWinLossRacingGroupForTote(fromDate, toDate, mails, Subject, Content);
        }

        public Mails GetDefaultToEmailAddress(string horseId)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racing.GetDefaultToEmailAddress(CastAide.AsInt32(horseId));
        }

        public RacingDailyBySiteBO WinlostRacingLicensee_SiteSel()
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racing.WinlostRacingLicensee_SiteSel();
        }

        public RacingDailyBySiteBO WinlostRacingLicensee_Details(string winlossdate, string site)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racing.WinlostRacingLicensee_Details(winlossdate, site);
        }

        public RacingDailyBySiteBO RacingWinLossByLicenseeHavingPt(string winlossdate, string site)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racing.RacingWinLossByLicenseeHavingPt(winlossdate, site);
        }

        public RacingDailyBySiteBetlistBO WinlostRacingLicensee_BetList_Details(string meetingid, string raceNo, string currency, string winlossdate, string site)
        {
            var racing = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racing.WinlostRacingLicensee_BetList_Details(meetingid, raceNo, currency, CastAide.AsDateTime(winlossdate), site);
        }

        public RacingDailyWinLossDTO SelWinlostHR_Details(string wldate)
        {
            var racingwinloss = CommonBootStrapper.Locator.GetInstance<Business.Report.Racing.IReportRacing>();
            return racingwinloss.SelWinlostHR_Details(wldate);
        }
    }
}