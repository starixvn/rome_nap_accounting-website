using System.Collections.Generic;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Accounting.Entities.Report.NumberGame;
using Sunplus.Accounting.Entities.ViewModel;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
namespace Sunplus.Accounting.Business.Report
{
    public interface INumberGameReport
    {
        NumberGameTreeGridBO WinLoss_ByMatch_NumberGame(string date);

        NumberGameTreeGridBO WinLoss_ByMatch_NumberGame(string date, int SiteID, int Currency);

        NumberGameByDateTreeGridBO WinLoss_ByDate_NumberGame(string fromdate, string todate);

        NumberGameByDateTreeGridBO WinLoss_ByDate_NumberGame(string fromdate, string todate, int SiteID, int Currency);

        NumberGameDataGridBO Acc_Rpt_NumberGameMemberWinlossByMatch_details(string date, bool islive, int bettype, int matchid);

        NumberGameDataGridBO Acc_Rpt_NumberGameMemberWinlossByMatch_details(string date, bool islive, int bettype, int matchid, int SiteID, int Currency);

        NumberGameDataGridBO Acc_Rpt_NumberGameMemberWinlossByMatch_Details_Paging(string date, bool islive, int bettype, int matchid, int pageIndex, int pageSize, bool fromcache);

        MonthlyWinloss MonthlyWinloss(int month, int year);

        void ExportToExcel_WinLoss_ByMatch_NumberGame(string date);

        void ExportToExcelNumberGameWinLossByMatch(string date, string companies, string currencies, string bettypes);

        void ExportToExcel_WinLoss_ByMatchDetail_NumberGame(string date, bool isLive, int bettype, int matchId);

        void ExportToExcelNumberGameWinLossDetailByMatch(string date, bool isLive, int bettype, int matchId, string companies, string currencies);

        void ExportToExcel_WinLoss_ByDate_NumberGame(string fromdate, string todate);

        void MonthlyWinlossExportExcel(int month, int year);

        List<Sunplus.Accounting.Entities.Report.NumberGame.Currency> GetCurrencyList();

        List<Item> GetBettypeList();

        List<Item> GetLicensees();

        NumberGameByDateTreeGridBO NumberGameWinLossByDate(string companies, string currencies, string bettypes, string fromdate, string todate);

        BettypesWinLossDTO NumberGameWinLossByBettype(string companies, string currencies, string date);

        BettypeWinLossDetail NumberGameWinLossDetailByBettype(string date, int isLive, int bettype, string companies, string currencies);

        BettypeWinLossDetailDTO NumberGameWinLossDetailByBettypeNew(string date, int isLive, int bettype, string companies, string currencies);

        void ExportToExcelNumberGameWinLossByBettype(string companies, string currencies, string date);

        void ExportToExcelNumberGameWinLossDetailByBettype(string date, int isLive, int bettype, string bettypeName, string companies, string currencies);

        NumberGameTreeGridBO NumberGameWinLossByMatch(string date, string companies, string currencies, string bettypes);

        NumberGameDataGridBO NumberGameWinLossByMatchDetails(string date, bool islive, int matchid, string companies, string currencies, int bettype);
    }
}