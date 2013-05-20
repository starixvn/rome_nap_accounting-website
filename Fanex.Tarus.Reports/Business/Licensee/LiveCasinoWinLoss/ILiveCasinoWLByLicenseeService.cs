// -----------------------------------------------------------------------
// <copyright file="ILiveCasinoWLByLicenseeService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Licensee.LiveCasinoWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Reports.Entities.Licensee.LiveCasinoWinLoss;
using Fanex.Tarus.Reports.Entities.Sport;


    public interface ILiveCasinoWLByLicenseeService
    {
        Licensees SelAllLicensees(string productCode);
        List<BaseModel> SelAllGameTypes();
        LiveCasinoWinLoss SelLiveCasinoWinLoss(string normalLicensee, string operators, DateTime fromDate, DateTime toDate, int gameType);
        void ExportExcelLiveCasinoWinLossReport(string normalLicensee, string operators, string fromDate, string toDate, int gameType, string reportTitle);
    }
}
