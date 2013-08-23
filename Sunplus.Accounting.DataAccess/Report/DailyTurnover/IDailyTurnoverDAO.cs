using DNAide.Data;

namespace Sunplus.Accounting.DataAccess.Report.DailyTurnoverDAO
{
    public interface IDailyTurnoverDAO
    {
        DbResult SelDailyTurnover(string date, bool isActual, string key);

        DbResult SelDailyChangeDetial(string date);
    }
}