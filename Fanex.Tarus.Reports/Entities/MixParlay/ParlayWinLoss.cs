// -----------------------------------------------------------------------
// <copyright file="ParlayCombination.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.MixParlay
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class DailyParlayWinLoss
    {
        public string Name { get; set; }

        public string ParentName { get; set; }

        public string WinLoss { get; set; }

        public string BetCount { get; set; }

        public string MemberTurnover { get; set; }

        public string Position { get; set; }

        public string Comm { get; set; }

        public string LineTotal { get; set; }

        public string Turnover { get; set; }

        public string Margin { get; set; }

        public int rowspan { get; set; }

        public List<DailyParlayWinLoss> Rows { get; set; }
    }
}
