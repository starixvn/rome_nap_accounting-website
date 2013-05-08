using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.BetAggregator;

namespace Sunplus.Accounting.WcfServiceLibrary.BetAggregator
{
     [ServiceContract]
    public interface ILicenseeConfigService
    {
         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         int CheckDownlineBA(int licenseeid);

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         List<LicenseeConfig> SelAllLicensee();

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         List<LicenseeConfig> SelBaLicenseeConfig();
         
         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         List<LicenseeLog> SelBaLicenseeLogList(int licenseeId);

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         Result InsBALicenseeConfig(int licenseeId, double hdpComm, double oneXTwoComm, double otherComm, double gameComm, bool enableBA);

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         Result UpdBALicenseeConfig(int licenseeId, double groupa, double groupb, double groupc, double groupd, double oneXTwoComm, double otherComm, bool enableBA);
         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         Result DelBALicenseeConfig(int licenseeId);

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         BACurrenyGroupDTO SelBACurrencyGroupBy(int licenseeId);

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         Result BAUpdateGroupSetting(int licenseeId, string setting);
    }
}
