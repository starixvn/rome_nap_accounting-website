using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IAdmin;
using Sunplus.Accounting.Entities;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.AdoNet.Admin
{
    public class ExternalSiteDAO : DataAccess, IExternalSiteDAO
    {
        protected StoreProcedureElement GetStoreProcedure(string site)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.AdminSession.StoreCollection.ExternalSite, site);
        }


        public DataTable SelGroupName()
        {
            DataTable dtResult = null;
            try
            {
                using (ObjectDb db = new ObjectDb("SelectGroupName"))
                {
                    var ds = db.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dtResult = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return dtResult;  
        }


        public DataTable SelInfoCust(string groupName)
        {
            DataTable dtResult = null;
            try
            {
                var param = new Hashtable();
                param.Add("groupname", groupName);

                using (ObjectDb db = new ObjectDb("SelectInfoCust"))
                {
                    db.AssignParamValues(param);

                    var ds = db.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dtResult = ds.Tables[0];
                    }
                }

                return dtResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dtResult;
        }


        public Entities.Result DelCust(int custId, int userId)
        {
            var result = new Result();
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("userid", userId);

                var spElement = GetStoreProcedure("DeleteCustomer");
                var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
                result.ErrorCode = CastAide.AsInt32(rs.Outputs["errcode"]);
                result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);
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
            var param = new Hashtable();
            param.Add("username", username);
            param.Add("userid",userId);
            param.Add("groupname",groupName);
            var spElement = GetStoreProcedure("AddExistCustomer");
            var rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            result.ErrorCode = CastAide.AsInt32(rs.Outputs["errcode"]);
            result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);

            return result;
        }
    }
}
