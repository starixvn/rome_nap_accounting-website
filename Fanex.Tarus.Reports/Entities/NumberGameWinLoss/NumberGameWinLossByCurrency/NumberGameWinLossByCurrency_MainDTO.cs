
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
    public class NumberGameWinLossByCurrency_MainDTO
    {
        public string Currency { get; set; }

        public string Turnover { get; set; }

        public string MemberWinLoss { get; set; }

        public string GamePayout { get; set; }

        public string NetTurnover { get; set; }

        public string PTWinLoss { get; set; }

        public string CommExpense { get; set; }

        public string NetIncome { get; set; }

        public string Margin { get; set; }
    }
}
