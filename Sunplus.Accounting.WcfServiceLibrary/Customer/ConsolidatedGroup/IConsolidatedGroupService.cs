using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Accounting.Entities.Customer.ConsolidatedGroup;
using System.ServiceModel.Web;
using System.ServiceModel;

namespace Sunplus.Accounting.WcfServiceLibrary.Customer.ConsolidatedGroup
{
    [ServiceContract]
    public interface IConsolidatedGroupService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<ConsolidatedGroupTable> GetAllGroup();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Sunplus.Accounting.Entities.Result UpdateGroup(string GroupID, string UserName, bool isHeader, string Remark);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<ConsolidatedMemberTable> GetMemberByGroupID(int GroupID);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Sunplus.Accounting.Entities.Result AddUserToGroup(int GroupID, string UserName);


        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Sunplus.Accounting.Entities.Result RemoveUsers(int GroupID, string CustIDList);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Sunplus.Accounting.Entities.Result ChangeHeadline(int GroupID, int CustID, string CustName);
    }
}
