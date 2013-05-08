


using System;
using System.Collections.Generic;

namespace Fanex.Tarus.Reports.Entities.MatchWinLoss
{
    [Serializable]
    public class WLBySportsGroupByBetType
    {
        public List<WLDetailBySportReport> BetTypeGroup { get; set; }
        public WLDetailByBetTypeModel Total { get; set; }
    }
}
