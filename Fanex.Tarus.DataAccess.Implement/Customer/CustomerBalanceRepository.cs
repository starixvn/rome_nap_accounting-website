using System.Data;
using Fanex.CustomerSetting.Module.Core;
using Fanex.Data;
using Fanex.Tarus.DataAccess.Interface.Customer;
using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Web.Models.Customer.Balance;
using Sunplus.Accounting.Core.Common;

namespace Fanex.Tarus.DataAccess.Implement.Customer
{
    public class CustomerBalanceRepository : ICustomerBalanceRepository
    {
        public CustomerBalanceViewModel GetCustomerBalance(CustomerBalanceCriteria criteria)
        {
            var result = new CustomerBalanceViewModel();

            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                var reader = db.ExecuteReader();

                while (reader.Read())
                {
                    result.CustomerBalance.Add(ParseCustomerBalance(reader));
                }

                reader.Close();
                result.TotalRows = Utils.ToInt32(db.GetOuputParams()["totalrows"]);
            }

            return result;
        }

        private CustomerBalance ParseCustomerBalance(IDataReader reader)
        {
            CustomerBalance customerBalance = new CustomerBalance()
            {
                RowNum = Utils.ToInt32(reader["RowNum"]),
                UserName = Utils.ToString(reader["UserName"]),
                CustId = Utils.ToInt32(reader["CustId"]),
                Closed = Utils.ToBoolean(reader["Closed"]),
                FirstName = Utils.ToString(reader["FirstName"]),
                LastName = Utils.ToString(reader["LastName"]),
                Outstanding = Utils.ToDouble(reader["Outstanding"]),
                Balance = Utils.ToDouble(reader["Balance"]),
                YesterdayBalance = Utils.ToDouble(reader["YesterdayBalance"]),
                Cash = Utils.ToDouble(reader["Cash"]),
                YesterdayCash = Utils.ToDouble(reader["YesterdayCash"]),
                GivenCredit = Utils.ToDouble(reader["GivenCredit"]),
                AvailableCredit = Utils.ToDouble(reader["AvailableCredit"]),
                Recommend = Utils.ToDouble(reader["Recommend"]),
                MRecommend = Utils.ToDouble(reader["MRecommend"]),
                SyncCasino = Utils.ToBoolean(reader["SyncCasino"])
            };

            if (customerBalance.Closed)
            {
                customerBalance.ClosedReason = new CustomerClosedReason(reader["closedreason"]);
            }

            return customerBalance;
        }
    }
}
