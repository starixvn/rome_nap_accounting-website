namespace Fanex.Tarus.Customer.Criteria.ManageRobots2
{
    using Fanex.NAP.Framework.Data;

    public class CheckIsRobotCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool IsRobot { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerRobotStatus_Sel";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
