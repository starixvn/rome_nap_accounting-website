using System.Data;
using DNAide.Core;
using Fanex.Data;
using Sunplus.Accounting.DataAccess.ICreditTermDAO;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.AdoNet.CreditTermDAO
{
    public class CreditTermGroupDAO : DataAccess, ICreditTermGroupDAO
    {
        public DataTable GetAllGroup()
        {
            using (ObjectDb objDB = new ObjectDb("GetAllGroup"))
            {
                var ds = objDB.ExecuteDataSet();
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            return null;
        }

        public Result UpdateConsolidationReportAllowStatus(int custId, bool isViewReport, int userID)
        {
            var result = new Result();
            using (ObjectDb objDB = new ObjectDb("Acc_CreditTermUpdIsViewReport"))
            {
                objDB.AddInParameter("CustId", DbType.Int32, custId);
                objDB.AddInParameter("IsRptConsol", DbType.Boolean, isViewReport);
                objDB.AddInParameter("UserID", DbType.Boolean, userID);
                objDB.AddOutParameter("ErrCode", DbType.Int32, sizeof(int));
                objDB.AddOutParameter("ErrMsg", DbType.String, 4000);
                objDB.ExecuteNonQuery();
                result.ErrorMessage = CastAide.AsString(objDB.GetParameterValue("ErrMsg"));
                result.ErrorCode = CastAide.AsInt32(objDB.GetParameterValue("ErrCode"));
            }

            return result;
        }
    }
}