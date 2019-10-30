namespace Fanex.Tarus.Customer.DataAccess.ManageRobots2
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Criteria.ManageRobots2;
    using Fanex.Tarus.Customer.Entities.ManageRobots2;

    public interface IManageRobots2DataAccess
    {
        IEnumerable<RobotStatus> CheckRobots(CheckIsRobotCriteria criteria);
    }
}