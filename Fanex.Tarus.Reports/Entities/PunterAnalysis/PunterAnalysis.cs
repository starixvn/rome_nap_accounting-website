
using System;
using System.Collections.Generic;
using Fanex.Tarus.Reports.Entities.Sport;

namespace Fanex.Tarus.Reports.Entities.PunterAnalysis
{
    [Serializable]
    public class PunterAnalysisBO : BasePunterAnalysisBO
    {
        public PunterAnalysisBO()
        {
            SportType = new Sporttype();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Ip { get; set; }

        public string CustId { get; set; }

        public Sporttype SportType { get; set; }
    }

    [Serializable]
    public class PunterAnalysisTotalDTO
    {
        public List<PunterAnalysisBO> Punters { get; set; }

        public string Name { get; set; }

        public PunterAnalysisBO Total { get; set; }
    }

    [Serializable]//will be removed later
    public class PunterAnalysisGrandTotalDTO_WilDeleted
    {
        public List<PunterAnalysisTotalDTO> Punters { get; set; }

        public PunterAnalysisTotalDTO Total { get; set; }
    }

}
