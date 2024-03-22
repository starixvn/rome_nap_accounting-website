using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fanex.Tarus.Customer.Requests.Customer
{
    public class SportBookInformationRequest
    {
        public int? CustId { get; set; }

        public byte RoleId { get; set; }

        public int TabIndex { get; set; }

        public string Site { get; set; }
        
        public string SiteLabel { get; set; }

        public int Currency { get; set; }
        

        internal string ToJson() => JsonConvert.SerializeObject(new
        {
            tabs = BuildTabs(),
            roleId = RoleId,
            site = Site,
            siteLabel = SiteLabel,
        }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        private object BuildTabs()
        {
            return new[]
            {
                BetSettingTab,
                MaxPayoutSettingTab,
                PTSettingTab,
                CommissionSettingTab,
                ProductCategorySettingTab,
                WinLossLimitTab
            };
        }

        private object BetSettingTab => new
        {
            name = "BetSetting",
            id = "BetSettingTab",
            napUrl = "",
            isActive = TabIndex == 0,
        };
        
        private object MaxPayoutSettingTab => new
        {
            name = "MaxPayout",
            id = "MaxPayoutSettingTab",
            napUrl = "/Customer/GetMaxPayoutSettingInformation?site=" + Site + "&siteLabel=" + SiteLabel,
            isActive = TabIndex == 1,
        };
        
        private object PTSettingTab => new
        {
            name = "PositionTaking",
            id = "PTSettingTab",
            napUrl = "/Customer/GetPTSettingInformation?custId=" + CustId + "&site=" + Site,
            isActive = TabIndex == 2,
        };
        
        private object CommissionSettingTab => new
        {
            name = "Commission",
            id = "CommissionHintTab",
            napUrl = "/Customer/GetCommissionHint?custId=" + CustId + "&site=" + Site,
            isActive = TabIndex == 3,
        };
        
        private object ProductCategorySettingTab => new
        {
            name = "ProductCategories",
            id = "ProductCategorySettingTab",
            napUrl = "/Customer/GetProductCategories?custId=" + CustId + "&site=" + Site + "&currency=" + Currency,
            isActive = TabIndex == 4,
        };

        private object WinLossLimitTab => new
        {
            name = "WinLossLimit",
            id = "WinLossLimitTab",
            isActive = TabIndex == 5,
        };
    }
}