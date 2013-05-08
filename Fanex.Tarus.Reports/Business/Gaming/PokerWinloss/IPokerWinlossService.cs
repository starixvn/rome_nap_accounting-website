// -----------------------------------------------------------------------
// <copyright file="IPokerWinlossService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Gaming.PokerWinloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Reports.Entities.Gaming.PokerWinloss;
using ExcelUtil.Spreadsheet;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPokerWinlossService
    {
        PokerWinloss GetPokerWinloss(DateTime fromDate, DateTime toDate, bool isViewWH);
        PokerWinlossByCurrency GetPokerWinlossDetail(DateTime wlDate, int currency, bool isViewWH);
        ExcelFile PokerWinlossExportExcel(DateTime fromDate, DateTime toDate, bool isViewWH);
        ExcelFile PokerWinlossDetailExportExcel(DateTime wlDate, int currency, bool isViewWH);
      
    }
}
