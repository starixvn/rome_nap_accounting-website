using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Customer;
using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
using Sunplus.Accounting.Entities.Report.Licensee;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Accounting.Business.BetList
{
    public class BetListManager : IBetListManager
    {
        private IBetListDAO _betlistDAO;

        public BetListManager()
        {
            _betlistDAO = CommonBootStrapper.Locator.GetInstance<IBetListDAO>();
        }

        public BetListManager(IBetListDAO betListDao)
        {
            _betlistDAO = betListDao;
        }

        public List<BetListDetail> GetLatestBetList(int totalRow)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return new List<BetListDetail>();
            }
            ;
        }

        public List<BetMonitor> GetLatestBABetList(int totalRow, int amount, Int64 lastTranIs)
        {
            try
            {
                var betList = _betlistDAO.GetLatestBABetList(totalRow, amount, lastTranIs);
                return betList;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return new List<BetMonitor>();
            }
            ;
        }

        //public BABetList GetBABetList(int pagesize, int pageindex, string fromdate, string todate, string refno, string username)
        //{
        //    try
        //    {
        //        var betList = _betlistDAO.GetBABetList(pagesize, pageindex, fromdate, todate, refno, username, IpManagement.IPVNList);
        //        return betList;

        //    }
        //    catch (Exception exception)
        //    {
        //        LogExtensions.ExceptionHandler(exception);
        //        return new BABetList() { Items=null, totalItem=0};
        //    }

        //}

        public BABetList GetBABetList(int pagesize, int pageindex, string fromdate, string todate, string refno, string username, int currency, int bettype, int fromStake, int toStake, int excludeTest)
        {
            try
            {
                var betlist = new BABetList();
                if (string.IsNullOrEmpty(refno))
                {
                    betlist = _betlistDAO.GetBABetList(pagesize, pageindex, fromdate, todate, username, IpManagement.IPVNList, currency, bettype, fromStake, toStake, excludeTest);
                }
                else
                {
                    betlist = _betlistDAO.GetBABetListByTransId(refno, IpManagement.IPVNList);
                }
                return betlist;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return new BABetList() { Items = null, totalItem = 0 };
            }
        }

        public List<BetListDetail> GetMixParlayTickets(string refNo)
        {
            try
            {
                var listMpTicket = _betlistDAO.GetMixParlayTickets(refNo);

                return listMpTicket;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return new List<BetListDetail>();
            }
        }

        public LicenseeDTO GetDatetimeOption(string dateOption)
        {
            DateTime fromdate, todate;
            switch (dateOption)
            {
                case "0": //default
                case "1": //today
                    fromdate = todate = DateTime.Now;
                    break;

                case "2": //Yesterday
                    fromdate = DateTime.Now.AddDays(-1);
                    todate = DateTime.Now.AddDays(-1);
                    break;

                case "3": //current week
                    fromdate = DateTime.Now.AddDays(-1 * Convert.ToInt32(DateTime.Now.DayOfWeek));
                    todate = fromdate.AddDays(6);
                    break;

                case "4": //last week
                    fromdate = DateTime.Now.AddDays(-1 * Convert.ToInt32(DateTime.Now.DayOfWeek) - 7);
                    todate = fromdate.AddDays(6);
                    break;

                case "5": //Current Month
                    fromdate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1));
                    //todate = DateTime.Now;
                    todate = DateTime.Now.AddDays((DateTime.Now.Day));
                    break;

                case "6": //Last Month
                    fromdate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1)).AddMonths(-1);
                    todate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day));
                    break;

                case "7": //Since Last Month
                    fromdate = DateTime.Now.AddDays(-1 * (DateTime.Now.Day - 1)).AddMonths(-1);
                    todate = DateTime.Now;
                    break;

                default:
                    fromdate = todate = DateTime.Now;
                    break;
            }
            LicenseeDTO depositDto = new LicenseeDTO();
            depositDto.fdate = fromdate.ToShortDateString();
            depositDto.tdate = todate.ToShortDateString();
            return depositDto;
        }

        public List<Bettype> GetBettype()
        {
            var bettypelist = new List<Bettype>();
            var dbResult = _betlistDAO.GetBettype();
            foreach (DataRow row in dbResult.Rows)
            {
                var bettype = new Bettype();
                bettype.BettypeId = CastAide.AsInt32(row["Bettype"]);
                bettype.BettypeName = CastAide.AsString(row["BettypeName"]);
                bettypelist.Add(bettype);
            }
            return bettypelist;
        }

        public List<CustDetail> GetCurrency()
        {
            var currencylist = new List<CustDetail>();
            var dbResult = _betlistDAO.GetCurrency();
            foreach (DataRow row in dbResult.Rows)
            {
                var currency = new CustDetail();
                currency.CurrencyId = CastAide.AsInt32(row["currency"]);
                currency.CurrencyName = CastAide.AsString(row["currencyname"]);
                currencylist.Add(currency);
            }
            return currencylist;
        }

        public TicketsListingBetlistDTO GetTicketsListingBetlist(bool isMain, int Type, int matchID, string Licensees, string bettypes, string bettypesLive, int groupBy, int Language, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDate, int SAFilter)
        {
            try
            {
                var betList = _betlistDAO.GetTicketsListingBetlist(false, isMain, Type, matchID, Licensees, bettypes, bettypesLive, groupBy, Language, LiveType, HomeScore, AwayScore, FromTransDate, ToTransDate, SAFilter);
                return betList;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                return null;
            }
        }

        #region IBetListManager Members

        public CombinationDetailMP GetCombinationDetailMP(string refNo)
        {
            throw new NotImplementedException();
        }

        #endregion IBetListManager Members

        public SystemParlayDetail GetBetListSystemParlayDetailByTransId(string refNo, int custId)
        {
            var result = new SystemParlayDetail();
            result.BetList = new List<BetListSystemParlayDetail>();
            result.DetailMp = new CombinationDetailMP();
            result.SystemParlayCombinationDetailList = new List<SystemParlayDetailGroup>();
            var movebetDay = ReportUtils.BalanceUp.MoveBetsDay;
            var balanceUpDay = ReportUtils.BalanceUp.BalanceUpDay;

            var betlist = _betlistDAO.GetBetListSystemParlayByTransId(refNo, custId, movebetDay, balanceUpDay);

            if (betlist != null && betlist.Tables.Count > 0)
            {
                var tbTransInfo = betlist.Tables[0];
                var tbChoice = betlist.Tables[1];
                var tbTransDetail = betlist.Tables[2];
                var tbEventDetail = betlist.Tables[3];

                if (tbTransInfo != null && tbTransInfo.Rows.Count > 0)
                {
                    foreach (DataRow dr in tbTransInfo.Rows)
                    {
                        int doubleComm = WebSettings.OfferDoubleComm ? CastAide.AsInt32(dr["comstatus"]) : 0;
                        var status = CastAide.AsString(dr["status"]);
                        bool bRunning = status.ToLower().Equals("running");
                        var betlistDetail = new BetListSystemParlayDetail()
                        {
                            Transid = CastAide.AsString(dr["transid"]),
                            TransDate = string.Format("<b>Ref No: {0}</b> {1}", dr["transid"], dr["transdate"]),
                            //Odds = status.ToLower().Equals("running") || status.ToLower().Equals("lose") ? "-" : IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDouble(dr["odds"])),
                            Odds = status.ToLower().Equals("running") ? "-" : (IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDouble(dr["odds"]))),
                            Stake =
                                string.Format(
                                    "{0}{1}",
                                    IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["actual_stake"])),
                                    (Math.Round(CastAide.AsDouble(dr["stake"], 0)) == 0
                                         ? string.Empty
                                         : "<br/>("
                                           + IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["stake"])) + ")")),
                            PlayerWinLoss =
                                status.ToLower().Equals("running")
                                    ? "-"
                                    : (((CastAide.AsDouble(dr["winlost"]) < 0
                                             ? ("<lable style=\"color:red\">"
                                                +
                                                IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["winlost"]))
                                                + "</lable>")
                                             : (IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["winlost"])))))
                                       + "<br/>"
                                       + IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["playercomm"]))),
                            Status =
                                SystemParlayStatus(
                                    status,
                                    CastAide.AsString(dr["refno"]),
                                    CastAide.AsString(dr["ip"]),
                                    CastAide.AsString(dr["transdate"])),
                            SuperPT = string.Format("<span class=\"bl-sporttype\">{0}%</span><br/>", IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["spositiontaking"]) * 100)),
                            SuperWinLost = string.Format("<span class=\"bl-sporttype\">{0}</span>", bRunning ? string.Empty : IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["swinlost"])) + "<br/>"),
                            SuperDiscount = string.Format("{0}%<br/>", doubleComm == 1 ? "2x " + IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["sdiscount"]) * 100) : IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["sdiscount"]) * 100)),
                            SuperComm = string.Format("{0}", bRunning ? string.Empty : IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["scomm"])) + "<br/>"),

                            MasterPT = string.Format("<span class=\"bl-sporttype\">{0}%</span><br/>", IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["mpositiontaking"]) * 100)),
                            MasterWinLost = string.Format("<span class=\"bl-sporttype\">{0}</span>", bRunning ? string.Empty : IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["mwinlost"])) + "<br/>"),
                            MasterDiscount = string.Format("{0}%<br/>", doubleComm == 1 ? "2x " + IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["adiscount"]) * 100) : IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["adiscount"]) * 100)),
                            MasterComm = string.Format("{0}", bRunning ? string.Empty : IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["acomm"])) + "<br/>"),

                            AgentPT = string.Format("<span class=\"bl-sporttype\">{0}%</span><br/>", IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["apositiontaking"]) * 100)),
                            AgentWinLost = string.Format("<span class=\"bl-sporttype\">{0}</span>", bRunning ? string.Empty : IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["awinlost"])) + "<br/>"),
                            AgentDiscount = string.Format("{0}%<br/>", doubleComm == 1 ? "2x " + IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["discount"]) * 100) : IBCUtils.FormatHandler.FormatNumber3(CastAide.AsDecimal(dr["discount"]) * 100)),
                            AgentComm = string.Format("{0}", bRunning ? string.Empty : IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["comm"])) + "<br/>"),

                            Choice = string.Format("<span style=\"color:blue\">System Parlay</span> - <span style=\"color:gray\">{0}</span>", CacheRef.GetCombinationName(CastAide.AsString(dr["betteam"]))),
                            Event = new BetlistGenerateEvent().GenMixParlay_WithHorizontalLine(tbChoice)
                        };
                        result.BetList.Add(betlistDetail);
                    }
                }

                if (tbTransDetail != null && tbTransDetail.Rows.Count > 0)
                {
                    var dateRange =
                        (from q in tbTransDetail.AsEnumerable().Cast<DataRow>() select q["winlostdate"]).Distinct().
                            ToList();

                    var runningTickets = new SystemParlayDetailGroup();
                    runningTickets.Items = new List<SystemParlayCombinationDetail>();

                    foreach (DateTime wlDate in dateRange)
                    {
                        var selWlDetail =
                            (from q in tbTransDetail.AsEnumerable()
                             where CastAide.AsDateTime(q["winlostdate"]) == wlDate
                             select q).ToList();

                        var wlDetailMerge = new SystemParlayDetailGroup();
                        wlDetailMerge.Items = new List<SystemParlayCombinationDetail>();
                        wlDetailMerge.SubTotal = new SystemParlayCombinationDetail();
                        double winlossTotal = 0;

                        foreach (DataRow dr in selWlDetail)
                        {
                            var status = CastAide.AsString(dr["status"]);
                            if (status.ToLower().Equals("running"))
                            {
                                runningTickets.Items.Add(
                                    new SystemParlayCombinationDetail()
                                        {
                                            WinLostDate = "-",
                                            Event =
                                                GetCombinationDetailBySerial(
                                                    CastAide.AsString(dr["serial"]), tbEventDetail),
                                            Odds = "-",
                                            Stake =
                                                IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["stake"])),
                                            WinLoss = "-",
                                            Status = status
                                        });
                            }
                            else
                            {
                                var combinationDetail = new SystemParlayCombinationDetail()
                                {
                                    WinLostDate =
                                        status.ToLower().Equals("running")
                                            ? "-"
                                            : CastAide.AsDateTime(dr["winlostdate"]).ToShortDateString(),
                                    Event = GetCombinationDetailBySerial(CastAide.AsString(dr["serial"]), tbEventDetail),
                                    Odds =
                                        status.ToLower().Equals("running") ? "-" : CastAide.AsDouble(dr["modds"]).ToString(),
                                    Stake = IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["stake"])),
                                    WinLoss = status.ToLower().Equals("running")
                                            ? "-"
                                            : (CastAide.AsDouble(dr["winlost"]) < 0
                                                   ? "<lable style=\"color:red\">"
                                                     +
                                                     IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["winlost"]))
                                                     + "</label>"
                                                   : IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(dr["winlost"]))),
                                    Status = status
                                };

                                wlDetailMerge.Items.Add(combinationDetail);
                                winlossTotal += CastAide.AsDouble(dr["winlost"]);
                            }
                        }

                        if (wlDetailMerge.Items != null && wlDetailMerge.Items.Count > 0)
                        {
                            wlDetailMerge.SubTotal.WinLoss = winlossTotal < 0
                                               ? "<lable style=\"color:red\">"
                                                 +
                                                 IBCUtils.FormatHandler.FormatNumber20(winlossTotal)
                                                 + "</label>"
                                               : IBCUtils.FormatHandler.FormatNumber20(winlossTotal);
                            wlDetailMerge.TotalItems = wlDetailMerge.Items.Count;

                            result.SystemParlayCombinationDetailList.Add(wlDetailMerge);
                        }
                    }

                    #region add running tickets to result

                    if (runningTickets.Items != null && runningTickets.Items.Count > 0)
                    {
                        runningTickets.TotalItems = runningTickets.Items.Count;
                        runningTickets.SubTotal = new SystemParlayCombinationDetail()
                        {
                            WinLoss = "-",
                            WinLostDate = "-"
                        };
                        result.SystemParlayCombinationDetailList.Add(runningTickets);
                    }

                    #endregion add running tickets to result
                }
            }

            return result;
        }

        private string SystemParlayStatus(string status, string refNo, string ip, string transDate)
        {
            var statusMapped = string.Empty;
            switch (status.ToLower())
            {
                case "running":
                case "waiting":
                    statusMapped = status.ToUpper();
                    break;

                case "reject":
                case "refund":
                case "draw":
                case "lose":
                case "won":
                    statusMapped = status.ToUpper();
                    statusMapped += "<br/><span class='hand' style='color: #1BA4ED;' onclick=\"ViewCombinationMixParlayResult('" + refNo + "')\">Result</span> ";
                    break;

                case "void":
                    statusMapped = string.Format("<span style='color:red'>{0}</span>", status.ToUpper());
                    break;

                default: break;
            }
            var betlistIpLink = string.Format("<a href=\"BetListByIP.aspx?bltype=3&fdate=" + (Convert.ToDateTime(transDate)).ToShortDateString() + "&ip={0}\">{0}</a>", IpManagement.ReplaceVNIP(CastAide.AsString(ip)));
            statusMapped += string.Format("<br/>{0}", betlistIpLink);
            return statusMapped;
        }

        private string GetCombinationDetailBySerial(string serialId, DataTable dtCombinations)
        {
            var result = string.Empty;

            foreach (DataRow dr in dtCombinations.Rows)
            {
                if (CastAide.AsInt64(dr["serial"]) == CastAide.AsInt64(serialId))
                {
                    result += string.Format("{0}-VS-{1}<br/>", CacheRef.GetTeamName(dr["homeid"]), CacheRef.GetTeamName(dr["awayid"]));
                }
            }

            return result;
        }
    }
}