using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.Requests.Customer;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class CreateCustomerViewModel : CustomerBaseViewModel
    {
        public CreateCustomerViewModel(
            CreateCustomerRequest request,
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion,
            string secretKey)
            : base(appConfig, cndUri, assetsVersion)
        {
            SecretKey = secretKey;
            Params = request.ToJson();

            switch (request.UplineRoleId)
            {
                case RoleIdConstants.SUPER:
                    PageTitle = $"Add New Master to {request.UplineUsername}";
                    break;

                case RoleIdConstants.MASTER:
                    PageTitle = $"Add New Agent to {request.UplineUsername}";
                    break;

                case RoleIdConstants.AGENT:
                    PageTitle = $"Add New Member to {request.UplineUsername}";
                    break;

                default:
                    PageTitle = "Add New Super";
                    break;
            }
        }

        public string SecretKey { get; }

        public string PageTitle { get; internal set; }
    }
}