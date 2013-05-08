using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Sunplus.Accounting.Entities.Report.MatchAnalysis;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.MatchAnalysis
{
    [ServiceContract]
    public interface IMatchAnalysisService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MatchAnalysisBO SelMatchAnalysisByType(int matchId, int statusId, int reportType);

       
    }
}
