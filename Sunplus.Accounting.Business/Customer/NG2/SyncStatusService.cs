using Fanex.CustomerSetting.Module.Core.Models;
using Sunplus.Accounting.DataAccess.NG2;

namespace Sunplus.Accounting.Business.Customer.NG2
{
    public class SyncStatusService : ISyncStatusService
    {
        private ISyncStatusRepository _repository = new SyncStatusRepository();

        public Result EnableStatus(int custId, int userId, string ip)
        {
            return _repository.EnableStatus(custId, userId, ip);
        }

        public Result SyncStatus(int custId, int userId, string ip)
        {
            return _repository.SyncStatus(custId, userId, ip);
        }
    }
}