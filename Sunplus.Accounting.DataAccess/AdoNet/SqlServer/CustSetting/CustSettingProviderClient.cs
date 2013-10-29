using System.Collections;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using Fanex.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Provider.CustSetting;
using Sunplus.Accounting.Entities;

/// <summary>
/// Summary description for CustSetting
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.CustSetting
{
    public class CustSettingProviderClient : CustSettingProvider
    {
        public override bool IsAllowCustomerMoveSite(int custid, ref int RoleId)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerSelCheckMoveSite", 200))
            {
                var hash = new Hashtable();
                hash.Add("custid", custid);
                hash.Add("isallow", 0);
                hash.Add("roleid", 0);
                hash.Add("userid", AppSession.AccId);
                objDb.AssignParamValues(hash);
                objDb.ExecuteNonQuery();

                RoleId = CastAide.AsInt32(objDb.GetParameterValue("roleid"));
                return CastAide.AsBoolean(objDb.GetParameterValue("isallow"));
            }
        }

        public override Result CustomerUdpMoveSite(int UserId, int CustId, string NewSite)
        {
            var result = new Result();
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdMoveSite", 1000))
            {
                var hash = new Hashtable();
                hash.Add("userid", UserId);
                hash.Add("custid", CustId);
                hash.Add("site", CastAide.AsString(NewSite));
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);

                objDb.AssignParamValues(hash);
                objDb.ExecuteNonQuery();

                result.ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"));
                result.ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg"));

                return result;
            }
        }

        public override DbResult GetAllSiteNames()
        {
            var spElement = new SpConfig().GetStoreProcedure("SiteSel");
            return DbHelper.Execute(spElement.Name, spElement.DbName);
        }

        private class SpConfig : DataAccess
        {
            public Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
            {
                return GetStoreProcedure(Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, aliasName);
            }
        }
    }
}