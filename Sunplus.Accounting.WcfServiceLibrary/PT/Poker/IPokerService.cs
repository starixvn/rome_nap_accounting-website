using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Accounting.Entities.PositionTaking.Poker;

namespace Sunplus.Accounting.WcfServiceLibrary.PT.Poker
{
    [ServiceContract]
    public interface IPokerService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        PokerSetting GetPokerSetting(int custId, int roleId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        PokerStatus GetPokerStatus(int custId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        DefaultSetting GetPokerDefaultSetting();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UpdatePoker(PokerSetting setting, int type);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UpdatePokerStatus(PokerStatus status);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UpdatePokerGameType(GameTypeSetting gameSetting, CustDetail custDetail, int type);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UpdatePokerDefaultSetting(DefaultSettingItem item);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UpdatePokerDefaultSettingMultiple(List<DefaultSettingItem> items);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result CheckEnableCustomer(int custId);
    }
}