using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Configurations;
using Fanex.Data;
using Fanex.Tarus.Infrastructure.Extensions;

namespace Sunplus.Accounting.DataAccess.AdoNet
{
    /// <summary>
    /// Summary description for UserDAO
    /// </summary>
    public class UserDAO : DataAccess, IUserDAO
    {
        #region IUserDAO Members

        protected StoreProcedureElement GetTransactionStoreProcedure(string storeAlias)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.TransactionGroupSection.StoreCollection.DailyTxn, storeAlias);
            return spElement;
        }

        public DbResult InsResetWinning(DateTime dateTime, int userId, string[] arrDesc, string IP)
        {
            DbResult result = null;
            try
            {
                var hsParam = new Hashtable();
                hsParam.Add("winlostdate", dateTime);
                hsParam.Add("userid", userId);
                hsParam.Add("ip", IP);

                for (int i = 0; i < arrDesc.Length; i++)
                {
                    if (!string.IsNullOrEmpty(arrDesc[i]) && arrDesc[i] != "undefined")
                        hsParam.Add("transdesc" + (i + 1), arrDesc[i]);
                }

                hsParam.Add("errcode", 0);
                hsParam.Add("errmsg", string.Empty);

                //return DbHelper.Execute("Acc_ResetWinning_Ins", hsParam, DBType.MAIN);
                using (ObjectDb db = new ObjectDb("Acc_ResetWinning_Ins", 600))
                {
                    db.AssignParamValues(hsParam);

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

        public List<ResetWinning> GetResetWinning(DateTime dtmMoveBetDate, DateTime dtmTodate)
        {
            var entityList = new List<ResetWinning>();

            var hsParam = new Hashtable();
            hsParam.Add("movebetdate", dtmMoveBetDate);
            hsParam.Add("todate", dtmTodate);

            using (ObjectDb db = new ObjectDb("Acc_ResetWinning_Sel", 600))
            {
                db.AssignParamValues(hsParam);
                var reader = db.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var entity = new ResetWinning();
                        entity.Date = CastAide.AsDateTime(reader["winlostdate"]);
                        entity.Amount = CastAide.AsDecimal(reader["balance"]);
                        entity.IsReset = CastAide.AsBoolean(reader["IsReset"]);
                        //entity.Balance = CastAide.AsDecimal(reader["balance"]);

                        entityList.Add(entity);
                    }
                }
            }

            return entityList;
        }

        public List<FinanceTransaction> GetResetWinning_ByDate(DateTime transDate, bool isReset)
        {
            var entityList = new List<FinanceTransaction>();
            var hsParam = new Hashtable();
            hsParam.Add("winlostdate", transDate);
            hsParam.Add("isreset", isReset);

            using (ObjectDb db = new ObjectDb("Acc_ResetWinning_SelByDate", 600))
            {
                db.AssignParamValues(hsParam);
                var reader = db.ExecuteReader();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var entity = new FinanceTransaction();
                        entity.BetType = CastAide.AsInt32(reader["BetType"]);
                        entity.Transdesc = CastAide.AsString(reader["Transdesc"]);
                        //entity.Remark = CastAide.AsString(reader["Remark"]);
                        entity.Amount = CastAide.AsDecimal(reader["Amount"]);

                        entityList.Add(entity);
                    }
                }
            }

            return entityList;
        }



        public DateTime MoveBetDateSel()
        {
            DateTime datetime = DateTime.Today;
            using (ObjectDb db = new ObjectDb("SelBalanceUpDay"))
            {
                var ds = db.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                {
                    var dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        datetime = CastAide.AsDateTime(dt.Rows[0]["MoveBetsDay"], DateTime.Now);
                    }
                }
            }
            return datetime;
        }

        public DataSet InsDataTmpExcel(int userId, string arrFromCust, string arrToCust, string arrAmount,
                                        string arrExRate, string arrDescription, ref Hashtable error)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_FinanceTransferMulInsTmpExcel", 6000))
            {
                var hsParam = new Hashtable();
                hsParam.Add("userid", userId);
                hsParam.Add("arrfromcust", arrFromCust);
                hsParam.Add("arrtocust", arrToCust);
                hsParam.Add("arramount", arrAmount);
                hsParam.Add("arrexrate", arrExRate);
                hsParam.Add("arrdescription", arrDescription);
                hsParam.Add("errcode", 0);
                hsParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hsParam);
                var result = objDb.ExecuteDataSet();

                error = objDb.GetOuputParams();
                return result;

            }
        }

        public DataSet InsDataTmpExcel(int userId, string xml, ref Hashtable error)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_FinanceTransferMulInsTmpExcel", 6000))
            {
                var hsParam = new Hashtable();
                hsParam.Add("userid", userId);
                hsParam.Add("listtransfer", xml);
                hsParam.Add("errcode", 0);
                hsParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hsParam);
                var result = objDb.ExecuteDataSet();

                error = objDb.GetOuputParams();
                return result;

            }
        }

        public Result InsDataTmpExcelFinanceCrossTransfer(int userId, string arrFromCust, string arrToCust, string arrFromAmount,
                                        string arrToAmount, string arrDescription)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_FinanceCrossTransferMulInsTmpExcel"))
            {
                var hsParam = new Hashtable();
                hsParam.Add("userid", userId);
                hsParam.Add("arrfromcust", arrFromCust);
                hsParam.Add("arrtocust", arrToCust);
                hsParam.Add("arramount", arrFromAmount);
                hsParam.Add("arrtoamount", arrToAmount);
                hsParam.Add("arrdescription", arrDescription);

                objDb.AssignParamValues(hsParam);
                objDb.ExecuteNonQuery();

                return new Result()
                {
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode")),
                    ErrorMessage = string.Format("{0}", objDb.GetParameterValue("errmsg"))
                };
            }
        }

        public bool DelDataTmpExcel(int userId)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_FinanceTransferMulDelTmpExcel"))
            {
                var hsParam = new Hashtable();
                hsParam.Add("userid", userId);
                hsParam.Add("errcode", 0);
                hsParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hsParam);
                objDb.ExecuteNonQuery();

                return (CastAide.AsInt32(objDb.GetParameterValue("errcode")) == 0 ? true : false);
            }
        }

        public DataSet UpdFinanceTransferMul(int userId, string fileName, int fileSize, string IP, ref Hashtable error)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_FinanceTransferMulUpd"))
            {
                var hsParam = new Hashtable();
                hsParam.Add("userid", userId);
                hsParam.Add("filename", fileName);
                hsParam.Add("filesize", fileSize);
                hsParam.Add("ip", IP);
                hsParam.Add("errcode", 0);
                hsParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hsParam);
                var result = objDb.ExecuteDataSet();

                error = objDb.GetOuputParams();
                return result;
            }
        }

        public DataSet UpdFinanceCrossTransferMul(int userId, string fileName, int fileSize, string IP, string forexName, ref Hashtable error)
        {

            using (ObjectDb objDb = new ObjectDb("Acc_FinanceCrossTransferMulUpd"))
            {
                var hsParam = new Hashtable();
                hsParam.Add("userid", userId);
                hsParam.Add("filename", fileName);
                hsParam.Add("filesize", fileSize);
                hsParam.Add("forexusername", forexName);
                hsParam.Add("ip", IP);
                hsParam.Add("errcode", 0);
                hsParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hsParam);
                var result = objDb.ExecuteDataSet();

                error = objDb.GetOuputParams();
                return result;
            }
        }


        /// <summary>
        /// Get daily finance transaction IRP 444.4
        /// </summary>
        /// <param name="transDate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <param name="type"></param>
        /// <param name="creator"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public List<DailyTransaction> GetDailyTransaction(DateTime transDate, int pageIndex, int pageSize,
                                                          int status, string type, string creator, ref int totalPages,
                                                          string verifyBy)
        {
            var entityList = new List<DailyTransaction>();
            var paramList = new Hashtable();
            paramList.Add("fromdate", transDate);
            paramList.Add("pageindex", pageIndex);
            paramList.Add("pagesize", pageSize);
            paramList.Add("type", type);
            paramList.Add("creator", creator);
            paramList.Add("checkerid", status);
            paramList.Add("checkername", verifyBy);


            using (ObjectDb db = new ObjectDb("SelDailyFinanceTxn"))
            {
                db.AssignParamValues(paramList);
                var reader = db.ExecuteReader();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var entity = new DailyTransaction();
                        //entity.TransId = (long)(reader["transid"]);
                        entity.RefNo = CastAide.AsString(reader["refno"]);
                        entity.Username = (string)reader["username"];
                        entity.transDate = (string)reader["transdate"];
                        entity.Amount = Convert.ToDouble(reader["amount"]);
                        entity.Description = (string)reader["transdesc"];
                        entity.Type = (string)reader["typenamee"];
                        entity.Creator = (string)reader["creator"];
                        entity.CheckerName = reader.IsDBNull(reader.GetOrdinal("checkerName"))
                                                 ? string.Empty
                                                 : reader["checkerName"].ToString();
                        entity.CheckedTime = reader.IsDBNull(reader.GetOrdinal("checktime"))
                                                 ? CastAide.AsString(DateTime.MinValue)
                                                 : Convert.ToString(reader["checktime"]);
                        entity.CheckerId = reader.IsDBNull(reader.GetOrdinal("checkerId"))
                                               ? 0
                                               : Convert.ToInt32(reader["checkerId"]);
                        entityList.Add(entity);
                    }

                    //get total record
                    if (reader.NextResult() && reader.Read())
                    {
                        totalPages = Convert.ToInt32(reader["TotalPages"]);
                    }
                }
            }


            return entityList;
        }

        public List<DailyTransaction> GetDailyAllTransaction(DateTime transDate, int pageIndex, int pageSize, string type, string creator, ref int totalPages)
        {
            var entityList = new List<DailyTransaction>();
            var paramList = new Hashtable();
            paramList.Add("fromdate", transDate);
            paramList.Add("pageindex", pageIndex);
            paramList.Add("totalpages", totalPages);
            paramList.Add("pagesize", pageSize);
            paramList.Add("type", type);
            paramList.Add("creator", creator);

            DbResult dbResult = null;
            using (ObjectDb db = new ObjectDb("SelDailyAllTxn", 6000))
            {
                db.AssignParamValues(paramList);
                var ds = db.ExecuteDataSet();
                var output = db.GetOuputParams();

                dbResult = new DbResult(output, ds);
            }

            if (dbResult != null && dbResult.ResultSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dbResult.ResultSet.Tables[0].Rows)
                {
                    var entity = new DailyTransaction();
                    entity.Username = (string)dr["username"];
                    entity.transDate = (string)dr["transdate"];
                    entity.Amount = Convert.ToDouble(dr["amount"]);
                    entity.Description = (string)dr["transdesc"];
                    entity.Type = (string)dr["typenamee"];
                    entity.Creator = (string)dr["creator"];
                    entity.RowIndex = Convert.ToInt32(dr["rownumber"]);
                    entityList.Add(entity);
                }

                //get total record                
                totalPages = (int)dbResult.Outputs["totalpages"];
            }

            return entityList;
        }

        public bool UpdateVerifyStatus(User user, string refNoList, bool isVerified)
        {
            using (ObjectDb db = new ObjectDb("Acc_DailyTxn_UpdChecker", 1200))
            {
                var parms = new Hashtable();
                parms.Add("checkerid", user.Id);
                parms.Add("checkername", user.Username);
                parms.Add("refnos", refNoList);
                parms.Add("ip", user.LastLoginIP);
                parms.Add("isverified", isVerified);

                db.AssignParamValues(parms);
                db.ExecuteNonQuery();
                return (CastAide.AsInt32(db.GetParameterValue("errcode")) != 0 ? false : true);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="balanceUpDay"></param>
        /// <param name="isInlucdeAccBal"></param>
        /// <returns></returns>
        public List<Statement> GetCustomerStatement(User customer, DateTime fromDate, DateTime toDate,
                                                    DateTime balanceUpDay, bool isInlucdeAccBal)
        {
            var entityList = new List<Statement>();

            var p = new Hashtable();
            p.Add("custid", customer.Id);
            p.Add("fromdate", fromDate);
            p.Add("todate", toDate);
            p.Add("balanceupday", balanceUpDay);
            p.Add("movebetday", Sunplus.Accounting.DataAccess.Report.ReportUtils.BalanceUp.MoveBetsDay);
            p.Add("includeaccbal", isInlucdeAccBal);
            var spElement = GetStoreProcedure(Core.Configurations.StoreProcedureGroup.BetListSection.StoreCollection.BetListStores, "AllStatement_Full");

            ObjectDb db = new ObjectDb("AllStatement_Full", 600);
            db.AssignParamValues(p);
            using (var reader = db.ExecuteReader())
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var entity = new Statement();
                        entity.RefNo = (long)(reader["refno"]);
                        entity.CreatedDate = Convert.ToDateTime(reader["accdate"]);
                        entity.TotalAmount = Convert.ToDouble(reader["totalamt"]);
                        entity.TransType = reader.IsDBNull(reader.GetOrdinal("transtype"))
                                               ? string.Empty
                                               : reader["transtype"].ToString();
                        entity.TransDescription = reader.IsDBNull(reader.GetOrdinal("transdesc"))
                                                      ? string.Empty
                                                      : reader["transdesc"].ToString();

                        var creator = new User();
                        creator.Username = reader.IsDBNull(reader.GetOrdinal("creator"))
                                               ? string.Empty
                                               : reader["creator"].ToString();
                        entity.Creator = creator;


                        if (!reader.IsDBNull(reader.GetOrdinal("checkerID")))
                        {
                            var checker = new User();
                            checker.Username = reader.IsDBNull(reader.GetOrdinal("checkerName"))
                                                   ? string.Empty
                                                   : reader["checkerName"].ToString();

                            checker.Id = Convert.ToInt32(reader["checkerID"]);

                            entity.Checker = checker;
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("checkTime")))
                        {
                            entity.CheckedTime = Convert.ToDateTime(reader["checkTime"]);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("transtypeid")))
                            entity.TransTypeId = CastAide.AsInt32(reader["transtypeid"]);
                        else
                            entity.TransTypeId = 0;

                        entity.IsAllowedVerified = Convert.ToBoolean(reader["isAllowVeify"]);

                        entityList.Add(entity);
                    }
                }
            }


            return entityList;
        }

        public int GetCustIdByUserName(string custname)
        {
            int custId;
            var p = new Hashtable();
            p.Add("custname", custname);
            using (var objectDb = new ObjectDb("GetCustIdByUsername", 120))
            {
                objectDb.AssignParamValues(p);
                custId = Convert.ToInt32(objectDb.ExecuteScalar());
            }

            return custId;
        }


        public List<MenuBO> GetUserMenu(int custId)
        {
            var listMenu = new List<MenuBO>();
            var parms = new Hashtable();
            parms.Add("custid", custId);
            parms.Add("system", AppSession.CurrentSystemNAME.ToString());

            DbResult result = null;

            using (ObjectDb db = new ObjectDb("Acc_MenuSelByCustID"))
            {
                db.AssignParamValues(parms);
                var ds = db.ExecuteDataSet();
                var output = db.GetOuputParams();

                result = new DbResult(output, ds);
            }

            if (result.ResultSet != null && result.ResultSet.Tables.Count > 0)
            {
                DataTable dt = result.ResultSet.Tables[0];
                RecursionMenu(dt, ref listMenu, 0);
            }
            return listMenu;
        }

        public List<MenuBO> ManageMenuGetMenuList(string system)
        {
            var listMenu = new List<MenuBO>();

            try
            {
                var hash = new Hashtable();
                hash.Add("system", system);

                using (ObjectDb db = new ObjectDb("Acc_MenuSelList"))
                {
                    db.AssignParamValues(hash);
                    var ds = db.ExecuteDataSet();

                    DataTable dt = null;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        RecursionMenu(dt, ref listMenu, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return listMenu;
        }


        public Result InsMenuItem(MenuBO menu)
        {
            var rs = new Result();
            try
            {
                var hash = new Hashtable();
                hash.Add("menu", menu.Name);
                hash.Add("parentid", menu.ParentId);
                hash.Add("funcname", menu.FunctionName);
                hash.Add("url", menu.Url);
                hash.Add("system", menu.System);
                hash.Add("target", menu.Target);
                hash.Add("ishide", menu.IsHide);
                hash.Add("order", menu.Order);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                //hash.Add("level", menu.Level);
                DbResult dbrs = DbHelper.Execute("Acc_MenuIns", hash, DBType.DB03);

                rs.ErrorCode = CastAide.AsInt32(dbrs.Outputs["errcode"]);
                rs.ErrorMessage = CastAide.AsString(dbrs.Outputs["errmsg"]);
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    rs.ErrorCode = 100;
                    rs.ErrorMessage = ex.Message;
                }
            }
            return rs;
        }

        public Result UdpMenuItem(MenuBO menu)
        {
            var rs = new Result();
            try
            {
                var hash = new Hashtable();
                hash.Add("menuid", menu.Id);
                hash.Add("menu", menu.Name);
                hash.Add("funcname", CastAide.AsString(menu.FunctionName));
                hash.Add("url", menu.Url);
                hash.Add("target", menu.Target);
                hash.Add("ishide", menu.IsHide);
                hash.Add("order", menu.Order);
                hash.Add("system", menu.System);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");

                using (ObjectDb db = new ObjectDb("Acc_MenuUpd"))
                {
                    db.AssignParamValues(hash);

                    db.ExecuteNonQuery();
                    var output = db.GetOuputParams();

                    rs.ErrorCode = CastAide.AsInt32(output["errcode"]);
                    rs.ErrorMessage = CastAide.AsString(output["errmsg"]);
                }
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    rs.ErrorCode = 100;
                    rs.ErrorMessage = ex.Message;
                }
            }
            return rs;
        }

        public Result DelMenuItem(int menuId)
        {
            var rs = new Result();
            try
            {
                var hash = new Hashtable();
                hash.Add("menuid", menuId);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                DbResult dbrs = DbHelper.Execute("Acc_MenuDel", hash, DBType.DB03);

                rs.ErrorCode = CastAide.AsInt32(dbrs.Outputs["errcode"]);
                rs.ErrorMessage = CastAide.AsString(dbrs.Outputs["errmsg"]);
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    rs.ErrorCode = 100;
                    rs.ErrorMessage = ex.Message;
                }
            }
            return rs;
        }

        public List<MenuBO> GetGroupFunc()
        {
            var lst = new List<MenuBO>();
            try
            {
                var hash = new Hashtable();
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");

                DbResult result = null;
                using (ObjectDb db = new ObjectDb("Acc_MenuSelGroupFunc"))
                {
                    db.AssignParamValues(hash);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, ds);
                }

                var dt = new DataTable();
                if (result.ResultSet.Tables.Count > 0)
                {
                    dt = result.ResultSet.Tables[0];
                    foreach (DataRow dr in dt.Select())
                    {
                        var m = new MenuBO();
                        m.FunctionName = CastAide.AsString(dr["FuncName"]);
                        lst.Add(m);
                    }
                }
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return lst;
        }

        public List<FinanceBettype> GetFinanceBettypes()
        {
            var lst = new List<FinanceBettype>();
            try
            {
                DbResult rs = DbHelper.Execute("Acc_DailyTxn_FinanceBettype_Sel", DBType.WAREHOUSE);
                var dt = new DataTable();
                if (rs.ResultSet.Tables.Count > 0)
                {
                    dt = rs.ResultSet.Tables[0];
                    foreach (DataRow dr in dt.Select())
                    {
                        var f = new FinanceBettype();
                        f.BetType = CastAide.AsInt32(dr["typeid"]);
                        f.TypeName = CastAide.AsString(dr["typenamee"]);

                        lst.Add(f);
                    }
                }
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return lst;
        }

        /// <summary>
        /// Insert customer to Site Group 
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="roleId"></param>
        /// <param name="system"></param>
        /// <returns></returns>
        public Result InsertCustomerToGroup(int custId, int roleId, string system)
        {
            var result = new Result { ErrorCode = 0, ErrorMessage = string.Empty };
            DbResult ds = null;
            var param = new Hashtable { { "custid", custId } };
            param.Add("roleid", roleId);
            param.Add("system", system.ToString().ToUpper());

            using (ObjectDb db = new ObjectDb("CustGroupIns", 600))
            {
                db.AssignParamValues(param);

                var dataset = db.ExecuteDataSet();
                var output = db.GetOuputParams();

                ds = new DbResult(output, dataset);
            }

            if (ds != null)
            {
                result.ErrorCode = CastAide.AsInt32(ds.Outputs["errcode"]);
                result.ErrorMessage = CastAide.AsString(ds.Outputs["errmsg"]);

            }
            return result;
        }

        public List<MenuBO> GetReportMenu(int custId, string system)
        {
            var listMenu = new List<MenuBO>();
            var parms = new Hashtable();
            parms.Add("custid", custId);
            parms.Add("system", system);

            DbResult result = null;

            using (ObjectDb db = new ObjectDb("Acc_MenuSelByCustID"))
            {
                db.AssignParamValues(parms);
                var ds = db.ExecuteDataSet();
                var output = db.GetOuputParams();

                result = new DbResult(output, ds);
            }

            if (result.ResultSet != null && result.ResultSet.Tables.Count > 0)
            {
                DataTable dt = result.ResultSet.Tables[0];
                RecursionMenu(dt, ref listMenu, 0);
            }
            return listMenu;
        }
        #endregion


        private void RecursionMenu(DataTable dt, ref List<MenuBO> listMenu, int parentId)
        {
            if (dt == null || dt.Rows.Count == 0) return;

            DataRow[] rows = dt.Select("parentid=" + parentId, "[Order] ASC");
            var sid = new Random();
            foreach (DataRow row in rows)
            {
                var menuItem = new MenuBO();
                menuItem.Id = int.Parse(row["MenuId"].ToString());
                menuItem.Name = row["Menu"].ToString();
                var url = row["Url"].ToString();
                menuItem.Url = (url.ToLower().IndexOf("crm") != -1) ? url + "?sid=" + sid.NextDouble() : url;
                menuItem.System = row["System"].ToString();
                menuItem.ParentId = CastAide.AsInt32(row["ParentId"]);
                menuItem.FunctionName = CastAide.AsString(row["funcname"]);
                menuItem.Target = CastAide.AsString(row["Target"]);
                menuItem.IsHide = CastAide.AsBoolean(row["IsHide"]);
                menuItem.Order = CastAide.AsInt32(row["Order"]);
                var listSubMenu = new List<MenuBO>();
                RecursionMenu(dt, ref listSubMenu, menuItem.Id);
                menuItem.SubMenu = listSubMenu;


                listMenu.Add(menuItem);
            }
        }

        #region IUserDAO Members


        public DbResult GetDailyAllTransaction(DateTime transDate, string type, string creator)
        {
            var spElement = GetTransactionStoreProcedure("SelDailyAllTxn");
            var paramList = new Hashtable();
            paramList.Add("fromdate", transDate);
            paramList.Add("type", type);
            paramList.Add("creator", creator);

            /*DbResult dbResult = DbHelper.Execute("Acc_RptStatementDaily_XTEST",
                                                               paramList, DBType.WAREHOUSE, 120);*/

            DbResult dbResult = DbHelper.Execute(spElement.Name,
                                                               paramList, spElement.DbName, 6000);
            return dbResult;
        }

        #endregion





        public DailyTransactionDTO GetDailyTransaction(DateTime transDate, int status, string type, string creator, string verifyBy)
        {
            var dailyFinance = new DailyTransactionDTO();
            dailyFinance.items = new List<DailyTransaction>();
            var spElement = GetTransactionStoreProcedure("SelDailyFinanceTxn");
            int count = 0;
            var paramList = new Hashtable();
            paramList.Add("fromdate", transDate);
            paramList.Add("type", type);
            paramList.Add("creator", creator);
            paramList.Add("checkerid", status);
            paramList.Add("checkername", verifyBy);

            /*using (IDataReader reader = DbHelper.ExecuteReader("Acc_DailyTxn_Sel_xtest",
                                                               paramList, DBType.WAREHOUSE, 120))*/
            using (IDataReader reader = DbHelper.ExecuteReader(spElement.Name,
                                                               paramList, spElement.DbName, 120))
            {
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        count++;
                        var entity = new DailyTransaction();
                        //entity.TransId = (long)(reader["transid"]);
                        entity.RefNo = CastAide.AsString(reader["refno"]);
                        entity.Username = (string)reader["username"];
                        entity.transDate = CastAide.AsString(reader["transdate"]);
                        entity.Amount = Convert.ToDouble(reader["amount"]);
                        entity.Description = (string)reader["transdesc"];
                        entity.Type = (string)reader["typenamee"];
                        entity.Creator = (string)reader["creator"];
                        entity.CheckerName = reader.IsDBNull(reader.GetOrdinal("checkerName"))
                                                 ? string.Empty
                                                 : reader["checkerName"].ToString();
                        entity.CheckedTime = reader.IsDBNull(reader.GetOrdinal("checktime"))
                                                 ? CastAide.AsString(DateTime.MinValue)
                                                 : CastAide.AsString(reader["checktime"]);
                        entity.CheckerId = reader.IsDBNull(reader.GetOrdinal("checkerId"))
                                               ? 0
                                               : Convert.ToInt32(reader["checkerId"]);
                        dailyFinance.items.Add(entity);
                    }

                    dailyFinance.totalRows = count;
                }
            }


            return dailyFinance;
        }


    }
}
