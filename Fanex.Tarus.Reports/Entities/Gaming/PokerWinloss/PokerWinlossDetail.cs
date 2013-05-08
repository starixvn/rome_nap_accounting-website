
namespace Fanex.Tarus.Reports.Entities.Gaming.PokerWinloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class PokerWinlossDetail
    {
        public string Name { get; set; }
        public string CurrencyId { get; set; }
        public string GrossComm { get; set; }
        public string MemberWL { get; set; }
        public string MemberComm { get; set; }
        public string TotalMember { get; set; }
        public string AgentComm { get; set; }
        public string MasterComm { get; set; }
        public string SuperComm { get; set; }
        public string CompanyComm { get; set; }
        public string GrossCommBase { get; set; }
        public string CompanyCommBase { get; set; }
    }
}
