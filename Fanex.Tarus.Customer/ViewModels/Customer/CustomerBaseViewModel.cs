using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class CustomerBaseViewModel
    {
        protected JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        public CustomerBaseViewModel(AppConfiguration appConfig, string cndUri, string assetsVersion)
        {
            AppConfig = JsonConvert.SerializeObject(appConfig, jsonSerializerSettings);
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
        }

        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string Params { get; protected set; }
    }
}