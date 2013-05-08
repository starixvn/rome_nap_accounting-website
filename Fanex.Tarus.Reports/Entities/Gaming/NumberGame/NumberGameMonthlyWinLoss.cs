
namespace Fanex.Tarus.Reports.Entities.Gaming.NumberGame
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class NumberGameMonthlyWinLoss
    {
        public IList<NumberGameMonthlyWinLossDetail> WinLossByDate { get; set; }
        public NumberGameMonthlyWinLossDetail GrandTotal { get; set; }
    }

    
    [Serializable]
    public class NumberGameMonthlyWinLossDetail
    {
        public string Title { get; set; }
        public string NoPlayers { get; set; }
        public string NoGames { get; set; }
        public string TotalBets { get; set; }
        public string TotalWins { get; set; }
        public string GameIncome { get; set; }
        public string Payout { get; set; }

        public IList<NumberGameMonthlyWinLossDetail> WinLossDetail { get; set; }
    }
}
