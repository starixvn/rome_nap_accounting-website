// -----------------------------------------------------------------------
// <copyright file="Licensees.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.Licensee.LiveCasinoWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class LicenseeDetail
    {
        public int ResellerId { get; set; }
        public string ResellerName { get; set; }
        public int ResellerType { get; set; }
        public int LicenseeId { get; set; }
        public string LicenseeName { get; set; }
    }


    [Serializable]
    public class Licensees
    {
        public List<LicenseeDetail> Licensee { get; set; }
    }
   
}
