using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.Tarus.Reports.Entities.PunterAnalysis;

namespace Fanex.Tarus.Reports.DataAccess.PunterAnalysis
{
    public interface IPunterAnalysicAccess
    {
        DataTable SelProductSport(bool IsSearchByLeague);

        DataTable SelLeagueBySportId(PunterAnalysisParam param);

        DataTable SelTopPunterSummary(PunterAnalysisParam param);

        DataTable SelTopPunterSummaryByLeague(PunterAnalysisParam param);

        DataTable SelTopPunterByCustID(PunterAnalysisParam param);

        DataTable SelTopPunterBySporttype(PunterAnalysisParam param);

        DataTable SelTopPunterByLeagueCustID(PunterAnalysisParam param);

        DataTable SelTopPunterByBettype(PunterAnalysisParam param);

        DataTable SelTopPunterBySporttypeSum(PunterAnalysisParam param);

        DataTable SelIPByCustomer(PunterAnalysisParam param);

        DataTable SelBetCountByCustomer(PunterAnalysisParam param);

        DataTable SelBetCountByCustomer_Detail(PunterAnalysisParam param);

        DataTable SelProduct();
    }
}
