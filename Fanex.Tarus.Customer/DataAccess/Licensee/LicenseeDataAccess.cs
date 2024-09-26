using Fanex.Data;
using Fanex.Tarus.Customer.Criteria.LicenseeMember;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.DataAccess.Implement.Customer;
using Fanex.Tarus.DataAccess.Interface.Customer;
using System;

namespace Fanex.Tarus.Customer.DataAccess.Licensee
{
    public interface ILicenseeDataAccess
    {
        CheckExistAccountResult CheckExistAccount(CheckExistAccountCriteria criteria);

        (Result, int) RegisterAccount(RegisterAccountCriteria criteria);

        Result InsertMember(InsertMemberCriteria criteria);

        void RollbackInsertAccount(int custId);
    }

    public class LicenseeDataAccess : ILicenseeDataAccess
    {
        private readonly IDepositCustomerAccess _depositCustomerAccess;

        public LicenseeDataAccess() : this(null)
        {
        }

        public LicenseeDataAccess(IDepositCustomerAccess depositCustomerAccess)
        {
            _depositCustomerAccess = depositCustomerAccess ?? new DepositCustomerAccess();
        }

        public CheckExistAccountResult CheckExistAccount(CheckExistAccountCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteReader(criteria.GetSpParams());

                var ouputParams = db.GetOuputParams();
                return new CheckExistAccountResult { CustId = Convert.ToInt32(ouputParams["custid"]), MRecommend = Convert.ToInt32(ouputParams["mrecommend"]) };
            }
        }

        public (Result, int) RegisterAccount(RegisterAccountCriteria criteria)
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
                RollbackInsertAccount(criteria.CustId);
            }

            return result;
        }

        public void RollbackInsertAccount(int custId)
        {
            var criteria = new RollbackInsertAccountCriteria(custId);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }
    }
}