namespace Fanex.Tarus.Reports.Business.ParlayWon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data;
    using DNAide.Core;
    using DNAide.Utils;

    using Sunplus.Accounting.Core.Extensions;
    using ExcelUtil.Spreadsheet;

    using Fanex.Tarus.Reports.Entities.ParlayWon;

    public interface IParlayWonService
    {
        ParlayWonModel GetParlayBusiness(string winlostDate, int betType, bool isSoccer, string moveBetsDay, string balanceUpDay, bool fromCache, string sortDirection, int pageIndex, int pageSize, int sortBy);

        void ExportExcelParlayWon(string winlostDate, int betType, bool isSoccer, string moveBetsDay, string balanceUpDay, bool fromCache);
    }
}
