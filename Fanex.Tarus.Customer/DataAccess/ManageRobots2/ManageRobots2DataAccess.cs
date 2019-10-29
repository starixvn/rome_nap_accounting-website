namespace Fanex.Tarus.Customer.DataAccess.ManageRobots2
{
    using System;
    using System.Collections;
    using System.Diagnostics;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Tarus.Customer.Criteria.ManageRobots2;

    public class ManageRobots2DataAccess : IManageRobots2DataAccess
    {
        public bool CheckIsRobot(CheckIsRobotCriteria criteria)
        {
            if (!criteria.IsValid())
            {
                ExceptionHandler.Handle(new Exception("Check Is Robot - CustId <= 0"), TraceEventType.Warning);
                return false;
            }

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
