// -----------------------------------------------------------------------
// <copyright file="IGamingService.cs" company="Microsoft">
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IGamingService
    {
        NumberGameWLModel SelNumberGameWinlossByDate(string fromdate, string todate);

        void ExportExcelNumberGameWinlossByDate(string fromdate, string todate);
    }
}
