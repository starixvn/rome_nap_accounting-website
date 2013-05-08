// -----------------------------------------------------------------------
// <copyright file="PunterAnalysisRowBO.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.PunterAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Reports.Entities.Sport;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    /// 

    [Serializable]
    public class PunterAnalysisRowByType : BasePunterAnalysisBO
    {
        public string Type { get; set; }
    }

    [Serializable]
    public class PunterAnalysisRowBO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Sporttype SportType { get; set; }

        public List<PunterAnalysisRowByType> Columns { get; set; }
    }

    [Serializable]
    public class PunterAnalysisGrandTotalDTO
    {
        public List<PunterAnalysisRowBO> Punters { get; set; }

        public PunterAnalysisRowBO Total { get; set; }

        public List<PunterAnalysisBO> GrandTotal { get; set; }
    }
}
