using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Entities.Criteria.Customer
{
    public class SportbookCommissionUpdateCriteria : CriteriaBase
    {
        public string ListUserName { get; set; }

        public string Discount { get; set; }

        public int UserID { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerCommissionSpecAccUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
