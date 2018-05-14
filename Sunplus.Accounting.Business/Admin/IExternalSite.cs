using System.Collections.Generic;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;

namespace Sunplus.Accounting.Business.Admin
{
    public interface IExternalSite
    {
        IEnumerable<string> GetGroupNames();

        ExternalSiteGridBO GetSuperList(string groupName, string username, bool fromcache, int pageIndex, int pageSize);

        IEnumerable<Result> AddSuperToGroup(string username, string groupName);

        Result RemoveSuperFromGroup(int custId, string groupName);

        IEnumerable<ExternalSiteExtraPTGroup> GetExtraPTGroups(int custId);

        Result ChangePrimaryGroup(int custId, string groupName, string effectiveDate);
    }
}