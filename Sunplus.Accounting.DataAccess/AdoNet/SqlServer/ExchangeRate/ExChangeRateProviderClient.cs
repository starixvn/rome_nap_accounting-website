using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.ExchangeRate;
using System;
using Fanex.Data;
using System.Data;

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.ExchangeRate
{
    public class ExChangeRateProviderClient : ExChangeRateProvider
    {
        public override DbResult ExchangeSelAccount()
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ExchangeRateSession.StoreCollection.ExchangeRateControl, "EchangeSelAcc");
            return DbHelper.Execute(spElement.Name, spElement.DbName);
        }

        public override void ExchangeIns(Hashtable param, ref int errcode, ref string errmsg, ref int exchangeid)
        {
            
            try
            {

                using (ObjectDb objDB = new ObjectDb("Acc_ExchangeIns", 600))
                {

                    objDB.AssignParamValues(param); 

                    objDB.ExecuteNonQuery();

                    errcode = Convert.ToInt32(objDB.GetParameterValue("errcode"));
                    errmsg = Convert.ToString(objDB.GetParameterValue("errmsg"));
                    exchangeid = Convert.ToInt32(objDB.GetParameterValue("exchangeid"));
                }
            }
            catch (Exception ex)
            {
                errcode = 100;
                errmsg = ex.Message;
            }
        }

        public override DbResult ExchangeSelByID(int exid)
        {
            var hash = new Hashtable { { "exchangeid", exid } };

            DbResult result = null;
            using (ObjectDb db = new ObjectDb("EchangeSelById", 600))
            {
                db.AssignParamValues(hash);

                var ds = db.ExecuteDataSet();
                var output = db.GetOuputParams();

                result = new DbResult(output, ds);
            }

            return result;
        }

        public override void ExchangeUpd(Hashtable param, ref int errcode, ref string errmsg)
        {
            try
            {
                using (ObjectDb objDB = new ObjectDb("EchangeUpd", 600))
                {
                    objDB.AssignParamValues(param);
                    objDB.ExecuteNonQuery();

                    errcode = Convert.ToInt32(objDB.GetParameterValue("errcode"));
                    errmsg = Convert.ToString(objDB.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                errcode = 100;
                errmsg = ex.Message;
            }
        }
    }
}