namespace Fanex.Tarus.BetList.DataAccess
{
    using Fanex.Data;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.Models.Exchange;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface IExchangeBetListRepository
    {
        ExchangeViewModel GetBetList(ExchangeBetListCriteria criteria);
    }

    public class ExchangeBetListRepository : IExchangeBetListRepository
    {
        public ExchangeViewModel GetBetList(ExchangeBetListCriteria criteria)
        {
            var summaries = new List<ExchangeSummaryTicket>();
            var details = new List<ExchangeDetailTicket>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        summaries.Add(GetSummaryTicket(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            details.Add(GetDetailTicket(reader));
                        }
                    }
                }

                var ouputParams = db.GetOuputParams();

                if (ouputParams != null && ouputParams.ContainsKey("totalrows"))
                {
                    criteria.TotalRows = Convert.ToInt32(ouputParams["totalrows"]);
                }
                if (ouputParams != null && ouputParams.ContainsKey("grandtotal"))
                {
                    criteria.GrandTotal = Convert.ToDecimal(ouputParams["grandtotal"]);
                }

            }

            return new ExchangeViewModel(summaries, details, criteria.GrandTotal, criteria.TotalRows);
        }

        private ExchangeSummaryTicket GetSummaryTicket(IDataReader reader)
        {
            return new ExchangeSummaryTicket
            {
                Commission = Convert.ToDecimal(reader["Commission"]),
                EventId = Convert.ToInt32(reader["EventId"]),
                EventName = Convert.ToString(reader["EventName"]),
                EventTypeName = Convert.ToString(reader["EventTypeName"]),
                MarketId = Convert.ToString(reader["MarketId"]),
                MarketName = Convert.ToString(reader["MarketName"]),
                WinLost = Convert.ToDecimal(reader["WinLost"]),
                SettledDate = Convert.ToDateTime(reader["SettledDate"])
            };
        }

        private ExchangeDetailTicket GetDetailTicket(IDataReader reader)
        {
            return new ExchangeDetailTicket
            {
                BetId = Convert.ToInt64(reader["BetId"]),
                BetPlaced = Convert.ToDateTime(reader["BetPlaced"]),
                EventId = Convert.ToInt32(reader["EventId"]),
                MarketId = Convert.ToString(reader["MarketId"]),
                OddsAvg = Convert.ToDecimal(reader["OddsAvg"]),
                SelectionName = Convert.ToString(reader["SelectionName"]),
                Stake = Convert.ToDecimal(reader["Stake"]),
                Type = Convert.ToString(reader["Type"]),
                Winlost = Convert.ToDecimal(reader["WinLost"])
            };
        }
    }
}
