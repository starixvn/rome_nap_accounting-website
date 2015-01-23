// --------------------------------------------------------------------------
// <copyright file="BetTypeModel.cs" company="Nexcel Solutions Vietnam">
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
    /// Bet Type Model.
    /// </summary>
    [Serializable]
    public class BetTypeModel : BaseModel
    {
        public int[] MultiBetTypes { get; set; }

        public string[] MultiBetTypeNames { get; set; }
    }
}
