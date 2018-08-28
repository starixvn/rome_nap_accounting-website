namespace Fanex.Tarus.BetList.Business
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using Builders.ExcelBuilders;
    using Constants;
    using Criteria;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Common;
    using Fanex.Data;
    using Models;
    using NPOI.SS.UserModel;
    using Sunplus.Accounting.DataAccess.Report;
    using AccBetListCore = Fanex.Tarus.BetList.Core;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class ExportExcelBetlist
    {
        /// <summary>
        /// Exports the excel bet list statement.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="transType">Type of the trans.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The excel bet list statement.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public MemoryStream ExportExcelBetListStatement(
            int custId,
            int transType,
            DateTime fromDate,
            ref string fileName)
        {
            try
            {
                List<ITicketData> outputLiveCasino = new List<ITicketData>();
                IList<TicketDetail> betDetails = new List<TicketDetail>();
                ITicketHelper ticketHelper = new TicketHelper();
                IList<TicketDetail> betDetailsModel = new List<TicketDetail>();
                StringBuilder content = new StringBuilder();
                DataSet dataSet = new DataSet();

                betDetails = AccBetListCore.SessionManager.Get<IList<TicketDetail>>(BetListConstants.BetListByStatementSessionKey);
                outputLiveCasino = AccBetListCore.SessionManager.Get<List<ITicketData>>(BetListConstants.BetListByStatementOutputLiveCasinoSessionKey);

                IList<TicketDetail> getCache = AccBetListCore.SessionManager.Get<IList<TicketDetail>>("Bet list statement");

                if (outputLiveCasino == null && outputLiveCasino.Count == 0)
                {
                    var criteria = new BetListStatement
                    {
                        CustId = custId,
                        TransType = transType,
                        FromDate = fromDate,
                        MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay
                    };

                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        dataSet = db.ExecuteDataSet(criteria);
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
                            TicketDetail ticketDetailTmp = new TicketDetail
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
                                    WinlostDate = Convert.ToDateTime(r["transdate"]),
                                    TransDesc = Converter.ToString2(r["TransDesc"]),
                                    OddsSpread = Convert.ToDecimal(r["OddsSpread"]),

                                    OriginalBetType = Convert.ToInt32(r["OriginalBetType"]),
                                    BetdaqId = Convert.ToInt64(r["BetdaqId"]),
                                    ExtraInfo = Converter.ToString2(r["ExtraInfo"]),

                                    Currency = Converter.ToString2(r["Currency"])
                                },
                                ChoiceHtml = string.Empty,
                                OddHtml = string.Empty,
                                StakeHtml = string.Empty,
                                IsShowOtExRate = Converter.To<bool>(r["isShowOTExRate"]),
                                VirtualRate = Converter.ToDecimal(r["virtualrate"]),
                                Creator = Converter.ToString2(r["creator"])
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
                            });
                        }
                    }
                }

                GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;
                BetListExcelFactory factory = new BetListExcelFactory();
                BetListExcelBuilder builder = factory.CreateBuilder();
                IList<ITicket> tickets = new List<ITicket>();
                if (betDetails != null && betDetails.Count > 0)
                {
                    string reportTitle = "Daily Statement - " + fromDate.ToShortDateString() + " - " + betDetails[0].Ticket.UserName;
                    fileName = "DailyStatement-" + fromDate.ToShortDateString() + "-" + betDetails[0].Ticket.UserName + ".xls";
                    int index = 0;
                    ticketHelper.HideStatusCashOutHistory = true;

                    builder.BuildWorksheet("BetList", 8);
                    builder.AddCaption(reportTitle, 8);
                    builder.AddHeaderStatement(true, false, false, false, true);
                    foreach (var betdetail in betDetails)
                    {
                        index++;
                        ticketHelper.Index = index;
                        builder.AddNo(index);

                        builder.AddTrans(betdetail.Ticket, ticketHelper);

                        builder.AddChoice(betdetail.Ticket, ticketHelper, outputLiveCasino)
                               .AddOdds(betdetail.Ticket, outputLiveCasino, funcGetOddsTypeName)
                               .AddStake(betdetail.Ticket);

                        builder.AddWinLoss(betdetail.Ticket);

                        builder.AddStatusStatement(betdetail.Ticket, ticketHelper, outputLiveCasino);

                        builder.AddCreator(betdetail, ticketHelper);
                        tickets.Add(betdetail.Ticket);
                    }

                    if (tickets.Count > 0)
                    {
                        builder.AddFooterHeader(isShowMember: false);
                        builder.AddFooter(new Footer(tickets), false, 8);
                    }

                    MemoryStream ms = new MemoryStream();
                    IWorkbook resultWorkbook = builder.GetBetList();
                    resultWorkbook.Write(ms);
                    return ms;
                }
            }
            catch (Exception ex)
            {
                AccBetListCore.LoggingHandler.Log(ex);
            }

            return null;
        }
    }
}
