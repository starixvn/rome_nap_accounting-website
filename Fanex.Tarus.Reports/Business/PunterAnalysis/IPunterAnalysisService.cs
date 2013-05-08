using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Reports.Entities.PunterAnalysis;
using Fanex.Tarus.Reports.Entities.Sport;

namespace Fanex.Tarus.Reports.Business.PunterAnalysis
{
    public interface IPunterAnalysisService
    {
        
        PunterAnalysisGroupSportsDTO SelSportsGroup();

        List<Model> SelLeagueBySportId(PunterAnalysisParam param);

        PunterAnalysisTotalDTO SelTopPunterSummary(PunterAnalysisParam param);

        PunterAnalysisTotalDTO SelTopPunterSummaryByLeague(PunterAnalysisParam param);

        PunterAnalysisTotalDTO SelTopPunterByCustID(PunterAnalysisParam param);               

        PunterAnalysisGrandTotalDTO SelTopPunterBySporttype(PunterAnalysisParam param);

        PunterAnalysisTotalDTO SelTopPunterByLeagueCustID(PunterAnalysisParam param);

        PunterAnalysisGrandTotalDTO SelTopPunterByBettype(PunterAnalysisParam param);

        PunterAnalysisTotalDTO SelTopPunterBySporttypeSum(PunterAnalysisParam param);

        List<PunterAnalysisIpBO> SelIPByCustomer(PunterAnalysisParam param);

        PunterAnalysisBetCountDTO SelBetCountByCustomer(PunterAnalysisParam param);

        PunterAnalysisBetCountDTO SelBetCountByCustomer_Detail(PunterAnalysisParam param);

        List<Product> SelProduct();

        List<Model> SelBettypeBySportId(int SportId);

        void ExportExcelTopPunterSummary(PunterAnalysisParam param);
        void ExportExcelTopPunterSummaryByLeague(PunterAnalysisParam param);
        void ExportExcelTopPunterByCustID(PunterAnalysisParam param);
        void ExportExcelTopPunterByLeagueCustID(PunterAnalysisParam param);
        void ExportExcelTopPunterBySporttypeSum(PunterAnalysisParam param);
        void ExportExcelTopPunterBySporttype(PunterAnalysisParam param);
        void ExportExcelTopPunterByBettype(PunterAnalysisParam param);
        void ExportExcelIPByCustId_TopPunterSummary(PunterAnalysisParam param);
        void ExportExcelBetCountByCustId_TopPunterSummary(PunterAnalysisParam param);
    }
}
