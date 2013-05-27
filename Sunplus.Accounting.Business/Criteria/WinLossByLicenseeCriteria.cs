// -----------------------------------------------------------------------
// <copyright file="WinLossByLicenseeCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Accounting.Business.Criteria
{
    using System;
    using Sunplus.Accounting.DataAccess.Report;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByLicenseeCriteria
    {
        public int LicenseeID { get; set; }

        public string LicenseeName { get; set; }

        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public int CustID { get; set; }

        public string UserName { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int ShowSB { get; set; }

        public int ShowRB { get; set; }

        public int ShowNG { get; set; }

        public int ShowBA { get; set; }

        public int ShowLC { get; set; }

        public int ShowVS { get; set; }

        public int ShowCS { get; set; }

        public int ReportType { get; set; }

        public int Drawfilter { get; set; }

        public int ConvertToCurrency { get; set; }

        public string ConvertToCurrencyName { get; set; }

        public DateTime MoveBetsDay
        {
            get
            {
                return ReportUtils.BalanceUp.MoveBetsDay;
            }
        }

        public static int ConvertCurrencyId { get; set; }

        /// <summary>
        /// Gets or sets IsOld: when report time before OldTableBaseTime
        ///  0 - query in table rptLicWinloss, 
        ///  1 - query in table rptLicWinloss_old. 
        /// </summary>
        public bool IsOld { get; set; }

        public int ExcludeTest { get; set; }

        #region PagingCriteria

        public bool IsGetAll { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalItem { get; set; }

        public int CurrentPage { get; set; }

        /// <summary>
        /// Gets StartIndex: base from zero
        /// </summary>
        public int StartIndex
        {
            get
            {
                if (this.CurrentPage <= 0)
                {
                    this.CurrentPage = 1;
                }

                return (this.CurrentPage - 1) * this.ItemsPerPage;
            }
        }

        /// <summary>
        /// Gets EndIndex: base from zero
        /// </summary>
        public int EndIndex
        {
            get
            {
                return this.StartIndex + this.ItemsPerPage - 1;
            }
        }

        #endregion PagingCriteria
    }
}
