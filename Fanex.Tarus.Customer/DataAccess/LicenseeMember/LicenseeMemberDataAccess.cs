namespace Fanex.Tarus.Customer.DataAccess.LicenseeMember
{
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.LicenseeMember;
    using Fanex.Tarus.DataAccess.Implement.Customer;
    using Fanex.Tarus.DataAccess.Interface.Customer;
    using Fanex.Tarus.Infrastructure.Extensions;
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

        public SiteDB GetSiteDBConfig(string username)
        {
            var criteria = new GetDepServerDBConfigByUsernameCriteria(username);
            SiteDB siteDBConfig = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                siteDBConfig = db.QueryEntity<SiteDB>(criteria);
            }

            return siteDBConfig;
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
                custId = ouputParams["custid"] != DBNull.Value ? Convert.ToInt32(ouputParams["custid"]) : 0;
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
            else
            {
                RollbackInsertMember(criteria.CustId);
            }

            return result;
        }

        private void RollbackInsertMember(int custId)
        {
            var criteria = new RollbackInsertMemberCriteria(custId);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }
    }
}