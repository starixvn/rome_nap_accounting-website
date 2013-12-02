using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.SpecialDelete;
using System;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Data;

/// <summary>
/// Summary description for SpecialDeleteProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.SpecialDelete
{
    public class SpecialDeleteProviderClient : SpecialDeleteProvider
    {
        public override DbResult BettransSelSpecialDel(Hashtable param)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("BettransSelSpecialDel", 600))
                {
                    db.AssignParamValues(param);

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

        public override DbResult BettransDelSpecial_OutrightSel(int LeagueID)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable();
                param.Add("leagueid", LeagueID);
                
                using (ObjectDb db = new ObjectDb("Special_OutrightSel", 600))
                {
                    db.AssignParamValues(param);

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

        public override DbResult BettransSelDelSpecial_Outright(Hashtable param)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Special_Outright", 600))
                {
                    db.AssignParamValues(param);

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

        public override DbResult BettransSelBet_BatchMsg(Hashtable param)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_BettransSelBet_BatchMsg", 600))
                {
                    db.AssignParamValues(param);

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