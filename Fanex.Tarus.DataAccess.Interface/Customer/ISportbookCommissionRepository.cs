using Fanex.Tarus.Entities.Criteria.Customer;
using Fanex.Tarus.Entities.Customer;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.DataAccess.Interface.Customer
{
    public interface ISportbookCommissionRepository
    {
        Result CheckMembers(SportbookCommissionCheckUsersCriteria criteria);

        EditSportbookCommissionViewModel GetSporttypeBettype(SportbookCommissionSporttypeBettypeSelectCriteria criteria);

        Result UpdateCommission(SportbookCommissionUpdateCriteria criteria);
    }
}
