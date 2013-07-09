using System.Collections.Generic;
using Sunplus.Accounting.Entities.CreditTerm;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.Business.CreditTerm
{
    public interface ICreditTermManager
    {
        List<Group> GetAllGroup();
        Result UpdateConsolidationReportAllowStatus(int custId, bool isViewReport);
    }
}