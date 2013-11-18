using System;
using DNAide.Core;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.IBCDLL
{
    public class SystemInfo
    {
        public static DateTime GetDBTime()
        {
            DNAide.Data.DbResult dr = null;
            DateTime loginTime;
            var p = new System.Collections.Hashtable();
            p.Add("logintime", null);
            try
            {
                using (ObjectDb db = new ObjectDb("Age_LoginAdminVerifyTicket", 600))
                {
                    db.AssignParamValues(p);

                    db.ExecuteNonQuery();

                    var output = db.GetOuputParams();
                    loginTime = CastAide.AsDateTime(output["logintime"]);
                }
            }
            catch { throw; }

            return loginTime;
        }
    }
}