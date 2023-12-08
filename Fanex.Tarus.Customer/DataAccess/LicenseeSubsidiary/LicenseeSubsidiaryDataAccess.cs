using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Resources;
using Fanex.Tarus.DataAccess.Implement.Customer;
using Fanex.Tarus.DataAccess.Interface.Customer;
using System;
using System.Collections.Generic;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeSubsidiary
{
    internal class LicenseeSubsidiaryDataAccess : ILicenseeSubsidiaryDataAccess
    {
        private readonly IDepositCustomerAccess _depositCustomerAccess;

        public LicenseeSubsidiaryDataAccess() : this(null) { }

        public LicenseeSubsidiaryDataAccess(IDepositCustomerAccess depositCustomerAccess)
        {
            _depositCustomerAccess = depositCustomerAccess ?? new DepositCustomerAccess();
        }

        public IEnumerable<string> Generate(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                yield break;
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return reader["username"].ToString();
                    }
                }
            }
        }

        public SubsidiaryResult Create(ICriteria criteria)
            => ExecuteNonQuery(criteria);

        private SubsidiaryResult ExecuteNonQuery(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                return new SubsidiaryResult(400, LicenseeDcsSettingLabels.BadRequest);
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                var insertBalanceCustList = db.GetParameterValue("insbalcustlist").ToString();
                var insBalExceptCustList = db.GetParameterValue("insbalexceptcustlist").ToString();
                int custid = Convert.ToInt32(outputParams["custid"]);
                _depositCustomerAccess.InsertBalanceWhenCreateNewAgent(custid, insertBalanceCustList, insBalExceptCustList);
                return new SubsidiaryResult(
                    Convert.ToInt32(outputParams["errcode"]),
                    outputParams["errmsg"].ToString(),
                    custid,
                    Convert.ToInt32(outputParams["existedcustid"]));
            }
        }
    }
}
