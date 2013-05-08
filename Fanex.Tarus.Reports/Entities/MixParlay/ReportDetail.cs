

using System;
using Fanex.Tarus.Reports.Entities.Sport;

namespace Fanex.Tarus.Reports.Entities.MixParlay
{
    [Serializable]
    public class ReportDetail : BaseModel
    {
        public decimal WinLoss { get; set; }
        public decimal Position { get; set; }
        public decimal Commission { get; set; }
        public decimal LineTotal { get; set; }
        public decimal Turnover { get; set; }
        public decimal Margin { get; set; }

        public string Status { get; set; }
        public string Hedge { get; set; }
        public bool IsLive { get; set; }

        public Match MatchInfo { get; set; }
        public DisplayRowType RowType { get; set; }
        public int RowIndex { get; set; }

        public Sporttype SportType { get; set; }
        public BaseModel BetType { get; set; }
        public BaseModel LiveType { get; set; }
    }
}
