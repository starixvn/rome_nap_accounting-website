namespace Fanex.Tarus.Customer.Entities.ManageRobots2
{
    public class RobotStatus
    {
        public RobotStatus(int custId, bool isRobot)
        {
            CustId = custId;
            IsRobot = isRobot;
        }

        public int CustId { get; }

        public bool IsRobot { get; }
    }
}