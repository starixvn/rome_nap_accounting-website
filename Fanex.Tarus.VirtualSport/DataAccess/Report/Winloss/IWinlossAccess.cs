// -----------------------------------------------------------------------
// <copyright file="IWinlossAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.VirtualSport.DataAccess.Report.Winloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinlossAccess
    {
        DataSet GetDailyWinloss(DateTime fromDate, DateTime toDate, bool isExclude, int DrawFilter, string sportType);
        DataTable GetSportType();
        DataTable GetDailyWinlossByCurrency(DateTime date, string sportType, bool isExclude, int DrawFilter, string currency);
    }
}
