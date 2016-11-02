using Fanex.Tarus.Security.Criteria;

namespace Fanex.Tarus.Security.Repositories
{
    public interface ISmartPunterRepository
    {
        bool CheckSmartPunter(CheckSmartPunterCriteria checkSmartPunterCriteria);
    }
}
