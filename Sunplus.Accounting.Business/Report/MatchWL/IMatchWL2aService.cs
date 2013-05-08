using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Entities.Report.MatchWL;
using Sunplus.Accounting.Entities.SpecialDelete;

namespace Sunplus.Accounting.Business.Report.MatchWL
{
    [ServiceContract]
    public interface IMatchWL2aService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MatchWL2aDTO SelMatchWL2a(DateTime wldate, int currpage);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        ExcelFile ExportMatchWL(ref bool hasData);

    }
}
