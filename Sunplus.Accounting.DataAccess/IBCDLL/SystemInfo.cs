using System;
using System.Collections;
using DNAide.Core;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.IBCDLL
{
    public class SystemInfo
    {
        public static DateTime GetDBTime()
        {
            DateTime loginTime;
            var param = new Hashtable();
            param.Add("logintime", null);
            using (ObjectDb db = new ObjectDb("Age_LoginAdminVerifyTicket"))
            {
                db.AssignParamValues(param);
                db.ExecuteNonQuery();
                var output = db.GetOuputParams();
                loginTime = CastAide.AsDateTime(output["logintime"]);
            }
            return loginTime;
        }
    }
}