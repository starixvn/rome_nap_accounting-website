using System.Data;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.ICreditTermDAO
{
    public interface ICreditTermGroupDAO
    {
        DataTable GetAllGroup();

        Result UpdateConsolidationReportAllowStatus(int custId, bool isViewReport, int userID);
    }
}