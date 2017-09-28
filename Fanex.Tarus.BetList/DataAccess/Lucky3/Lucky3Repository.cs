namespace Fanex.Tarus.BetList.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using BetDetails.Lucky3.Criteria;
    using BetDetails.Lucky3.DataAccesses;
    using BetDetails.Lucky3.Models;
    using Criteria.Lucky3;
    using Data;

    public class Lucky3Repository : ILucky3DataAccess
    {
        public IEnumerable<Lucky3DetailModel> GetLucky3Detail(IGetLucky3DetailParameter criteria)
        {
            IList<Lucky3DetailModel> details = new List<Lucky3DetailModel>();
            var parameter = new GetLucky3DetailCriteria(criteria);

            using (var db = new ObjectDb(parameter.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(parameter.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        details.Add(ToLucky3DetailModel(reader));
                    }
                }
            }

            return details;
        }

        private static Lucky3DetailModel ToLucky3DetailModel(IDataReader reader)
        {
            var model = new Lucky3DetailModel
            {
                MasterCommission = Convert.ToDecimal(reader["AComm"]),
                MasterDiscount = Convert.ToDecimal(reader["aDiscount"]),
                Bets = Convert.ToDecimal(reader["Bets"]),
                BetType = Convert.ToInt32(reader["BetType"]),
                CustomerId = Convert.ToInt32(reader["CustId"]),
                Hdp1 = Convert.ToDecimal(reader["Hdp1"]),
                Hits = Convert.ToInt32(reader["Hits"]),
                AgentCommission = Convert.ToDecimal(reader["Comm"]),
                AgentDiscount = Convert.ToDecimal(reader["Discount"]),
                MatchId = Convert.ToInt32(reader["MatchId"]),
                Prize = Convert.ToInt32(reader["Prize"]),
                PrizeAmount = Convert.ToDecimal(reader["PrizeAmount"]),
                Stake = Convert.ToDecimal(reader["Stake"]),
                Status = reader["Status"].ToString(),
                StatusId = Convert.ToInt32(reader["StatusId"]),
                SuperCommission = Convert.ToDecimal(reader["SComm"]),
                SuperDiscount = Convert.ToDecimal(reader["sDiscount"]),
                TransactionDescription = reader["TransDesc"].ToString(),
                TransactionId = Convert.ToInt64(reader["TransId"])
            };

            if (reader["PlayerComm"] != DBNull.Value)
            {
                model.PlayerCommission = Convert.ToDecimal(reader["PlayerComm"]);
            }

            if (reader["Winlost"] != DBNull.Value)
            {
                model.Winlost = Convert.ToDecimal(reader["Winlost"]);
            }

            return model;
        }
    }
}
