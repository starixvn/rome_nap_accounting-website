namespace Fanex.Tarus.Customer.Criteria.LicenseeMember
{
    using Fanex.NAP.Framework.Data;

    public class CheckExistAgentCriteria : CriteriaBase
    {
        public CheckExistAgentCriteria(string username)
        {
            Username = username;
            RoldId = 2;
        }

        public string Username { get; set; }

        public int RoldId { get; set; }

        public int CustId { get; set; }

        public override string GetSettingKey()
            => "Acc_Dep_CheckExistedAccount";
    }
}
