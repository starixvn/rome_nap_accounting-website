namespace Fanex.Tarus.Customer.Criteria.ManageRobots2
{
    using Fanex.NAP.Framework.Data;

    public class CheckIsRobotCriteria : CriteriaBase
    {
        public CheckIsRobotCriteria(string custIds)
        {
            CustIds = custIds;
        }

        public string CustIds { get; }

        public override string GetSettingKey() => "Acc_CustomerRobotStatus_Sel";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(CustIds);
    }
}