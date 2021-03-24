namespace Fanex.Tarus.Customer.ManageLicenseeCode.Models
{
    using System.Collections.Generic;

    public class SelLicenseeCodeResponse
    {
        public IEnumerable<LicenseeCode> LicenseeCodes { get; set; }

        public string CurrentBillingMonth { get; set; }

        public int Totals { get; set; }
    }
}
