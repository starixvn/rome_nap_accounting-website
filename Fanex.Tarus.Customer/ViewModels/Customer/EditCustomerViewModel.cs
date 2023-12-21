using Fanex.Tarus.Customer.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class EditCustomerViewModel
    {
        public EditCustomerViewModel(
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion,
            int roleId,
            string uname
            )
        {
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
            AppConfig = JsonConvert.SerializeObject(appConfig, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            switch (roleId)
            {
                case RoleIdConstants.SUPER:
                    PageTitle = $"Super {uname}";
                    break;

                case RoleIdConstants.MASTER:
                    PageTitle = $"Master {uname}";
                    break;

                case RoleIdConstants.AGENT:
                    PageTitle = $"Agent {uname}";
                    break;

                default:
                    PageTitle = $"{uname}";
                    break;
            }
        }

        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string PageTitle { get; }
    }
}
