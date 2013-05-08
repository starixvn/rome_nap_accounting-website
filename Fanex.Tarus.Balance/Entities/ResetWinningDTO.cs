// -----------------------------------------------------------------------
// <copyright file="ResetWinningDTO.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Balance.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    /// 
    public class ResetWinning
    {
        
        public string Products { get; set; }

        
        public string Winloss { get; set; }

        
        public string Reset { get; set; }

        
        public string Variance { get; set; }
    }
    
    public class ResetWinningDTO
    {
        
        public List<ResetWinning> Items { get; set; }

        
        public string Total { get; set; }
    }
}
