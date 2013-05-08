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
    public interface IMatchWL2aServiceNEW
    {
        
        WLbyBettypeDTO SelWLbyBettype(DateTime wldate);

        
        MatchWLBySportDTO SelMatchWLBySport(DateTime wldate, int currIndex, int currIndexBtype);

        
        MatchWLFooterDTO SelMatchWl3RdFooter(DateTime wldate);

        
        ExcelFile ExportMatchWL(ref bool hasData);
    }
}
