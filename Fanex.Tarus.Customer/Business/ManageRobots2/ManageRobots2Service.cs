namespace Fanex.Tarus.Customer.Business.ManageRobots2
{
    using Fanex.Tarus.Customer.Criteria.ManageRobots2;
    using Fanex.Tarus.Customer.DataAccess.ManageRobots2;

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

        public bool CheckIsRobot(string username)
        {
            return dataAccess.CheckIsRobot(new CheckIsRobotCriteria { Username = username });
        }
    }
}
