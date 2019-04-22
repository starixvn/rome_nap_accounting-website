namespace Fanex.Tarus.Customer.Criteria.Suspend
{
    using Fanex.NAP.Framework.Data;

    public class GetMultipleSuspendCustomerInfoCriteria : CriteriaBase
    {
        public GetMultipleSuspendCustomerInfoCriteria(string custIdList, int roleId)
        {
            CustIdList = custIdList;

        }

        public string CustIdList { get; }

        public int RoleId { get; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerList_SuspendInfoSel";
        }

        public override bool IsValid()
        {
            return string.IsNullOrWhiteSpace(CustIdList);
        }
    }
}
