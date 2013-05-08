// --------------------------------------------------------------------------
// <copyright file="ILiveCasinoGameFinancialReportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Reports.DataAccess.Gaming.LiveCasinoGameFinancialReport
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Reports.Criteria.Gaming.LiveCasinoGameFinancialReport;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ILiveCasinoGameFinancialReportDataAccess
    {
        List<dynamic> GetCurrencies();
        List<dynamic> GetGameTypes();
        List<dynamic> GetGameTables(GetGameTableCriteria criteria);
        List<dynamic> GetReseller(GetResellerCriteria criteria);
        List<dynamic> GetFinancialReport(GetFinancialReportCriteria criteria, ref Hashtable outputParam);
    }
}
