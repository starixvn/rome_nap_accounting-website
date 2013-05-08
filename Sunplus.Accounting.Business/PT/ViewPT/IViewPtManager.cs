using Sunplus.Accounting.Entities.Customer.PT.ViewPT;

namespace Sunplus.Accounting.Business.PT.ViewPT
{
    public interface IViewPtManager
    {
        SportDTO GetPtSportType(string userName);
        BettypeDTO GetPtBetType(int sportId);
        ViewPtDTO ViewPositionTaking(string userName, int sportId, int bettypeId, bool isLive);
        ViewCommissionDTO ViewCommission(string userName);
    }
}
