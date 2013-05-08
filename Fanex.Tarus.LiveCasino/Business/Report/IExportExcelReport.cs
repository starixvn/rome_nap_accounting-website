using Fanex.Tarus.LiveCasino.Entities.Report;

namespace Fanex.Tarus.LiveCasino.Business.Report
{
    public interface IExportExcelReport
    {
        void DailyWinloss();
        void ExportExcelDailyWLDirectLC(string winlostdate, string reportTitle);
        void ExportExcelGameStatisticsReport(GameStatisticsParams request);
    }
}