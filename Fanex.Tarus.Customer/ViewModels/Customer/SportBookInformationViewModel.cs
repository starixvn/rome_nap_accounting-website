namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    
    public class SportBookInformationViewModel
    {
        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string PageTitle { get; }
        
        public SportBookInformationViewModel(
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion
        )
        {
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
            AppConfig = JsonConvert.SerializeObject(appConfig, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            PageTitle = "Sport Book Information";
        }
    }
}