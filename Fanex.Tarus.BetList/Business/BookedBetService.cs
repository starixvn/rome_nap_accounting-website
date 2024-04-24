namespace Fanex.Tarus.BetList.Business
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Alpha.IPInfo.DataAccess;
    using Alpha.IPInfo.Models;
    using Alpha.IPInfo.Services;
    using Constants;
    using Core;
    using Criteria;
    using DataAccess;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.BetList.Core.Utils;
    using Fanex.Caching;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.Data;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Tarus.BetList.DataAccess.Results;
    using Fanex.Tarus.BetList.Param;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Models;
    using Resources;
    using Sunplus.Accounting.Business;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.DataAccess.Report;

    public interface IBookedBetService
    {
        IList<BookedBet> Get(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string statusIds,
            int pageSize,
            int pageIndex,
            ref int quantity);
    }

    public class BookedBetService : IBookedBetService
    {
        private readonly IIPInfoService _ipInfoService;
        private readonly IBetListRepository _betListRepository;
        private readonly IIPInfoDataAccess _iPInfoDataAccess;
        private readonly ICacheService _cacheService;
        private readonly ITicketHelper ticketHelper = new TicketHelper();
        private readonly BookedBetRepository repository;
        private readonly Func<DataRow, BookedBet> mapTicketObjectFunction = r => new BookedBet
        {
            Ticket = new Ticket
            {
                AwayId = Converter.To<long>(r["awayid"]),
                BetTeam = Converter.ToString2(r["betteam"]),
                BetTypeId = Converter.To<int>(r["bettype"]),
                Handicap1 = Converter.ToDecimal(r["hdp1"]),
                Handicap2 = Converter.ToDecimal(r["hdp2"]),
                HomeId = Converter.To<long>(r["homeid"]),
                EventDate = Converter.ToDateTime(r["eventdate"]),
                IP = Converter.ToString2(r["ip"]),
                IsLive = Converter.ToBool(r["liveindicator"]),
                IsNeutral = Converter.ToBool(r["isNeutral"]),
                LeagueId = Converter.To<int>(r["leagueid"]),
                MatchId = Converter.To<int>(r["matchid"]),
                OddsType = Converter.ToString2(r["OddsType"]),
                SportTypeId = Converter.To<int>(r["sporttype"]),
                Stake = Converter.To<decimal>(r["stake"]),
                StatusId = Converter.To<int>(r["statusid"]),
                TransDate = Converter.ToDateTime(r["transdate"]),
                TransId = Converter.To<long>(r["transid"]),
                UserName = Converter.ToString2(r["username"]),
                LiveAwayScore = Converter.To<int>(r["liveawayscore"]),
                LiveHomeScore = Converter.To<int>(r["livehomescore"])

            },
            OddsLower = Converter.ToDecimal(r["oddslower"]),
            OddsUpper = Converter.ToDecimal(r["oddsupper"]),
            Reason = Converter.ToString2(r["reason"]),
            ChoiceHtml = string.Empty,
            OddsValue = string.Empty,
            StakeHtml = string.Empty,
        };

        public BookedBetService()
        {
            repository = new BookedBetRepository();
            _cacheService = new CacheService();
            _betListRepository = new BetListRepository();
            _iPInfoDataAccess = new IPInfoDataAccess();
            _ipInfoService = new IPInfoService(new IPInformationConfig
            {
                IPV4TopCountries = AppSettings.IPV4TopCountries,
                IPV4TopCountriesCacheInMinutes = AppSettings.IPV4TopCountriesCacheInMinutes,
                IPV4CacheInMinutes = AppSettings.IPV4CacheInMinutes,
                IPV6CacheInMinutes = AppSettings.IPV6CacheInMinutes
            }, _cacheService, _iPInfoDataAccess);
        }


        public IList<BookedBet> Get(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string statusIds,
            int pageSize,
            int pageIndex,
            ref int totalRows)
        {
            var criteria = new GetBookedBetCriteria
            {
                CustId = custId,
                FromDate = fromDate,
                ToDate = toDate,
                StatusIdList = statusIds,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var dataTickets = new List<ITicketData>();
            var tickets = repository.FetchWithOutputs(criteria, mapTicketObjectFunction, ref totalRows);

            var betlistHelper = new BetListHelper();
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;

            var newTickets = new List<BookedBet>();
            foreach (var ticket in tickets)
            {
                ticket.TransDateHtml = builder.BuildTrans(ticket.Ticket).Replace("Ref No", "Id");
                ticket.ChoiceHtml = builder.BuildChoice(ticket.Ticket, ticketHelper, null, false);
                ticket.StakeHtml = builder.BuildStake(ticket.Ticket, ticketHelper);
                ticket.OddsType = ticketHelper.GetOddsTypeNameById(ticket.Ticket.OddsType);
                ticket.OddsValue = GetOdds(ticket.OddsLower, ticket.OddsUpper);
                ticket.StatusHtml = BookedBetStatusConstants.GetText(ticket.Ticket.StatusId.Value);
                ticket.ReasonText = GetReason(ticket.Ticket.StatusId.Value, ticket.Reason);
                newTickets.Add(ticket);
            }

            AssignCountry(tickets);

            return newTickets;
        }

        private string GetReason(int statusId, string reasonId)
            => statusId == BookedBetStatusConstants.Unsuccessful ?
            $"Reason: {ticketHelper.GetResourceData("BookedBet_Reason", reasonId)}" : string.Empty;

        private string GetOdds(decimal lower, decimal upper)
            => lower != 0 && upper != 0 ?
            $"{FormatOdds(lower)} ~ {FormatOdds(upper)}" :
            $"{FormatOdds(lower)}";
        private string FormatOdds(decimal odds)
            => string.Format(Formatter.DECIMALFORMAT3, odds);

        private void AssignCountry(IEnumerable<BookedBet> tickets)
        {
            var ipList = tickets
             .Where(t => !string.IsNullOrWhiteSpace(t.Ticket.IP) && (AppSession.ViewIPVN || !IpManagement.IsVNIP(t.Ticket.IP.Trim())))
             .Select(t => t.Ticket.IP.Trim())
             .Distinct();
            var countryDictionary = _ipInfoService.GetCountries(ipList).GetAwaiter().GetResult();

            foreach (var item in tickets)
            {
                if (countryDictionary.TryGetValue(item.Ticket.IP.Trim(), out string country))
                {
                    ((BookedBet)item).Country = country;
                }
            }
        }

    }
}