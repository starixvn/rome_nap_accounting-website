using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Security.Criteria
{
    public class CheckSmartPunterCriteria : CriteriaBase
    {
        public int? CustId { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }

        public bool IsRestricted { get; set; }

        public int ActionId { get; set; }

        public string Ip { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelCheckSmartPunter";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
