

namespace Fanex.Tarus.Reports.Entities.Gaming.PokerWinloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class PokerWinlossByDate
    {
        public string Date { get; set; }
        public List<PokerWinlossDetail> ListPokerWLDetail { get; set; }
    }
}
