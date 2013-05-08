// -----------------------------------------------------------------------
// <copyright file="IRacingDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.Racing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRacingDataAccess
    {
        DataTable SelNumberActiveMember(string fromdate, string todate, string balanceUpDay);
    }
}
