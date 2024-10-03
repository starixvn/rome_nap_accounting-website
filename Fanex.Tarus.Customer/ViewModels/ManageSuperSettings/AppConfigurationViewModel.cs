namespace Fanex.Tarus.Customer.ViewModels.ManageSuperSettings
{
    public class AppConfigurationViewModel
    {
        public AppConfigurationViewModel(string appPath, int systemId)
        {
            ApplicationPath = appPath;
            SystemId = systemId;
        }

        public string ApplicationPath { get; }

        public int SystemId { get; }
    }
}
