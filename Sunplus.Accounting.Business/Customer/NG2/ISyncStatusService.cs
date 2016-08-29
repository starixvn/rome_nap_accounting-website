using Fanex.CustomerSetting.Module.Core.Models;

namespace Sunplus.Accounting.Business.Customer.NG2
{
    public interface ISyncStatusService
    {
        Result SyncStatus(int custId, int userId, string ip);

        Result EnableStatus(int custId, int userId, string ip);
    }
}