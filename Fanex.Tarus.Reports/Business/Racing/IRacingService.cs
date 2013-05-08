// -----------------------------------------------------------------------
// <copyright file="IRacingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Racing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Fanex.Tarus.Reports.Entities.Racing;
using ExcelUtil.Spreadsheet;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRacingService
    {
        List<ActiveMember> SelActiveMembers(string fromdate, string todate, string balanceUpDay);
        void ExportExcelActiveMembers(string fromdate, string todate);
    }
}
