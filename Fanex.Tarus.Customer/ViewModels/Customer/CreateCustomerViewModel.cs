using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Fanex.Tarus.Customer.Constants;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class CreateCustomerViewModel
    {
        public CreateCustomerViewModel(
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion,
            string secretKey,
            int uplineId = 0,
            byte uplineRoleId = 0,
            string uplineUsername = "",
            string site = "",
            string siteLabel = "")
        {
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
            SecretKey = secretKey;
            AppConfig = JsonConvert.SerializeObject(appConfig, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            Params = JsonConvert.SerializeObject(new { uplineId, uplineRoleId, site, siteLabel }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            switch (uplineRoleId)
            {
                case RoleIdConstants.SUPER:
                    PageTitle = $"Add New Master to {uplineUsername}";
                    break;

                case RoleIdConstants.MASTER:
                    PageTitle = $"Add New Agent to {uplineUsername}";
                    break;

                case RoleIdConstants.AGENT:
                    PageTitle = $"Add New Member to {uplineUsername}";
                    break;

                default:
                    PageTitle = "Add New Super";
                    break;
            }
        }

        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string Params { get; }

        public string SecretKey { get; }

        public string PageTitle { get; }
    }
}