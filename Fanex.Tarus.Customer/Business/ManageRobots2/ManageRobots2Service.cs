namespace Fanex.Tarus.Customer.Business.ManageRobots2
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Criteria.ManageRobots2;
    using Fanex.Tarus.Customer.DataAccess.ManageRobots2;
    using Fanex.Tarus.Customer.Entities.ManageRobots2;

    public class ManageRobots2Service : IManageRobots2Service
    {
        private readonly IManageRobots2DataAccess dataAccess;

        public ManageRobots2Service() : this(null)
        {
        }

        public ManageRobots2Service(IManageRobots2DataAccess dataAccess)
        {
            this.dataAccess = dataAccess ?? new ManageRobots2DataAccess();
        }

        public IEnumerable<RobotStatus> CheckRobots(string custIds)
        {
            return dataAccess.CheckRobots(new CheckIsRobotCriteria(custIds));
        }
    }
}