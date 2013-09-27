// --------------------------------------------------------------------------
// <copyright file="ViewPTRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.ViewPT
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.ViewPT;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ViewPTRepository : IViewPTRepository
    {
        public List<dynamic> GetPTSetting_Bingo(GetPTSettingCriteria criteria)
        {
            DataTable dataTable = null;
            List<dynamic> rawData = new List<dynamic>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AddInParameter("username", DbType.String, criteria.UserName);
                objDb.AddInParameter("GameKind", DbType.Int32, Math.Abs(criteria.BetTypeId));
                objDb.AddInParameter("GameType", DbType.Int32, 0);
                DataSet ds = objDb.ExecuteDataSet();
                if (ds == null || ds.Tables.Count==0)
                {
                    return rawData;
                }

                dataTable = ds.Tables[0];
            }

            foreach (DataRow row in dataTable.Rows)
            {
                rawData.Add(new
                {
                    roleid = row["roleid"],
                    rolename = row["rolename"],
                    gamekindid = row["gamekindid"],
                    gametypeid = row["gametypeid"],
                    gametype = row["gametype"],
                    custid = row["custid"],
                    username = row["username"],
                    SPT = row["SPT"],
                    mPT = row["mPT"],
                    aPT = row["aPT"],
                    MinPt = row["MinPt"],
                    IsAutoPT = row["IsAutoPT"],
                    AutoPT = row["AutoPT"],
                    RollingRate = row["RollingRate"],
                    Incentive = row["IncentiveRate"]
                });
            }
           

            return rawData;
        }
    }
}
