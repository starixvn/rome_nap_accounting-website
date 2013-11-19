using System.Collections;
using Fanex.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Libraries.APILogger;

namespace Sunplus.Accounting.Business
{
    /// <summary>
    /// Summary description for CasinoAPILogger
    /// </summary>
    public class CasinoAPILogger : IAPILogger
    {
        public void Write(int actionId, string functionName, string trackingValue, string returnValue, int returnCode)
        {
            var param = new Hashtable();
            param.Add("systemid", 3);
            param.Add("userid", AppSession.AccId);
            param.Add("custid", AppSession.AccId);
            param.Add("actionid", actionId);
            param.Add("funcname", functionName);
            param.Add("trackingvalue", trackingValue);
            param.Add("returnvalue", returnValue);
            param.Add("returncode", returnCode);
            //DbHelper.Execute("WriteLog_UserLog_CasinoAPI", param, DBType.LOG);
            using (ObjectDb db = new ObjectDb("WriteLog_UserLog_CasinoAPI", 600))
            {
                db.AssignParamValues(param);
                var ds = db.ExecuteNonQuery();
            }
        }
    }
}