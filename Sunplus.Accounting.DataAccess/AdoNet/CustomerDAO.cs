using System;
using System.Collections;
using System.Data;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities.Customer;
using DNAide.Utils;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.AdoNet
{
    public class CustomerDAO : DataAccess, ICustomerDAO
    {

        protected Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, aliasName);
        }
        
        public DataTable GetCustomerInfo(string username)
        {
            var param = new System.Collections.Hashtable();
            param.Add("username", username);
            DataTable dtResult = null;
            var spElement = GetStoreProcedure("CustomerSelSiteByUserName");
            DbResult ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                dtResult = ds.ResultSet.Tables[0];
            return dtResult;
        }

        public Result UpdateCustomerSite(int custId, string newSite, int userId, string ip)
        {
            var result = new Result();

            using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdMoveSite", 1000))
            {
                var hash = new Hashtable
                           {
                               {"userid", userId},
                               {"custid", custId},
                               {"site", newSite},
                               {"errcode", 0},
                               {"errmsg", ""},
                               {"ip", ip}
                           };

                objDb.AssignParamValues(hash);
                objDb.ExecuteNonQuery();

                result.ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                result.ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg"));

                return result;
            }
        }

        public Result UpdateLevelDangerous(int custId, int updateDownline, int dangerType, int dangerLevel, int userId, string ip)
        {
            using (ObjectDb objdb = new ObjectDb("Acc_CustomerUpdDangercust", 300))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("updatedownline", updateDownline);
                param.Add("dangertype", dangerType);
                param.Add("dangerlevel", dangerLevel);
                param.Add("userid", userId);
                param.Add("ip", ip);

                objdb.AssignParamValues(param);
                objdb.ExecuteNonQuery();
                return new Result()
                {
                    ErrorCode = CastAide.AsInt32(objdb.GetParameterValue("errcode")),
                    ErrorMessage = string.Format("{0}", objdb.GetParameterValue("errmsg"))
                };
            }
        }

        public DbResult GetAllCustInfo(string username)
        {
            DbResult ds = null;
            var dtResult = new DataTable();
            try
            {
                var param = new Hashtable
                                {
                                    {"username", username}
                                };
                var spElement = GetStoreProcedure("SelAllInfo");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
              
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        #region Enable Number Game for downlines

        public BetSettingNG CheckEnableNGDownline(string username)
        {
            var betsetting = new BetSettingNG();            
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerSelNGUpline", 600))
            {
                var param = new Hashtable();
                param.Add("username", username);
                param.Add("userid", AppSession.AccId);

                objDb.AssignParamValues(param);

                objDb.ExecuteNonQuery();

                betsetting.errCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                betsetting.errMsg = CastAide.AsString(objDb.GetParameterValue("errmsg"));
                betsetting.minbet = NumberFormat.DecFormat(objDb.GetParameterValue("minbet"), 0);
                betsetting.maxbet = NumberFormat.DecFormat(objDb.GetParameterValue("maxbet"), 0);
                betsetting.maxperball = NumberFormat.DecFormat(objDb.GetParameterValue("maxperball"), 0);
                betsetting.maxpermach = NumberFormat.DecFormat(objDb.GetParameterValue("maxpermatch"), 0);
                betsetting.isEnable = CastAide.AsBoolean(objDb.GetParameterValue("isenable")) ? 1 : 0;
                return betsetting;
            }
        }

        public Result UpdateBetSettingNGDownline(string username, int isEnable, string maxBet, string minBet, string maxMatch, string maxBall)
        {            
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdNGEnableBetSetting", 600))
            {
                var param = new Hashtable();
                param.Add("username", username);
                param.Add("isenable", isEnable);
                param.Add("minbet", CastAide.AsDecimal(minBet));
                param.Add("maxbet", CastAide.AsDecimal(maxBet));
                param.Add("maxperball", CastAide.AsDecimal(maxBall));
                param.Add("maxpermatch", CastAide.AsDecimal(maxMatch));
                param.Add("userid", AppSession.AccId);
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);

                objDb.AssignParamValues(param);

                objDb.ExecuteNonQuery();

                return new Result() { 
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode")),
                    ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg"))
                };
            }
        }

        #endregion

        public string GetRNGSiteNameByCustId(int custId)
        {
            using (ObjectDb objdb = new ObjectDb("Acc_SiteSel_RNGCS", 180))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                
                objdb.AssignParamValues(param);
                objdb.ExecuteNonQuery();
                var errorCode = CastAide.AsInt32(objdb.GetParameterValue("errcode"));
                if(errorCode == 0)
                    return CastAide.AsString(objdb.GetParameterValue("siterng"));
                return null;
            }
        }
    }
}
