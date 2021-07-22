namespace Fanex.Tarus.Customer.Entities.SettingCustomer.OpenCustomer
{
    using Sunplus.Accounting.Core.Common;

    public class SingleOpenDownline
    {
        public SingleOpenDownline(int roleId, string userName)
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