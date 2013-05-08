using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Sunplus.Accounting.Entities.Report.External;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.External
{
    [ServiceContract]  
    public interface IExternalReportService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExternalWLDailyBO SelWinLossDailyByCom(string winlostdate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExternalWLDailyBO SelWinLossDailyBySuper(string winlostdate, int custid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExternalWLDailyBO SelWinLossDailyByMaster(string winlostdate, int custid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExternalWLDailyBO SelWinLossDailyByAgent(string winlostdate, int custid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExternalWLDetail SelWinLossDetailByCom(string param, Sorting sorting);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExternalWLDetail SelWinLossDetailBySuper(int custid, string param, Sorting sorting, int pageIndex, int pageSize);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExternalWLDetail SelWinLossDetailByMaster(int custid, string param, Sorting sorting, int pageIndex, int pageSize);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExternalWLDetail SelWinLossDetailByAgent(int custid, string param, Sorting sorting, int pageIndex, int pageSize);
    }
}
