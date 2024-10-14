using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CreditCustomer
{
    public class CheckSyncedNovaCasinoCriteria : CriteriaBase
    {
        public CheckSyncedNovaCasinoCriteria(string username)
        {
            Username = username;
        }

        public string Username { get; }

        public bool IsSynced { get; set; }

        public override string GetSettingKey() => "Acc_CustomerCheckSynced";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(Username);
    }
}