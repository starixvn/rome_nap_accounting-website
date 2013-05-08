using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities.PositionTaking.Poker;
using Sunplus.Accounting.Business.PT.Poker;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.WcfServiceLibrary.PT.Poker
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PokerService : IPokerService
    {
        public PokerSetting GetPokerSetting(int custId, int roleId)
        {
            var pokerManager = CommonBootStrapper.Locator.GetInstance<IPokerManager>();
            var pokerSetting = pokerManager.GetPokerSetting(custId, roleId);
            return pokerSetting;
        }

        public PokerStatus GetPokerStatus(int custId)
        {
            var pokerManager = CommonBootStrapper.Locator.GetInstance<IPokerManager>();
            var pokerStatus = pokerManager.GetPokerStatus(custId);
            return pokerStatus;
        }

        public DefaultSetting GetPokerDefaultSetting()
        {
            var pokerManager = CommonBootStrapper.Locator.GetInstance<IPokerManager>();
            var pokerStatus = pokerManager.GetPokerDefaultSetting();
            return pokerStatus;
        }

        public Result UpdatePokerGameType(GameTypeSetting gameSetting, CustDetail custDetail, int type)
        {
            var result = new Result();
            var pokerManager = CommonBootStrapper.Locator.GetInstance<IPokerManager>();

            result = pokerManager.UpdatePokerGameType(gameSetting, custDetail , type);
           
            return result;
        }
        public Result CheckEnableCustomer(int custId)
        { 
            var result = new Result();
            var pokerManager = CommonBootStrapper.Locator.GetInstance<IPokerManager>();
            result = pokerManager.CheckEnableCustomer(custId);
            return result;
        }
        public Result UpdatePoker(PokerSetting setting, int type)
        {
            var result = new Result();
            var pokerManager = CommonBootStrapper.Locator.GetInstance<IPokerManager>();
            //if (type == 2)
            //{
            //    for (int i = 0; i < setting.GameSettings.Count; i++)
            //    {
            //        var r = pokerManager.UpdatePoker(setting, type);
            //        foreach (KeyValuePair<int, string> msg in r.ErrorHandler)
            //        {
            //            result.ErrorHandler.Add(msg.Key, msg.Value);
            //        }
            //    }
            //}
            //else
            //{
                result = pokerManager.UpdatePoker(setting, type);
            //}
            return result;
        }

        public Result UpdatePokerStatus(PokerStatus status)
        {
            var pokerManager = CommonBootStrapper.Locator.GetInstance<IPokerManager>();
            var result = pokerManager.UpdatePokerStatus(status);
            return result;
        }

        public Result UpdatePokerDefaultSetting(DefaultSettingItem item)
        {
            var pokerManager = CommonBootStrapper.Locator.GetInstance<IPokerManager>();
            var result = pokerManager.UpdatePokerDefaultSetting(item);
            return result;
        }

        public Result UpdatePokerDefaultSettingMultiple(List<DefaultSettingItem> items)
        {
            var pokerManager = CommonBootStrapper.Locator.GetInstance<IPokerManager>();
            var result = pokerManager.UpdatePokerDefaultSettingMultiple(items);
            return result;
        }
    }
}
