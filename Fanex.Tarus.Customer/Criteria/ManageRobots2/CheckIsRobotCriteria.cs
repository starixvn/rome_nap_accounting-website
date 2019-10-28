namespace Fanex.Tarus.Customer.Criteria.ManageRobots2
{
    using Fanex.NAP.Framework.Data;

    public class CheckIsRobotCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public bool IsRobot { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerRobotStatus_Sel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }
    }
}
