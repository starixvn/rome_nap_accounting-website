using Fanex.Tarus.Customer.Requests.Customer;

namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class CreateDirectMemberViewModel : CreateCustomerViewModel
    {
        public CreateDirectMemberViewModel(
            CreateDirectMemberRequest request,
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion,
            string secretKey)
            : base(request, appConfig, cndUri, assetsVersion, secretKey)
        {
            PageTitle = "Add New Direct Member";
        }
    }
}