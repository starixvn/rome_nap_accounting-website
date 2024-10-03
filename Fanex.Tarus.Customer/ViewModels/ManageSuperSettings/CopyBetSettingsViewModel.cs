using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Fanex.Tarus.Customer.ViewModels.ManageSuperSettings
{
    public class CopyBetSettingsViewModel
    {
        public CopyBetSettingsViewModel(
            AppConfigurationViewModel appConfig,
            string cndUri,
            string assetsVersion,
            string paramters)
        {
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
            Params = paramters;
            AppConfig = JsonConvert.SerializeObject(appConfig, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public string CdnUri { get; set; }
        public string AssetsVersion { get; set; }
        public string AppConfig { get; set; }
        public string Params { get; set; }
    }
}
