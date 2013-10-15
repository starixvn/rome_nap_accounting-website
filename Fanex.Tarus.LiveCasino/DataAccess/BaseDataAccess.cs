using System;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using Fanex.Data;
using Fanex.Tarus.LiveCasino.Entities;

namespace Fanex.Tarus.LiveCasino.DataAccess
{
    using Sunplus.Accounting.Core.Configurations;

    public class BaseDataAccess
    {
        public BaseDataAccess()
        {
            
        }

        public bool GetSyncLiveCasinoStatus(int custId, string userName, int userId)
        {

            using (ObjectDb objDB = new ObjectDb("CustomerSelStatus"))
                {
                    objDB.AddInParameter("CustID", DbType.Int32, custId);
                    objDB.AddInParameter("AdminID", DbType.Int32, userId);
                    objDB.AddInParameter("username", DbType.String, userName);
                    DataSet ds = objDB.ExecuteDataSet();
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        return WebSettings.EnableLiveCasino
                                   ? CastAide.AsBoolean(ds.Tables[0].Rows[0]["SyncLiveCasino"])
                                   : false;
                    }
                }

            return false;
        }

        public CustomerStatusInfo SelCustStatusInfo(int custId, string userName, int userId)
        {
            CustomerStatusInfo customer = new CustomerStatusInfo();
            using (ObjectDb objDB = new ObjectDb("CustomerSelStatus"))
            {
                objDB.AddInParameter("CustID", DbType.Int32, custId);
                objDB.AddInParameter("AdminID", DbType.Int32, userId);
                objDB.AddInParameter("username", DbType.String, userName);
                DataSet ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    customer.SyncLiveCasino = WebSettings.EnableLiveCasino
                                                  ? CastAide.AsBoolean(ds.Tables[0].Rows[0]["SyncLiveCasino"])
                                                  : false;
                    customer.DisableLiveCs = WebSettings.EnableLiveCasino
                                                 ? CastAide.AsBoolean(ds.Tables[0].Rows[0]["DisableLiveCasino"])
                                                 : true;
                    customer.IsEnableDownLines = !customer.DisableLiveCs;
                    
                }
            }

            return customer;
        }

        public Dictionary<int, string> GetExchange()
        {
            var exchange = new Dictionary<int, string>();
            
            //get data from database
            using (ObjectDb objDB = new ObjectDb("Acc_ExchangeCurrencySel"))
            {
                DataSet ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = ds.Tables[0].Select();
                    foreach (DataRow row in dr)
                    {
                        exchange.Add(CastAide.AsInt32(row["exchangeid"]), row["currency"].ToString());
                    }
                }
            }
            return exchange;
            
        }

        
    }
}
