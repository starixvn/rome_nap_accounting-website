// --------------------------------------------------------------------------
// <copyright file="LogBySportEntity.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.VirtualRate
{
    using System;

    /// <summary>
    /// Log By Sport Entity.
    /// </summary>
    [Serializable]
    public class LogBySportEntity
    {
        public int SportType { get; set; }

        public int BetType { get; set; }

        public int LiveType { get; set; }

        public decimal VirtualRate { get; set; }
    }
}
