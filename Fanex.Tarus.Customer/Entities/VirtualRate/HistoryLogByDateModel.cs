// --------------------------------------------------------------------------
// <copyright file="HistoryLogByDateModel.cs" company="Nexcel Solutions Vietnam">
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
    /// History Log Model.
    /// </summary>
    public class HistoryLogByDateModel
    {
        public string EditDateString { get; set; }

        public string Editor { get; set; }

        public List<HistoryLogDetailModel> HistoryLogDetailList { get; set; }
    }
}
