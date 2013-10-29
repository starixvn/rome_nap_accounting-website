using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.PT.ViewPT;
using Sunplus.Accounting.Entities.Customer.PT.ViewPT;

namespace Sunplus.Accounting.WcfServiceLibrary.PT.ViewPT
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ViewPTService : IViewPTService
    {
        public SportDTO GetPtSportType(string userName)
        {
            var ptService = CommonBootStrapper.Locator.GetInstance<IViewPtManager>();
            return ptService.GetPtSportType(userName);
        }

        public BettypeDTO GetPtBetType(int sportId)
        {
            var ptService = CommonBootStrapper.Locator.GetInstance<IViewPtManager>();
            return ptService.GetPtBetType(sportId);
        }

        public ViewPtDTO ViewPositionTaking(string userName, int sportId, int bettypeId, bool isLive)
        {
            var ptService = CommonBootStrapper.Locator.GetInstance<IViewPtManager>();
            return ptService.ViewPositionTaking(userName, sportId, bettypeId, isLive);
        }

        public ViewCommissionDTO ViewCommission(string userName)
        {
            var ptService = CommonBootStrapper.Locator.GetInstance<IViewPtManager>();
            return ptService.ViewCommission(userName);
        }
    }
}