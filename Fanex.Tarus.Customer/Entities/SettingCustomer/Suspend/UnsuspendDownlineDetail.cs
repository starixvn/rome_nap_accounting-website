namespace Fanex.Tarus.Customer.ViewModels.CustomerSetting.Suspend
{
    using Sunplus.Accounting.Core.Common;

    public class UnsuspendDownlineDetail
    {
        public UnsuspendDownlineDetail(int roleId, string userName)
        {
            RoleId = roleId;
            UserName = userName;
        }

        public int RoleId { get; }

        public string LevelName
        {
            get
            {
                return AccCommon.GetRoleName(RoleId);
            }
        }

        public string UserName { get; }
    }
}
