using System;
using System.Collections.Generic;
using Sunplus.Accounting.Entities.Report.MemberStatistics;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Sunplus.Accounting.Business.Report.MemberStatistics
{
    public interface IGamesActiveMembersManager
    {
        List<GamesActiveMembers> GetNumberOfActiveMembers(string fromDate, string toDate);
        void NumberOfActiveMembersExportExcel(string fromDate, string toDate);
    }
}
