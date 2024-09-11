namespace Fanex.Tarus.Customer.DangerousGrouping.Criterias
{
    using Fanex.NAP.Framework.Data;

    public class GetCustomerDangerCriteria : CriteriaBase
    {
        public GetCustomerDangerCriteria(string username, int userId, string adName, string permission, string deniedList)
        {
            UserId = userId;
            Username = username;
            AdName = adName;
            Permission = permission;
            DeniedList = deniedList;
        }

        public int UserId { get; }

        public string AdName { get; }

        public string Username { get; }

        public string Permission { get; }

        public string DeniedList { get; }

        public override string GetSettingKey()
            => "Acc_CustomerSelDangerCust";

        public override bool IsValid() => true;
    }
}