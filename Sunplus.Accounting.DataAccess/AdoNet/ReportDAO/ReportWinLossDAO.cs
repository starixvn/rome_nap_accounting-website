using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Fanex.Data;
using System;
using Fanex.Tarus.Infrastructure.Extensions;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class ReportWinLossDAO: DataAccessReport, IReportWinLossDAO
    {
        public DbResult SelLeagueWLbySports(string winlostdate)
        {
            DbResult dbresult = null;
            try
            {
                string subfix = GetStoreMappingSubfix(DNAide.Core.CastAide.AsDateTime(winlostdate));
                string spName = "LeagueWL_Sporttype" + subfix;
                
                using (ObjectDb db = new ObjectDb(spName))
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("winlostdate", winlostdate);
                    hashtable.Add("winlost_8rm", 0);
                    hashtable.Add("winlost_barm", 0);

                    db.AssignParamValues(hashtable);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    dbresult = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dbresult;
        }


        public DbResult SelLeagueWLByBettype(int sportId, int leagueId, string winlostDate)
        {
            DbResult result = null;
            try
            {
                var subfix = GetStoreMappingSubfix(DNAide.Core.CastAide.AsDateTime(winlostDate));
                string spName = "LeagueWL_Bettype" + subfix;                

                using (ObjectDb db = new ObjectDb(spName))
                {
                    var hashtable = new Hashtable();
                    hashtable.Add("winlostdate", winlostDate);
                    hashtable.Add("sporttype", sportId);
                    hashtable.Add("leagueid", leagueId);
                    hashtable.Add("winlost_8rm", 0);
                    hashtable.Add("winlost_barm", 0);

                    db.AssignParamValues(hashtable);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }


        public DbResult SeleLeagueWLDetailByBettype(string winlostDate, int sportId, int leagueId, int bettypeId, bool isLive, bool isHedge)
        {
            DbResult result = null;
            try
            {
                var subfix = GetStoreMappingSubfix(DNAide.Core.CastAide.AsDateTime(winlostDate));
                string spName = "LeagueWL_BettypeDetail" + subfix;

                using (ObjectDb db = new ObjectDb(spName))
                {
                    var hashtable = new Hashtable();
                    hashtable.Add("winlostdate", winlostDate);
                    hashtable.Add("leagueid", leagueId);
                    hashtable.Add("sporttype", sportId);
                    hashtable.Add("bettype", bettypeId);
                    hashtable.Add("liveindicator", isLive);
                    hashtable.Add("ishedge", isHedge);
                    hashtable.Add("winlost_8rm", 0);
                    hashtable.Add("winlost_barm", 0);

                    db.AssignParamValues(hashtable);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

       
    }
}
