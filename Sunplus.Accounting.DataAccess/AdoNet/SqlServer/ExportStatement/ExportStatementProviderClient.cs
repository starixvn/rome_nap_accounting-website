using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.ExportStatement;
using System;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Data;

/// <summary>
/// Summary description for ExportStatementProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.ExportStatement
{
    public class ExportStatementProviderClient : ExportStatementProvider
    {
        public override DbResult AllStatement_SelCheckByUser(string userlist)
        {
            DbResult result = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("userlist", userlist);

                using (ObjectDb db = new ObjectDb("Acc_AllStatement_SelCheckUsersList", 600))
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

        public override DbResult AllStatement_UsersList(Hashtable param)
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_AllStatement_UsersList", 600))
                {
                    db.AssignParamValues(param);
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
    }
}