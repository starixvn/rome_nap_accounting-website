using Fanex.Data;
using Fanex.Tarus.Admin.Criteria.SiteManagement;
using Fanex.Tarus.Admin.Entities;

namespace Fanex.Tarus.Admin.DataAccess.SiteManagement
{
    public class SiteManagementDataAccess : ISiteManagementDataAccess
    {
        public Result ExecuteNonQueryWithResult(CriteriaBase criteria)
        {
            Result result = null;

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSPParams());
                var outputParams = db.GetOuputParams();

                if (outputParams != null)
                {
                    result = new Result((int)outputParams["errcode"], outputParams["errmsg"].ToString());
                }
            }

            return result;
        }
    }
}
