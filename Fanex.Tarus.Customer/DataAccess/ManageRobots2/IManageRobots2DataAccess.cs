namespace Fanex.Tarus.Customer.DataAccess.ManageRobots2
{
    using Fanex.Tarus.Customer.Criteria.ManageRobots2;

    public interface IManageRobots2DataAccess
    {
        bool CheckIsRobot(CheckIsRobotCriteria criteria);
    }
}
