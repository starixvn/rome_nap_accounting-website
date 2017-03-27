using System;
using System.Collections.Generic;
using System.Data;
using Fanex.BetList.Accounting.Models;
using Fanex.BetList.Core.Entities;
using Fanex.Data;
using Fanex.NAP.Framework.Data;

namespace Fanex.BetList.Accounting.Repositories
{
    public class BetListDetailRepository : IBetListDetailRepository
    {
        public Ticket GetBetListDetail(ICriteria criteria)
        {
            IList<TicketDetail> ticketDetails = new List<TicketDetail>();
            IList<TicketData> ticketData = new List<TicketData>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        ticketDetails.Add(BuildTicketDetail(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            ticketData.Add(BuildTicketData(reader));
                        }
                    }
                }
            }

            throw new NotImplementedException();
        }

        private TicketDetail BuildTicketDetail(IDataReader reader, int custId = 0)
        {
            return new TicketDetail
            {
                Ticket = BuildCoreTicket(reader, custId),
                ChoiceHtml = string.Empty,
                OddHtml = string.Empty,
                StakeHtml = string.Empty,
                IsShowOtExRate = Convert.ToBoolean(reader["isShowOTExRate"]),
                VirtualRate = Convert.ToDecimal(reader["virtualrate"]),
                AgentPositionTaking2 = Convert.ToDecimal(reader["apositiontaking2"]),
                MasterPositionTaking2 = Convert.ToDecimal(reader["mpositiontaking2"]),
                SuperPositionTaking2 = Convert.ToDecimal(reader["spositiontaking2"]),
                BetFrom = reader["betfrom"].ToString(),
                AllowDelete = Convert.ToBoolean(reader["allowdelete"])
            };
        }

        private ITicket BuildCoreTicket(IDataReader reader, int custId = 0)
        {
            return new Ticket
            {
                CustId = custId,
                ActualStake = Convert.ToDecimal(reader["actual_stake"]),
                AgentWinlost = Convert.ToDecimal(reader["awinlost"]),
                AgentPositionTaking = Convert.ToDecimal(reader["apositiontaking"]),
                Comm = Convert.ToDecimal(reader["comm"]),
                AgentDiscount = Convert.ToDecimal(reader["discount"]),

                AwayId = Convert.ToInt64(reader["awayid"]),
                BetId = Convert.ToInt64(reader["betid"]),
                BetTeam = Convert.ToString(reader["betteam"]),
                BetTypeId = Convert.ToInt32(reader["bettype"]),
                CommStatus = Convert.ToString(reader["comstatus"]),
                Handicap1 = Convert.ToDecimal(reader["hdp1"]),
                Handicap2 = Convert.ToDecimal(reader["hdp2"]),
                HomeId = Convert.ToInt64(reader["homeid"]),
                EventDate = Convert.ToDateTime(reader["eventdate"]),
                IP = Convert.ToString(reader["ip"]),
                IsLive = Convert.ToBoolean(reader["liveindicator"]),
                IsNeutral = Convert.ToBoolean(reader["isNeutral"]),
                LeagueId = Convert.ToInt32(reader["leagueid"]),
                LiveHomeScore = Convert.ToInt32(reader["livehomescore"]),
                LiveAwayScore = Convert.ToInt32(reader["liveawayscore"]),

                MasterWinlost = Convert.ToDecimal(reader["mwinlost"]),
                MasterPositionTaking = Convert.ToDecimal(reader["mpositiontaking"]),
                AgentComm = Convert.ToDecimal(reader["acomm"]),
                MasterDiscount = Convert.ToDecimal(reader["adiscount"]),

                MasterOdds = Convert.ToDecimal(reader["modds"]),
                MatchCode = Convert.ToString(reader["matchcode"]),
                MatchId = Convert.ToInt32(reader["matchid"]),
                Odds = Convert.ToDecimal(reader["odds"]),
                OddsType = Convert.ToString(reader["OddsType"]),
                PlayerComm = Convert.ToDecimal(reader["commission"]),
                Race = Convert.ToString(reader["race"]),
                RefNo = Convert.ToString(reader["refno"]),
                ShowTime = Convert.ToString(reader["showtime"]),
                SportTypeId = Convert.ToInt32(reader["sporttype"]),
                Stake = Convert.ToDecimal(reader["stake"]),
                Status = Convert.ToString(reader["status"]),

                SuperWinlost = Convert.ToDecimal(reader["swinlost"]),
                SuperPositionTaking = Convert.ToDecimal(reader["spositiontaking"]),
                SuperComm = Convert.ToDecimal(reader["scomm"]),
                SuperDiscount = Convert.ToDecimal(reader["sdiscount"]),

                TransDate = Convert.ToDateTime(reader["transdate"]),
                TransId = Convert.ToInt64(reader["transid"]),
                UserName = Convert.ToString(reader["username"]),
                Winlost = Convert.ToDecimal(reader["winlost"]),
                WinlostDate = Convert.ToDateTime(reader["winlostdate"]),
                TransDesc = Convert.ToString(reader["transdesc"]),

                BetCheck = Convert.ToString(reader["betcheck"]),
                OddsSpread = Convert.ToDecimal(reader["OddsSpread"]),

                OriginalBetType = Convert.ToInt32(reader["OriginalBetType"]),
                BetdaqId = Convert.ToInt64(reader["BetdaqId"]),
                ExtraInfo = Convert.ToString(reader["ExtraInfo"])
            };
        }

        private TicketData BuildTicketData(IDataReader reader)
        {
            return new TicketData
            {
                BetTypeId = Convert.ToInt32(reader["bettype"]),
                RefNo = Convert.ToString(reader["refno"]),
                Status = Convert.ToString(reader["status"]),
                Stake = Convert.ToDecimal(reader["stake"]),
                TransDesc = Convert.ToString(reader["TransDesc"]),
                BetTeam = Convert.ToString(reader["betteam"])
            };
        }
    }
}
