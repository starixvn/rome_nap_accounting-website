using Sunplus.Accounting.Entities.Report.NumberGame;

namespace Sunplus.Accounting.DataAccess.IReportDAO
{
    public interface INumberGameReportDAO
    {
        MonthlyWinloss MonthlyWinloss(int month, int year);
    }
}