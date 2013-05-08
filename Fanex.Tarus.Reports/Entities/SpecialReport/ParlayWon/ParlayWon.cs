namespace Fanex.Tarus.Reports.Entities.ParlayWon
{
    using System;
    using System.Collections.Generic;

     [Serializable]
    public class ParlayWonModel
    {
        public List<ParlayWon> ListParlayWon { get; set; }

        public string BaseWinlostTotal { get; set; }

        public string BaseStakeTotal { get; set; }

        public string CompanyWinlostTotal { get; set; }

        public string CompanyStakeTotal { get; set; }

        public int Count { get; set; }

        public ParlayWonModel()
        {
            this.ListParlayWon = new List<ParlayWon>();
        }
    }

    [Serializable]
    public class ParlayWon
    {
        public string RefNo { get; set; }

        public string UserName { get; set; }

        public string Currency { get; set; }

        public string Exchange { get; set; }

        public string OriginalStake { get; set; }

        public string OriginalWinlost { get; set; }

        public string BaseStake { get; set; }

        public string BaseWinlost { get; set; }

        public string CompanyStake { get; set; }

        public string CompanyWinlost { get; set; }

        public string Selection { get; set; }

        public string WinOdds { get; set; }
    }
}
