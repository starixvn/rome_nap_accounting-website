using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities.ManageRobots2;

namespace Fanex.Tarus.Customer.Business.ManageRobots2
{
    public interface IManageRobots2Service
    {
        IEnumerable<RobotStatus> CheckRobots(string custIds);
    }
}