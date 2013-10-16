// --------------------------------------------------------------------------
// <copyright file="EditLicenseeRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.EditLicensee
{
    using System.Collections;
    using System.Data;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.EditLicensee;
    using Fanex.Tarus.Customer.Entities.EditLicensee;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditLicenseeRepository : IEditLicenseeRepository
    {
        public LicenseeInfo GetLicenseeInfo(GetLicenseeInfoCriteria criteria)
        {
            DataTable dataTable = new DataTable();
            Hashtable param = new Hashtable();
            param.Add("licenseename", criteria.LicenseeName);
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(param);
                var dataset = objDb.ExecuteDataSet();
                if (dataset != null && dataset.Tables.Count > 0)
                {
                    dataTable = dataset.Tables[0];
                }
            }

            LicenseeInfo result = new LicenseeInfo();
            return result;
        }
    }
}
