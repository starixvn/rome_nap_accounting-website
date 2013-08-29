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
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Customer.Criteria.ViewPT;
    using Microsoft.Practices.Unity;
    using Sunplus.Bingo.Core;
    using Sunplus.Bingo.Services;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ViewPTRepository : IViewPTRepository
    {
        public List<dynamic> GetPTSetting_Bingo(GetPTSettingCriteria criteria)
        {
            var customerService = BingoContainer.Current.Resolve<ICustomerService>();
            var dataTable = criteria.BetTypeId < 0
                        ? customerService.GetCustomerPt(criteria.UserName,
                             criteria.BetTypeId * (-1),
                             0)
                        : customerService.GetCustomerPt(criteria.UserName,
                            0,
                            criteria.BetTypeId);

            List<dynamic> rawData = new List<dynamic>();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
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
                        Incentive = 0.68
                    });
                }
            }

            return rawData;
        }
    }
}
