namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Sunplus.Accounting.Core.Extensions;
    using Fanex.Tarus.Customer.Requests.Customer;

    public class SportBookInformationViewModel : CustomerBaseViewModel
    {
        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string PageTitle { get; }

        public string Params { get; }

        public SportBookInformationViewModel(
            SportBookInformationRequest query,
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion) : base(appConfig, cndUri, assetsVersion)
        {
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
            AppConfig = JsonConvert.SerializeObject(appConfig,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            PageTitle = "Sport Book Information";
            Params = JsonConvert.SerializeObject(
                new
                {
                    tabs = BuildTabs(query),
                    is789y = false,
                    roleId = query.RoleId,
                    site = query.Site
                }
                , new JsonSerializerSettings
                    { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        private object BuildTabs(SportBookInformationRequest query)
        {
            return new[]
            {
                BuildBetSettingTabs(query),
                BuildMaxPayoutSettingTabs(query),
                BuildPTSettingTabs(query),
                BuildCommissionSettingTabs(query),
                BuildProductCategorySettingTabs(query),
            };
        }

        private object BuildBetSettingTabs(SportBookInformationRequest query)
        {
            return new
            {
                name = "Bet Setting",
                id = "BetSetting",
                napUrl = "",
                index = 0,
                isActive = query.TabIndex == 0,
            };
        }

        private object BuildMaxPayoutSettingTabs(SportBookInformationRequest query)
        {
            return new
            {
                name = "Max Payout Setting",
                id = "MaxPayoutSetting",
                napUrl = "/Customer/GetMaxPayoutSettingInformation?site=" + query.Site + "&isSpecialPlatform=false",
                index = 1,
                isActive = query.TabIndex == 1,
            };
        }

        private object BuildPTSettingTabs(SportBookInformationRequest query)
        {
            return new
            {
                name = "Position Taking",
                id = "PTSetting",
                napUrl =   "/Customer/GetPTSettingInformation?custId=" + query.CustId,
                index = 2,
                isActive = query.TabIndex == 2,
            };
        }

        private object BuildCommissionSettingTabs(SportBookInformationRequest query)
        {
            return new
            {
                name = "Commission",
                id = "Commission",
                napUrl = "/Customer/GetCommissionHint?custId=" + query.CustId,
                index = 3,
                isActive = query.TabIndex == 3,
            };
        }

        private object BuildProductCategorySettingTabs(SportBookInformationRequest query)
        {
            //  var theme = query.IsDarkMode ? "dark" : "light";
            return new
            {
                name = "Product Category",
                id = "ProductCategorySetting",
                napUrl = "/Customer/GetProductCategories?custId=" + query.CustId,
                index = 4,
                isActive = query.TabIndex == 4,
            };
        }
    }
}