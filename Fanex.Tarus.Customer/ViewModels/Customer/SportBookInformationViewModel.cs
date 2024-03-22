namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    using Fanex.Tarus.Customer.Requests.Customer;

    public class SportBookInformationViewModel : CustomerBaseViewModel
    {
        public SportBookInformationViewModel(
            SportBookInformationRequest query,
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion) : base(appConfig, cndUri, assetsVersion)
        {
            Params = query.ToJson();
        }
    }
}