namespace Sunplus.Accounting.WcfServiceLibrary.PT.ViewPT
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using Sunplus.Accounting.Entities.Customer.PT.ViewPT;

    [ServiceContract]
    public interface IViewPTService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        SportDTO GetPtSportType(string userName);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        BettypeDTO GetPtBetType(int sportId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ViewPtDTO ViewPositionTaking(string userName, int sportId, int bettypeId, bool isLive);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ViewCommissionDTO ViewCommission(string userName);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        IList<DCSProduct> GetProducts();
    }
}