using System.Collections;
using Fanex.CustomerSetting.Module.Core;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.NG2
{
    public class SyncStatusRepository : ISyncStatusRepository
    {
        public Result EnableStatus(int custId, int userId, string ip)
        {
            var result = new Result();
            using (var db = new ObjectDb("Acc_CustSetting_DisableGameStatusUpd"))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("isdisabled", "<ROOT><ProductStatus SportType=\"213\" BetType=\"2501\" IsDisabled=\"0\" IsDisabledDowline=\"0\" /></ROOT>");
                param.Add("msgtype", 0);
                param.Add("userid", userId);
                param.Add("subaccid", 0);
                param.Add("ip", ip);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                db.ExecuteNonQuery(param);
                result.ErrorCode = Utils.ToInt32(db.GetParameterValue("errcode"));
                result.ErrorMessage = Utils.ToString(db.GetParameterValue("errmsg"));
            }

            return result;
        }

        public Result SyncStatus(int custId, int userId, string ip)
        {
            var result = new Result();
            using (var db = new ObjectDb("Acc_CustSetting_SyncGameStatusUpd"))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("issynced", true);
                param.Add("productid", 23);
                param.Add("userid", userId);
                param.Add("subaccid", 0);
                param.Add("ip", ip);

                db.ExecuteNonQuery(param);
            }

            return result;
        }
    }
}