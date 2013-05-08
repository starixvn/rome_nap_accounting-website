using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.DataAccess.Interface
{
    using Fanex.Tarus.Entities;

    using Result = Sunplus.Accounting.Entities.Result;

    public interface IAdminAccess
    {
        List<AgentAccess> GetListAgentAcess();
        Result AddUser(string userName, int creator, string remark);
        Result Update(int custId, bool isAllowed, string remark);
        Result Delete(int custId);
    }
}
