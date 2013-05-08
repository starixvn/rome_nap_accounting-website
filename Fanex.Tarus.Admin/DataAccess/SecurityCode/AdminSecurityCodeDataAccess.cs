// --------------------------------------------------------------------------
// <copyright file="AdminSecurityCodeDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.DataAccess.SecurityCode
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using Fanex.Data;
    using Fanex.Tarus.Admin.Criteria.SecurityCode;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class AdminSecurityCodeDataAccess : IAdminSecurityCodeDataAccess
    {
        public List<dynamic> GetUserList(GetUserListCriteria criteria)
        {
            DataTable dataTable = new DataTable();
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AddInParameter("ListUserName", DbType.String, criteria.ListUserName);
                var ds = objDb.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dataTable = ds.Tables[0];
                }
            }

            var result = new List<dynamic>();
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new
                    {
                        CustId = CastAide.AsInt32(row["CustId"]),
                        UserName = CastAide.AsString(row["UserName"]),
                        RoleId = CastAide.AsInt32(row["RoleId"]),
                        SubCustId = CastAide.AsInt32(row["SubCustId"]),
                        SubUserName = CastAide.AsString(row["SubUserName"])
                    });
                }
            }

            return result;
        }

        public void ResetSecurityCode(ResetSecurityCodeCriteria criteria, ref Hashtable outputParams)
        {
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AddInParameter("Custid", System.Data.DbType.Int32, criteria.Custid);
                objDb.AddInParameter("Userid", System.Data.DbType.Int32, criteria.Userid);
                objDb.AddInParameter("IsForcedChangeSecurityCode", System.Data.DbType.Boolean, criteria.IsForcedChangeSecurityCode);
                objDb.AddInParameter("Securitycode", System.Data.DbType.String, criteria.Securitycode);
                objDb.AddInParameter("Language", System.Data.DbType.String, criteria.Language);
                objDb.AddInParameter("IP", System.Data.DbType.String, criteria.IP);
                objDb.AddOutParameter("ErrCode", System.Data.DbType.Int32, 8);
                objDb.AddOutParameter("ErrMsg", System.Data.DbType.String, 1000);
                objDb.ExecuteNonQuery();
                outputParams = objDb.GetOuputParams();
            }
        }
    }
}
