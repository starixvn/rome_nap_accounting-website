namespace Fanex.Tarus.Customer.DangerousGrouping.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class GetCustomerDangerCriteria : CriteriaBase
    {
        public GetCustomerDangerCriteria(string username, int userId)
        {
            UserId = userId;
            Username = username;
        }

        public int UserId { get; }

        public string Username { get; }

        public override string GetSettingKey()
            => "Acc_CustomerSelDangerCust";

        public override bool IsValid() => true;
    }
}