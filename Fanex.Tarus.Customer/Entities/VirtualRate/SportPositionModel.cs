// --------------------------------------------------------------------------
// <copyright file="SportPositionModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.VirtualRate
{
    /// <summary>
    /// Sport Position Model.
    /// </summary>
    public class SportPositionModel
    {
        public int SportIndex { get; set; }

        public bool IsSubSport { get; set; }

        public int SubSportIndex { get; set; }
    }
}
