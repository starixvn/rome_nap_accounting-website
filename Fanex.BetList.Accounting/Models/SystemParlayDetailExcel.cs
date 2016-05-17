namespace Fanex.BetList.Accounting.Models
{
    using System.Collections.Generic;

    public class SystemParlayDetailExcel
    {
        public IList<EventExcel> Events { get; set; }

        public decimal Total { get; set; }

        public decimal Outstanding { get; set; }
    }
}
