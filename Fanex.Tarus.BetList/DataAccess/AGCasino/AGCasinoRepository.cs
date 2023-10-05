namespace Fanex.Tarus.BetList.DataAccess
{
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.BetList.Criteria;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Sunplus.Accounting.Core.Extensions;

    public class AGCasinoRepository : IAGCasinoRepository
    {
        public IEnumerable<TicketDetail> BetDetails(GetAGCasinoBetDetailsCriteria criteria)
        {
            var result = new List<TicketDetail>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new TicketDetail
                        {
                            Ticket = new Ticket
                            {
                                CustId = Converter.To<long>(reader["CustId"]),
                                TransId = Converter.To<long>(reader["transid"]),
                                SportTypeId = Converter.ToInt(reader["SportType"]),
                                BetTypeId = Converter.ToInt(reader["BetType"]),
                                TransDate = Converter.ToDateTime(reader["transdate"]),
                                WinlostDate = Converter.ToDateTime(reader["winlostdate"]),

                                Status = Converter.ToString2(reader["status"]),
                                StatusId = Converter.ToInt(reader["statusid"]),

                                Odds = Converter.ToDecimal(reader["odds"]),
                                Stake = Converter.ToDecimal(reader["stake"]),
                                ActualStake = Converter.ToDecimal(reader["actual_stake"]),
                                Winlost = Converter.ToDecimal(reader["WinLost"]),
                                IP = Converter.ToString2(reader["ip"]),
                                TransDesc = Converter.ToString2(reader["transdesc"]),

                                PlayerComm = Converter.ToDecimal(reader["commission"]),
                                AgentWinlost = Converter.ToDecimal(reader["awinlost"]),
                                AgentPositionTaking = Converter.ToDecimal(reader["apositiontaking"]),
                                Comm = Converter.ToDecimal(reader["comm"]),
                                AgentDiscount = Converter.ToDecimal(reader["discount"]),

                                MasterWinlost = Converter.ToDecimal(reader["mwinlost"]),
                                MasterPositionTaking = Converter.ToDecimal(reader["mpositiontaking"]),
                                AgentComm = Converter.ToDecimal(reader["acomm"]),
                                MasterDiscount = Converter.ToDecimal(reader["adiscount"]),

                                SuperWinlost = Converter.ToDecimal(reader["swinlost"]),
                                SuperPositionTaking = Converter.ToDecimal(reader["spositiontaking"]),
                                SuperComm = Converter.ToDecimal(reader["scomm"]),
                                SuperDiscount = Converter.ToDecimal(reader["sdiscount"]),

                                MatchCode = Converter.ToString2(reader["matchcode"]),
                                BetCheck = Converter.ToString2(reader["betcheck"]),
                                BetTeam = HasColumn(reader, "BetTeam") ? Converter.ToString2(reader["BetTeam"]) : string.Empty,
                                Currency = HasColumn(reader, "Currency") ? Converter.ToString2(reader["Currency"]) : string.Empty,
                                CurrencyId = HasColumn(reader, "CurrencyId") ? Converter.ToInt(reader["CurrencyId"]) : 0,
                                SiteId = HasColumn(reader, "SiteId") ? Converter.ToString2(reader["SiteId"]) : "",
                                LeagueId = Converter.ToInt(reader["leagueid"]),
                                EventDate = HasColumn(reader, "EventDate") ? Converter.ToDateTime(reader["EventDate"]) : (DateTime?)null,
                            },
                            ChoiceHtml = string.Empty,
                            OddHtml = string.Empty,
                            StakeHtml = string.Empty,
                            IP = Converter.ToString2(reader["ip"]),
                        });
                    }
                }

                criteria.TotalRows = Converter.ToInt(objDb.GetOuputParams()["totalrows"]);
            }

            return result;
        }

        public IEnumerable<TicketDetail> BetDetails(GetAGCasinoBetDetailsInSiteDBCriteria criteria, string AGIP)
        {
            var result = new List<TicketDetail>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReaderScaleoutDB(criteria.GetSpParams(), AGIP))
                {
                    while (reader.Read())
                    {
                        result.Add(new TicketDetail
                        {
                            Ticket = new Ticket
                            {
                                CustId = Converter.To<long>(reader["CustId"]),
                                TransId = Converter.To<long>(reader["transid"]),
                                SportTypeId = Converter.ToInt(reader["SportType"]),
                                BetTypeId = Converter.ToInt(reader["BetType"]),
                                TransDate = Converter.ToDateTime(reader["transdate"]),
                                WinlostDate = Converter.ToDateTime(reader["winlostdate"]),

                                Status = Converter.ToString2(reader["status"]),
                                StatusId = Converter.ToInt(reader["statusid"]),

                                Odds = Converter.ToDecimal(reader["odds"]),
                                Stake = Converter.ToDecimal(reader["stake"]),
                                ActualStake = Converter.ToDecimal(reader["actual_stake"]),
                                Winlost = Converter.ToDecimal(reader["WinLost"]),
                                IP = Converter.ToString2(reader["ip"]),
                                TransDesc = Converter.ToString2(reader["transdesc"]),

                                PlayerComm = Converter.ToDecimal(reader["commission"]),
                                AgentWinlost = Converter.ToDecimal(reader["awinlost"]),
                                AgentPositionTaking = Converter.ToDecimal(reader["apositiontaking"]),
                                Comm = Converter.ToDecimal(reader["comm"]),
                                AgentDiscount = Converter.ToDecimal(reader["discount"]),

                                MasterWinlost = Converter.ToDecimal(reader["mwinlost"]),
                                MasterPositionTaking = Converter.ToDecimal(reader["mpositiontaking"]),
                                AgentComm = Converter.ToDecimal(reader["acomm"]),
                                MasterDiscount = Converter.ToDecimal(reader["adiscount"]),

                                SuperWinlost = Converter.ToDecimal(reader["swinlost"]),
                                SuperPositionTaking = Converter.ToDecimal(reader["spositiontaking"]),
                                SuperComm = Converter.ToDecimal(reader["scomm"]),
                                SuperDiscount = Converter.ToDecimal(reader["sdiscount"]),

                                MatchCode = Converter.ToString2(reader["matchcode"]),
                                BetCheck = Converter.ToString2(reader["betcheck"]),
                                BetTeam = HasColumn(reader, "BetTeam") ? Converter.ToString2(reader["BetTeam"]) : string.Empty,
                                Currency = HasColumn(reader, "Currency") ? Converter.ToString2(reader["Currency"]) : string.Empty,
                                CurrencyId = HasColumn(reader, "CurrencyId") ? Converter.ToInt(reader["CurrencyId"]) : 0,
                                SiteId = HasColumn(reader, "SiteId") ? Converter.ToString2(reader["SiteId"]) : "",
                                LeagueId = Converter.ToInt(reader["leagueid"]),
                                EventDate = HasColumn(reader, "EventDate") ? Converter.ToDateTime(reader["EventDate"]) : (DateTime?)null,
                                JackpotInfo = Converter.ToString2(reader["JackpotInfo"]),
                            },
                            ChoiceHtml = string.Empty,
                            OddHtml = string.Empty,
                            StakeHtml = string.Empty
                        });
                    }
                }

                criteria.TotalRows = Converter.ToInt(objDb.GetOuputParams()["totalrows"]);
            }

            return result;
        }

        public IEnumerable<TicketDetail> BetDetails(GetBBinBetDetailsCriteria criteria)
        {
            var result = new List<TicketDetail>();

            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        result.Add(new TicketDetail
                        {
                            Ticket = new Ticket
                            {
                                CustId = Converter.To<long>(reader["CustId"]),
                                TransId = Converter.To<long>(reader["transid"]),
                                SportTypeId = Converter.ToInt(reader["SportType"]),
                                BetTypeId = Converter.ToInt(reader["BetType"]),
                                TransDate = Converter.ToDateTime(reader["transdate"]),
                                WinlostDate = Converter.ToDateTime(reader["winlostdate"]),

                                Status = Converter.ToString2(reader["status"]),
                                StatusId = Converter.ToInt(reader["statusid"]),

                                Odds = Converter.ToDecimal(reader["odds"]),
                                Stake = Converter.ToDecimal(reader["stake"]),
                                ActualStake = Converter.ToDecimal(reader["actual_stake"]),
                                Winlost = Converter.ToDecimal(reader["WinLost"]),
                                IP = Converter.ToString2(reader["ip"]),
                                TransDesc = Converter.ToString2(reader["transdesc"]),

                                PlayerComm = Converter.ToDecimal(reader["commission"]),
                                AgentWinlost = Converter.ToDecimal(reader["awinlost"]),
                                AgentPositionTaking = Converter.ToDecimal(reader["apositiontaking"]),
                                Comm = Converter.ToDecimal(reader["comm"]),
                                AgentDiscount = Converter.ToDecimal(reader["discount"]),

                                MasterWinlost = Converter.ToDecimal(reader["mwinlost"]),
                                MasterPositionTaking = Converter.ToDecimal(reader["mpositiontaking"]),
                                AgentComm = Converter.ToDecimal(reader["acomm"]),
                                MasterDiscount = Converter.ToDecimal(reader["adiscount"]),

                                SuperWinlost = Converter.ToDecimal(reader["swinlost"]),
                                SuperPositionTaking = Converter.ToDecimal(reader["spositiontaking"]),
                                SuperComm = Converter.ToDecimal(reader["scomm"]),
                                SuperDiscount = Converter.ToDecimal(reader["sdiscount"]),

                                MatchCode = Converter.ToString2(reader["matchcode"]),
                                BetCheck = Converter.ToString2(reader["betcheck"]),
                                BetTeam = HasColumn(reader, "BetTeam") ? Converter.ToString2(reader["BetTeam"]) : string.Empty,
                                Currency = HasColumn(reader, "Currency") ? Converter.ToString2(reader["currency"]) : string.Empty,
                                SiteId = Converter.ToString2(reader["SiteId"]),
                                CurrencyId = Converter.ToInt(reader["CurrencyId"]),
                                JackpotInfo = Converter.ToString2(reader["JackpotInfo"])
                            },
                            ChoiceHtml = string.Empty,
                            OddHtml = string.Empty,
                            StakeHtml = string.Empty
                        });
                    }
                }

                criteria.TotalRows = Converter.ToInt(objDb.GetOuputParams()["totalrows"]);
            }

            return result;
        }

        private bool HasColumn(IDataRecord dataRecord, string columnName)
        {
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                if (dataRecord.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
