using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.Requests.Customer;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class EditCustomerViewModel : CustomerBaseViewModel
    {
        public EditCustomerViewModel(
            EditCustomerRequest request,
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion)
            : base(appConfig, cndUri, assetsVersion)
        {
            Params = request.ToJson();
            switch (request.RoleId)
            {
                case RoleIdConstants.SUPER:
                    PageTitle = $"Super {request.UName}";
                    break;

                case RoleIdConstants.MASTER:
                    PageTitle = $"Master {request.UName}";
                    break;

                case RoleIdConstants.AGENT:
                    PageTitle = $"Agent {request.UName}";
                    break;

                default:
                    PageTitle = $"{request.UName}";
                    break;
            }
        }

        public string PageTitle { get; }
    }
}