// --------------------------------------------------------------------------
// <copyright file="NumberGameMonthlyWinLossReportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Reports.DataAccess.Gaming.NumberGameMonthlyWinLossReport
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using Fanex.Data;
    using Fanex.Tarus.Reports.Criteria.Gaming.NumberGameMonthlyWinLossReport;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class NumberGameMonthlyWinLossReportDataAccess : INumberGameMonthlyWinLossReportDataAccess
    {
        public List<dynamic> GetReport(GetReportCriteria criteria)
        {
            DataTable dataTable = null;
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AddInParameter("@fromdate", DbType.DateTime, criteria.Fromdate);
                objDb.AddInParameter("@todate", DbType.DateTime, criteria.Todate);
                DataSet ds = objDb.ExecuteDataSet();
                if (ds != null)
                {
                    dataTable = ds.Tables[0];
                }
            }

            var result = new List<dynamic>();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new
                    {
                        Month = CastAide.AsInt32(row["Month"]),
                        Year = CastAide.AsInt32(row["Year"]),
                        winlost = CastAide.AsDecimal(row["winlost"]),
                        position = CastAide.AsDecimal(row["position"]),
                        commission = CastAide.AsDecimal(row["commission"]),
                        linetotal = CastAide.AsDecimal(row["linetotal"]),
                        Turnover = CastAide.AsDecimal(row["Turnover"]),
                        margin = CastAide.AsDecimal(row["margin"])
                    });
                }
            }

            return result;
        }
    }
}
