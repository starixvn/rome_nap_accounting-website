// -----------------------------------------------------------------------
// <copyright file="INumberGameWinLossByBettypeAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.DataAccess.NumberGameWinLoss.NumberGameWinLossByBettype
{
    using System;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface INumberGameWinLossByBettypeAccess
    {
        DataSet SelNumberGameWinLossByBettype(DateTime wlDate, string currencies, string companies, int liveStatus);
        DataSet SelNumberGameWinLossByBettypeDetail(DateTime wlDate, string currencies, string companies, int liveStatus, int bettype);
    }
}
