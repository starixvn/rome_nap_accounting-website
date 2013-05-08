// -----------------------------------------------------------------------
// <copyright file="PunterAnalysisIpBO.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.PunterAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class PunterAnalysisIpBO
    {
        public string Month { get; set; }

        public string Year { get; set; }

        public string IP { get; set; }

        public string Country { get; set; }
    }
}
