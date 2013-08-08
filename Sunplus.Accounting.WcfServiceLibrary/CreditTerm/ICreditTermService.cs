using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.CreditTerm;

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

		[OperationContract]
		[WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
		void ExportAllGroup(string searchKey, string sortOrder, string sortName);

		[OperationContract]
		[WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
		List<GroupEditing> GetMemberByGroupId(int groupId);

		[OperationContract]
		void ExportExcelMemberByGroupId(int groupId);
    }
}