using System.ServiceModel;
using System.ServiceModel.Web;
using ExcelUtil.Spreadsheet;

namespace Sunplus.Accounting.Business.ExportStatement
{
    /// <summary>
    /// Summary description for IServiceExportStatement
    /// </summary>
    [ServiceContract]
    public interface IServiceExportStatement
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string ReadUsers();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExcelFile Export2Excel(string userslistvalid, string fdate, string tdate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string AllStatement_UsersList(string userslistvalid, string fdate, string tdate);
    }
}