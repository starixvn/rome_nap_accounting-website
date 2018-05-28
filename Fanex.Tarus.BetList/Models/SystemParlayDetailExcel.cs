namespace Fanex.Tarus.BetList.Models
{
    using System.Collections.Generic;

    public class SystemParlayDetailExcel
    {
        public IList<EventExcel> Events { get; set; }

        public decimal Total { get; set; }

        public decimal Outstanding { get; set; }
    }
}
