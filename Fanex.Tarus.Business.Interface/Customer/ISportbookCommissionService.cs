using Fanex.Tarus.Entities.Criteria.Customer;
using Fanex.Tarus.Entities.Customer;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.Business.Interface.Customer
{
    public interface ISportbookCommissionService
    {
        Result CheckMembers(SportbookCommissionCheckUsersCriteria criteria);

        EditSportbookCommissionViewModel GetSporttypeBettype(SportbookCommissionSporttypeBettypeSelectCriteria criteria);

        Result UpdateCommission(SportbookCommissionUpdateCriteria criteria);
    }
}
