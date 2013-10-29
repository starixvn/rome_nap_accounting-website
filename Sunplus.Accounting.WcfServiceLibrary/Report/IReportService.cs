using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.Report;
using Sunplus.Accounting.Entities.Report.CheckBet;
using Sunplus.Accounting.Entities.Report.MatchWL;
using Sunplus.Accounting.Entities.Report.MemberStatistics;
using Sunplus.Accounting.Entities.Report.Mixparlay;
using Sunplus.Accounting.Entities.ViewModel;

namespace Sunplus.Accounting.WcfServiceLibrary.Report
{
    [ServiceContract]
    public interface IReportService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<MenuReport> GetMenuList(string system);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MatchWL2aDTO SelMatchWL2a(string wldate, int currpage);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        WLbyBettypeDTO SelWLbyBettype(string wldate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MixparlayDataGridBO GetMixparlayReport(bool isSoccer, string date, int pageIndex, int pageSize, bool fromcache, int sortType);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MixparlayDataGridBO GetCombinationMixparlayReport(bool isSoccer, string date, int pageIndex, int pageSize, bool fromcache);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void MixparlayWonExportToExcel(bool isSoccer, string date, int sortType);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void CombinationMixparlayWonExportToExcel(bool isSoccer, string date);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        CheckBet GetCheckBet(string fromDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<GamesActiveMembers> GetNumberOfActiveMembers(string fromDate, string toDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MatchWLBySportDTO SelMatchWLBySport(string wldate, int currIndex, int currIndexBtype);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MatchWLFooterDTO SelMatchWl3RdFooter(string wldate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        MixParlayView GetMixParlayDetailDailyWl(string wldate, int bettype);
    }
}