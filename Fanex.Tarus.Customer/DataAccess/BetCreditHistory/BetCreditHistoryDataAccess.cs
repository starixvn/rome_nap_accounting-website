using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Tarus.Customer.Criteria.BetCreditHistory;
using Fanex.Tarus.Customer.Entities.BetCreditHistory;

namespace Fanex.Tarus.Customer.DataAccess.BetCreditHistory
{
    public interface IBetCreditHistoryDataAccess
    {
        IEnumerable<BetCreditHistoryItem> GetBetCreditHistory(GetBetCreditHistoryCriteria criteria);
    }

    public class BetCreditHistoryDataAccess : IBetCreditHistoryDataAccess
    {
        public IEnumerable<BetCreditHistoryItem> GetBetCreditHistory(GetBetCreditHistoryCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var outputParams = db.GetOuputParams();
                    criteria.ErrorCode = Convert.ToInt32(outputParams["errorcode"]);
                    criteria.ErrorMess = outputParams["errormess"].ToString();

                    if (criteria.ErrorCode == 0)
                    {
                        while (reader.Read())
                        {
                            yield return GetBetCreditHistoryItem(reader);
                        }
                    }
                }
            }
        }

        private static BetCreditHistoryItem GetBetCreditHistoryItem(IDataReader reader)
            => new BetCreditHistoryItem(
                Convert.ToDateTime(reader["actiondate"]),
                ToNullableDateTime(reader["winlostdate"]),
                ToNullableLong(reader["refno"]),
                ToNullableDecimal(reader["actual_stake"]),
                ToNullableDecimal(reader["winloss"]),
                ToNullableDecimal(reader["playercomm"]),
                ToNullableDecimal(reader["balancechanged"]),
                ToNullableDecimal(reader["newbalance"]),
                Convert.ToDecimal(reader["givencredit"]),
                ToNullableDecimal(reader["betcredit"]),
                reader["action"].ToString());

        private static DateTime? ToNullableDateTime(object value)
            => value == null || value == DBNull.Value
            ? default(DateTime?)
            : Convert.ToDateTime(value);

        private static long? ToNullableLong(object value)
            => value == null || value == DBNull.Value
            ? default(long?)
            : Convert.ToInt64(value);

        private static decimal? ToNullableDecimal(object value)
            => value == null || value == DBNull.Value
            ? default(decimal?)
            : Convert.ToDecimal(value);
    }
}
