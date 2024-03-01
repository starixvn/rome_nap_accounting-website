// --------------------------------------------------------------------------
// <copyright file="Event.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Models
{
    using Fanex.BetList.Core.Utils;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Event
    {
        public Event()
        {
            MatchList = new List<Match>();
        }

        public DateTime WinLossDate { get; set; }

        public List<Match> MatchList { get; set; }

        public decimal Odds { get; set; }

        public string OddsText => ConvertByBetType.CommValue(Odds);

        public decimal MasterOdds { get; set; }

        public string MasterOddsText => ConvertByBetType.CommValue(MasterOdds);

        public bool ShowBothOdds => ConvertByBetType.IsShowSecondOdds(Odds, MasterOdds);

        public decimal BoostRate { get; set; }

        public string BoostRateText => ConvertByBetType.FormatBoostRate(BoostRate);

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public string Status { get; set; }
    }
}