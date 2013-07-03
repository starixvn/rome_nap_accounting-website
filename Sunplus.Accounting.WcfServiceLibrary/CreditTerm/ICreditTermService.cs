using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.CreditTerm;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.WcfServiceLibrary.CreditTerm
{
    [ServiceContract]
    public interface ICreditTermService
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<Group> GetAllGroup();

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UpdateConsolidationReportAllowStatus(int custId, bool isViewReport);
    }
}