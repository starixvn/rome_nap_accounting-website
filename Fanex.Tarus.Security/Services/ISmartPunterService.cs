using Fanex.Tarus.Security.Criteria;

namespace Fanex.Tarus.Security.Services
{
    public interface ISmartPunterService
    {
        bool IsDeniedViewSmartPunter(CheckSmartPunterCriteria criteria);

        void TryLogIfSmartPunter(CheckSmartPunterCriteria criteria);
    }
}