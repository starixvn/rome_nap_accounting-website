using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class CreateCustomerViewModel
    {
        public CreateCustomerViewModel(AppConfiguration appConfig, string cndUri, string assetsVersion, int uplineId = 0, byte uplineRoleId = 0, string username = "")
        {
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
            AppConfig = JsonConvert.SerializeObject(appConfig, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            Params = JsonConvert.SerializeObject(new { uplineId, uplineRoleId }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            Username = username;
        }

        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string Params { get; }

        public string Username { get; }
    }
}