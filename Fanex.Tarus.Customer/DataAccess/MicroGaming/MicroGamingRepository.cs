namespace Fanex.Tarus.Customer.DataAccess.MicroGaming
{
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.MicroGaming;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.MicroGaming;
    using Fanex.Tarus.Customer.Resources;

    public class MicroGamingRepository : IMicroGamingRepository
    {
        public MicroGamingStatus GetStatus(GetMicroGamingStatusCriteria criteria)
        {
            MicroGamingStatus status = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var outputParams = db.GetOuputParams();
                    if (outputParams != null && outputParams.Count > 0)
                    {
                        status = new MicroGamingStatus
                        {
                            CustId = criteria.CustId,
                            Username = criteria.Username,
                            RoleId = criteria.RoleId,
                            Enable = Converter.ToBool(outputParams["supperstatus"]),
                            DownlineEnable = Converter.ToBool(outputParams["downlinestatus"])
                        };
                    }
                }
            }

            return status;
        }

        public Result UpdateStatus(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var outputParams = db.GetOuputParams();
                string message = outputParams["errmsg"].ToString();
                return string.IsNullOrEmpty(message) ?
                    new Result(0, Customer_Labels.MicroGamingStatusUpdatedSuccessfully) :
                    new Result(-1, message);
            }
        }
    }
}
