// --------------------------------------------------------------------------
// <copyright file="HistoryLogReportModel.cs" company="Nexcel Solutions Vietnam">
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
    /// History Log Report Model.
    /// </summary>
    public class HistoryLogReportModel
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public List<HistoryLogByDateModel> HistoryLog { get; set; }
    }
}
