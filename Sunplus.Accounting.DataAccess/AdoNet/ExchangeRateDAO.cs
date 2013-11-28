using System.Web;
using Sunplus.Accounting.Core.Common;
using DNAide.Data;
using System.Collections;
using Fanex.Data;
using System;
using Fanex.Tarus.Infrastructure.Extensions;

namespace Sunplus.Accounting.DataAccess.AdoNet
{
    /// <summary>
    /// Summary description for ExchangeRateDAO
    /// </summary>
    public class ExchangeRateDAO: IExchangeRateDAO
    {
        public ExchangeRateDAO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public DbResult ExChangeRateUpdateBaseRate(string currency, string baserate, string minbet, string minbetlive)
        {
            DbResult result = null;
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                hash.Add("actualrate", baserate);
                hash.Add("currency", currency.Trim());
                hash.Add("minbet", minbet);
                hash.Add("minbetlive", minbetlive);
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");

                using (ObjectDb db = new ObjectDb("Acc_ExchangeUpdateBaseRate", 600))
                {
                    db.AssignParamValues(hash);

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
        public void ExChangeRateUpdateOpenTote(string currency, string GBPRate, string AUDRate, string ZARRate, string EURORate, string USDRate, ref int errcode, ref string errmsg)
        {
            try
            {
                using (ObjectDb objDB = new ObjectDb("Acc_ExchangeUpdateOpenToteRate", 600))
                {
                    Hashtable hash = new Hashtable();
                    hash.Add("userid", AppSession.AccId);
                    hash.Add("currency", currency.Trim());
                    if (GBPRate.Length != 0)
                        hash.Add("gbprate", GBPRate);
                    if (AUDRate.Length != 0)
                        hash.Add("audrate", AUDRate);
                    if (ZARRate.Length != 0)
                        hash.Add("zarrate", ZARRate);
                    if (EURORate.Length != 0)
                        hash.Add("eurorate", EURORate);
                    if (USDRate.Length != 0)
                        hash.Add("us$rate", USDRate);
                    hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                    hash.Add("errcode", 0);
                    hash.Add("errmsg", "");

                    objDB.AssignParamValues(hash);

                    objDB.ExecuteNonQuery();

                    errcode = Convert.ToInt32(objDB.GetParameterValue("errcode"));
                    errmsg = Convert.ToString(objDB.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                errcode = 100;
                errmsg = ex.Message;
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}
