using System.Collections.Generic;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.Admin;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;

namespace Sunplus.Accounting.WcfServiceLibrary.Admin
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExternalSiteService : IExternalSiteService
    {
        private readonly IExternalSite _externalSiteService;

        public ExternalSiteService()
        {
            _externalSiteService = CommonBootStrapper.Locator.GetInstance<IExternalSite>();
        }

        public IEnumerable<string> GetGroupNames()
        {
            return _externalSiteService.GetGroupNames();
        }

        public ExternalSiteGridBO GetSuperList(string groupName, string username, bool fromcache, int pageIndex, int pageSize)
        {
            return _externalSiteService.GetSuperList(groupName, username, fromcache, pageIndex, pageSize);
        }

        public IEnumerable<Result> AddSuperToGroup(string username, string groupName)
        {
            return _externalSiteService.AddSuperToGroup(username, groupName);
        }

        public Result RemoveSuperFromGroup(int custId, string groupName)
        {
            return _externalSiteService.RemoveSuperFromGroup(custId, groupName);
        }

        public IEnumerable<ExternalSiteExtraPTGroup> GetExtraPTGroups(int custId)
        {
            return _externalSiteService.GetExtraPTGroups(custId);
        }

        public Result ChangePrimaryGroup(int custId, string groupName, string effectiveDate)
        {
            return _externalSiteService.ChangePrimaryGroup(custId, groupName, effectiveDate);
        }
    }
}