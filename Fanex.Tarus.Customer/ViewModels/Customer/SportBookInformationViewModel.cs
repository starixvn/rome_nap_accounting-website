namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class SportBookInformationViewModel
    {
        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string PageTitle { get; }

        public string Params { get; }

        public SportBookInformationViewModel(
            byte tabIndex,
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
                    tabs = BuildTabs(tabIndex, appConfig.ApplicationPath),
                },
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        private object BuildTabs(byte tabIndex, string applicationPath)
        {
            return new[]
            {
                BuildBetSettingTabs(tabIndex),
                BuildMaxPayoutSettingTabs(tabIndex, applicationPath),
                BuildPTSettingTabs(tabIndex, applicationPath)
            };
        }

        private object BuildPTSettingTabs(byte tabIndex, string applicationPath)
        {
            return new
            {
                name = "Resources.PTSetting",
                url = applicationPath + "Matrix/ViewPT/GetBettypesReference?custId=0",
                index = 2,
                isActive = tabIndex == 2,
            };
        }

        private object BuildMaxPayoutSettingTabs(byte tabIndex, string applicationPath)
        {
            return new
            {
                name = "Resources.MaxPayoutSetting",
                url = applicationPath + "ViewMaxPayoutSettingInformation/Index",
                index = 1,
                isActive = tabIndex == 1,
            };
        }

        private object BuildBetSettingTabs(byte tabIndex)
        {
            return new
            {
                name = "Resources.Betsetting",
                url = "",
                index = 0,
                isActive = tabIndex == 0,
            };
        }
    }
}