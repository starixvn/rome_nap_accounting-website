// --------------------------------------------------------------------------
// <copyright file="HistoryLogDetailModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.VirtualRate
{
    using System.Collections.Generic;

    /// <summary>
    /// History Log Detail Model.
    /// </summary>
    public class HistoryLogDetailModel
    {
        public bool EnableVirtualRate { get; set; }

        public int UpdateType { get; set; }

        public int SportId { get; set; }

        public string SportName { get; set; }

        public int LiveType { get; set; }

        public List<BetTypeModel> BetTypeList { get; set; }

        public List<VirtualRateModel> VirtualRate { get; set; }
    }
}
