// -----------------------------------------------------------------------
// <copyright file="IVirtualSportsWinlossByLicenseeAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.Licensee.VirtualSportWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;

    public interface IVirtualSportsWinlossByLicenseeAccess
    {
        DataTable GetReseller(string product, bool isExcludeBA, bool isExcludeTest);
        DataTable GetSportType();
        DataTable GetVirtualSportWinloss(string normalLicensee, string operators, DateTime fromDate, DateTime toDate, string sportList);
    }
}
