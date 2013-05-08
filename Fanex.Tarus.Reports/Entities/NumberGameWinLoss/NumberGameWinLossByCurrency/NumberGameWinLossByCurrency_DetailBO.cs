
namespace Fanex.Tarus.Reports.Entities.NumberGameWinLoss.NumberGameWinLossByCurrency
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class NumberGameWinLossByCurrency_DetailBO
    {
        public List<NumberGameWinLossByCurrency_DetailDTO> ListUsername { get; set; }

        public int Quantity { get; set; }

        public NumberGameWinLossByCurrency_DetailDTO Total { get; set; }

        public string Currencies { get; set; }

        public string DetailName { get; set; }
    }
}
