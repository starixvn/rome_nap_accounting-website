using Fanex.Tarus.Business.Interface.Customer;
using Fanex.Tarus.DataAccess.Implement.Customer;
using Fanex.Tarus.DataAccess.Interface.Customer;
using Fanex.Tarus.Entities.Criteria.Customer;
using Fanex.Tarus.Entities.Customer;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.Business.Implement.Customer
{
    public class SportbookCommissionService: ISportbookCommissionService
    {
        private ISportbookCommissionRepository sportbookCommissionRepository;

        public SportbookCommissionService()
        {
            sportbookCommissionRepository = new SportbookCommissionRepository();
        }

        public Result CheckMembers(SportbookCommissionCheckUsersCriteria criteria)
        {
            criteria.ListUserName = RefineListUserName(criteria.ListUserName);
            var result = sportbookCommissionRepository.CheckMembers(criteria);
            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = criteria.ListUserName;
            }

            return result;
        }

        public EditSportbookCommissionViewModel GetSporttypeBettype(SportbookCommissionSporttypeBettypeSelectCriteria criteria)
        {
            return sportbookCommissionRepository.GetSporttypeBettype(criteria);
        }

        public Result UpdateCommission(SportbookCommissionUpdateCriteria criteria)
        {
            criteria.ListUserName = RefineListUserName(criteria.ListUserName);
            return sportbookCommissionRepository.UpdateCommission(criteria);
        }

        private string RefineListUserName(string listUserName)
        {
            listUserName = listUserName.Replace(" ", string.Empty);
            listUserName = listUserName.Replace("\n", ",");
            listUserName = listUserName.Replace(",,", ",").Trim(',');
            return listUserName;
        }
    }
}
