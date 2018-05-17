// --------------------------------------------------------------------------
// <copyright file="SystemParlayDetail.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SystemParlayDetail
    {
        public const int MaxMatchRowsToShow = 5000;

        public SystemParlayDetail()
        {
            EventList = new List<Event>();
        }

        public List<Event> EventList { get; set; }

        public decimal SubTotal { get; set; }

        public bool ShowAlternateLink { get; set; }
    }
}