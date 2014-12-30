// --------------------------------------------------------------------------
// <copyright file="SportVirtualRateModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.VirtualRate
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Sport Virtual Rate Model.
    /// </summary>
    [Serializable]
    public class SportVirtualRateModel : BaseModel
    {
        public List<VirtualRateModel> LiveVirtualRate { get; set; }

        public List<VirtualRateModel> NonLiveVirtualRate { get; set; }
    }
}
