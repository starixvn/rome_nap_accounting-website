using System.Collections.Generic;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;

namespace Sunplus.Accounting.DataAccess.IAdmin
{
    public interface IExternalSiteDAO
    {
        IEnumerable<string> GetGroupNames();

        IEnumerable<ExternalSiteSuper> GetSuperList(string groupName);

        Result AddSuperToGroup(string username, string groupName, int userId);

        Result RemoveSuperFromGroup(int custId, int userId);

        IEnumerable<ExternalSiteExtraPTGroup> GetExtraPTGroups(int custId);
    }
}