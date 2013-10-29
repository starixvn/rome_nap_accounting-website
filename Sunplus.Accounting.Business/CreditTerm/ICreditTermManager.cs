using System.Collections.Generic;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.CreditTerm;

namespace Sunplus.Accounting.Business.CreditTerm
{
    public interface ICreditTermManager
    {
        List<Group> GetAllGroup();

        Result UpdateConsolidationReportAllowStatus(int custId, bool isViewReport);

        void ExportExcelAllGroup(string searchKey, string sortOrder, string sortName);

        List<GroupEditing> GetMemberByGroupId(int groupId);

        void ExportExcelMemberByGroupId(int groupId);
    }
}