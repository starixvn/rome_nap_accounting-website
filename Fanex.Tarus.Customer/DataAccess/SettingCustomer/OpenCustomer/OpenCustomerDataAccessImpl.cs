namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.OpenCustomer
{
    using Common;
    using Criteria.OpenCustomer;
    using Data;
    using Entities.SettingCustomer.OpenCustomer;
    using Fanex.CustomerSetting.Module.Core;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using ViewModels.CustomerSetting.OpenCustomer;

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

        public ClosedMatrixViewModel GetClosedMatrixInfo(GetCloseInfoMatrixCriteria criteria)
        {
            var customerInfoList = new List<CustomerInfo>();
            var closedMatrixList = new List<ClosedMatrix>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        customerInfoList.Add(BuildCustomerInfo(reader));
                        closedMatrixList.Add(BuildClosedMatrix(reader));
                    }
                }
            }

            return new ClosedMatrixViewModel(customerInfoList, closedMatrixList);
        }

        private SingleOpenAffectedDownline BuildSingleOpenAffectedDownline(IDataReader reader)
            => new SingleOpenAffectedDownline(
                Utils.ToInt32(reader["roleid"]),
                Utils.ToInt32(reader["cntAffected"]),
                Utils.ToInt32(reader["cntUnchanged"]));

        private SingleOpenDownline BuildSingleOpenDownline(IDataReader reader)
            => new SingleOpenDownline(
                Utils.ToInt32(reader["roleid"]),
                Utils.ToString(reader["username"]));

        private CustomerInfo BuildCustomerInfo(IDataReader reader)
            => new CustomerInfo(
                 Utils.ToInt32(reader["roleid"]),
                 Utils.ToString(reader["username"]),
                 Utils.ToInt32(reader["custid"])
                );

        private ClosedMatrix BuildClosedMatrix(IDataReader reader)
            => new ClosedMatrix(
                Utils.ToInt32(reader["roleid"]),
                Utils.ToBoolean(reader["closed"]),
                Utils.ToBoolean(reader["ClosedBySuper"]),
                Utils.ToBoolean(reader["ClosedByMaster"]),
                Utils.ToBoolean(reader["ClosedByAgent"]),
                Utils.ToBoolean(reader["ClosedMember"]));
    }
}