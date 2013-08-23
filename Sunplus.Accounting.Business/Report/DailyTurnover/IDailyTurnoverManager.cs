using System.Collections.Generic;
using Sunplus.Accounting.Entities.Report.DailyTurnover;

namespace Sunplus.Accounting.Business.Report.DailyTurnover
{
    public interface IDailyTurnoverManager
    {
        List<DailyTurnoverDTO> SelDailyTurnover(string date);

        List<ActionTurnover> SelDetailTurnover(string date);
    }
}