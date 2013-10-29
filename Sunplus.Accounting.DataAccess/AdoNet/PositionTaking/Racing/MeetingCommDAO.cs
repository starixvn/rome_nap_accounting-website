using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.PositionTaking.Racing;
using System;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.AdoNet.PositionTaking.Racing
{
    public class MeetingCommDAO : DataAccess, IMeetingCommDAO
    {
        public DbResult GetPartnerList()
        {
            DbResult result = null;
            try
            {
                using (ObjectDb db = new ObjectDb("GetPartnerList", 600))
                {
                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, dataset);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public DbResult GetPartnerDetail(int partnerId)
        {
            DbResult result = null;
            try
            {
                var param = new Hashtable { { "partnerid", partnerId } };

                using (ObjectDb db = new ObjectDb("GetPartnerDetail", 600))
                {
                    db.AssignParamValues(param);

                    var dataset = db.ExecuteDataSet();
                    var output = db.GetOuputParams();

                    result = new DbResult(output, dataset);
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