using Fanex.Tarus.Security.Criteria;
using Fanex.Tarus.Security.Repositories;

namespace Fanex.Tarus.Security.Services
{
    public class SmartPunterService : ISmartPunterService
    {
        private readonly ISmartPunterRepository _smartPunterRepository;

        private string _smartPunterFunctionName = "SmartPunter";

        public SmartPunterService() : this(new SmartPunterRepository())
        {
        }

        public SmartPunterService(ISmartPunterRepository smartPunterRepository)
        {
            _smartPunterRepository = smartPunterRepository;
        }

        public bool IsDeniedViewSmartPunter(CheckSmartPunterCriteria criteria)
        {
            bool isSmartPunter = _smartPunterRepository.CheckSmartPunter(criteria);

            if (PermissionService.HasPermission(_smartPunterFunctionName))
            {
                return false;
            }

            return isSmartPunter;
        }

        public void TryLogIfSmartPunter(CheckSmartPunterCriteria criteria)
        {
            _smartPunterRepository.CheckSmartPunter(criteria);
        }
    }
}