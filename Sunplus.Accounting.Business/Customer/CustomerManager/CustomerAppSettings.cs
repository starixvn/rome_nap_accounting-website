using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Sunplus.Accounting.Business.Customer
{
    public static class CustomerAppSettings
    {
        private static IDictionary<string, bool> protectedModeOfferSites;

        static CustomerAppSettings()
        {
            protectedModeOfferSites = GetProtectedModeOfferSites();
        }

        public static bool IsNotOfferProtectedMode(string customerSite)
            => !IsOfferProtectedMode(customerSite);

        public static bool IsOfferProtectedMode(string customerSite)
            => protectedModeOfferSites.ContainsKey("*") || protectedModeOfferSites.ContainsKey(customerSite);

        private static IDictionary<string, bool> GetProtectedModeOfferSites()
        {
            var offerSites = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            var configValue = ConfigurationManager.AppSettings["ProtectedMode:OfferSites"] ?? string.Empty;
            configValue
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(site => offerSites[site.Trim()] = true);

            return offerSites;
        }
    }
}
