using Fanex.Tarus.Security.Criteria;
using Fanex.Tarus.Security.Repositories;

namespace Fanex.Tarus.Security.Services
{
    public class SmartPunterPermissionService : ISmartPunterPermissionService
    {
        private readonly ISmartPunterRepository _smartPunterRepository;

        private string _smartPunterFunctionName = "SmartPunter";

        public SmartPunterPermissionService()
            : this(new SmartPunterRepository())
        {
        }

        public SmartPunterPermissionService(ISmartPunterRepository smartPunterRepository)
        {
            _smartPunterRepository = smartPunterRepository;
        }

        public bool IsDeniedViewSmartPunter(int? custID, string userName)
        {
            bool isSmartPunter = _smartPunterRepository.CheckSmartPunter(new CheckSmartPunterCriteria
            {
                CustID = custID.HasValue ? custID : null,
                UserName = userName
            });

            if (PermissionService.HasPermission(_smartPunterFunctionName))
            {
                return false;
            }

            return isSmartPunter;
        }
    }
}