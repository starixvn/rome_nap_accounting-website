namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Sunplus.Accounting.Core.Extensions;

    public class SportBookInformationViewModel
    {
        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string PageTitle { get; }

        public string Params { get; }

        public string TabBetSettingContent { get; }

        public SportBookInformationViewModel(
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion, byte tabIndex,
            int roleId)
        {
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
            AppConfig = JsonConvert.SerializeObject(appConfig,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            PageTitle = "Sport Book Information";
            Params = JsonConvert.SerializeObject(new JsonSerializerSettings
                { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            TabBetSettingContent =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                    roleId > 1 ? "SMAUnlimitedBetSettingWarningMessage" : "MemberUnlimitedBetSettingWarningMessage");
        }
    }
}