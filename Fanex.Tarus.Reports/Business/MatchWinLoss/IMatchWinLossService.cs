


using System;
using Fanex.Tarus.Reports.Entities.MatchWinLoss;

namespace Fanex.Tarus.Reports.Business.MatchWinLoss
{
    public interface IMatchWinLossService
    {
        WLSummaryBySportReport SelSummaryReportBySportType(DateTime reportDate);
        WLThirdPartyReport SelThirdPartyCasinoReport(DateTime reportDate);

        WLBySportsGroupByBetType SelSportTypeReportDetail(DateTime reportDate, string sportName);
        WLDetailBySportReport SelBetTypeReportDetail(DateTime reportDate, string sportName, int betType, int liveType);

        WLAllDetailsReport SelMatchWinLossReportAllDetails(DateTime reportDate);

        void ExportWLSummaryBySportTypeReport(DateTime reportDate, string reportName);
        void ExportSportTypeReportDetail(DateTime reportDate, string reportName, string sportName);
        void ExportBetTypeReportDetail(DateTime reportDate, string reportName,
            string sportName, int betType, string betTypeName, int liveType);
        void ExportWLAllDetails(DateTime reportDate, string reportName);
        void ExportWLSummaryMainReports(DateTime reportDate, string reportName);
    }
}
