


using System;
using System.Collections.Generic;

namespace Fanex.Tarus.Reports.Entities.MatchWinLoss
{
    [Serializable]
    public class WLThirdPartyReport
    {
        public List<WLDetailBySportReport> SportReportDetails { get; set; }
        public WLDetailBySportReport ReportByCurrency { get; set; }
        public WLDetailByBetTypeModel GrandTotal { get; set; }
    }
}
