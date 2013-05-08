using Sunplus.Accounting.Entities.PositionTaking.Poker;
using DNAide.Data;
using Sunplus.Accounting.Entities;
using System.Collections.Generic;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.Business.PT.Poker
{
    public interface IPokerManager
    {
        PokerStatus GetPokerStatus(int custId);
        PokerSetting GetPokerSetting(int custId, int roleId);
        Result UpdatePoker(PokerSetting setting, int type);
        Result UpdatePokerGameType(GameTypeSetting gameSetting, CustDetail custDetail, int type);
        Result UpdatePokerStatus(PokerStatus status);

        DefaultSetting GetPokerDefaultSetting();
        Result UpdatePokerDefaultSetting(DefaultSettingItem item);
        Result UpdatePokerDefaultSettingMultiple(List<DefaultSettingItem> items);
        Result CheckEnableCustomer(int custId);
    }
}