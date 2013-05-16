// --------------------------------------------------------------------------
// <copyright file="ScoreInformationDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.DataAccess.ScoreInformation
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using Fanex.Data;
    using Fanex.Tarus.BetList.Criteria.ScoreInformation;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ScoreInformationDataAccess : IScoreInformationDataAccess
    {
        public List<dynamic> GetScoreInformation(GetScoreInformationCriteria criteria, ref Hashtable outputParam)
        {
            var dataTable = new DataTable();
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AddInParameter("MatchID", DbType.Int32, criteria.MatchID);
                objDb.AddInParameter("Bettype", DbType.Int32, criteria.Bettype);
                objDb.AddOutParameter("tradername", DbType.String, 1000);
                var ds = objDb.ExecuteDataSet();
                outputParam = objDb.GetOuputParams();
                if (ds != null && ds.Tables.Count > 0)
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
                        homeID = CastAide.AsInt32(row["homeID"]),
                        awayID = CastAide.AsInt32(row["awayID"]),

                        hthomescore = CastAide.AsInt32(row["hthomescore"]),
                        finalhomescore = CastAide.AsInt32(row["finalhomescore"]),
                        htawayscore = CastAide.AsInt32(row["htawayscore"]),
                        finalawayscore = CastAide.AsInt32(row["finalawayscore"]),
                        eventstatus = CastAide.AsString(row["eventstatus"]),
                        eventdate = CastAide.AsString(row["eventdate"]),
                        IsRefund = CastAide.AsBoolean(row["IsRefund"]),
                        IsCalRefund = CastAide.AsBoolean(row["IsCalRefund"]),
                        IsNeutral = CastAide.AsBoolean(row["IsNeutral"]),
                        betstatus = CastAide.AsString(row["betstatus"]),
                        IsPending = CastAide.AsBoolean(row["IsPending"])
                    });
                }
            }

            return result;
        }
    }
}
