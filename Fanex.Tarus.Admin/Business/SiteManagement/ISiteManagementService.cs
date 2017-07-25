using Fanex.Tarus.Admin.Criteria.SiteManagement;
using Fanex.Tarus.Admin.Entities;

namespace Fanex.Tarus.Admin.Business.SiteManagement
{
    public interface ISiteManagementService
    {
        bool ValidateSite(ValidateSiteCriteria criteria);

        Result AddSite(AddSiteCriteria criteria);
    }
}
