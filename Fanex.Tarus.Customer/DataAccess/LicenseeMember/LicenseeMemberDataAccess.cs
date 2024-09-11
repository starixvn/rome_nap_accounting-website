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

        public LicenseeMemberDataAccess() : this(null)
        {
        }

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

        public (Result, int) RegisterMember(RegisterMemberCriteria criteria)
        {
            Result result;
            int custId = 0;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteReader(criteria.GetSpParams());

                var ouputParams = db.GetOuputParams();
                custId = Convert.ToInt32(ouputParams["custid"]);
                result = new Result(Convert.ToInt32(ouputParams["errcode"]), ouputParams["errmsg"].ToString());
            }

            return (result, custId);
        }

        public Result InsertMember(InsertMemberCriteria criteria)
        {
            Result result;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteReader(criteria.GetSpParams());

                var ouputParams = db.GetOuputParams();
                result = new Result(Convert.ToInt32(ouputParams["errcode"]), ouputParams["errmsg"].ToString());
            }

            if (result.IsSuccess)
            {
                _depositCustomerAccess.InsertBalanceWhenCreateNewMember(criteria.CustId, criteria.Username);
            }

            return result;
        }
    }
}