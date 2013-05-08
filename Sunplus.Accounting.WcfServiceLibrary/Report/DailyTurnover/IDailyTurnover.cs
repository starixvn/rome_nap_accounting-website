using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using Sunplus.Accounting.Entities.Report.DailyTurnover;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.DailyTurnover
{
    [ServiceContract]
    public interface IDailyTurnover
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<DailyTurnoverDTO> SelDailyTurnover(string date);

        void DailyTurnoverExportExcel(string date);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<ActionTurnover> SelDetailTurnover(string date);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<ActionDetail> SelDetailActionTurnOver(string date, string action, string username);

        void DetailTurnoverExportExcel(string date);


        void DetailActionTurnoverExportExcel(string datetime, string action, string username);
    }

   
}
