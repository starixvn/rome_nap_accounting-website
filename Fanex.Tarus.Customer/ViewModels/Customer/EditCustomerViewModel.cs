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
        }
    }
}