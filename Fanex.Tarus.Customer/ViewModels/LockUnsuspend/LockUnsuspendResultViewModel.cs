using Sunplus.Accounting.Core.Common;

namespace Fanex.Tarus.Customer.ViewModels.LockUnsuspend
{
    public class LockUnsuspendResultViewModel
    {
        public string UserName { get; set; }

        public int RoleId { get; set; }

        public string Level => AccCommon.GetRoleName(RoleId);

        public string Status { get; set; }
    }
}
