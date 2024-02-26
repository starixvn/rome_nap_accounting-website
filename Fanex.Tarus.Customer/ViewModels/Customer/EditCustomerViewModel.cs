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
            PageTitle = BuildPageTilte(request);
        }

        public string PageTitle { get; }

        private string BuildPageTilte(EditCustomerRequest request)
        {
            if (request.RoleId == RoleIdConstants.SUPER)
            {
                return $"Super {request.UName}";
            }

            if (request.RoleId == RoleIdConstants.MASTER)
            {
                return $"Master {request.UName}";
            }

            if (request.RoleId == RoleIdConstants.AGENT)
            {
                return $"Agent {request.UName}";
            }

            if (request.RoleId == RoleIdConstants.MEMBER)
            {
                if (request.UplineRoleId == RoleIdConstants.SUPER)
                {
                    return "Edit Super On Member";
                }

                if (request.UplineRoleId == RoleIdConstants.MASTER)
                {
                    return "Edit Master On Member";
                }

                return $"Member {request.UName}";
            }

            return request.UName;
        }
    }
}