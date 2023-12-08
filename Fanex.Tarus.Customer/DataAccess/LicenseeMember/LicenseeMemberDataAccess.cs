namespace Fanex.Tarus.Customer.DataAccess.LicenseeMember
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.LicenseeMember;
    using Fanex.Tarus.DataAccess.Implement.Customer;
    using Fanex.Tarus.DataAccess.Interface.Customer;
    using System;

    public class LicenseeMemberDataAccess : ILicenseeMemberDataAccess
    {
        private readonly IDepositCustomerAccess _depositCustomerAccess;

        public LicenseeMemberDataAccess() : this(null) { }

        public LicenseeMemberDataAccess(IDepositCustomerAccess depositCustomerAccess)
        {
            _depositCustomerAccess = depositCustomerAccess ?? new DepositCustomerAccess();
        }

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
            Result result;
            int newMemberId = 0;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteReader(criteria.GetSpParams());

                var ouputParams = db.GetOuputParams();
                newMemberId = Convert.ToInt32(ouputParams["custid"]);
                result = new Result(Convert.ToInt32(ouputParams["errcode"]), ouputParams["errmsg"].ToString());
            }

            if (newMemberId > 0)
            {
                _depositCustomerAccess.InsertBalanceWhenCreateNewMember(newMemberId, criteria.Username);
            }

            return result;
        }
    }
}
