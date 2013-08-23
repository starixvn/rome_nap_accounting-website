using System.ServiceModel.Activation;

namespace Sunplus.Accounting.WcfServiceLibrary.Customer
{
    /// <summary>
    /// Summary description for ServiceCustomerList
    /// </summary>
    ///
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceSuspend : IServiceSuspend
    {
        public string GenSuspendListContent(string searchText, string selectAlpha, int PageIndex)
        {
            var customerService = new Sunplus.Accounting.Business.ServiceSuspend();
            return customerService.GenSuspendListContent(searchText, selectAlpha, PageIndex);
        }
    }
}