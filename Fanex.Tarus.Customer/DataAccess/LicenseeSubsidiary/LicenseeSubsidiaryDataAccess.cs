using System;
using System.Collections.Generic;
using Fanex.Data;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Resources;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeSubsidiary
{
    internal class LicenseeSubsidiaryDataAccess : ILicenseeSubsidiaryDataAccess
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

        public Result Create(ICriteria criteria)
            => ExecuteNonQuery(criteria);

        private static Result ExecuteNonQuery(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                return new Result(400, LicenseeDcsSettingLabels.BadRequest);
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    outputParams["errmsg"].ToString());
            }
        }
    }
}
