// -----------------------------------------------------------------------
// <copyright file="PunterAnalysisGroupSports.cs" company="Microsoft">
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
    /// 

    [Serializable]
    public class Sporttype : Model
    {
        public Sporttype ()
        {
            Bettypes = new List<Model>();
        }

        public List<Model> Bettypes { get; set; }        
    }

    [Serializable]
    public class PunterAnalysisGroupSports : Model
    {
        public PunterAnalysisGroupSports()
        {
            GroupSports = new List<Sporttype>();
        }

        public List<Sporttype> GroupSports { get; set; }
    }

    [Serializable]
    public class PunterAnalysisGroupSportsDTO
    {
        public List<PunterAnalysisGroupSports> GroupName { get; set; }
    }
}
