// --------------------------------------------------------------------------
// <copyright file="ViewKenoPTCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    using DNAide.Data;
    using Fanex.Data;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;

    /// <summary>
    /// View Keno PT Repository.
    /// </summary>
    public class ViewKenoPTRepository : IViewKenoPTRepository
    {
        public DbResult GetPT(ViewKenoPTCriteria criteria)
        {
            DbResult result = null;
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey(), 600))
            {
                db.AssignParamValues(criteria.GetSpParam());
                var ds = db.ExecuteDataSet();
                var output = db.GetOuputParams();
                result = new DbResult(output, ds);
            }

            return result;
        }
    }
}
