// -----------------------------------------------------------------------
// <copyright file="INumberGameWinLoss.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.Licensee
{
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INumberGameWinLossAccess
    {
        DataSet SelAllLicensees();
        DataSet SelNumberGameWinLoss(string normalLicensee, string operators, string fromDate, string toDate);
    }
}
