namespace Fanex.Tarus.Customer.DataAccess.LicenseeMember
{
    using System;
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.LicenseeMember;

    public class LicenseeMemberDataAccess : ILicenseeMemberDataAccess
    {
        public int CheckExistAgent(CheckExistAgentCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteReader(criteria.GetSpParams());

                var ouputParams = db.GetOuputParams();
                return Convert.ToInt32(ouputParams["custid"]);
            }
        }

        public Result InsertMember(InsertMemberCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteReader(criteria.GetSpParams());

                var ouputParams = db.GetOuputParams();

                return new Result(Convert.ToInt32(ouputParams["errcode"]), ouputParams["errmsg"].ToString());
            }
        }
    }
}
