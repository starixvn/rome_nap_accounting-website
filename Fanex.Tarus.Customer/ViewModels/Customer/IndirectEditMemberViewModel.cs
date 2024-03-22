using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.Requests.Customer;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class IndirectEditMemberViewModel : CustomerBaseViewModel
    {
        public IndirectEditMemberViewModel(
          IndirectEditMemberRequest request,
          AppConfiguration appConfig,
          string cndUri,
          string assetsVersion)
          : base(appConfig, cndUri, assetsVersion)
        {
            Params = request.ToJson();
            switch (request.UplineRoleId)
            {
                case RoleIdConstants.SUPER:
                    PageTitle = $"Edit Super On Member";
                    break;

                case RoleIdConstants.MASTER:
                    PageTitle = $"Edit Master On Member";
                    break;

                default:
                    PageTitle = "";
                    break;
            }
        }

        public string PageTitle { get; }
    }
}