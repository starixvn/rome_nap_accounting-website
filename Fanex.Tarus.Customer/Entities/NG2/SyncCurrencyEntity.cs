using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Customer.Entities.NG2
{
    public class SyncCurrencyEntity
    {
        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencyCode { get; set; }

        public double BaseRate { get; set; }
    }
}