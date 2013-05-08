using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.PT.Racing;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.PositionTaking.Racing;

namespace Sunplus.Accounting.WcfServiceLibrary.PT.Racing
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class RacingService : IRacingService
    {
        public Setting GetCustomerStatus(int custId)
        {
            var racingService = CommonBootStrapper.Locator.GetInstance<IRacingManager>();
            return racingService.GetStatusRacebook(custId);
        }
        public Setting GetCustomerSetting(int custId, int roleId)
        {
            var racingService = CommonBootStrapper.Locator.GetInstance<IRacingManager>();
            return racingService.GetCustomerSetting(custId, roleId);
        }

        public Result UpdateCustomerSetting(Setting setting)
        {
            var racingService = CommonBootStrapper.Locator.GetInstance<IRacingManager>();
            return racingService.UpdateCustomerSetting(setting);
        }

        public Result UpdateCustomerStatus(Setting setting)
        {
            var racingService = CommonBootStrapper.Locator.GetInstance<IRacingManager>();
            return racingService.UpdateCustomerStatus(setting);
        }

        public List<Partner> GetPartnerList()
        {
            var objPartner = CommonBootStrapper.Locator.GetInstance<IMeetingCommManager>();
            return objPartner.GetPartnerList();
        }

        public List<PartnerItem> GetPartnerDetail(int partnerId)
        {
            var objPartner = CommonBootStrapper.Locator.GetInstance<IMeetingCommManager>();
            return objPartner.GetPartnerDetail(partnerId);
        }
    }
}
