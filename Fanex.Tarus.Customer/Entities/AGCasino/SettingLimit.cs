// --------------------------------------------------------------------------
// <copyright file="SettingLimit.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.AGCasino
{
    using System.Collections.Generic;

    /// <summary>
    /// Setting Limit.
    /// </summary>
    public class SettingLimit
    {
        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }
    }
}
