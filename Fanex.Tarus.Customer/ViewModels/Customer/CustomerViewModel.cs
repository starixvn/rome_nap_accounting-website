using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class CustomerViewModel
    {
        public CustomerViewModel(AppConfiguration appConfig, string cndUri, string assetsVersion)
        {
            AppConfig = JsonConvert.SerializeObject(appConfig, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
        }

        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }
    }
}