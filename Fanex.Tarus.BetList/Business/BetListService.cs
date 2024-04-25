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
    using System.Threading;
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
    using Fanex.Caching;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.Data;
    using Fanex.Logging.Extensions.Web;
    using Fanex.RemotingRef;
    using Fanex.Tarus.BetList.DataAccess.Results;
    using Fanex.Tarus.BetList.Param;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Models;
    using Resources;
    using Sunplus.Accounting.Business;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.DataAccess.Report;

    public class BetListService : IBetListService
    {
        public const string BetListSessionKey = "Fanex.BetList.Accounting.WinLossByLicensee";
        private readonly IRemotingRefService _remotingRefService = new RemotingRefService();
        private readonly IBetListRepository _betListRepository;
        private readonly ITicketHelper ticketHelper = new TicketHelper();
        private const int DefaultBetdaqId = -1;
        private readonly List<int> SabaclubProductIdList = new List<int> { 42, 66 };
        private readonly IIPInfoService _ipInfoService;
        private readonly ICacheService _cacheService;
        private readonly IIPInfoDataAccess _iPInfoDataAccess;
        private readonly Func<DataRow, TicketDetail> mapTicketObjectFunction = r => new TicketDetail
        {
            Ticket = new Ticket
            {
                ActualStake = r.Table.Columns.Contains("actual_stake") ? Converter.To<decimal>(r["actual_stake"]) : 0,

                AgentWinlost = r.Table.Columns.Contains("actual_stake") ? Converter.ToDecimal(r["awinlost"]) : 0,
                AgentPositionTaking = r.Table.Columns.Contains("apositiontaking") ? Converter.ToDecimal(r["apositiontaking"]) : 0,
                Comm = r.Table.Columns.Contains("comm") ? Converter.ToDecimal(r["comm"]) : 0,
                AgentDiscount = r.Table.Columns.Contains("discount") ? Converter.ToDecimal(r["discount"]) : 0,

                AwayId = r.Table.Columns.Contains("awayid") ? Converter.To<long>(r["awayid"]) : 0,
                BetId = r.Table.Columns.Contains("betid") ? Converter.To<long>(r["betid"]) : 0,
                BetTeam = r.Table.Columns.Contains("betteam") ? Converter.ToString2(r["betteam"]) : string.Empty,
                BetTypeId = r.Table.Columns.Contains("bettype") ? Converter.To<int>(r["bettype"]) : 0,
                CommStatus = r.Table.Columns.Contains("comstatus") ? Converter.To<string>(r["comstatus"]) : string.Empty,
                CustId = r.Table.Columns.Contains("custid") ? Converter.To<long>(r["custid"]) : 0,
                Handicap1 = r.Table.Columns.Contains("hdp1") ? Converter.ToDecimal(r["hdp1"]) : 0,
                Handicap2 = r.Table.Columns.Contains("hdp2") ? Converter.ToDecimal(r["hdp2"]) : 0,
                HomeId = r.Table.Columns.Contains("homeid") ? Converter.To<long>(r["homeid"]) : 0,
                EventDate = r.Table.Columns.Contains("eventdate") ? Converter.ToDateTime(r["eventdate"]) : DateTime.Now,
                IP = r.Table.Columns.Contains("ip") ? Converter.ToString2(r["ip"]) : string.Empty,
                IsLive = r.Table.Columns.Contains("liveindicator") ? Converter.ToBool(r["liveindicator"]) : false,
                IsNeutral = r.Table.Columns.Contains("isNeutral") ? Converter.ToBool(r["isNeutral"]) : false,
                LeagueId = r.Table.Columns.Contains("leagueid") ? Converter.To<int>(r["leagueid"]) : 0,
                LiveHomeScore = r.Table.Columns.Contains("livehomescore") ? Converter.To<int>(r["livehomescore"]) : 0,
                LiveAwayScore = r.Table.Columns.Contains("liveawayscore") ? Converter.To<int>(r["liveawayscore"]) : 0,

                MasterWinlost = r.Table.Columns.Contains("mwinlost") ? Converter.ToDecimal(r["mwinlost"]) : 0,
                MasterPositionTaking = r.Table.Columns.Contains("mpositiontaking") ? Converter.ToDecimal(r["mpositiontaking"]) : 0,
                AgentComm = r.Table.Columns.Contains("acomm") ? Converter.ToDecimal(r["acomm"]) : 0,
                MasterDiscount = r.Table.Columns.Contains("adiscount") ? Converter.ToDecimal(r["adiscount"]) : 0,

                MasterOdds = r.Table.Columns.Contains("modds") ? Converter.ToDecimal(r["modds"]) : 0,
                MatchCode = r.Table.Columns.Contains("matchcode") ? Converter.ToString2(r["matchcode"]) : string.Empty,
                MatchId = r.Table.Columns.Contains("matchid") ? Converter.To<int>(r["matchid"]) : 0,
                Odds = r.Table.Columns.Contains("odds") ? Converter.ToDecimal(r["odds"]) : 0,
                OddsType = r.Table.Columns.Contains("OddsType") ? Converter.ToString2(r["OddsType"]) : string.Empty,
                PlayerComm = r.Table.Columns.Contains("commission") ? Converter.ToDecimal(r["commission"]) : 0,
                Race = r.Table.Columns.Contains("race") ? Converter.ToString2(r["race"]) : string.Empty,
                RefNo = r.Table.Columns.Contains("refno") ? Converter.ToString2(r["refno"]) : string.Empty,
                ShowTime = r.Table.Columns.Contains("showtime") ? Converter.ToString2(r["showtime"]) : string.Empty,
                SportTypeId = r.Table.Columns.Contains("sporttype") ? Converter.To<int>(r["sporttype"]) : 0,
                Stake = r.Table.Columns.Contains("stake") ? Converter.To<decimal>(r["stake"]) : 0,
                Status = r.Table.Columns.Contains("status") ? Converter.ToString2(r["status"]) : string.Empty,

                SuperWinlost = r.Table.Columns.Contains("swinlost") ? Converter.ToDecimal(r["swinlost"]) : 0,
                SuperPositionTaking = r.Table.Columns.Contains("spositiontaking") ? Converter.ToDecimal(r["spositiontaking"]) : 0,
                SuperComm = r.Table.Columns.Contains("scomm") ? Converter.ToDecimal(r["scomm"]) : 0,
                SuperDiscount = r.Table.Columns.Contains("sdiscount") ? Converter.ToDecimal(r["sdiscount"]) : 0,

                TransDate = r.Table.Columns.Contains("transdate") ? Converter.ToDateTime(r["transdate"]) : DateTime.MinValue,
                TransId = r.Table.Columns.Contains("transid") ? Converter.To<long>(r["transid"]) : 0,
                UserName = r.Table.Columns.Contains("username") ? Converter.ToString2(r["username"]) : string.Empty,
                Winlost = r.Table.Columns.Contains("winlost") ? Converter.ToDecimal(r["winlost"]) : 0,
                WinlostDate = r.Table.Columns.Contains("winlostdate") ? Convert.ToDateTime(r["winlostdate"]) : DateTime.MinValue,
                TransDesc = r.Table.Columns.Contains("transdesc") ? Converter.ToString2(r["transdesc"]) : string.Empty,

                BetCheck = r.Table.Columns.Contains("betcheck") ? Converter.ToString2(r["betcheck"]) : string.Empty,
                OddsSpread = r.Table.Columns.Contains("OddsSpread") ? Converter.ToDecimal(r["OddsSpread"]) : 0,

                OriginalBetType = r.Table.Columns.Contains("OriginalBetType") ? Converter.To<int>(r["OriginalBetType"]) : 0,
                BetdaqId = r.Table.Columns.Contains("BetdaqId") ? Converter.To<long>(r["BetdaqId"]) : DefaultBetdaqId,
                ExtraInfo = r.Table.Columns.Contains("ExtraInfo") ? Converter.ToString2(r["ExtraInfo"]) : string.Empty,

                Currency = r.Table.Columns.Contains("Currency") ? Converter.ToString2(r["Currency"]) : string.Empty,
                CurrencyId = r.Table.Columns.Contains("CurrencyId") ? Converter.ToInt(r["CurrencyId"]) : 0,

                WalletType = r.Table.Columns.Contains("wallettype") ? Converter.To<byte>(r["wallettype"]) : (byte)0,
                RefCode = r.Table.Columns.Contains("refcode") ? Converter.To<long>(r["refcode"]) : 0,
                SettlementStatus = r.Table.Columns.Contains("SettlementStatus") ? Converter.To<byte>(r["SettlementStatus"]) : (byte)0,
                SiteId = r.Table.Columns.Contains("SiteId") ? Converter.ToString2(r["SiteId"]) : "",
                JackpotInfo = r.Table.Columns.Contains("JackpotInfo") ? Converter.ToString2(r["JackpotInfo"]) : string.Empty,
                LeagueGroupId = r.Table.Columns.Contains("LeagueGroupId") ? Converter.ToInt(r["LeagueGroupId"]) : 0,
            },
            ChoiceHtml = string.Empty,
            OddHtml = string.Empty,
            StakeHtml = string.Empty,
            IsShowOtExRate = r.Table.Columns.Contains("isShowOTExRate") ? Converter.To<bool>(r["isShowOTExRate"]) : false,
            VirtualRate = r.Table.Columns.Contains("virtualrate") ? Converter.ToDecimal(r["virtualrate"]) : 0,
            AgentPositionTaking2 = r.Table.Columns.Contains("apositiontaking2") ? Converter.ToDecimal(r["apositiontaking2"]) : 0,
            MasterPositionTaking2 = r.Table.Columns.Contains("mpositiontaking2") ? Converter.ToDecimal(r["mpositiontaking2"]) : 0,
            SuperPositionTaking2 = r.Table.Columns.Contains("spositiontaking2") ? Converter.ToDecimal(r["spositiontaking2"]) : 0,
            IsChinaView = r.Table.Columns.Contains("IsChinaView") && Converter.ToBool(r["IsChinaView"]),
            SpecialPlatfrom = r.Table.Columns.Contains("SpecialPlatfrom") ? Converter.ToInt(r["SpecialPlatfrom"]) : 0,            
            IP = Converter.ToString2(r["ip"]),
            DeviceType = r.Table.Columns.Contains("DeviceType") && !r.IsNull("DeviceType") ? Converter.To<int>(r["DeviceType"]) : (int?)null
        };

        private readonly Func<DataRow, TicketDetail> mapTicketObjectFunctionForMixParlay = r => new TicketDetail
        {
            Ticket = new Ticket
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
                Status = Converter.ToString2(r["status"]),
                SettlementStatus = r.Table.Columns.Contains("SettlementStatus") ? Convert.ToByte(r["SettlementStatus"]) : (byte)0,
                TransDate = Converter.ToDateTime(r["TransDate"]),
                LeagueGroupId = r.Table.Columns.Contains("LeagueGroupId") ? Converter.ToInt(r["LeagueGroupId"]) : 0,
            },
            ChoiceHtml = string.Empty,
            OddHtml = string.Empty,
            StakeHtml = string.Empty,
            IsChinaView = r.Table.Columns.Contains("IsChinaView") && Converter.ToBool(r["IsChinaView"]),
            SpecialPlatfrom = r.Table.Columns.Contains("SpecialPlatfrom") ? Converter.ToInt(r["SpecialPlatfrom"]) : 0,            
            DeviceType = r.Table.Columns.Contains("DeviceType") && !r.IsNull("DeviceType") ? Converter.To<int>(r["DeviceType"]) : (int?)null
        };
        public BetListService()
        {
            _cacheService = new CacheService();
            _iPInfoDataAccess = new IPInfoDataAccess();
            _ipInfoService = new IPInfoService(new IPInformationConfig
            {
                IPV4TopCountries = AppSettings.IPV4TopCountries,
                IPV4TopCountriesCacheInMinutes = AppSettings.IPV4TopCountriesCacheInMinutes,
                IPV4CacheInMinutes = AppSettings.IPV4CacheInMinutes,
                IPV6CacheInMinutes = AppSettings.IPV6CacheInMinutes
            }, _cacheService, _iPInfoDataAccess);
            _betListRepository = new BetListRepository();
        }

        public void AssignCountry(IEnumerable<TicketDetail> tickets)
        {
            var ipList = tickets
             .Where(t => !string.IsNullOrWhiteSpace(t.IP) && (AppSession.ViewIPVN || !IpManagement.IsVNIP(t.IP.Trim())))
             .Select(t => t.IP.Trim())
             .Distinct();
            var countryDictionary = GetCountries(ipList);

            string country;
            foreach (var item in tickets)
            {
                if (!string.IsNullOrWhiteSpace(item.IP)
                    && countryDictionary.TryGetValue(item.IP.Trim(), out country))
                {
                    ((TicketDetail)item).Country = country;
                }
            }
        }

        private IDictionary<string, string> GetCountries(IEnumerable<string> ipList)
        {
            try
            {
                return _ipInfoService.GetCountries(ipList).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return new Dictionary<string, string>();
        }

        public IList<TicketDetail> GetMixParlayTickets(long transId, long refNo, int custId, bool removeSabaWord)
        {
            var siteDB = ObjectDbExtensions.GetSiteDBConfigByCustId(custId);
            if (siteDB != null)
            {
                IList<TicketDetail> siteDBTickets = new List<TicketDetail>();
                if (siteDB.IsSiteDB)
                {
                    var siteDBCriteria = new SiteDBMixParlayDetailCriteria
                    {
                        CustId = custId,
                        RefNo = refNo,
                        TransId = transId,
                        MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                        BalanceUpDay = ReportService.BalanceUp.BalanceUpDay
                    };
                    siteDBTickets = _betListRepository.Fetch<TicketDetail>(siteDBCriteria, siteDB.AGIP, mapTicketObjectFunctionForMixParlay);
                }
                var mainCriteria = new MixParlayDetailCriteria
                {
                    CustId = custId,
                    RefNo = refNo,
                    TransId = transId,
                    ExcludeBettrans = siteDB.IsSiteDB,
                    MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                    BalanceUpDay = ReportService.BalanceUp.BalanceUpDay
                };

                var mainTickets = _betListRepository.Fetch<TicketDetail>(mainCriteria, mapTicketObjectFunctionForMixParlay);
                var tickets = siteDBTickets.Concat(mainTickets).Distinct().ToList();
                AssignCountry(tickets);
                var factory = new BetListHTMLFactory();
                var builder = factory.CreateBuilder();
                ticketHelper.IsShowOddsWithChoice = true;
                var totalItem = tickets.Count;
                foreach (TicketDetail betDetail in tickets)
                {
                    totalItem--;
                    ticketHelper.HideSabaText = removeSabaWord && IsHideSabaWord(betDetail);
                    betDetail.ChoiceHtml =
                        "<div class='ticketList'>"
                            + builder.BuildChoice(betDetail.Ticket, ticketHelper, null, false)
                            + builder.BuildTransEventDateAndStatusResult(betDetail.Ticket, null)
                        + (totalItem > 0 ? "</div>" + "<div class=\"line\"></div>" : "</div>");                    
                }

                return tickets;
            }

            return new List<TicketDetail>();
        }

        /// <summary>
        /// Get Bet List by ref no.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Bet List by ref no.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private IList<TicketDetail> GetBetListBy(dynamic criteria)
        {
            try
            {
                List<ITicketData> outputLiveCasino = new List<ITicketData>();
                IList<TicketDetail> betDetails = _betListRepository.Fetch<TicketDetail>(criteria, mapTicketObjectFunction, ref outputLiveCasino);
                BetListHelper betListHelper = new BetListHelper();

                if (betDetails != null)
                {
                    BetListHTMLFactory factory = new BetListHTMLFactory();
                    BetListHTMLBuilder builder = factory.CreateBuilder();
                    GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;

                    foreach (TicketDetail betDetail in betDetails)
                    {
                        ticketHelper.IsShowOddsWithChoice = IsShowOddsWithChoice(betDetail.Ticket.BetTypeId);

                        betDetail.TransDateHtml = builder.BuildTrans(betDetail.Ticket);
                        betDetail.ChoiceHtml = builder.BuildChoice(betDetail.Ticket, ticketHelper, outputLiveCasino, false);
                        betDetail.StakeHtml = BetListExtension.BuildStake(betDetail, ref builder, ticketHelper);
                        betDetail.OddHtml = builder.BuildOdds(betDetail.Ticket, outputLiveCasino, funcGetOddsTypeName);
                        betDetail.StatusHtml = betListHelper.BuildStatus(betDetail.Ticket, ticketHelper, outputLiveCasino, factory, builder);
                    }
                    AssignCountry(betDetails);

                    return betDetails;
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return new List<TicketDetail>();
        }

        public IList<TicketDetail> GetBetListByRefNo(BetListByRefNoParam param)
        {
            IList<TicketDetail> tickets = new List<TicketDetail>();
            List<ITicketData> outputLiveCasino = new List<ITicketData>();
            var betListHelper = new BetListHelper();
            var allSite = ObjectDbExtensions.GetAllSiteDBConfig();
            foreach (var siteDB in allSite.Where(i => i.IsSiteDB))
            {
                if (!tickets.Any())
                {
                    var siteDBCriteria = new SiteDBBetListByRefNoCriteria
                    {
                        UserId = param.UserId,
                        RefNo = param.RefNo,
                        MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                        Permission = param.Permission
                    };
                    tickets = _betListRepository.Fetch<TicketDetail>(siteDBCriteria, siteDB.AGIP, mapTicketObjectFunction, ref outputLiveCasino).ToList();
                }
            }

            if (!tickets.Any())
            {
                var mainCriteria = new BetListByRefNoCriteria
                {
                    UserId = param.UserId,
                    RefNo = param.RefNo,
                    MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                    BalanceUpDay = ReportService.BalanceUp.BalanceUpDay,
                    Permission = param.Permission,
                    ExcludeBettrans = false
                };
                tickets = _betListRepository.Fetch<TicketDetail>(mainCriteria, mapTicketObjectFunction, ref outputLiveCasino);
            }

            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;

            foreach (var betDetail in tickets)
            {
                ticketHelper.HideSabaText = IsHideSabaWord(betDetail);
                ticketHelper.IsShowOddsWithChoice = IsShowOddsWithChoice(betDetail.Ticket.BetTypeId);

                betDetail.TransDateHtml = builder.BuildTrans(betDetail.Ticket);
                betDetail.ChoiceHtml = builder.BuildChoice(betDetail.Ticket, ticketHelper, outputLiveCasino, false);
                betDetail.StakeHtml = BetListExtension.BuildStake(betDetail, ref builder, ticketHelper);
                betDetail.OddHtml = builder.BuildOdds(betDetail.Ticket, outputLiveCasino, funcGetOddsTypeName);
                betDetail.StatusHtml = betListHelper.BuildStatus(betDetail.Ticket, ticketHelper, outputLiveCasino, factory, builder);
            }

            AssignCountry(tickets);

            return tickets;
        }

        private bool IsHideSabaWord(TicketDetail ticket)
        {
            var leagueGroupIds = new List<int> { 42, 74, 108 };
            var sportTypes = new List<int> { SportContants.SabaxD, SportContants.SabaPromotion };
            if (ticket.IsChinaView || ticket.SpecialPlatfrom >= 2)
            {
                return sportTypes.Any(i => i == ticket.Ticket.SportTypeId) ||
                    leagueGroupIds.Any(i => i == ticket.Ticket.LeagueGroupId);
            }

            return false;
        }
        
        public IList<TicketDetail> GetBetListBetTradeDetails(BetListBetTradeDetailCriteria criteria)
        {
            return GetBetListBy(criteria);
        }

        public IList<TicketDetail> GetBetListByProduct(BetListByProductParam betListByProductCriteria)
        {
            var isSabaclub = SabaclubProductIdList.Any(i => i == betListByProductCriteria.ProductId);
            var needToMapPlayerId3rdToCustId = isSabaclub && !string.IsNullOrWhiteSpace(betListByProductCriteria.PlayerId3rd);

            var mappedCustId = needToMapPlayerId3rdToCustId
                ? _betListRepository.MapPlayerIdToCustId(new MapPlayerId3rdToCustIdCriteria
                {
                    PlayerId3rd = betListByProductCriteria.PlayerId3rd,
                    Vendor = betListByProductCriteria.VendorId,
                    ProductId = betListByProductCriteria.ProductId
                })
                : 0;

            if (needToMapPlayerId3rdToCustId
                && betListByProductCriteria.CustId != 0
                && mappedCustId != betListByProductCriteria.CustId)
            {
                return new List<TicketDetail>();
            }

            var custId = needToMapPlayerId3rdToCustId && betListByProductCriteria.CustId == 0
                ? mappedCustId
                : betListByProductCriteria.CustId;

            return GetBetListBy(new BetListByProductCriteria
            {
                UserId = betListByProductCriteria.UserId,
                ProductId = betListByProductCriteria.ProductId,
                Username = betListByProductCriteria.Username,
                GameId = betListByProductCriteria.GameId,
                BetId = betListByProductCriteria.BetId,
                FromDate = betListByProductCriteria.FromDate,
                ToDate = betListByProductCriteria.ToDate,
                MoveBetsDay = betListByProductCriteria.MoveBetsDay,
                BalanceUpDay = betListByProductCriteria.BalanceUpDay,
                CustId = custId,
                Permission = betListByProductCriteria.Permission
            });
        }

        public IList<BetType> GetBetTypeByDate(BetTypeByDateCriteria mainCriteria)
        {
            var siteDB = ObjectDbExtensions.GetSiteDBConfigByCustId(mainCriteria.CustId);
            if (siteDB != null)
            {
                IList<BetType> siteDBTickets = new List<BetType>();
                IList<BetType> mainDBTickets = new List<BetType>();
                if (siteDB.IsSiteDB && mainCriteria.ToDate > ReportService.BalanceUp.MoveBetsDay)
                {
                    var siteDBCriteria = new BetTypeByDateInSiteDBCriteria
                    {
                        CustId = mainCriteria.CustId,
                        FromDate = mainCriteria.FromDate,
                        ToDate = mainCriteria.ToDate
                    };
                    siteDBTickets = _betListRepository.GetBetTypeByDate(siteDBCriteria, siteDB.AGIP);
                }

                if (!siteDB.IsSiteDB ||
                    (siteDB.IsSiteDB && mainCriteria.FromDate <= ReportService.BalanceUp.MoveBetsDay))
                {
                    mainCriteria.ExcludeBettrans = siteDB.IsSiteDB;
                    mainDBTickets = _betListRepository.GetBetTypeByDate(mainCriteria);
                }

                var tickets = siteDBTickets.Concat(mainDBTickets).GroupBy(g => g.BetTypeID).Select(g => g.First()).ToList();
                foreach (var ticket in tickets)
                {
                    ticket.BetTypeName += GetSportNameSuffixByBetType(ticket.BetTypeID);
                }

                if (tickets.Any())
                {
                    tickets.Insert(0, new BetType
                    {
                        BetTypeName = BetListResources.All,
                        BetTypeID = 0
                    });
                    return tickets;
                }
            }

            return new List<BetType>();
        }

        public KeyValuePair<string, IList<TicketDetail>> GetBetListByCustInfo_DateRange(
            int custId,
            DateTime fromDate,
            DateTime toDate,
            int betType,
            string statusIds,
            int pageSize,
            int pageIndex,
            ref int quantity)
        {
            var siteDB = ObjectDbExtensions.GetSiteDBConfigByCustId(custId);
            if (siteDB != null)
            {
                var ticketData = new List<ITicketData>();
                var siteDBTickets = new KeyValuePair<dynamic, IList<TicketDetail>>(null, new List<TicketDetail>());
                var mainTickets = new KeyValuePair<dynamic, IList<TicketDetail>>(null, new List<TicketDetail>());
                if (siteDB.IsSiteDB && toDate > ReportService.BalanceUp.MoveBetsDay)
                {
                    var siteDBCriteria = new SiteDBBetListByCustInfoCriteria
                    {
                        CustId = custId,
                        FromDate = fromDate,
                        ToDate = toDate,
                        BetType = betType,
                        MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                        StatusIdList = statusIds,
                        ErrCode = 0,
                        ErrMsg = string.Empty,
                        MaxTotalRow = ReportService.BetlistMaxTotalRow
                    };
                    siteDBTickets = _betListRepository.FetchWithOutputs<TicketDetail>(siteDBCriteria, siteDB.AGIP, mapTicketObjectFunction, ref ticketData);
                    if (siteDBTickets.Key.errCode != 0)
                    {
                        return new KeyValuePair<string, IList<TicketDetail>>(
                            string.Format(BetListResources.BetlistMaxTotalRowMessageError, siteDBTickets.Key.maxTotalRow, ReportService.BetlistMaxTotalRow),
                            new List<TicketDetail>());
                    }
                }
                if (!siteDB.IsSiteDB ||
                    (siteDB.IsSiteDB && fromDate <= ReportService.BalanceUp.MoveBetsDay))
                {
                    var mainCriteria = new BetListByCustInfoCriteria
                    {
                        CustId = custId,
                        FromDate = fromDate,
                        ToDate = toDate,
                        BetType = betType,
                        MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                        BalanceUpDay = ReportService.BalanceUp.BalanceUpDay,
                        StatusIdList = statusIds,
                        ErrCode = 0,
                        ErrMsg = string.Empty,
                        MaxTotalRow = ReportService.BetlistMaxTotalRow,
                        ExcludeBettrans = siteDB.IsSiteDB
                    };
                    mainTickets = _betListRepository.FetchWithOutputs<TicketDetail>(mainCriteria, mapTicketObjectFunction, ref ticketData);
                    if (mainTickets.Key.errCode != 0)
                    {
                        return new KeyValuePair<string, IList<TicketDetail>>(
                            string.Format(BetListResources.BetlistMaxTotalRowMessageError, mainTickets.Key.maxTotalRow, ReportService.BetlistMaxTotalRow),
                            new List<TicketDetail>());
                    }
                }

                var tickets = siteDBTickets.Value.Concat(mainTickets.Value).OrderBy(x => x.Ticket.TransDate).ToList();
                quantity = tickets.Count;

                var betlistHelper = new BetListHelper();
                if (pageIndex > 0)
                {
                    var factory = new BetListHTMLFactory();
                    var builder = factory.CreateBuilder();
                    GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;

                    var from = pageSize * (pageIndex - 1);
                    var newTickets = new List<TicketDetail>();
                    foreach (var betDetail in tickets.Skip(from).Take(pageSize))
                    {
                        ticketHelper.IsShowOddsWithChoice = IsShowOddsWithChoice(betDetail.Ticket.BetTypeId);

                        betDetail.TransDateHtml = builder.BuildTrans(betDetail.Ticket);
                        betDetail.ChoiceHtml = builder.BuildChoice(betDetail.Ticket, ticketHelper, ticketData, false);
                        betDetail.StakeHtml = BetListExtension.BuildStake(betDetail, ref builder, ticketHelper);
                        betDetail.OddHtml = builder.BuildOdds(betDetail.Ticket, ticketData, funcGetOddsTypeName);
                        betDetail.StatusHtml = betlistHelper.BuildStatus(betDetail.Ticket, ticketHelper, ticketData, factory, builder);
                        betDetail.IP = betDetail.Ticket.IP;
                        newTickets.Add(betDetail);

                    }
                    AssignCountry(newTickets);

                    return new KeyValuePair<string, IList<TicketDetail>>(string.Empty, newTickets);
                }
            }

            return new KeyValuePair<string, IList<TicketDetail>>(string.Empty, new List<TicketDetail>());
        }

        public KeyValuePair<string, IList<TicketDetail>> GetBetListByCustInfo_Refno(string transactionId, int customerId)
        {
            var siteDB = ObjectDbExtensions.GetSiteDBConfigByCustId(customerId);
            if (siteDB != null)
            {
                IList<TicketDetail> tickets = new List<TicketDetail>();
                List<ITicketData> outputLiveCasino = new List<ITicketData>();
                if (siteDB.IsSiteDB)
                {
                    var siteDBCriteria = new SiteDBBetListByCustInfoByRefnoCriteria()
                    {
                        RefNo = transactionId,
                        CustId = customerId,
                        MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                        BalanceUpDay = ReportService.BalanceUp.BalanceUpDay
                    };
                    tickets = _betListRepository.Fetch<TicketDetail>(siteDBCriteria, siteDB.AGIP, mapTicketObjectFunction, ref outputLiveCasino);
                }

                if (!tickets.Any())
                {
                    var mainCriteria = new BetListByCustInfoByRefnoCriteria()
                    {
                        RefNo = transactionId,
                        CustId = customerId,
                        MoveBetsDay = ReportService.BalanceUp.MoveBetsDay,
                        BalanceUpDay = ReportService.BalanceUp.BalanceUpDay,
                        ExcludeBettrans = siteDB.IsSiteDB
                    };
                    tickets = _betListRepository.Fetch<TicketDetail>(mainCriteria, mapTicketObjectFunction, ref outputLiveCasino);
                }

                var factory = new BetListHTMLFactory();
                var builder = factory.CreateBuilder();
                GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;
                var betListHelper = new BetListHelper();
                foreach (var ticket in tickets)
                {
                    ticketHelper.IsShowOddsWithChoice = IsShowOddsWithChoice(ticket.Ticket.BetTypeId);

                    ticket.TransDateHtml = builder.BuildTrans(ticket.Ticket);
                    ticket.ChoiceHtml = builder.BuildChoice(ticket.Ticket, ticketHelper, outputLiveCasino, false);
                    ticket.StakeHtml = BetListExtension.BuildStake(ticket, ref builder, ticketHelper);
                    ticket.OddHtml = builder.BuildOdds(ticket.Ticket, outputLiveCasino, funcGetOddsTypeName);
                    ticket.StatusHtml = betListHelper.BuildStatus(ticket.Ticket, ticketHelper, outputLiveCasino, factory, builder);
                    ticket.IP = ticket.Ticket.IP;
                }
                AssignCountry(tickets);

                return new KeyValuePair<string, IList<TicketDetail>>(string.Empty, tickets);
            }

            return new KeyValuePair<string, IList<TicketDetail>>(string.Empty, new List<TicketDetail>());
        }

        public SystemParlayModel GetBetListByCustInfo_SystemParLayTab(string transId, int custId)
        {
            var siteDB = ObjectDbExtensions.GetSiteDBConfigByCustId(custId);
            var systemParlayCriteria = new BetListSystemParlayByRefnoCriteria
            {
                IsSiteDB = siteDB != null && siteDB.IsSiteDB,
                CustomerId = custId,
                TransactionId = transId,
                MoveBetDay = ReportService.BalanceUp.MoveBetsDay,
                BalanceUpDay = ReportService.BalanceUp.BalanceUpDay
            };

            var outputLiveCasino = new List<ITicketData>();
            var systempParlayModel = _betListRepository.FetchSystemParlay(systemParlayCriteria);

            var betListHelper = new BetListHelper();
            if (systempParlayModel.TicketsDetailList != null &&
                systempParlayModel.TicketsDetailList.Count > 0)
            {
                var factory = new BetListHTMLFactory();
                var builder = factory.CreateBuilder();
                GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;

                foreach (TicketDetail betDetail in systempParlayModel.TicketsDetailList)
                {
                    ticketHelper.IsShowOddsWithChoice = IsShowOddsWithChoice(betDetail.Ticket.BetTypeId);

                    betDetail.TransDateHtml = builder.BuildTrans(betDetail.Ticket);
                    betDetail.ChoiceHtml = builder.BuildChoice(betDetail.Ticket, ticketHelper, outputLiveCasino, false);
                    betDetail.StakeHtml = BetListExtension.BuildStake(betDetail, ref builder, ticketHelper);
                    betDetail.OddHtml = builder.BuildOdds(betDetail.Ticket, outputLiveCasino, funcGetOddsTypeName);
                    betDetail.StatusHtml = betListHelper.BuildStatus(betDetail.Ticket, ticketHelper, outputLiveCasino, factory, builder);
                    if (betDetail.StatusHtml.Contains("Void")
                        || betDetail.StatusHtml.Contains("Abnormal Bet"))
                    {
                        systempParlayModel.HideCombinationDetail = true;
                    }
                    betDetail.IP = betDetail.Ticket.IP;
                }
                AssignCountry(systempParlayModel.TicketsDetailList);

                if (systempParlayModel.SystemParlayChoice != null)
                {
                    // Combo Parlay.
                    var firstTicketDetail = systempParlayModel.TicketsDetailList[0];
                    ITicket firstTicket = firstTicketDetail.Ticket;
                    if (!string.IsNullOrEmpty(firstTicket.BetCheck))
                    {
                        var combinationTypeComboNames = GetCombinationTypeComboNames(
                            firstTicket.BetTeam,
                            firstTicket.BetCheck,
                            firstTicket.Handicap1,
                            firstTicket.Handicap2,
                            systempParlayModel.SystemParlayChoice);

                        firstTicketDetail.ChoiceHtml += string.Format(
                            "<div class=\"hide\" id=\"combinationTypeComboNames\">{0}</div><div class=\"line\"></div>",
                            combinationTypeComboNames
                        );
                        firstTicketDetail.IP = firstTicketDetail.Ticket.IP;

                    }

                    var factoryParlayDetaily = new BetListHTMLFactory();
                    var builderParlayDetail = factoryParlayDetaily.CreateBuilder();
                    int totalItem = systempParlayModel.SystemParlayChoice.Count;
                    ticketHelper.IsShowOddsWithChoice = true;
                    foreach (TicketDetail betDetail in systempParlayModel.SystemParlayChoice)
                    {
                        totalItem--;

                        betDetail.TransDateHtml = builderParlayDetail.BuildTrans(betDetail.Ticket);
                        betDetail.ChoiceHtml =
                            "<div class='ticketList'>"
                                + builder.BuildChoice(betDetail.Ticket, ticketHelper, null, false)
                                + builder.BuildTransEventDateAndStatusResult(betDetail.Ticket, null)
                            + (totalItem > 0 ? "</div>" + "<div class=\"line\"></div>" : "</div>");

                        firstTicketDetail.ChoiceHtml += betDetail.ChoiceHtml;
                        betDetail.IP = betDetail.Ticket.IP;
                    }
                    AssignCountry(systempParlayModel.SystemParlayChoice);
                }

                systempParlayModel.SystemParlayDetail.EventList = systempParlayModel.SystemParlayDetail.EventList.OrderBy(i => SystemParlayConstants.GetStatusOrderId(i.Status)).ToList();

                return systempParlayModel;
            }

            return new SystemParlayModel();
        }

        /// <summary>
        /// Gets the list by IP address username.
        /// </summary>
        /// <param name="ip">The IP address.</param>
        /// <param name="fromDate">The from date.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="quality">The quality.</param>
        /// <returns>The list by IP address username.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<BetListByIP_Username> GetListByIP_Username(
            string ip,
            DateTime fromDate,
            int pageSize,
            int pageIndex,
            string permission,
            ref int quality)
        {
            BetListByIP_UsernameCriteria criteria = new BetListByIP_UsernameCriteria
            {
                FromDate = fromDate,
                IP = ip,
                MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay,
                BalanceUpDay = ReportUtils.BalanceUp.BalanceUpDay,
                Permission = permission
            };

            try
            {
                IList<BetListByIP_Username> usernameList = new List<BetListByIP_Username>();
                IList<BetListByIP_Username> usernameListModel = new List<BetListByIP_Username>();
                int soccerBetsTotalTmp = 0;
                int othersBetsTotalTmp = 0;
                int totalBetsTmp = 0;
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    DataSet dataSet = db.ExecuteDataSet(criteria);
                    if (dataSet != null &&
                        dataSet.Tables.Count > 0)
                    {
                        foreach (DataRow r in dataSet.Tables[0].Rows)
                        {
                            usernameList.Add(new BetListByIP_Username
                            {
                                CustID = Converter.To<int>(r["custid"]),
                                Username = r["username"].ToString(),
                                SoccerBets = Converter.To<int>(r["soccerBet"]),
                                OtherBets = Converter.To<int>(r["otherBet"]),
                                TotalBets = Converter.To<int>(r["totalBet"])
                            });

                            soccerBetsTotalTmp += Converter.To<int>(r["soccerBet"]);
                            othersBetsTotalTmp += Converter.To<int>(r["otherBet"]);
                            totalBetsTmp += Converter.To<int>(r["totalBet"]);
                        }
                    }
                }

                usernameList.Add(new BetListByIP_Username
                {
                    CustID = 0,
                    Username = BetListResources.Total,
                    SoccerBets = soccerBetsTotalTmp,
                    OtherBets = othersBetsTotalTmp,
                    TotalBets = totalBetsTmp
                });

                if (pageIndex > 0)
                {
                    quality = usernameList.Count - 1;
                    int from = pageSize * (pageIndex - 1);
                    int to = pageSize * pageIndex;

                    for (int i = from; i < usernameList.Count && i < to; i++)
                    {
                        usernameListModel.Add(usernameList[i]);
                    }

                    int pageIndexTmp = quality / pageSize;
                    if (pageIndex < pageIndexTmp)
                    {
                        usernameListModel.Add(new BetListByIP_Username
                        {
                            CustID = 0,
                            Username = BetListResources.Total,
                            SoccerBets = soccerBetsTotalTmp,
                            OtherBets = othersBetsTotalTmp,
                            TotalBets = totalBetsTmp
                        });
                    }

                    return usernameListModel;
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return new List<BetListByIP_Username>();
        }

        /// <summary>
        /// Gets the bet list by IP address soccer.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="isSoccer">If set to <c>true</c> [is soccer].</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="quality">The quality.</param>
        /// <param name="totalComm">The total commission.</param>
        /// <param name="totalWinLoss">The total win loss.</param>
        /// <returns>The bet list by IP address soccer.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<TicketDetail> GetBetListByIP_Soccer(
            int custId,
            string ip,
            DateTime fromDate,
            bool isSoccer,
            int pageSize,
            int pageIndex,
            DateTime moveBetsDay,
            DateTime balanceUpDay,
            ref int quality,
            ref decimal totalComm,
            ref decimal totalWinLoss)
        {
            try
            {
                List<ITicketData> outputLiveCasino = new List<ITicketData>();
                IList<TicketDetail> betDetails = new List<TicketDetail>();
                IList<TicketDetail> betDetailsModel = new List<TicketDetail>();
                DataSet dataSet = new DataSet();

                if (isSoccer)
                {
                    var criteria = new BetListByIP_SoccerCriteria
                    {
                        CustId = custId,
                        IP = ip,
                        FromDate = fromDate,
                        MoveBetsDay = moveBetsDay,
                        BalanceUpDay = balanceUpDay
                    };

                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        dataSet = db.ExecuteDataSet(criteria);
                    }
                }
                else
                {
                    var criteria = new BetListByIP_OthersCriteria
                    {
                        CustId = custId,
                        IP = ip,
                        FromDate = fromDate,
                        MoveBetsDay = moveBetsDay,
                        BalanceUpDay = balanceUpDay
                    };

                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        dataSet = db.ExecuteDataSet(criteria);
                    }
                }

                if (dataSet != null &&
                    dataSet.Tables.Count >= 2)
                {
                    DataTable ticketDetailTable = dataSet.Tables[0];
                    DataTable ticketEventTable = dataSet.Tables[1];
                    foreach (DataRow r in ticketDetailTable.Rows)
                    {
                        decimal commTmp = Converter.ToDecimal(r["commission"]);
                        decimal winlostTmp = Converter.ToDecimal(r["winlost"]);
                        string status = Converter.ToString2(r["status"]).ToLower();

                        if (status != "running" &&
                            status != "waiting" &&
                            status != "void" &&
                            status != "reject" &&
                            status != "pending")
                        {
                            totalComm += commTmp;
                            totalWinLoss += winlostTmp;
                        }

                        TicketDetail ticketDetailTmp = new TicketDetail
                        {
                            Ticket = new Ticket
                            {
                                ActualStake = Converter.To<decimal>(r["actual_stake"]),
                                AgentPositionTaking = 0,
                                Comm = 0,
                                AwayId = Converter.To<long>(r["awayid"]),
                                BetId = Converter.To<long>(r["betid"]),
                                BetTeam = Converter.ToString2(r["betteam"]),
                                BetTypeId = Converter.To<int>(r["bettype"]),
                                CommStatus = Converter.To<string>(r["comstatus"]),
                                CustId = custId,
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

                                MasterPositionTaking = 0,
                                AgentComm = 0,
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
                                SuperPositionTaking = 0,
                                SuperComm = 0,
                                TransDate = Converter.ToDateTime(r["transdate"]),
                                TransId = Converter.To<long>(r["transid"]),
                                UserName = Converter.ToString2(r["username"]),
                                Winlost = Converter.ToDecimal(r["winlost"]),
                                WinlostDate = Convert.ToDateTime(r["winlostdate"]),
                                TransDesc = Converter.ToString2(r["transdesc"]),

                                BetCheck = Convert.ToString(r["betcheck"]),
                                OddsSpread = Convert.ToDecimal(r["OddsSpread"]),

                                OriginalBetType = Utils.ToInt32(r["OriginalBetType"]),
                                BetdaqId = Convert.ToInt64(r["BetdaqId"]),
                                ExtraInfo = Converter.ToString2(r["ExtraInfo"]),

                                Currency = r.Table.Columns.Contains("Currency") ? Converter.ToString2(r["Currency"]) : string.Empty,
                                WalletType = Utils.ToByte(r["wallettype"]),
                                RefCode = Utils.ToInt64(r["refcode"]),
                                SettlementStatus = r.Table.Columns.Contains("SettlementStatus") ? Convert.ToByte(r["SettlementStatus"]) : (byte)0,
                                CurrencyId = r.Table.Columns.Contains("CurrencyId") ? Converter.ToInt(r["CurrencyId"]) : 0,
                                SiteId = r.Table.Columns.Contains("SiteId") ? Converter.ToString2(r["SiteId"]) : ""
                            },
                            ChoiceHtml = string.Empty,
                            OddHtml = string.Empty,
                            StakeHtml = string.Empty,
                            Creator = Converter.ToString2(r["creator"]),
                            IP = Converter.ToString2(r["ip"]),
                            DeviceType = r.Table.Columns.Contains("DeviceType") && !r.IsNull("DeviceType") ? Converter.To<int>(r["DeviceType"]) : (int?)null
                        };

                        betDetails.Add(ticketDetailTmp);
                    }

                    foreach (DataRow row in ticketEventTable.Rows)
                    {
                        outputLiveCasino.Add(new TicketData
                        {
                            RefNo = Converter.ToString2(row["refno"]),
                            BetTypeId = Converter.To<int>(row["bettype"]),
                            Status = Converter.ToString2(row["status"]),
                            Odds = Converter.To<decimal>(row["odds"]),
                            Stake = Converter.To<decimal>(row["stake"]),
                            BetTeam = Converter.ToString2(row["betteam"])
                        });
                    }
                }

                if (betDetails != null && pageIndex > 0)
                {
                    BetListHTMLFactory factory = new BetListHTMLFactory();
                    BetListHTMLBuilder builder = factory.CreateBuilder();
                    GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;
                    BetListHelper betlistHelper = new BetListHelper();

                    int from = pageSize * (pageIndex - 1);
                    int to = pageSize * pageIndex;
                    quality = betDetails.Count;

                    for (int i = from; i < betDetails.Count && i < to; i++)
                    {
                        betDetailsModel.Add(betDetails[i]);
                    }

                    foreach (TicketDetail betDetail in betDetailsModel)
                    {
                        ticketHelper.IsShowOddsWithChoice = IsShowOddsWithChoice(betDetail.Ticket.BetTypeId);

                        betDetail.TransDateHtml = builder.BuildTrans(betDetail.Ticket);
                        betDetail.ChoiceHtml = builder.BuildChoice(betDetail.Ticket, ticketHelper, outputLiveCasino, false);
                        betDetail.StakeHtml = BetListExtension.BuildStake(betDetail, ref builder, ticketHelper);
                        betDetail.OddHtml = builder.BuildOdds(betDetail.Ticket, outputLiveCasino, funcGetOddsTypeName);
                        betDetail.StatusHtml = betlistHelper.BuildStatus(betDetail.Ticket, ticketHelper, outputLiveCasino, factory, builder);
                        betDetail.IP = betDetail.Ticket.IP;
                    }
                    AssignCountry(betDetails);

                    return betDetailsModel;
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return new List<TicketDetail>();
        }

        public IList<TicketDetail> GetBetListStatement(
            int custId,
            int transType,
            DateTime fromDate,
            ref int quality,
            ref decimal totalComm,
            ref decimal totalWinLoss)
        {
            var siteDB = ObjectDbExtensions.GetSiteDBConfigByCustId(custId);
            if (siteDB != null)
            {
                List<ITicketData> outputLiveCasino = new List<ITicketData>();
                IList<TicketDetail> betDetails = new List<TicketDetail>();
                var betListHelper = new BetListHelper();
                var dataSet = new DataSet();

                var criteria = new BetListStatement
                {
                    CustId = custId,
                    TransType = transType,
                    FromDate = fromDate,
                    MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay,
                    IsSiteDB = siteDB.IsSiteDB
                };

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    dataSet = siteDB.IsSiteDB ? db.ExecuteDataSetScaleoutDB(criteria.GetSpParams(), siteDB.AGIP) :
                        db.ExecuteDataSet(criteria.GetSpParams());
                }

                if (dataSet != null &&
                    dataSet.Tables.Count >= 2)
                {
                    var ticketDetailTable = dataSet.Tables[0];
                    var ticketEventTable = dataSet.Tables[1];
                    foreach (DataRow r in ticketDetailTable.Rows)
                    {
                        var commTmp = Converter.ToDecimal(r["commission"]);
                        var winlostTmp = Converter.ToDecimal(r["winlost"]);
                        var status = Converter.ToString2(r["status"]).ToLower();

                        if (status != "running" &&
                            status != "waiting" &&
                            status != "void" &&
                            status != "reject" &&
                            status != "pending")
                        {
                            totalComm += commTmp;
                            totalWinLoss += winlostTmp;
                        }

                        var ticketDetailTmp = new TicketDetail
                        {
                            Ticket = new Ticket
                            {
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
                                CustId = custId,
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
                                UserName = Converter.ToString2(r["username"]),
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
                                SettlementStatus = r.Table.Columns.Contains("SettlementStatus") ? Convert.ToByte(r["SettlementStatus"]) : (byte)0,
                                SiteId = r.Table.Columns.Contains("SiteId") ? Converter.ToString2(r["SiteId"]) : "",
                                CurrencyId = r.Table.Columns.Contains("CurrencyId") ? Converter.ToInt(r["CurrencyId"]) : 0,
                            },
                            ChoiceHtml = string.Empty,
                            OddHtml = string.Empty,
                            StakeHtml = string.Empty,
                            IsShowOtExRate = Converter.To<bool>(r["isShowOTExRate"]),
                            VirtualRate = Converter.ToDecimal(r["virtualrate"]),
                            Creator = Converter.ToString2(r["creator"]),
                            FinalHomeScore = Converter.ToString2(r["finalhomescore"]),
                            FinalAwayScore = Converter.ToString2(r["finalawayscore"]),
                            HTHomeScore = Converter.ToString2(r["hthomescore"]),
                            HTAwayScore = Converter.ToString2(r["htawayscore"]),
                            IP = Converter.ToString2(r["ip"]),
                            DeviceType = r.Table.Columns.Contains("DeviceType") && !r.IsNull("DeviceType") ? Converter.To<int>(r["DeviceType"]) : (int?)null
                        };

                        foreach (DataRow row in ticketEventTable.Rows)
                        {
                            if (Converter.ToString2(row["refno"]) == ticketDetailTmp.Ticket.RefNo)
                            {
                                ticketDetailTmp.Ticket.AgentPositionTaking = Converter.ToDecimal(row["apositiontaking"]);
                                ticketDetailTmp.Ticket.MasterPositionTaking = Converter.ToDecimal(row["mpositiontaking"]);
                                ticketDetailTmp.Ticket.SuperPositionTaking = Converter.ToDecimal(row["spositiontaking"]);
                                break;
                            }
                        }

                        betDetails.Add(ticketDetailTmp);
                    }

                    foreach (DataRow row in ticketEventTable.Rows)
                    {
                        outputLiveCasino.Add(new TicketData
                        {
                            RefNo = Converter.ToString2(row["refno"]),
                            BetTypeId = Converter.To<int>(row["bettype"]),
                            Status = Converter.ToString2(row["status"]),
                            Odds = Converter.To<decimal>(row["odds"]),
                            Stake = Converter.To<decimal>(row["stake"]),
                            BetTeam = Converter.ToString2(row["betteam"]),
                            TransDesc = Converter.ToString2(row["TransDesc"])
                        });
                    }
                }

                if (betDetails != null)
                {
                    var factory = new BetListHTMLFactory();
                    var builder = factory.CreateBuilder();
                    GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;
                    SessionManager.Set<IList<TicketDetail>>(BetListConstants.BetListByStatementSessionKey, betDetails);
                    foreach (var betDetail in betDetails)
                    {
                        ticketHelper.IsShowOddsWithChoice = IsShowOddsWithChoice(betDetail.Ticket.BetTypeId);

                        betDetail.TransDateHtml = builder.BuildTrans(betDetail.Ticket);
                        betDetail.ChoiceHtml = builder.BuildChoice(betDetail.Ticket, ticketHelper, outputLiveCasino, false);
                        betDetail.StakeHtml = BetListExtension.BuildStake(betDetail, ref builder, ticketHelper);
                        betDetail.OddHtml = builder.BuildOdds(betDetail.Ticket, outputLiveCasino, funcGetOddsTypeName);
                        betDetail.StatusHtml = betListHelper.BuildStatus(betDetail.Ticket, ticketHelper, outputLiveCasino, factory, builder);
                        betDetail.IP = betDetail.Ticket.IP;
                    }
                    AssignCountry(betDetails);
                    return betDetails;
                }
            }


            return new List<TicketDetail>();
        }

        /// <summary>
        /// Gets the customer identifier by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The customer identifier by username.</returns>
        public int GetCustIDByUsername(string username = "")
        {
            int custid = 0;
            try
            {
                DataSet dataSet = new DataSet();
                var criteria = new CustIDByUsernameCriteria
                {
                    CustomerName = username
                };

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    dataSet = db.ExecuteDataSet(criteria.GetSpParams());
                }

                if (dataSet != null &&
                    dataSet.Tables.Count > 0 &&
                    dataSet.Tables[0].Rows.Count > 0)
                {
                    custid = Converter.To<int>(dataSet.Tables[0].Rows[0]["custid"]);
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return custid;
        }

        /// <summary>
        /// Determines whether [is u3 special account] [the specified customer identifier].
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <returns>Is U3 special customer or not.</returns>
        public bool IsU3SpecialCustomers(int custId)
        {
            string[] specialCustomersU3 = Convert.ToString(ConfigurationManager.AppSettings["U3SpecialCustomers"]).Split(',');
            string customerId = Convert.ToString(custId);

            for (int i = 0; i < specialCustomersU3.Length; i++)
            {
                if (customerId == specialCustomersU3[i].Trim())
                {
                    return true;
                }
            }

            return false;
        }

        public string GetCombinationTypeComboNames(
            string betTeam,
            string betCheck,
            decimal hdp1,
            decimal hdp2,
            IList<TicketDetail> ticketDetails)
        {
            var combinationTypeCombo = Fanex.Tarus.Infrastructure.Caching.CacheRef.GetCombinationTypeCombo(
                betTeam, betCheck, (int)hdp1, (int)hdp2);

            if (combinationTypeCombo == null || combinationTypeCombo.Length == 0)
            {
                return string.Empty;
            }

            var combinationTypeComboTickets = ticketDetails
                .Where(t => t.Ticket.Stake > 0)
                .OrderBy(t => t.Ticket.MatchId)
                .ToList();
            StringBuilder result = new StringBuilder();
            TicketDetail ticketDetail;
            for (int i = 0, max = combinationTypeCombo.Length; i < max; i++)
            {
                ticketDetail = hdp1 == 0 ? combinationTypeComboTickets[0] : combinationTypeComboTickets[i];
                result.Append(string.Format(
                    "<div>{0} x <span class=\"unit-stake\">{1} {2}</span></div>",
                    combinationTypeCombo[i],
                    ticketDetail.Ticket.Currency,
                    ticketDetail.Ticket.Stake.ToString("N0")));
            }

            return "<div class=\"offer-name-list\">" + result.ToString() + "</div>";
        }

        public string GetSiteNameByCustId(int custId)
        {
            return _betListRepository.GetSiteNameByCustId(new BetListGetSiteIdByCustIdCriteria { CustId = custId });
        }

        private bool IsShowOddsWithChoice(int betTypeId)
        {
            return betTypeId == BetTypeContants.MixParlay ||
                betTypeId == BetTypeContants.SystemParlay;
        }

        private string GetSportNameSuffixByBetType(int betTypeId)
        {
            var sportName = string.Empty;

            if (ReportService.IsVirtualSportBetType(betTypeId))
            {
                sportName = BetListResources.Virtual_Sports;
            }
            else if (betTypeId == BetTypeContants.FootballCup)
            {
                const int VirtualSports3ProductId = 49;
                sportName = GetProductName(VirtualSports3ProductId);
            }
            else if (ReportService.IsAGCasino(betTypeId))
            {
                sportName = BetListResources.AGCasino;
            }
            else if (betTypeId == BetTypeContants.OverUnder ||
                betTypeId == BetTypeContants.OddEven)
            {
                sportName += BetListResources.Number_Game;
            }
            else if (ReportService.IsRNGGame_RNGCasino(betTypeId))
            {
                sportName += ticketHelper.GetSportNameById(SportContants.RNG_RNGGame);
            }
            else if (ReportService.IsMiniGame_RNGCasino(betTypeId))
            {
                sportName += ticketHelper.GetSportNameById(SportContants.RNG_MiniGame);
            }
            else if (ReportService.IsMobile_RNGCasino(betTypeId))
            {
                sportName += ticketHelper.GetSportNameById(SportContants.RNG_Mobile);
            }
            else if (ReportService.IsAllBet(betTypeId))
            {
                sportName += ticketHelper.GetSportNameById(SportContants.AllBet);
            }
            else if (ReportService.IsSAGaming(betTypeId))
            {
                sportName += ticketHelper.GetSportNameById(SportContants.SaGaming);
            }
            else if (ReportService.IsSexyGaming(betTypeId))
            {
                sportName += ticketHelper.GetSportNameById(SportContants.SexyGaming);
            }
            else if (ReportService.IsGTCasino(betTypeId))
            {
                sportName += ticketHelper.GetSportNameById(SportContants.GTCasino);
            }
            else if (ReportService.IsVoidbridge(betTypeId))
            {
                sportName += ticketHelper.GetSportNameById(SportContants.Voidbridge);
            }
            else if (ReportService.IsPP(betTypeId))
            {
                sportName += ticketHelper.GetSportNameById(SportContants.PP);
            }
            else if (ReportService.IsVirtualSports2(betTypeId))
            {
                const int VirtualSports2ProductId = 27;
                sportName = GetProductName(VirtualSports2ProductId);
            }
            else if (ReportService.IsVirtualSports3(betTypeId))
            {
                const int VirtualSports3ProductId = 77;
                sportName = GetProductName(VirtualSports3ProductId);
            }
            else if (ReportService.IsVirtualSports4(betTypeId))
            {
                const int VirtualSports4ProductId = 83;
                sportName = GetProductName(VirtualSports4ProductId);
            }
            else if (ReportService.IsSabaclub(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.Sabaclub);
            }
            else if (ReportService.IsHappy5Bettype(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.Happy5);
            }
            else if (ReportService.IsCardClub(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.CardClub);
            }
            else if (ReportService.IsSpadegaming(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.Spadegaming);
            }
            else if (ReportService.IsVirtualGames(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.VirtualGames);
            }
            else if (ReportService.IsTableGames(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.TableGames);
            }
            else if (ReportService.IsBBin(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.BBin);
            }
            else if (ReportService.IsFGG(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.FGG);
            }
            else if (ReportService.IsSealEntertainment(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.SealEntertainment);
            }
            else if (ReportService.IsMGRNG(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.MGRNG);
            }
            else if (ReportService.IsJili(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.Jili);
            }
            else if (ReportService.IsYeeBet(betTypeId))
            {
                sportName = ticketHelper.GetSportNameById(SportContants.YeeBet);
            }

            else
            {
                var sport = ReportService.GetSportFromBetTypeId(betTypeId);
                sportName = sport == 0 ? string.Empty : ticketHelper.GetSportNameById(sport);
            }

            return string.IsNullOrEmpty(sportName) ? string.Empty : " (" + sportName + ")";
        }

        private string GetProductName(int productId)
        {
            return ticketHelper.GetResourceData("Product", productId.ToString());
        }
    }
}