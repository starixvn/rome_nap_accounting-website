// --------------------------------------------------------------------------
// <copyright file="INumberGameMonthlyWinLossReportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Reports.DataAccess.Gaming.NumberGameMonthlyWinLossReport
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Reports.Criteria.Gaming.NumberGameMonthlyWinLossReport;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface INumberGameMonthlyWinLossReportDataAccess
    {
        List<dynamic> GetReport(GetReportCriteria criteria);
    }
}
