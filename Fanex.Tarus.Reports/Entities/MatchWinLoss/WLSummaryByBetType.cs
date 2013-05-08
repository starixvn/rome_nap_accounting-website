

using System;

namespace Fanex.Tarus.Reports.Entities.MatchWinLoss
{
    [Serializable]
    public class WLSummaryByBetType
    {
        public string BetType { get; set; }
        public int BetTypeId { get; set; }

        public string LineTotal_Soccer_Live { get; set; }
        public string VirtualLineTotal_Soccer_Live { get; set; }

        public string LineTotal_Soccer { get; set; }
        public string VirtualLineTotal_Soccer { get; set; }

        public string LineTotal_Others_Live { get; set; }
        public string VirtualLineTotal_Others_Live { get; set; }

        public string LineTotal_Others { get; set; }
        public string VirtualLineTotal_Others { get; set; }

        public string LineTotalReal { get; set; }
        public string LineTotalVirtual { get; set; }

        public string Variance { get; set; }
        public string Turnover { get; set; }
        public string Margin { get; set; }
        public bool IsTotal { get; set; }
    }
}
