using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Entities.Customer;
using System.Collections.Generic;

// NOTE: If you change the interface name "IServiceCustomerInfo" here, you must also update the reference to "IServiceCustomerInfo" in Web.config.
namespace Sunplus.Accounting.WcfServiceLibrary.Customer
{
    [ServiceContract]
    public interface IServiceCustomerInfo
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenDownlineList(int custId, int roleId, int pageIndex);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenOnlineList(int custId);

        //[OperationContract]
        //[WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        //string GenMenuMinMaxPT(int custId, int roleId, string currency, string userName, int agentId, int masterId, int superId );


        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        CustDetail GetCustomerInfo(string username);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UpdateCustomerSite(int custId, string newSite);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UpdateDangerousCustomerSer(int custId, int updateDownline, int dangerType, int dangerLevel);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Sunplus.Accounting.Entities.Customer.CustomerExRate SelectCustomerExRate(string username);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result CustomerCrossTransferInsert(int fromCustID, int toCustID, double fromAmount, double toAmount, double fromExRate, double toExRate, string forexUsername, string remark, int custid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<Sunplus.Accounting.Entities.Customer.ForexAccount> SelectAllForexAccount();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UpdBetSettingEnableNGCustomer(string username, int isEnable, string maxbet, string minbet, string maxperball, string maxpermatch);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        BetSettingNG SelBetSettingUpline(string username);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result AutoInsertThirdPartyAccount(CSCustomerDetail custInfo, CustDetail uplineInfo, int userId, string userName, string ip);
    }
}