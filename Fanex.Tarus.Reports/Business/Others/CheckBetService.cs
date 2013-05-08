// -----------------------------------------------------------------------
// <copyright file="CheckBetService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Others
{
    using DNAide.Core;
    using DNAide.Utils;
    using Fanex.Tarus.Infrastructure.Helper.SportHelper;
    using Fanex.Tarus.Reports.DataAccess.Others;
    using Fanex.Tarus.Reports.Entities.Others;
    using Sunplus.Accounting.Core.Cache;
    using Sunplus.Accounting.Core.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Data;
   

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckBetService: ICheckBetService
    {
        public readonly ICheckBetAccess _checkBetAccess;

        public CheckBetService()
            : this(null)
        {
        }

        public CheckBetService(ICheckBetAccess checkBetAccess) 
        {
            this._checkBetAccess = checkBetAccess ?? new CheckBetAccess();
        }

        public CheckBet GetCheckBet(string date)
        {
            var checkbet = new CheckBet();
            try
            {
                var datatable = _checkBetAccess.GetDataCheckBet(date, "CheckBet");
                foreach (DataRow row in datatable.Rows) {
                    checkbet.NormalTickets = CastAide.AsInt32(row["checknormalbet"]);
                    checkbet.RejectTickets = CastAide.AsInt32(row["checkrejectbet"]);
                }
                    
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return checkbet;
        }


        public ChangedTickets GetCheckNormalBet(string date)
        {
            var checkNormalBet = new ChangedTickets();
            var listTicket = new List<ChangedTicketDetail>();
            try
            {
                DataTable dtb = _checkBetAccess.GetDataCheckBet(date, "CheckBetNormal");
                foreach (DataRow row in dtb.Rows)
                {
                    var transid = row["transid"].ToString();
                    var transdate = row["transdate"].ToString();
                    var currency = row["currencyname"].ToString();
                    var bettype = CastAide.AsInt32(row["bettype"]);
                    var creator = row["creator"].ToString();
                    var username = row["username"].ToString();
                    var oldStake = NumberFormat.DecFormat(row["oldstake"], 2);
                    var newStake = NumberFormat.DecFormat(row["newstake"].ToString(), 2);
                    var oldOdds = NumberFormat.DecFormat(row["oldodds"].ToString(), 2);
                    var newOdds = NumberFormat.DecFormat(row["newodds"].ToString(), 2);
                    var oldBetteam = BetTeam.GetBetTeam(row["oldbetteam"].ToString(), bettype, "e");
                    var newBetteam = BetTeam.GetBetTeam(row["newbetteam"].ToString(), bettype, "e"); 
                    var oldTotalPT = NumberFormat.DecFormat(row["oldpt"].ToString(), 2);
                    var newTotalPT = NumberFormat.DecFormat(row["newpt"].ToString(), 2);
                    var oddstype = CalcOdds.GetOddsType(bettype, CastAide.AsInt32(row["oddstype"]));
                    var oldStatus = row["status"].ToString();
                    var home = CacheRef.GetTeamName(row["homeid"]);
                    
                    var newmatchcode = row["newmatchcode"].ToString();
                    var oldmatchcode = row["oldmatchcode"].ToString();
                    var hdp1 = row["hdp1"].ToString();
                    var oldaway = "";
                    var newaway = "";
                    string betypeName = CacheRef.GetBettype(bettype);
                    if (bettype > 1200 && bettype < 1299)
                    {
                        betypeName  += " (Virtual Sports)";
                    }
                    int liveindicator = CastAide.AsInt32(row["liveindicator"]);
                    ///if (bettype == 1201 || bettype == 1203 || bettype == 1204 || bettype == 1205 || bettype == 1206 || bettype == 1224)
                    ///    BetypeName += " (Virtual Soccer)";
                    if (bettype == 85 || bettype == 86)
                        betypeName = liveindicator == 1 ? "Next " + betypeName : "FT." + betypeName;
                    if (bettype == 31 || bettype == 32 || bettype == 33 || bettype == 41 || bettype == 42 || bettype == 43 || bettype == 1231 || bettype == 1232 || bettype == 1233 || bettype == 1237 || bettype == 1238)
                    {
                        newaway = newmatchcode + " - " + CacheRef.GetHorseTeamName(row["newawayid"]);
                        oldaway = oldmatchcode + " - " + CacheRef.GetHorseTeamName(row["oldawayid"]);
                       
                    }
                    else
                    {
                        newaway = CacheRef.GetTeamName(row["newawayid"]);
                        oldaway = CacheRef.GetTeamName(row["oldawayid"]);
                    }
                    if (bettype == 10)
                    {
                        newBetteam = !string.IsNullOrEmpty(newaway) ? newaway : home;
                        oldBetteam = !string.IsNullOrEmpty(oldaway) ? oldaway : home;
                    }
                    if (oldStake != newStake)
                        checkNormalBet.TotalChangedStake++;
                    if (oldOdds != newOdds)
                        checkNormalBet.TotalChangedOdds++;
                    if (oldBetteam != newBetteam)
                        checkNormalBet.TotalChangedBetteam++;
                    if (oldTotalPT != newTotalPT)
                        checkNormalBet.TotalChangedPT++;
                    
                    var normalTicket = new ChangedTicketDetail()
                    {
                        RefNo = transid,
                        TransDate = transdate,
                        CurrencyName = currency,
                        Bettype = betypeName,
                        Creator = creator,
                        OldStake = oldStake,
                        NewStake = newStake,
                        OldOdds = CalcOdds.GetOdds(CastAide.AsDecimal(oldOdds), bettype, oddstype).ToString(),
                        NewOdds = CalcOdds.GetOdds(CastAide.AsDecimal(newOdds), bettype, oddstype).ToString(),
                        OldBetteam = GetBetteam(home, oldaway, bettype, oldBetteam, hdp1),
                        NewBetteam = GetBetteam(home, newaway, bettype, newBetteam, hdp1),
                        OldTotalPT = oldTotalPT,
                        NewTotalPT = newTotalPT,
                        OldStatus = oldStatus,
                        Username = username
                    };
                    listTicket.Add(normalTicket);
                }

                checkNormalBet.listTicket = listTicket;
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return checkNormalBet;
        }

        public ChangedTickets GetCheckRejectBet(string date)
        {
            var checkRejectBet = new ChangedTickets();
            var listTicket = new List<ChangedTicketDetail>();

            try
            {
                DataTable dtb = _checkBetAccess.GetDataCheckBet(date, "CheckBetReject");
                foreach (DataRow row in dtb.Rows)
                {
                    var transid = row["transid"].ToString();
                    var transdate = row["transdate"].ToString();
                    var currency = row["currencyname"].ToString();
                    var bettype = CastAide.AsInt32(row["bettype"]);
                    var creator = row["creator"].ToString();
                    var username = row["username"].ToString();
                    var oldStake = NumberFormat.DecFormat(row["stake"], 2);
                    var oldOdds = NumberFormat.DecFormat(row["odds"].ToString(), 2);
                    var oldBetteam = BetTeam.GetBetTeam(row["betteam"].ToString(), bettype, "e"); 
                    var oldTotalPT = NumberFormat.DecFormat(row["tpositiontaking"].ToString(), 2);
                    var oldStatus = row["oldstatus"].ToString();
                    var newStatus = row["newstatus"].ToString();
                    var oddstype = CalcOdds.GetOddsType(bettype, CastAide.AsInt32(row["oddstype"]));
                    var home = CacheRef.GetTeamName(row["homeid"]);
                    var away = CacheRef.GetTeamName(row["awayid"]);
                    var matchcode = row["matchcode"].ToString();
                    var hdp1 = row["hdp1"].ToString();
                    int liveindicator = CastAide.AsInt32(row["liveindicator"]);
                    string BetypeName = CacheRef.GetBettype(bettype);
                    if (bettype > 1200 && bettype < 1299)
                    {
                        BetypeName += " (Virtual Sports)";
                    }
                    ///if (bettype == 1201 || bettype == 1203 || bettype == 1204 || bettype == 1205 || bettype == 1206 || bettype == 1224)
                    ///    BetypeName += " (Virtual Soccer)";
                    if (bettype == 85 || bettype == 86)
                        BetypeName = liveindicator == 1 ? "Next " + BetypeName : "FT." + BetypeName;
                    if (bettype == 31 || bettype == 32 || bettype == 33 || bettype == 41 || bettype == 42 || bettype == 43 || bettype == 1231 || bettype == 1232 || bettype == 1233 || bettype == 1237 || bettype == 1238)
                    {
                        away = row["matchcode"] + " - " + CacheRef.GetHorseTeamName(row["awayid"]);
                    }

                    if (bettype == 10)
                    {
                        oldBetteam = !string.IsNullOrEmpty(away) ? away : home;
                    }
                    var rejectTicket = new ChangedTicketDetail()
                    {
                        RefNo = transid,
                        TransDate = transdate,
                        CurrencyName = currency,
                        Bettype = BetypeName,
                        Username = username,
                        Creator = creator,
                        OldStake = oldStake,
                        OldOdds = Fanex.Tarus.Infrastructure.Helper.SportHelper.CalcOdds.GetOdds(CastAide.AsDecimal(oldOdds), bettype, oddstype).ToString(),
                        OldBetteam = GetBetteam(home, away, bettype, oldBetteam, hdp1),
                        OldTotalPT = oldTotalPT,
                        OldStatus = oldStatus,
                        NewStatus = newStatus
                    };
                    listTicket.Add(rejectTicket);
                    if (oldStatus != newStatus)
                        checkRejectBet.TotalChangedStatus++;

                }
                checkRejectBet.listTicket = listTicket;
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return checkRejectBet;
        }

        public string GetBetteam(string home, string away,  int bettype, string betteam, string hdp1)
        {
            string result = "";
            switch (bettype)
            { 
                case 1:
                case 7:
                case 124:
                case 125:
                case 20:
                case 22:
                case 21:
                case 1201:
                    result = betteam == "h" ? home : away;
                    break;
                case 2:
                case 12:
                    result = betteam == "h" ? "ODD" : "EVEN";
                    break;
                case 3:
                case 8:
                case 1203:
                    result = betteam == "h" ? "OVER" : "UNDER";
                    break;
                case 4:
                case 30:
                case 1204:
                    result = (betteam == "5:0" || betteam == "0:5") ? betteam + " UP" : betteam;
                    if(bettype == 30)
                        result = (betteam == "4:0" || betteam == "0:4") ? betteam + " UP" : betteam;
                    break;
                case 5:
                case 1205:
                    result = "FT." + betteam;
                    break;
                case 15:
                    result = "HT." + betteam;
                    break;
                case 6:
                case 14:
                case 16:
                case 1206:
                    result = betteam;
                    break;
                case 10:
                    result = betteam;
                   
                    break;
                case 121:
                    result = betteam == "h" ? "Draw" : away;
                    break;
                case 122:
                    result = betteam == "h" ? home : "Draw";
                    break;
                case 123:
                    result = betteam == "h" ? "Draw" : "No Draw";
                    break;
                case 31:
                case 32:
                case 33:
                case 41:
                case 42:
                case 43:
                case 1231: // Virtual Horse racing
                case 1232:
                case 1233:
                    result = away;
                    break;
                    // Virtual Tennis
                case 1220:
                case 1235:
                case 1236:
                    result = betteam == "h" ? home : away;
                    break;
                case 81://Number Game
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                    if (bettype == 81 || bettype == 82 || bettype == 85)
                    {
                        result = betteam.ToLower() == "h" ? " OVER " : " UNDER ";
                    }
                    else if (bettype == 83 || bettype == 84 || bettype == 86)
                    {
                        result = betteam.ToLower() == "h" ? "ODD " : "EVEN ";
                    }
                    else if (bettype == 87)//High-Low
                    {
                        result = betteam.ToLower() == "h" ? "HIGH " : "LOW ";
                    }
                    else if (bettype == 88)//warrior        
                    {
                        result = betteam.ToLower() == "h" ? "2nd ball to win " : "3rd ball to win ";
                    }
                    else if (bettype == 89)//next combo
                    {
                        switch (betteam.ToLower())
                        {
                            case "1:1":
                                result = "Over/Odd ";
                                break;
                            case "1:2":
                                result = "Over/Even ";
                                break;
                            case "2:1":
                                result = "Under/Odd ";
                                break;
                            case "2:2":
                                result = "Under/Even ";
                                break;
                            default:
                                result = "Over/Odd ";
                                break;
                        }
                    }
                    else if (bettype == 90)
                    {
                        result = "Group " + NumberFormat.DecFormat(hdp1, 0) + "-" + betteam + " ";
                    }

                    break;
                case 24:
                case 1224:
                    if (betteam.ToLower().Equals("1x"))
                        result = home + " or Draw";
                    else if (betteam.ToLower().Equals("2x"))
                        result = away + " or Draw";
                    else if (betteam.ToLower().Equals("12"))
                        result = home + " or " + away;
                    break;
                case 25:
                    result = betteam == "h" ? home : away;
                    break;
                case 26 :
                    if (betteam.ToLower().Equals("b"))
                        result = "Both";
                    else if (betteam.ToLower().Equals("o"))
                        result = "One";
                    else if (betteam.ToLower().Equals("n"))
                        result = "No Goal";
                    break;
                case 27:
                    if (betteam.ToLower().Equals("h"))
                        result = home;
                    else if (betteam.ToLower().Equals("a"))
                        result = away;
                    break;
                case 13:
                    if (betteam.ToLower().Equals("hy"))
                        result = home + " YES";
                    else if (betteam.ToLower().Equals("hn"))
                        result = home + " NO";
                    else if (betteam.ToLower().Equals("ay"))
                        result = away + " YES";
                    else if (betteam.ToLower().Equals("an"))
                        result = away + " NO";
                    break;
                case 28:
                    if (betteam.ToLower().Equals("1"))
                        result = home;
                    else if (betteam.ToLower().Equals("2"))
                        result = away;
                    break;
                case 9:
                    result = "Mix Parlay";
                    break;
                case 29:
                    result = "System Parlay";
                    break;

            }
            return result;

        }
    }
}
