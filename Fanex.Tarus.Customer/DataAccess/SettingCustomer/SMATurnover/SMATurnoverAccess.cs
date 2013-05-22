using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.Data;
using DNAide.Core;
using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;
using Sunplus.Accounting.Core.Cache;
using Fanex.Tarus.Customer.Entities;

namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.SMATurnover
{
    public class SMATurnoverAccess : ISMATurnoverAccess
    {
        public static Dictionary<string, DateTime> SelBalanceUpMovebetDays()
        {
            var datatable = new DataTable();
            var balmoveday = new Dictionary<string, DateTime>();
            using (ObjectDb objDB = new ObjectDb("SelBalanceUpDay"))
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

        public DataTable GetLeague(int sport, DateTime fromDate, DateTime toDate)
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GetLeague"))
            {
                objDB.AddInParameter("sporttype", DbType.Int32, sport);
                objDB.AddInParameter("fromdate", DbType.DateTime, fromDate);
                objDB.AddInParameter("todate", DbType.DateTime, toDate);
                objDB.AddInParameter("movebetsday", DbType.DateTime, MoveBetDay());
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
                matchInfo.ErrCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                matchInfo.ErrMsg = CastAide.AsString(objDB.GetParameterValue("errmsg"));
                if (matchInfo.ErrCode == 0)
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


        public Result AddSMATurnover(int matchId, string custName, double maxTurnover, int userId, string ip)
        {
            var result = new Result();
            using (ObjectDb objDB = new ObjectDb("AddSMATurnover"))
            {
                objDB.AddInParameter("matchid", DbType.Int32, matchId);
                objDB.AddInParameter("username", DbType.String, custName);
                objDB.AddInParameter("maxturnover", DbType.Double, maxTurnover);
                objDB.AddInParameter("userid", DbType.Int32, userId);
                objDB.AddInParameter("ip", DbType.String, ip);
                objDB.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                objDB.AddOutParameter("errmsg", DbType.String, 1000);
                var ds = objDB.ExecuteNonQuery();
                result.ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                result.ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
            }
            return result;
        }


        public Result UpdateSMATurnover(int matchId, int custId, double maxTurnover, int userId, string ip)
        {
            var result = new Result();
            using (ObjectDb objDB = new ObjectDb("UpdateSMATurnover"))
            {
                objDB.AddInParameter("matchid", DbType.Int32, matchId);
                objDB.AddInParameter("custid", DbType.Int32, custId);
                objDB.AddInParameter("maxturnover", DbType.Double, maxTurnover);
                objDB.AddInParameter("userid", DbType.Int32, userId);
                objDB.AddInParameter("ip", DbType.String, ip);
                objDB.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                objDB.AddOutParameter("errmsg", DbType.String, 1000);
                var ds = objDB.ExecuteNonQuery();
                result.ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                result.ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
            }
            return result;
        }


        public Result CheckMaxTurnoverValue(int matchId, string custName, double maxTurnover, int mode)
        {
            var result = new Result();
            using (ObjectDb objDB = new ObjectDb("CheckMaxTurnover"))
            {
                objDB.AddInParameter("matchid", DbType.Int32, matchId);
                objDB.AddInParameter("username", DbType.String, custName);
                objDB.AddInParameter("maxturnover", DbType.Double, maxTurnover);
                objDB.AddInParameter("mode", DbType.Int32, mode);
                objDB.AddOutParameter("errcode", DbType.Int32, sizeof(int));
                objDB.AddOutParameter("errmsg", DbType.String, 1000);
                var ds = objDB.ExecuteNonQuery();
                result.ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("errcode"));
                result.ErrorMessage = CastAide.AsString(objDB.GetParameterValue("errmsg"));
            }
            return result;
        }
    }
}
