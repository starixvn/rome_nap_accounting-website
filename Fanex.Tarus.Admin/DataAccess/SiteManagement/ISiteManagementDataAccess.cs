using Fanex.Tarus.Admin.Criteria.SiteManagement;
using Fanex.Tarus.Admin.Entities;

namespace Fanex.Tarus.Admin.DataAccess.SiteManagement
{
    public interface ISiteManagementDataAccess
    {
        Result ExecuteNonQueryWithResult(CriteriaBase criteria);
    }
}
