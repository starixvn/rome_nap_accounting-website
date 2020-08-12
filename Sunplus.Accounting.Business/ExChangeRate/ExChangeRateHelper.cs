namespace Sunplus.Accounting.Business.ExChangeRate
{
    public static class ExChangeRateHelper
    {
        public static string BuildOfferSiteSelection(string offerSite)
        {
            return "<option value=\"0\"" + (offerSite == "0" ? " selected" : "") + ">All</option>" +
                "<option value = \"1\"" + (offerSite == "1" ? " selected" : "") + "> Credit </option>" +
                "<option value = \"2\"" + (offerSite == "2" ? " selected" : "") + "> Deposit </option>";
        }

        public static string GetOfferSiteName(string offerSiteId)
        {
            switch (offerSiteId)
            {
                case "0":
                    return "All";
                case "1":
                    return "Credit";
                case "2":
                    return "Deposit";
                default:
                    return string.Empty;
            }
        }
    }
}
