using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Web;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;
using Fanex.Data;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    /// <summary>
    /// @Author: Simon20090820
    /// </summary>
    public sealed class VirtualRate
    {
        private static Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
        {
            var sps = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[aliasName] : null;

        }
        public static DataTable GetDataFromExcel(string ExcelPath)
        {
            var dtb = new DataTable();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelPath +
                             ";Extended Properties=Excel 8.0;";
            var excelCon = new OleDbConnection(strConn);
            try
            {
                excelCon.Open();
                var myCommand = new OleDbCommand("SELECT * FROM [Sheet1$]", excelCon);
                var objAdap = new OleDbDataAdapter();
                objAdap.SelectCommand = myCommand;
                objAdap.Fill(dtb);
                excelCon.Close();
                excelCon.Dispose();
                return dtb;
            }
            catch (Exception ex)
            {
                excelCon.Close();
                excelCon.Dispose();
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
            return null;
        }

        public static DbResult InsDataTmpExcel(string arrUsernames, string arrVirtualRates)
        {
            DbResult rs = null;
            try
            {

                arrUsernames = arrUsernames.Replace('\b', Convert.ToChar(8));

                var hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                hash.Add("arrusernames", arrUsernames);
                hash.Add("arrvirtualrates", arrVirtualRates);                

                using (ObjectDb db = new ObjectDb("Acc_VirtualRateMulInsTmpExcel"))
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

        public static DbResult CheckData()
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                using (ObjectDb db = new ObjectDb("Acc_VirtualRateMulSelCheck"))
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

        public static DbResult DelDataTmpExcel()
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                
                using (ObjectDb db = new ObjectDb("Acc_VirtualRateMulDelTmpExcel"))
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

        public static void UpdateVirtualRateIntoCustinfo(string filename, int filesize, int index, ref int errcode, ref string errmsg)
        {
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_VirtualRateMulUpd", 600))
                {
                    var hash = new Hashtable();
                    hash.Add("userid", AppSession.AccId);
                    hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                    hash.Add("filename", filename);
                    hash.Add("filesize", filesize);
                    hash.Add("index", index);

                    objDB.AssignParamValues(hash);

                    objDB.ExecuteNonQuery();

                    errcode = Convert.ToInt32(objDB.GetParameterValue("errcode"));
                    errmsg = Convert.ToString(objDB.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static DbResult ViewVRHistory(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                using (ObjectDb db = new ObjectDb("CustomerSelVirtualRateHistory"))
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
    }
}