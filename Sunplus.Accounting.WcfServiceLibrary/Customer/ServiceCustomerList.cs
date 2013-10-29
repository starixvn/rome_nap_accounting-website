using System.ServiceModel.Activation;

namespace Sunplus.Accounting.WcfServiceLibrary.Customer
{
    /// <summary>
    /// Summary description for ServiceCustomerList
    /// </summary>
    ///
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceCustomerList : IServiceCustomerList
    {
        public string GenSuperListContent(string searchText, string selectAlpha, string selectCurreny, int type, string status, int PageIndex)
        {
            var customerService = new Sunplus.Accounting.Business.ServiceCustomerList();
            return customerService.GenSuperListContent(searchText, selectAlpha, selectCurreny, type, status, PageIndex);
        }

        public string GenMasterListContent(string superId, string hr, string searchText, string selectAlpha, string selectCurreny, int type, string status, int PageIndex, string synccasinoUpline)
        {
            var customerService = new Sunplus.Accounting.Business.ServiceCustomerList();
            return customerService.GenMasterListContent(superId, hr, searchText, selectAlpha, selectCurreny, type, status, PageIndex, synccasinoUpline);
        }

        public string GenAgentListContent(string masterId, string hr, int PageIndex, string syncCasinoUpline)
        {
            var customerService = new Sunplus.Accounting.Business.ServiceCustomerList();
            return customerService.GenAgentListContent(masterId, hr, PageIndex, syncCasinoUpline);
        }

        public string GenMemberListContent(string agentId, string hr, string isInternal, int PageIndex, string isSynccasino)
        {
            var customerService = new Sunplus.Accounting.Business.ServiceCustomerList();
            return customerService.GenMemberListContent(agentId, hr, isInternal, PageIndex, isSynccasino);
        }

        public string GenOperatorListContent(string searchText, string selectAlpha, int PageIndex)
        {
            var customerService = new Sunplus.Accounting.Business.ServiceCustomerList();
            return customerService.GenOperatorListContent(searchText, selectAlpha, PageIndex);
        }
    }
}