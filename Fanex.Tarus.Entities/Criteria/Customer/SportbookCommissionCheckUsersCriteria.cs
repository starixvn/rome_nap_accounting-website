using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Entities.Criteria.Customer
{
    public class SportbookCommissionCheckUsersCriteria : CriteriaBase
    {
        public string ListUserName { get; set; }

        public SportbookCommissionCheckUsersCriteria()
        {
            ListUserName = string.Empty;
        }

        public override string GetSettingKey()
        {
            return "Acc_CustomerNoCommCheck";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
