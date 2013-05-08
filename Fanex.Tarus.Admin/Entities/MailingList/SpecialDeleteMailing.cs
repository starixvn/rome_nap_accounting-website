// -----------------------------------------------------------------------
// <copyright file="SpecialDeleteMailing.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.Entities.MailingList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SpecialDeleteMailing
    {
        public List<MailLicensee> LicenseeList { get; set; }

        public string NonMailingList { get; set; }

        public SpecialDeleteMailing()
        {
            LicenseeList = new List<MailLicensee>();
        }
    }

    public class MailLicensee
    {
        public string SiteName { get; set; }

        public int SiteID { get; set; }

        public string ToList { get; set; }

        public string CCList { get; set; }

        public string BCCList { get; set; }

        public bool IsMailExist { get; set; }
    }
}
