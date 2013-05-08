// --------------------------------------------------------------------------
// <copyright file="DailyWinLoss2hReportTotal.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Tarus.Reports.Entities.DailyWinLoss2h
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class DailyWinLoss2hReportTotal
    {
        public DailyWinLoss2hItem NormalTotal { get; set; }
        public DailyWinLoss2hItem EightRMTotal { get; set; }
        public DailyWinLoss2hItem BARMTotal { get; set; }
        public DailyWinLoss2hItem AllTotal { get; set; }
        public DailyWinLoss2hGrandTotal GrandTotal { get; set; }

        public DailyWinLoss2hItem SoccerEightRMTotal { get; set; }
        public DailyWinLoss2hItem SoccerBARMTotal { get; set; }
    }
}
