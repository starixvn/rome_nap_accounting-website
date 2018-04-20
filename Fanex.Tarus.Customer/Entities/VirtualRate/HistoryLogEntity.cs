// --------------------------------------------------------------------------
// <copyright file="HistoryLogEntity.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.VirtualRate
{
    using System;

    /// <summary>
    /// History Log Entity.
    /// </summary>
    public class HistoryLogEntity
    {
        public int ActionId { get; set; }

        public string TrackingValue { get; set; }

        public DateTime LogDate { get; set; }

        public string Username { get; set; }
    }
}
