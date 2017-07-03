using Fanex.Tarus.Admin.Criteria.SiteManagement;
using Fanex.Tarus.Admin.DataAccess.SiteManagement;
using Fanex.Tarus.Admin.Entities;

namespace Fanex.Tarus.Admin.Business.SiteManagement
{
    public class SiteManagementService : ISiteManagementService
    {
        private readonly ISiteManagementDataAccess siteManagementDataAccess;

        public SiteManagementService(ISiteManagementDataAccess siteManagementDataAccess = null)
        {
            this.siteManagementDataAccess = siteManagementDataAccess ?? new SiteManagementDataAccess();
        }

        public bool ValidateSite(ValidateSiteCriteria criteria)
        {
            var result = siteManagementDataAccess.ExecuteNonQueryWithResult(criteria);

            return result.ErrorCode == 0;
        }

        public Result AddSite(AddSiteCriteria criteria)
        {
            return siteManagementDataAccess.ExecuteNonQueryWithResult(criteria);
        }
    }
}
