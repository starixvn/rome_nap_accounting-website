using System;
using Sunplus.Accounting.Entities.Report.Others;

namespace Sunplus.Accounting.Business.Report.Other
{
    public interface INumberOfBetsManager
    {
        NumberOfBetsDTO SelectNumberOfBetsForCreditSite(DateTime fromDate, DateTime toDate);
        void ExportExcelNumberOfBetsForCreditSites(DateTime fromDate, DateTime toDate);
    }
}
