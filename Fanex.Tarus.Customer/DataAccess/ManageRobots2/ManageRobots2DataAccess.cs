namespace Fanex.Tarus.Customer.DataAccess.ManageRobots2
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.ManageRobots2;
    using Fanex.Tarus.Customer.Entities.ManageRobots2;

    public class ManageRobots2DataAccess : IManageRobots2DataAccess
    {
        public IEnumerable<RobotStatus> CheckRobots(CheckIsRobotCriteria criteria)
        {
            var result = new List<RobotStatus>();

            if (criteria.IsValid())
            {
                using (var objDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            result.Add(new RobotStatus(Convert.ToInt32(reader["custid"]), Convert.ToBoolean(reader["isrobot"])));
                        }
                    }
                }
            }

            return result;
        }
    }
}