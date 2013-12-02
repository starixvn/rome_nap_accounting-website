// --------------------------------------------------------------------------
// <copyright file="SMATurnoverAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.SMATurnover
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.SMATurnover;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
    using Sunplus.Accounting.Core.Cache;
    using Sunplus.Accounting.Core.Extensions;

    public class SMATurnoverAccess : ISMATurnoverAccess
    {
        public static Dictionary<string, DateTime> SelBalanceUpMovebetDays()
        {
            var datatable = new DataTable();
            var balmoveday = new Dictionary<string, DateTime>();
            using (ObjectDb objDB = new ObjectDb("Core_ReportService_GetBalanceUpDay"))
            {
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
                balmoveday.Add("BalanceUpDay", CastAide.AsDateTime(datatable.Rows[0]["BalanceUpDay"]));
                balmoveday.Add("MovebetsDay", CastAide.AsDateTime(datatable.Rows[0]["MovebetsDay"]));
            }
            return balmoveday;
        }

        public static DateTime BalanceUpDay()
        {
            Dictionary<string, DateTime> balmoveDays = SelBalanceUpMovebetDays();
            return balmoveDays["BalanceUpDay"];
        }

        public static DateTime MoveBetDay()
        {
            Dictionary<string, DateTime> balmoveDays = SelBalanceUpMovebetDays();
            return balmoveDays["MovebetsDay"];
        }

        public DataTable GetSportType(int mode)
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("SMASportType"))
            {
                objDB.AddInParameter("mode", DbType.Int32, mode);
                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
            }
            return datatable;
        }

        public DataTable GetLeague(int sport,
            DateTime fromDate,
            DateTime toDate,
            bool isGetList)
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GetLeague"))
            {
                objDB.AddInParameter("sporttype", DbType.Int32, sport);
                objDB.AddInParameter("fromdate", DbType.DateTime, fromDate);
                objDB.AddInParameter("todate", DbType.DateTime, toDate);
                if (isGetList)
                {
                    objDB.AddInParameter("movebetsday", DbType.DateTime, MoveBetDay());
                }
                else
                {
                    objDB.AddInParameter("movebetsday", DbType.DateTime, null);
                }

                var ds = objDB.ExecuteDataSet();
                datatable = ds.Tables[0];
            }
            return datatable;
        }

        public MatchInfo GetInfoByMatchId(int matchId)
        {
            var matchInfo = new MatchInfo();
            using (ObjectDb objDB = new ObjectDb("GetInfoByMatch"))
            {
                objDB.AddInParameter("matchid", DbType.Int32, matchId);
                objDB.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                objDB.AddOutParameter("errmsg", DbType.String, 1000);
                var ds = objDB.ExecuteDataSet();
                matchInfo.ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                matchInfo.ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                if (matchInfo.ErrorCode == 0)
                {
                    var dtb = ds.Tables[0];
                    var row = dtb.Rows[0];
                    matchInfo.EventDate = string.Format("{0:M/d/yyyy}", CastAide.AsDateTime(row["eventdate"]));
                    matchInfo.LeagueId = CastAide.AsString(row["leagueid"]);
                    matchInfo.LeagueName = CacheRef.GetLeagueName(matchInfo.LeagueId);
                    matchInfo.HomeTeam = CacheRef.GetTeamName(row["homeid"]);
                    matchInfo.AwayTeam = CacheRef.GetTeamName(row["awayid"]);
                }
            }
            return matchInfo;
        }

        public DataTable GetSMATurnoverList(int sportId, int leagueId, DateTime fromDate, DateTime toDate)
        {
            var dtb = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GetSMATurnoverList"))
            {
                objDB.AddInParameter("sporttype", DbType.Int32, sportId);
                objDB.AddInParameter("leagueid", DbType.Int32, leagueId);
                objDB.AddInParameter("fromdate", DbType.DateTime, fromDate);
                objDB.AddInParameter("todate", DbType.DateTime, toDate);
                objDB.AddInParameter("movebetsday", DbType.DateTime, MoveBetDay());
                var ds = objDB.ExecuteDataSet();
                if (ds != null)
                {
                    dtb = ds.Tables[0];
                }
            }
            return dtb;
        }

        public Result AddSMATurnover(int matchId, string custName, decimal maxTurnover, int userId, string ip)
        {
            var result = new Result();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Customer_SMATurnover_AddSMAList"))
                {
                    objDB.AddInParameter("matchid", DbType.Int32, matchId);
                    objDB.AddInParameter("username", DbType.String, custName);
                    objDB.AddInParameter("maxturnover", DbType.Decimal, maxTurnover);
                    objDB.AddInParameter("userid", DbType.Int32, userId);
                    objDB.AddInParameter("ip", DbType.String, ip);
                    objDB.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                    objDB.AddOutParameter("errmsg", DbType.String, 1000);
                    var ds = objDB.ExecuteNonQuery();
                    result.ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                    result.ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrorCode = -1;
                result.ErrorMessage = "Add failed!";
            }

            return result;
        }

        public Result UpdateSMATurnover(int matchId, int custId, decimal maxTurnover, int userId, string ip)
        {
            var result = new Result();
            try
            {
                using (ObjectDb objDB = new ObjectDb("UpdateSMATurnover"))
                {
                    objDB.AddInParameter("matchid", DbType.Int32, matchId);
                    objDB.AddInParameter("custid", DbType.Int32, custId);
                    objDB.AddInParameter("maxturnover", DbType.Decimal, maxTurnover);
                    objDB.AddInParameter("userid", DbType.Int32, userId);
                    objDB.AddInParameter("ip", DbType.String, ip);
                    objDB.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                    objDB.AddOutParameter("errmsg", DbType.String, 1000);
                    var ds = objDB.ExecuteNonQuery();
                    result.ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                    result.ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrorCode = -1;
                result.ErrorMessage = "Update failed!";
            }

            return result;
        }

        public Result CheckMaxTurnoverValue(int matchId, string custName, decimal maxTurnover, int mode)
        {

            var result = new Result();
            try
            {
                using (ObjectDb objDB = new ObjectDb("CheckMaxTurnover"))
                {
                    objDB.AddInParameter("matchid", DbType.Int32, matchId);
                    objDB.AddInParameter("username", DbType.String, custName);
                    objDB.AddInParameter("maxturnover", DbType.Decimal, maxTurnover);
                    objDB.AddInParameter("mode", DbType.Int32, mode);
                    objDB.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                    objDB.AddOutParameter("errmsg", DbType.String, 1000);
                    var ds = objDB.ExecuteNonQuery();
                    result.ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                    result.ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrorCode = -1;
                result.ErrorMessage = "Check max turnover failed!";
            }
            return result;
        }

        public List<dynamic> GetMatchInfo_AddSMAList(GetMatchInfo_AddSMAListCriteria criteria,
            ref Hashtable outputParams)
        {
            DataTable dataTable = null;
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AddInParameter("LeagueID", DbType.Int32, criteria.LeagueId);
                objDb.AddInParameter("FromDate", DbType.DateTime, criteria.FromDate);
                objDb.AddInParameter("ToDate", DbType.DateTime, criteria.ToDate);
                //objDb.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                //objDb.AddOutParameter("errmsg", DbType.String, 1000);

                var ds = objDb.ExecuteDataSet();
                outputParams = objDb.GetOuputParams();
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
                        matchid = row["matchid"],
                        eventdate = row["eventdate"],
                        leagueid = criteria.LeagueId,
                        homeid = row["homeid"],
                        awayid = row["awayid"]
                    });
                }
            }

            return result;
        }

        public List<dynamic> CheckSMAList(CheckSMAListCriteria criteria,
            ref Hashtable outputParams)
        {
            DataSet ds = null;
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AddInParameter("UserID", DbType.Int32, criteria.UserID);
                objDb.AddInParameter("ListSMA", DbType.String, criteria.ListSMA);
                objDb.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                objDb.AddOutParameter("errmsg", DbType.String, 1000);

                ds = objDb.ExecuteDataSet();
                outputParams = objDb.GetOuputParams();
            }

            var result = new List<dynamic>();
            if (ds != null)
            {
                DataTable dataTable = null;
                var succeededTable = new List<dynamic>();
                if (ds.Tables.Count >= 1)
                {
                    dataTable = ds.Tables[0];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        succeededTable.Add(new
                        {
                            //Seq = row["Seq"],
                            SMAName = row["SMAName"],
                            CustID = row["CustID"],
                            MatchID = row["matchid"],
                            MaxTurnover = row["MaxTurnover"],
                            eventdate = row["eventdate"],
                            homeid = row["homeid"],
                            awayid = row["awayid"]
                        });
                    }
                }

                result.Add(succeededTable);
                var failedTable = new List<dynamic>();
                if (ds.Tables.Count >= 2)
                {
                    dataTable = ds.Tables[1];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        failedTable.Add(new
                        {
                            //Seq = row["Seq"],
                            SMAName = row["SMAName"],
                            CustID = row["CustID"],
                            //MatchID = row["MatchID"],
                            MaxTurnover = row["MaxTurnover"],
                            Reason = row["Reason"]
                        });
                    }
                }

                result.Add(failedTable);
            }

            return result;
        }

        public List<dynamic> AddValidSMAList(AddSMAListCriteria criteria, ref Hashtable outputParams)
        {
            DataSet ds = null;
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AddInParameter("UserID", DbType.Int32, criteria.UserID);
                objDb.AddInParameter("ListSMA", DbType.String, criteria.ListSMA);
                objDb.AddInParameter("IP", DbType.String, criteria.IP);
                objDb.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                objDb.AddOutParameter("errmsg", DbType.String, 1000);
                ds = objDb.ExecuteDataSet();
                outputParams = objDb.GetOuputParams();
            }

            var result = new List<dynamic>();
            if (ds != null)
            {
                DataTable dataTable = null;
                if (ds.Tables.Count >= 1)
                {
                    dataTable = ds.Tables[0];
                    foreach (DataRow row in dataTable.Rows)
                    {
                        result.Add(new
                        {
                            username = row["username"],
                            matchid = row["matchid"]
                        });
                    }
                }
            }

            return result;
        }
    }
}
