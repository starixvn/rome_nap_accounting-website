namespace Fanex.Tarus.Customer.ManageLicenseeCode.Request
{
    public class UpdNewLicenseeCodeRequest
    {
        public string LicenseeName { get; set; }

        public string LicenseeCode { get; set; }

        public bool CodeStatus { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string FromDateOld { get; set; }

        public string ToDateOld { get; set; }
    }
}
