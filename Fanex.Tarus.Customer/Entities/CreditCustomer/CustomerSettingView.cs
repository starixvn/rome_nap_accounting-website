// -----------------------------------------------------------------------
// <copyright file="CustomerSettingView.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Customer.CreditCustomer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerSettingView
    {
        public string TitlePage { get; set; }

        public bool IsInsertPage { get; set; }

        public bool IsMasterSetting { get; set; }

        public bool IsAgentSetting { get; set; }

        public bool IsSuperSetting { get; set; }

        public bool IsMemberSetting { get; set; }

        public bool IsUpdateMinMaxPt { get; set; }

        public int CustId { get; set; }

        public int UplineCustId { get; set; }

        public string CustUserName { get; set; }

        public string SiteName { get; set; }

        public bool CanEditCredit { get; set; }
    }
}