using DNAide.Core;
using Fanex.Data;
using Fanex.Tarus.Infrastructure.Extensions;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IAdmin;
using Sunplus.Accounting.Entities;
using System.Data;

namespace Sunplus.Accounting.DataAccess.AdoNet.Admin
{
    public class ExternalSiteDAO : DataAccess, IExternalSiteDAO
    {
        public DataTable SelGroupName()
        {
            DataTable rawdata = null;

            try
            {
                using (ObjectDb db = new ObjectDb("SelectGroupName", 600))
                {
                    var ds = db.ExecuteDataSet();

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        rawdata = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return rawdata;  
        }

        public DataTable SelInfoCust(string groupName)
        {
            DataTable rawdata = null;

            try
            {
                var param = new Hashtable();
                param.Add("groupname", groupName);
                param.Add("userid", AppSession.AccId);
                using (ObjectDb db = new ObjectDb("SelectInfoCust", 600))
                {
                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();

                    if (ds != null && ds.Tables.Count > 0)
                    {
                        rawdata = ds.Tables[0];
                    }
                }

                return rawdata;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return rawdata;
        }

        public Result DelCust(int custId, int userId)
        {
            var result = new Result();

            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("userid", userId);
                
                Hashtable output = new Hashtable();

                using (ObjectDb db = new ObjectDb("DeleteCustomer", 600))
                {
                    db.AssignParamValues(param);
                    db.ExecuteNonQuery();

                    output = db.GetOuputParams();
                }

                result.ErrorCode = CastAide.AsInt32(output["errcode"]);
                result.ErrorMessage = CastAide.AsString(output["errmsg"]);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public Result AddExistCust(string username, string groupName, int userId)
        {
            var result = new Result();

            try
            {
                var param = new Hashtable();
                param.Add("username", username);
                param.Add("userid", userId);
                param.Add("groupname", groupName);

                Hashtable output = new Hashtable();

                using (ObjectDb db = new ObjectDb("AddExistCustomer", 600))
                {
                    db.AssignParamValues(param);

                    db.ExecuteNonQuery();
                    output = db.GetOuputParams();
                }

                result.ErrorCode = CastAide.AsInt32(output["errcode"]);
                result.ErrorMessage = CastAide.AsString(output["errmsg"]);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
    }
}
