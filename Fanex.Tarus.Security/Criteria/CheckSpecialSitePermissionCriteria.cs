using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Security.Criteria
{
    public class CheckSpecialSitePermissionCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        public int? CustID { get; set; }

        public string CustName { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerIsAllowedAccess";
        }
    }
}
