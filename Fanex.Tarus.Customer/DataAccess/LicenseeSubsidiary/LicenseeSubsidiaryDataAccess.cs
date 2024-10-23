using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.DataAccess.Licensee;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Resources;
using System;
using System.Collections.Generic;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeSubsidiary
{
    internal class LicenseeSubsidiaryDataAccess : LicenseeDataAccess, ILicenseeSubsidiaryDataAccess
    {
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
                int custid = Convert.ToInt32(outputParams["custid"]);
                return new SubsidiaryResult(
                    Convert.ToInt32(outputParams["errcode"]),
                    outputParams["errmsg"].ToString(),
                    custid,
                    Convert.ToInt32(outputParams["existedcustid"]));
            }
        }
    }
}