

using System;
using Fanex.Tarus.Reports.Entities.Sport;
namespace Fanex.Tarus.Reports.Entities.MatchWinLoss
{
    [Serializable]
    public class WLDetailByBetTypeModel : BaseModel
    {
        public BaseModel LiveIndicator { get; set; }
        public BaseModel BetType { get; set; }
        public Sporttype SportType { get; set; }
        public string Commission { get; set; }
        public string VirtualComm { get; set; }
        public string LineTotal { get; set; }
        public string VirtualLineTotal { get; set; }
        public string Variance { get; set; }
        public string Turnover { get; set; }
        public string Margin { get; set; }

        public string MatchCode { get; set; }
        public string EventDate { get; set; }
        public string KickOffTime { get; set; }
        public string HomeName { get; set; }
        public string AwayName { get; set; }
        public string WinLost { get; set; }
        public string Position { get; set; }
        public string LeagueName { get; set; }
        public bool IsNeutral { get; set; }

        public int TotalType { get; set; }
        public int ThirdPartyType { get; set; }
        public string Currency { get; set; }
    }
}
