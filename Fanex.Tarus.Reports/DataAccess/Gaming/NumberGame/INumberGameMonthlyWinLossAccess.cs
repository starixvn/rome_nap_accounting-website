
namespace Fanex.Tarus.Reports.DataAccess.Gaming.NumberGame
{
    using System;
    using System.Data;

    public interface INumberGameMonthlyWinLossAccess
    {
        DataSet GetNumberGameMonthlyWinLossReport(DateTime fromDate, DateTime toDate);
    }
}
