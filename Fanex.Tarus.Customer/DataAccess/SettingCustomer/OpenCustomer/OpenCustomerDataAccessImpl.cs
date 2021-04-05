namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.OpenCustomer
{
    using Common;
    using Common.Extensions;
    using Criteria.OpenCustomer;
    using Data;
    using Entities.SettingCustomer.OpenCustomer;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class OpenCustomerDataAccessImpl : OpenCustomerDataAccess
    {
        public IEnumerable<SingleOpenAffectedDownline> GetSingleOpenAffectedDownlines(GetSingleOpenCustomerInfoCriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            yield return BuildSingleOpenAffectedDownline(reader);
                        }
                    }
                }
            }
        }

        public Tuple<int, List<SingleOpenDownline>> GetSingleOpenDownlines(GetSingleOpenDownlinesInfoCriteria criteria)
        {
            var singleOpenDownlines = new List<SingleOpenDownline>();
            var totalRows = 0;

            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            singleOpenDownlines.Add(BuildSingleOpenDownline(reader));
                        }
                    }

                    totalRows = Converter.ToInt(db.GetOuputParams()["totalrows"]);
                }
            }

            return Tuple.Create(totalRows, singleOpenDownlines);
        }

        private SingleOpenAffectedDownline BuildSingleOpenAffectedDownline(IDataReader reader)
            => new SingleOpenAffectedDownline(
                reader.ToInt("roleid"),
                reader.ToInt("cntAffected"),
                reader.ToInt("cntUnchanged"));

        private SingleOpenDownline BuildSingleOpenDownline(IDataReader reader)
            => new SingleOpenDownline(
                reader.ToInt("roleid"),
                reader.ToString("username"));
    }
}