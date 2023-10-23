namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class CustomerViewModel
    {
        public CustomerViewModel(string baseUrl, string cndUri, string assetsVersion)
        {
            BaseUrl = baseUrl;
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
        }

        public string BaseUrl { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }
    }
}