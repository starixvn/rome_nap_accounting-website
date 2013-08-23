using System;
using System.ServiceModel;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Entities.Report.MatchWL;

namespace Sunplus.Accounting.Business.Report.MatchWL
{
    [ServiceContract]
    public interface IMatchWL2aServiceNEW
    {
        WLbyBettypeDTO SelWLbyBettype(DateTime wldate);

        MatchWLBySportDTO SelMatchWLBySport(DateTime wldate, int currIndex, int currIndexBtype);

        MatchWLFooterDTO SelMatchWl3RdFooter(DateTime wldate);

        ExcelFile ExportMatchWL(ref bool hasData);
    }
}