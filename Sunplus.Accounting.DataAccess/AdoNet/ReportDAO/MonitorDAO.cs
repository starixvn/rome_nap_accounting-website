using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Report.Monitor;
using Sunplus.Core.LoggingLib;
using DNAide.Core;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class MonitorDAO : DataAccess, IMonitorDAO
    {
        private enum GetKeySpMapping
        {
            GetCurrenies,
            Monitor_SportSel,
            Monitor_BettypeSel,
            Monitor_LeagueSel,
            Monitor_Report,
            Monitor_ReportArchive
        }

        #region getSPMapping
        private StoreProcedureElement getSPMapping(GetKeySpMapping mapping)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Others, mapping.ToString());
        }

        
        #endregion

        #region GetCurrency
        public DataTable GetCurrency()
        {

            DataTable dataTable = new DataTable();
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_ExchangeSel", 600))
                {
                    var dataset = objDB.ExecuteDataSet();
                    if (dataset != null && dataset.Tables.Count > 0)
                    {
                        dataTable = dataset.Tables[0];
                    }
                }   
            }
            catch (Exception ex)
            {
                
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dataTable;
        }
        #endregion

        #region GetSports
        public DataTable GetSports()
        {
            DataTable dataTable = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Monitor_SportSel", 1000))
                {
                    var ds = db.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dataTable = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
            return dataTable;
        }
        #endregion

        #region GetBettypes
        public DataTable GetBettypes(List<Items> option, int sporttype)
        {
            DataTable dataTable = null;
            try
            {
                Hashtable hashtable = new Hashtable();

                foreach (Items items in option)
                {
                    if(items.Key.Length>0)
                        hashtable.Add(items.Key,CastAide.AsInt16(items.Value) );
                }
                hashtable.Add("sporttype", sporttype);

                using (ObjectDb db = new ObjectDb("Monitor_BettypeSel", 1000))
                {
                    db.AssignParamValues(hashtable);
                    var ds = db.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dataTable = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
            return dataTable;
        }
        #endregion

        #region GetLeague
        public DataTable GetLeague(List<Items> option, int sportId, string fromdate, string todate, int currency, int bettype, int live)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Monitor_LeagueSel", 600))
                {
                    Hashtable hashtable = new Hashtable();

                    foreach (Items items in option)
                    {

                        if (items.Key.Length > 0)
                            hashtable.Add(items.Key,
                                          CastAide.AsInt16(option[0].Value) == 1 ? 1 : CastAide.AsInt16(items.Value));
                    }

                    hashtable.Add("sporttype", sportId);
                    hashtable.Add("fromdate", fromdate);
                    hashtable.Add("todate", todate);
                    hashtable.Add("currency", currency);
                    hashtable.Add("bettype", bettype);
                    if (live > -1)
                    {
                        hashtable.Add("live", live);
                    }

                    db.AssignParamValues(hashtable);
                    var ds = db.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dataTable = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
            return dataTable;
        }
        #endregion

        #region GetMAMonitorReport
        public DataTable GetMAMonitorReport(Param param)
        {
            DataTable dataTable = new DataTable();
            try
            {
                DataTable dtWH = null;
                Hashtable hashtable = new Hashtable();
                string key = "";

                string fdate = param.fromdate;
                string tdate = param.todate;

                hashtable.Add("fromdate", param.fromdate);
                hashtable.Add("todate", param.todate);
                hashtable.Add("currency", param.currency);

                foreach (Items items in param.product)
                {
                    if(items.Key.Length>0)
                        hashtable.Add(items.Key, items.Value);
                }

                bool showsb = CastAide.AsBoolean(hashtable["showsb"]);
                if(showsb)
                    hashtable.Add("sporttype", param.sporttype);

                hashtable.Add("bettype", param.bettype);
                if(param.live<2 && param.live>-1)
                    hashtable.Add("live", param.live);
                if (param.usernames != "")
                {
                    hashtable.Add("usernames", param.usernames);
                }
                if (param.leagues != "")
                {
                    hashtable.Add("leagues", param.leagues);
                }
                hashtable.Add("reportby", param.reportby);

                if(CastAide.AsDateTime(param.fromdate) >= ReportUtils.BalanceUp.BalanceUpDay )//archive
                {
                    key = "Monitor_Report";                                    
                }
                else //wh
                {
                    key = "Monitor_ReportArchive";                   
                }

                using (ObjectDb db = new ObjectDb(key, 600))
                {
                    db.AssignParamValues(hashtable);
                    var ds = db.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dataTable = ds.Tables[0];
                    }
                }    
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
            return dataTable;
        }
        #endregion


       
    }
}
