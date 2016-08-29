using Fanex.CustomerSetting.Module.Core.Models;

namespace Sunplus.Accounting.DataAccess.NG2
{
    public interface ISyncStatusRepository
    {
        Result SyncStatus(int custId, int userId, string ip);

        Result EnableStatus(int custId, int userId, string ip);
    }
}