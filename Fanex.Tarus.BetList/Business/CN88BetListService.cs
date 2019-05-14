namespace Fanex.Tarus.BetList.Business
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Criteria;
    using DataAccess;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Models.CN88;

    public class CN88BetListService : ICN88BetListService
    {
        private readonly IBetListRepository _betListRepository;

        public CN88BetListService()
        {
            _betListRepository = new BetListRepository();
        }

        public string GetCombinationTypeComboNames(
            string betTeam,
            string betCheck,
            decimal hdp1,
            decimal hdp2,
            IList<TicketDto> ticketDetails)
        {
            var combinationTypeCombo = Fanex.Tarus.Infrastructure.Caching.CacheRef.GetCombinationTypeCombo(
                betTeam, betCheck, (int)hdp1, (int)hdp2);

            if (combinationTypeCombo == null || combinationTypeCombo.Length == 0)
            {
                return Resources.BetListResources.ThereIsNoData;
            }

            var combinationTypeComboTickets = ticketDetails
                .Where(t => t.Stake > 0)
                .OrderBy(t => t.MatchId)
                .ToList();
            if(combinationTypeComboTickets.Any())
            {
                var result = new StringBuilder();
                TicketDto ticketDetail;
                for (int i = 0, max = combinationTypeCombo.Length; i < max; i++)
                {
                    ticketDetail = hdp1 == 0 ? combinationTypeComboTickets[0] : combinationTypeComboTickets[i];
                    result.Append(string.Format(
                        "<div>{0} x <span class=\"unit-stake\">{1} {2}</span></div>",
                        combinationTypeCombo[i],
                        ticketDetail.Currency,
                        ticketDetail.Stake.ToString("N0")));
                }

                return "<div class=\"offer-name-list\">" + result.ToString() + "</div>";
            }

            return Resources.BetListResources.ThereIsNoData;
        }

        public IList<TicketDto> GetMixParlayTickets(MixParlayDetailCriteria criteria)
        {
            Func<DataRow, TicketDto> mapTicketObjectFunction = r => new TicketDto
            {
                SportTypeId = Converter.ToInt(r["Sporttype"]),
                TransId = Converter.To<long>(r["TransID"]),
                BetTypeId = Converter.ToInt(r["Bettype"]),
                Stake = Converter.ToDecimal(r["Stake"]),
                Currency = Converter.ToString2(r["Currency"]),
                Handicap1 = Converter.ToDecimal(r["hdp1"]),
                Handicap2 = Converter.ToDecimal(r["hdp2"]),
                Odds = Converter.ToDecimal(r["Odds"]),
                BetTeam = Converter.ToString2(r["Betteam"]),
                IsLive = Converter.ToBool(r["Liveindicator"]),
                LiveHomeScore = Converter.ToInt(r["Livehomescore"]),
                LiveAwayScore = Converter.ToInt(r["Liveawayscore"]),
                LeagueId = Converter.ToInt(r["LeagueID"]),
                MatchId = Converter.ToInt(r["MatchID"]),
                MatchCode = Converter.ToString2(r["MatchCode"]),
                HomeId = Converter.To<long>(r["HomeID"]),
                AwayId = Converter.To<long>(r["AwayID"]),
                IsNeutral = Converter.ToBool(r["isNeutral"]),
                BetCheck = Converter.ToString2(r["BetCheck"]),
                TransDesc = Converter.ToString2(r["transdesc"]),
                ShowTime = Converter.ToString2(r["showtime"]),
                EventDate = Converter.ToDateTime(r["eventdate"]),
                Status = Converter.ToString2(r["status"])
            };

            IList<TicketDto> betDetails = _betListRepository.Fetch<TicketDto>(criteria, mapTicketObjectFunction);

            return betDetails;
        }

        public BetListData GetBetListByCustInfo_DateRange(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            int betType,
            int pageSize,
            int pageIndex)
        {
            Func<DataRow, TicketDto> mapTicketObjectFunction = r => new TicketDto
            {
                UserName = Converter.To<string>(r["username"]),
                UGroup = Converter.To<string>(r["ugroup"]),
                ActualStake = Converter.To<decimal>(r["actual_stake"]),
                AgentWinlost = Converter.ToDecimal(r["awinlost"]),
                AgentPositionTaking = Converter.ToDecimal(r["apositiontaking"]),
                Comm = Converter.ToDecimal(r["comm"]),
                AgentDiscount = Converter.ToDecimal(r["discount"]),

                AwayId = Converter.To<long>(r["awayid"]),
                BetId = Converter.To<long>(r["betid"]),
                BetTeam = Converter.ToString2(r["betteam"]),
                BetTypeId = Converter.To<int>(r["bettype"]),
                CommStatus = Converter.To<string>(r["comstatus"]),
                CustId = Converter.To<long>(r["custid"]),
                Handicap1 = Converter.ToDecimal(r["hdp1"]),
                Handicap2 = Converter.ToDecimal(r["hdp2"]),
                HomeId = Converter.To<long>(r["homeid"]),
                EventDate = Converter.ToDateTime(r["eventdate"]),
                IP = Converter.ToString2(r["ip"]),
                IsLive = Converter.ToBool(r["liveindicator"]),
                IsNeutral = Converter.ToBool(r["isNeutral"]),
                LeagueId = Converter.To<int>(r["leagueid"]),
                LiveHomeScore = Converter.To<int>(r["livehomescore"]),
                LiveAwayScore = Converter.To<int>(r["liveawayscore"]),

                MasterWinlost = Converter.ToDecimal(r["mwinlost"]),
                MasterPositionTaking = Converter.ToDecimal(r["mpositiontaking"]),
                AgentComm = Converter.ToDecimal(r["acomm"]),
                MasterDiscount = Converter.ToDecimal(r["adiscount"]),

                MasterOdds = Converter.ToDecimal(r["modds"]),
                MatchCode = Converter.ToString2(r["matchcode"]),
                MatchId = Converter.To<int>(r["matchid"]),
                Odds = Converter.ToDecimal(r["odds"]),
                OddsType = Converter.ToString2(r["OddsType"]),
                PlayerComm = Converter.ToDecimal(r["commission"]),
                Race = Converter.ToString2(r["race"]),
                RefNo = Converter.ToString2(r["refno"]),
                ShowTime = Converter.ToString2(r["showtime"]),
                SportTypeId = Converter.To<int>(r["sporttype"]),
                Stake = Converter.To<decimal>(r["stake"]),
                Status = Converter.ToString2(r["status"]),

                SuperWinlost = Converter.ToDecimal(r["swinlost"]),
                SuperPositionTaking = Converter.ToDecimal(r["spositiontaking"]),
                SuperComm = Converter.ToDecimal(r["scomm"]),
                SuperDiscount = Converter.ToDecimal(r["sdiscount"]),

                TransDate = Converter.ToDateTime(r["transdate"]),
                TransId = Converter.To<long>(r["transid"]),
                Winlost = Converter.ToDecimal(r["winlost"]),
                WinlostDate = Convert.ToDateTime(r["winlostdate"]),
                TransDesc = Converter.ToString2(r["transdesc"]),

                BetCheck = Convert.ToString(r["betcheck"]),
                OddsSpread = Convert.ToDecimal(r["OddsSpread"]),

                OriginalBetType = Convert.ToInt32(r["OriginalBetType"]),
                BetdaqId = Convert.ToInt64(r["BetdaqId"]),
                ExtraInfo = Converter.ToString2(r["ExtraInfo"]),

                Currency = Converter.ToString2(r["Currency"]),

                WalletType = r.Table.Columns.Contains("wallettype") ? Convert.ToByte(r["wallettype"]) : (byte)0,
                RefCode = r.Table.Columns.Contains("refcode") ? Convert.ToInt64(r["refcode"]) : 0,
                SettlementStatus = r.Table.Columns.Contains("SettlementStatus") ? Convert.ToByte(r["SettlementStatus"]) : (byte)0
            };

            var betListByCustInfoCriteria = new BetListByCustInfoCriteria
            {
                CustId = custId,
                FromDate = fromDate,
                ToDate = toDate,
                BetType = betType,
                MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                IsCN88 = true
            };

            var betListByCustInfoCriteria_Old = new BetListByCustInfoCriteria_Old
            {
                CustId = custId,
                FromDate = fromDate,
                ToDate = toDate,
                BetType = betType,
                IsCN88 = true
            };

            List<ITicketData> outputLiveCasino = new List<ITicketData>();
            List<ITicketData> outputLiveCasino_Old = new List<ITicketData>();
            IList<TicketDto> betDetails = new List<TicketDto>();
            IList<TicketDto> betDetailsOld = new List<TicketDto>();

            if (toDate >= ReportService.BalanceUp.BalanceUpDay)
            {
                betDetails = _betListRepository.Fetch<TicketDto>(betListByCustInfoCriteria, mapTicketObjectFunction, ref outputLiveCasino);
            }

            if (fromDate < ReportService.BalanceUp.BalanceUpDay)
            {
                betDetailsOld = _betListRepository.Fetch<TicketDto>(betListByCustInfoCriteria_Old, mapTicketObjectFunction, ref outputLiveCasino_Old);
            }

            var betDetailsMerge = betDetails.Concat(betDetailsOld).OrderBy(x => x.TransDate);
            var outputLiveCasino_Merge = outputLiveCasino.Concat(outputLiveCasino_Old);

            var from = pageSize * (pageIndex - 1);

            return new BetListData(
                betDetailsMerge.Skip(from).Take(pageSize),
                outputLiveCasino_Merge,
                betDetailsMerge.Count());
        }

        public BetListData GetBetListByCustInfo_Refno(string transactionId, int customerId)
        {
            var criteria = new BetListByCustInfoByRefnoCriteria()
            {
                RefNo = transactionId,
                CustId = customerId,
                MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                BalanceUpDay = ReportService.BalanceUp.BalanceUpDay,
                IsCN88 = true
            };

            Func<DataRow, TicketDto> mapTicketObjectFunction = r => new TicketDto
            {
                UserName = Converter.To<string>(r["username"]),
                UGroup = Converter.To<string>(r["ugroup"]),
                ActualStake = Converter.To<decimal>(r["actual_stake"]),
                AgentWinlost = Converter.ToDecimal(r["awinlost"]),
                AgentPositionTaking = Converter.ToDecimal(r["apositiontaking"]),
                Comm = Converter.ToDecimal(r["comm"]),
                AgentDiscount = Converter.ToDecimal(r["discount"]),

                AwayId = Converter.To<long>(r["awayid"]),
                BetId = Converter.To<long>(r["betid"]),
                BetTeam = Converter.ToString2(r["betteam"]),
                BetTypeId = Converter.To<int>(r["bettype"]),
                CommStatus = Converter.To<string>(r["comstatus"]),
                CustId = Converter.To<long>(r["custid"]),
                Handicap1 = Converter.ToDecimal(r["hdp1"]),
                Handicap2 = Converter.ToDecimal(r["hdp2"]),
                HomeId = Converter.To<long>(r["homeid"]),
                EventDate = Converter.ToDateTime(r["eventdate"]),
                IP = Converter.ToString2(r["ip"]),
                IsLive = Converter.ToBool(r["liveindicator"]),
                IsNeutral = Converter.ToBool(r["isNeutral"]),
                LeagueId = Converter.To<int>(r["leagueid"]),
                LiveHomeScore = Converter.To<int>(r["livehomescore"]),
                LiveAwayScore = Converter.To<int>(r["liveawayscore"]),

                MasterWinlost = Converter.ToDecimal(r["mwinlost"]),
                MasterPositionTaking = Converter.ToDecimal(r["mpositiontaking"]),
                AgentComm = Converter.ToDecimal(r["acomm"]),
                MasterDiscount = Converter.ToDecimal(r["adiscount"]),

                MasterOdds = Converter.ToDecimal(r["modds"]),
                MatchCode = Converter.ToString2(r["matchcode"]),
                MatchId = Converter.To<int>(r["matchid"]),
                Odds = Converter.ToDecimal(r["odds"]),
                OddsType = Converter.ToString2(r["OddsType"]),
                PlayerComm = Converter.ToDecimal(r["commission"]),
                Race = Converter.ToString2(r["race"]),
                RefNo = Converter.ToString2(r["refno"]),
                ShowTime = Converter.ToString2(r["showtime"]),
                SportTypeId = Converter.To<int>(r["sporttype"]),
                Stake = Converter.To<decimal>(r["stake"]),
                Status = Converter.ToString2(r["status"]),

                SuperWinlost = Converter.ToDecimal(r["swinlost"]),
                SuperPositionTaking = Converter.ToDecimal(r["spositiontaking"]),
                SuperComm = Converter.ToDecimal(r["scomm"]),
                SuperDiscount = Converter.ToDecimal(r["sdiscount"]),

                TransDate = Converter.ToDateTime(r["transdate"]),
                TransId = Converter.To<long>(r["transid"]),
                Winlost = Converter.ToDecimal(r["winlost"]),
                WinlostDate = Convert.ToDateTime(r["winlostdate"]),
                TransDesc = Converter.ToString2(r["transdesc"]),

                BetCheck = Convert.ToString(r["betcheck"]),
                OddsSpread = Convert.ToDecimal(r["OddsSpread"]),

                OriginalBetType = Convert.ToInt32(r["OriginalBetType"]),
                BetdaqId = Convert.ToInt64(r["BetdaqId"]),
                ExtraInfo = Converter.ToString2(r["ExtraInfo"]),
                Currency = Converter.ToString2(r["Currency"]),
                WalletType = Convert.ToByte(r["wallettype"]),
                RefCode = Convert.ToInt64(r["refcode"]),
                SettlementStatus = Convert.ToByte(r["SettlementStatus"])
            };

            List<ITicketData> outputLiveCasino = new List<ITicketData>();
            var betDetails = _betListRepository.Fetch<TicketDto>(criteria, mapTicketObjectFunction, ref outputLiveCasino);

            return new BetListData(betDetails, outputLiveCasino, betDetails.Count());
        }
    }
}