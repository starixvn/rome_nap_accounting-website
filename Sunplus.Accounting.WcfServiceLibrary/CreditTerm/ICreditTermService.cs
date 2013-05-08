using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.CreditTerm;

namespace Sunplus.Accounting.WcfServiceLibrary.CreditTerm
{
    [ServiceContract]
    public interface ICreditTermService
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<Group> GetAllGroup();
    }
}