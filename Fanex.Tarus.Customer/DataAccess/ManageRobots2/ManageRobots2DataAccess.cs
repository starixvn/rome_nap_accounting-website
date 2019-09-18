namespace Fanex.Tarus.Customer.DataAccess.ManageRobots2
{
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.ManageRobots2;

    public class ManageRobots2DataAccess : IManageRobots2DataAccess
    {
        public bool CheckIsRobot(CheckIsRobotCriteria criteria)
        {
            var outputParams = new Hashtable();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                objDb.ExecuteNonQuery();
                outputParams = objDb.GetOuputParams();
            }

            return Converter.ToBool(outputParams["isrobot"]);
        }
    }
}
