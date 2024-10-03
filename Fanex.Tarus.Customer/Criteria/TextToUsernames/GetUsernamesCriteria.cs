using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.TextToUsernames
{
    public class GetUsernamesCriteria : CriteriaBase
    {
        public GetUsernamesCriteria(string username, int userId, string deniedList)
        {
            Username = username;
            UserId = userId;
            DeniedList = deniedList;
        }

        public string Username { get; set; }

        public string DeniedList { get; set; }

        public int UserId { get; set; }

        public override string GetSettingKey()
            => "Acc_CustomerUsername_Split";

        public override bool IsValid()
            => UserId > 0;
    }
}
