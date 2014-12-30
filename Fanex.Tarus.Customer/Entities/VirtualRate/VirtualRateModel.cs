// --------------------------------------------------------------------------
// <copyright file="VirtualRateModel.cs" company="Nexcel Solutions Vietnam">
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
    /// Virtual Rate Model.
    /// </summary>
    [Serializable]
    public class VirtualRateModel
    {
        public decimal PercentRate { get; set; }

        public decimal Value { get; set; }

        public bool HasData { get; set; }
    }
}
