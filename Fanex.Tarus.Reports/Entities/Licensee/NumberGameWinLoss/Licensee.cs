// -----------------------------------------------------------------------
// <copyright file="Licensee.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Entities.Licensee.NumberGameWinLoss
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    ///
    [Serializable]
    public class Licensees
    {
        public List<LicenseeDetail> Licensee { get; set; }
    }

    [Serializable]
    public class LicenseeDetail
    {
        public int ResellerId { get; set; }
        public string ResellerName { get; set; }
        public int ResellerType { get; set; }
        public int LicenseeId { get; set; }
        public string LicenseeName { get; set; }
    }
}
