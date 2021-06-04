namespace Fanex.Tarus.Customer.Criteria.OpenCustomer
{
    using Fanex.NAP.Framework.Data;

    public class GetCloseInfoMatrixCriteria : CriteriaBase
    {
        public GetCloseInfoMatrixCriteria(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; }

        public override string GetSettingKey()
        {
            return "Acc_Customer_CloseInfoMatrixSel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(UserName);
        }
    }
}