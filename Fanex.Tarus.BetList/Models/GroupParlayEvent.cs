// --------------------------------------------------------------------------
// <copyright file="GroupParlayEvent.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupParlayEvent
    {
        public DateTime WinLossDate { get; set; }

        public IList<ParlayEvent> ParlayEvents { get; set; }

        public ParlayEvent SubTotal { get; set; }
    }
}