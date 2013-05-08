// -----------------------------------------------------------------------
// <copyright file="ILiveCasinoWLByLicenseeAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.Licensee.LiveCasinoWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Fanex.Tarus.Reports.Entities.Licensee;
using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ILiveCasinoWLByLicenseeAccess
    {
        DataTable SelAllLicensees(string productCode);
        DataTable SelLiveCasinoWLByLicensees(string normalLicensee, string Operator, DateTime fromDate, DateTime toDate);

    }
}
