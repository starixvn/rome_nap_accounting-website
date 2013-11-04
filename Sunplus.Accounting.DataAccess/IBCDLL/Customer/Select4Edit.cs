using System;
using System.Collections;
using System.Data;
using DNAide.Data;
using Fanex.Data;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    public class Select4Edit : DataAccess
    {
        private static Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
        {
            var sps = CustomerSession.Instance[Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[aliasName] : null;
        }

        /// <summary>
        /// Get super information for editting
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static DbResult GetSuperInfo(int custid)
        {
            DbResult ds = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);

                using (ObjectDb db = new ObjectDb("Acc_CustomerSelCEditCustomer", 600))
                {
                    db.AssignParamValues(param);

                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    ds = new DbResult(output, dataset);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2208
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static DbResult GetSuperInfo_acc(int custid)
        {
            DbResult ds = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);

                using (ObjectDb db = new ObjectDb("Acc_CustomerSelCEditCustomer", 600))
                {
                    db.AssignParamValues(param);

                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    ds = new DbResult(output, dataset);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Get master info
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="sRecommend"></param>
        /// <returns></returns>
        public static DbResult GetMasterInfo(int custId, int sRecommend)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId);
                hash.Add("srecommend", sRecommend);

                using (ObjectDb db = new ObjectDb("Acc_CustomerSelSEditCustomer", 600))
                {
                    db.AssignParamValues(hash);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    rs = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// execute SP CustomerSelMEditCustomer to get data for editing an agent
        /// @PhuongDN
        /// Using DNAide's Lib
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="masterId"></param>
        /// <returns>dataresult</returns>
        public static DbResult SelAgentForEdit(int agentId, int masterId)
        {
            DbResult ds = null;
            try
            {
                var p = new Hashtable();
                p.Add("custid", agentId);
                p.Add("mrecommend", masterId);

                using (ObjectDb db = new ObjectDb("Acc_CustomerSelMEditCustomer", 600))
                {
                    db.AssignParamValues(p);

                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    ds = new DbResult(output, dataset);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu
        /// 06/12/2006
        /// execute store procedure "CustomerSelEditCustomer"
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static DbResult CustomerSelEditCustomerRpt(object custid)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custid);
                
                using (ObjectDb db = new ObjectDb("Acc_CustomerSelEditCustomer", 600))
                {
                    db.AssignParamValues(param);

                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    ds = new DbResult(output, dataset);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// execute store procedure "Acc_CustomerSelMemberSearch_AutoSuspend" to get data for Suspend List.
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DbResult GetSuspendCustomerList(Hashtable hash)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("CustomerSelMemberSearch_AutoSuspend"))
                {
                    db.AssignParamValues(hash);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// execute store procedure "Acc_CustomerSelMemberSearch_AutoSuspend_1" to get data for Suspend List.
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DbResult GetSuspendCustomerList_1(Hashtable hash)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("CustomerSelMemberSearch_AutoSuspend"))
                {
                    db.AssignParamValues(hash);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// CITS
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DataSet acc_CustomerSelAgentInfo(Hashtable param)
        {
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_CustomerSelAgentInfo", 600))
                {
                    objDb.AssignParamValues(param);

                    return objDb.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        /// <summary>
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DbResult CustomerSelCustomerInfo(Hashtable param)
        {
            DbResult ds = null;
            try
            {
                using (ObjectDb db = new ObjectDb("CustomerSetting_CustomerSelCustomerInfo", 600))
                {
                    db.AssignParamValues(param);

                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    ds = new DbResult(output, dataset);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static DbResult TransferOptionSel(object custId)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custId);
                param.Add("transferoption", "");

                using (ObjectDb db = new ObjectDb("Acc_CustomerSelTransferOption", 600))
                {
                    db.AssignParamValues(param);

                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    ds = new DbResult(output, dataset);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        public static DbResult VirtualRateSel(object custId)
        {
            DbResult result = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custId);
                
                using (ObjectDb db = new ObjectDb("CustomerSelVirtualMultiplier"))
                {
                    db.AssignParamValues(param);

                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult EditSuperOnMemberSel(object custId, object agentId)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custId);
                param.Add("agentid", agentId);

                using (ObjectDb db = new ObjectDb("Acc_CustomerSelEditCustomer_SAg", 600))
                {
                    db.AssignParamValues(param);

                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    ds = new DbResult(output, dataset);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        public static DbResult EditMasterOnMemberSel(object custId, object agentId)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custId);
                param.Add("agentid", agentId);

                using (ObjectDb db = new ObjectDb("Acc_CustomerSelEditCustomer_MAg", 600))
                {
                    db.AssignParamValues(param);

                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    ds = new DbResult(output, dataset);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }
    }
}