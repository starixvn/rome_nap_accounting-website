using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class CreateCustomerViewModel
    {
        public CreateCustomerViewModel(AppConfiguration appConfig, string cndUri, string assetsVersion, string secretKey, int uplineId = 0, byte uplineRoleId = 0)
        {
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
            SecretKey = secretKey;
            AppConfig = JsonConvert.SerializeObject(appConfig, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            Params = JsonConvert.SerializeObject(new { uplineId, uplineRoleId }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string Params { get; }

        public string SecretKey { get; }
    }
}