

namespace Fanex.Tarus.Reports.Entities.Gaming.PokerWinloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

   [Serializable]
    public class PokerWinlossByCurrency
    {
       public List<PokerWinlossDetail> PokerWinlossDetailList { get; set; }
       public string GrossCommBaseGrand { get; set; }
       public string CompanyCommBaseGrand { get; set; }
    }
}
