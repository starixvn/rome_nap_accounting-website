namespace Fanex.Tarus.Reports.Entities.NumberGameWinLoss.NumberGameWinLossByCurrency
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// By David: 28/06/2012
    /// Params for SP [Acc_Rpt_NumberGameWinlossByCurrency_detail]
    /// </summary>
    public class NumberGameWinLossByCurrency_DetailParams
    {
        public string Companies { get; set; }

        public int LiveStatus { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string Currencies { get; set; }

        public string Bettypes { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool FromCache { get; set; }

        public bool IsShowComm { get; set; }

        public int RoleID { get; set; }

        public int CustID { get; set; }

        public string DetailName { get; set; }

    }
}
