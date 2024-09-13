namespace Fanex.Tarus.Customer.Criteria.LicenseeMember
{
    using Fanex.NAP.Framework.Data;

    public class CheckExistAccountCriteria : CriteriaBase
    {
        public CheckExistAccountCriteria(string username, int roleId, int custDBGroupId)
        {
            Username = username;
            CustDBGroupId = custDBGroupId;
            RoleId = roleId;
        }

        public string Username { get; set; }

        public int RoleId { get; set; }

        public int CustId { get; set; }

        [SpParam(Ignored = true)]
        public int CustDBGroupId { get; }

        public override string GetSettingKey()
        {
            if (CustDBGroupId == 0)
            {
                return "Acc_Dep_CheckExistedAccount";
            }

            return $"SiteDB{CustDBGroupId}_Acc_Dep_CheckExistedAccount";
        }
    }
}