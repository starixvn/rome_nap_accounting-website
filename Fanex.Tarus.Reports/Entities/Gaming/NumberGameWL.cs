// -----------------------------------------------------------------------
// <copyright file="NumberGameWL.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.Gaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class NumberGameWL
    {
        public string Name { get; set; }
        
        public string WinLoss { get; set; }

        public string Position { get; set; }

        public string Comm { get; set; }

        public string LineTotal { get; set; }

        public string Turnover { get; set; }

        public string Margin { get; set; }

        public List<NumberGameWL> Childs { get; set; }
    }

    [Serializable]
    public class NumberGameWLModel 
    {
        public List<NumberGameWL>  LiveIndicators { get; set; }       

        public NumberGameWL Total { get; set; }
    }
}
