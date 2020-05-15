using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.Player3rd
{
    public class SearchPlayer3rdCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string Permission { get; set; }

        public int ProductId { get; set; }

        public string PlayerIDs { get; set; }

        public string Usernames { get; set; }

        public override bool IsValid()
            => ProductId > 0
                && (!string.IsNullOrWhiteSpace(PlayerIDs) || !string.IsNullOrWhiteSpace(Usernames))
                && UserId > 0;

        public override string GetSettingKey()
            => "Acc_CustomerSelPlayerId";
    }
}
