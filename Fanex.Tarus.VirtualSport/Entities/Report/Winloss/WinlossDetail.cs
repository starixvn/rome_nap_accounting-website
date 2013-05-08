
using System;
namespace Fanex.Tarus.VirtualSport.Entities.Report.Winloss
{

    [Serializable]
    public class WinlossDetail
    {
        public string Name { get; set; }
        public string Betcount { get; set; }
        public string Turnover { get; set; }
        public string WLMember { get; set; }
        public string Position { get; set; }
        public string WLCompany { get; set; }
        public string AmountDueto { get; set; }
    }

}
