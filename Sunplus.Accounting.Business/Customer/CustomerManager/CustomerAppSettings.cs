using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Sunplus.Accounting.Business.Customer
{
    public static class CustomerAppSettings
    {
        private static IDictionary<string, bool> protectedModeOfferSiteLabel;

        static CustomerAppSettings()
        {
            protectedModeOfferSiteLabel = GetProtectedModeOfferSiteLabel();
        }

        public static bool IsNotOfferProtectedMode(string siteLabel)
            => !IsOfferProtectedMode(siteLabel);

        public static bool IsOfferProtectedMode(string customerSite)
            => protectedModeOfferSiteLabel.ContainsKey("*") || protectedModeOfferSiteLabel.ContainsKey(customerSite);

        private static IDictionary<string, bool> GetProtectedModeOfferSiteLabel()
        {
            var offerSites = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            var configValue = ConfigurationManager.AppSettings["ProtectedMode:OfferSiteLabel"] ?? string.Empty;
            configValue
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(site => offerSites[site.Trim()] = true);

            return offerSites;
        }
    }
}
