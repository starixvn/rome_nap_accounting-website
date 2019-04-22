namespace Fanex.Tarus.Customer.Criteria.Suspend
{
    using Fanex.NAP.Framework.Data;

    public class GetSuspendInfoMatrixCriteria : CriteriaBase
    {
        public GetSuspendInfoMatrixCriteria(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }

        public override string GetSettingKey()
        {
            return "Acc_Customer_SuspendInfoMatrixSel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(UserName);
        }
    }
}
