// -----------------------------------------------------------------------
// <copyright file="IGamingDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.Gaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGamingDataAccess
    {
        DataTable SelNumberGameWinlossByDate(string fromdate, string todate);
    }
}
