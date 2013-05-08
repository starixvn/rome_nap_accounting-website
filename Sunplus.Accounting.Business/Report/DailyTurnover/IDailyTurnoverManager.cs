using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Accounting.Entities.Report.DailyTurnover;

namespace Sunplus.Accounting.Business.Report.DailyTurnover
{
    public interface IDailyTurnoverManager
    {
        List<DailyTurnoverDTO> SelDailyTurnover(string date);

        List<ActionTurnover> SelDetailTurnover(string date);
    }
}
