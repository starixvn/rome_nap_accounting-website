namespace Fanex.Tarus.Customer.ManageLicenseeCode.Request
{
    public class SearchRequest
    {
        public string LicenseeIds { get; set; }

        public string StatusIds { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
