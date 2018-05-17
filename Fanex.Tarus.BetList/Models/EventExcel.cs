namespace Fanex.Tarus.BetList.Models
{
    using System;
    using System.Collections.Generic;

    public class EventExcel
    {
        public EventExcel()
        {
            Matches = new List<Match>();
        }

        public string WinLossDate { get; set; }

        public string Status { get; set; }

        public decimal Odds { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public decimal PlayerComm { get; set; }

        public IList<Match> Matches { get; set; }
    }
}
