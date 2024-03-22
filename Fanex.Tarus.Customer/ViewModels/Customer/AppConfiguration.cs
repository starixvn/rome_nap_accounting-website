namespace Fanex.Tarus.Customer.ViewModels.Customer
{
    public class AppConfiguration
    {
        public AppConfiguration(string appPath, int systemId = 1)
        {
            ApplicationPath = appPath;
            SystemId = systemId;
        }

        public string ApplicationPath { get; }

        public int SystemId { get; }
    }
}