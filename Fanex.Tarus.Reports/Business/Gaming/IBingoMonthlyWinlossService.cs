// -----------------------------------------------------------------------
// <copyright file="IBingoMonthlyWinloss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Gaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Reports.Entities.Gaming;
using ExcelUtil.Spreadsheet;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBingoMonthlyWinlossService
    {
        BingoMonthlyWL GetDataBingoMonthlyWL(string month, string year);
        ExcelFile ExportExcelBingoMonthly(string month, string year);
    }
}
