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
        
        public int Currency { get; set; }
        
        internal string ToJson()
            => JsonConvert.SerializeObject(
                new
                {
                    tabs = BuildTabs(),
                    is789y = false,
                    roleId = RoleId,
                    site = Site
                }
                , new JsonSerializerSettings
                    { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        
         private object BuildTabs()
        {
            return new[]
            {
                BuildBetSettingTabs,
                BuildMaxPayoutSettingTabs,
                BuildPTSettingTabs,
                BuildCommissionSettingTabs,
                BuildProductCategorySettingTabs,
            };
        }

        private object BuildBetSettingTabs
        {
            get =>  new
            {
                name = "BetSetting",
                id = "BetSettingTab",
                napUrl = "",
                index = 0,
                isActive = TabIndex == 0,
            };
        }

        private object BuildMaxPayoutSettingTabs
        {
            get =>  new
            {
                name = "MaxPayout",
                id = "MaxPayoutSettingTab", 
                napUrl = "/Customer/GetMaxPayoutSettingInformation?site=" + Site + "&isSpecialPlatform=false",
                index = 1,
                isActive = TabIndex == 1,
            };
        }

        private object BuildPTSettingTabs
        {
            get =>  new
            {
                name = "PositionTaking",
                id = "PTSettingTab",
                napUrl =   "/Customer/GetPTSettingInformation?custId=" + CustId + "&site=" + Site,
                index = 2,
                isActive = TabIndex == 2,
            };
        }

        private object BuildCommissionSettingTabs
        {
            get =>  new
            {
                name = "Commission",
                id = "CommissionHintTab",
                napUrl = "/Customer/GetCommissionHint?custId=" + CustId + "&site=" + Site,
                index = 3,
                isActive = TabIndex == 3,
            };
        }

        private object BuildProductCategorySettingTabs
        {
            get =>  new
            {
                name = "ProductCategories",
                id = "ProductCategorySettingTab",
                napUrl = "/Customer/GetProductCategories?custId=" + CustId + "&site=" + Site + "&currency=" + Currency,
                index = 4,
                isActive = TabIndex == 4,
            };
        }
        
    }
}