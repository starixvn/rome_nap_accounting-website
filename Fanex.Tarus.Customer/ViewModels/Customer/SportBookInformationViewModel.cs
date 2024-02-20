namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Sunplus.Accounting.Core.Extensions;
    using Fanex.Tarus.Customer.Requests.Customer;

    public class SportBookInformationViewModel
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
            string assetsVersion)
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
                    uplineRoleId = query.UplineRoleId,
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
                BuildPTCommissionSettingTabs(query),
                BuildProductCategorySettingTabs(query),
            };
        }

        private object BuildBetSettingTabs(SportBookInformationRequest query)
        {
            return new
            {
                name = "Resources.Betsetting",
                id = "BetSetting",
                url = "",
                axiosUrl = "",
                index = 0,
                isActive = query.TabIndex == 0,
            };
        }

        private object BuildMaxPayoutSettingTabs(SportBookInformationRequest query)
        {
            return new
            {
                name = "Resources.MaxPayoutSetting",
                id = "MaxPayoutSetting",
                url =" MicroSites.OldMainUrl() + \"/ViewMaxPayoutSettingInformation/Index.aspx\"",
                axiosUrl = "",
                index = 1,
                isActive = query.TabIndex == 1,
            };
        }

        private object BuildPTSettingTabs(SportBookInformationRequest query)
        {
            return new
            {
                name = "Resources.PTSetting",
                id = "PTSetting",
                axiosUrl = "MicroSites.MemberInfoUrl(\"~/Customer/GetPTSettingInformation\")",
                url = "MicroSites.OldMainUrl() + \"/PositionTakingInformation/PositionTakingBetTypeReference\"",
                index = 2,
                isActive = query.TabIndex == 2,
            };
        }

        private object BuildPTCommissionSettingTabs(SportBookInformationRequest query)
        {
            return new
            {
                name = "Resources.Commission",
                id = "Commission",
                url = "MicroSites.OldMainUrl(\"~/MemberInformation/Commission/CommissionHint\")",
                axiosUrl = "",
                index = 3,
                isActive = query.TabIndex == 3,
            };
        }

        private object BuildProductCategorySettingTabs(SportBookInformationRequest query)
        {
          //  var theme = query.IsDarkMode ? "dark" : "light";
            return new
            {
                name = "Product Category Setting",
                id = "ProductCategorySetting",
             //   url = MicroSites.MemberInfoUrl($"~/ProductSettingInfo/?theme={theme}&lng={query.Language}"),
                axiosUrl = "MicroSites.MemberInfoUrl($\"~/ProductSettingInfo/GetProductSettingInfo\")",
                index = 4,
                isActive = query.TabIndex == 4,
            };
        }
    }
}