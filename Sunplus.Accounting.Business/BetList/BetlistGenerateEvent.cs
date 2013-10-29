using System;
using System.Data;
using DNAide.Core;
using DNAide.Utils;
using DNAide.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;

/// <summary>
/// Summary description for BetlistGenerateEvent
/// </summary>
namespace Sunplus.Accounting.Business.BetList
{
    public class BetlistGenerateEvent
    {
        #region oddMP

        private string oddMP(decimal odds)
        {
            return "<span class='bl_num'><b> @ " + NumberFormat.DecFormat(odds, 3) + "</b></span>";
        }

        #endregion oddMP

        private string normalOdd(decimal odds)
        {
            return " <font color='#606060'>" + NumberFormat.DecFormat(odds, 2) + "</font> ";
        }

        private BetlistRow InputMPBetList(DataRow dr)
        {
            var row = new BetlistRow();
            row.Sporttype = CastAide.AsInt32(dr["Sporttype"]);
            row.Bettype = CastAide.AsInt32(dr["Bettype"]);
            row.hdp1 = CastAide.AsDecimal(dr["hdp1"]);
            row.hdp2 = CastAide.AsDecimal(dr["hdp2"]);
            row.Odds = CastAide.AsDecimal(dr["Odds"]);
            row.Betteam = Fanex.Tarus.Infrastructure.Helper.SportHelper.BetTeam.GetBetTeam(dr["betteam"].ToString(), Convert.ToInt32(dr["bettype"]), "e");
            row.Liveindicator = CastAide.AsBoolean(dr["Liveindicator"]);
            row.Livehomescore = CastAide.AsInt32(dr["Livehomescore"]);
            row.Liveawayscore = CastAide.AsInt32(dr["Liveawayscore"]);
            row.LeagueID = CastAide.AsInt32(dr["LeagueID"]);
            row.HomeID = CastAide.AsInt32(dr["HomeID"]);
            row.AwayID = CastAide.AsInt32(dr["AwayID"]);
            row.isNeutral = CastAide.AsBoolean(dr["isNeutral"]);
            row.StatusId = CastAide.AsInt32(dr["statusid"]);

            //row.BetID = CastAide.AsInt32(dr["betid"]);
            return row;
        }

        #region GenHdpEvent

        private void GenHdpEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            bool liveStyle = bl.Liveindicator;
            string liveHomeScore = bl.Livehomescore.ToString();
            string liveAwayScore = bl.Liveawayscore.ToString();
            string home = CacheRef.GetTeamName(bl.HomeID) + ((bl.isNeutral) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
            string away = CacheRef.GetTeamName(bl.AwayID);
            int BetType = bl.Bettype;
            string betteam = bl.Betteam;
            decimal hdp1 = bl.hdp1;
            decimal hdp2 = bl.hdp2;
            string liveScore = (!liveStyle)
                                   ? string.Empty
                                   : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            string choice = "";

            if (BetType == 20) //Money line
            {
                if (betteam == "h") choice = home;
                else choice = away;
            }
            else
            {
                if (betteam == "h") choice = home + " " + IBCUtils.FormatHandler.FormatNumber20(hdp2 - hdp1);
                else choice = away + " " + IBCUtils.FormatHandler.FormatNumber20(hdp1 - hdp2);
            }

            decimal odds = bl.Odds;
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);

            if (isMP) choice += oddMP(odds);

            choice += liveScore;
            var Event = home + " -VS- " + away;

            if (bl.StatusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
                Event = string.Format("<s>{0}</s>", Event);
            }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenHdpEvent

        #region GenOEEvent

        private void GenOEEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            string home = CacheRef.GetTeamName(bl.HomeID) + ((bl.isNeutral) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
            string away = CacheRef.GetTeamName(bl.AwayID);
            string betteam = bl.Betteam;
            decimal odds = bl.Odds;
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);

            string choice = "";
            if (betteam == "h") choice = "Odds";
            else choice = "Even";
            if (isMP) choice += oddMP(odds);

            var Event = home + " -VS- " + away;
            if (bl.StatusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
                Event = string.Format("<s>{0}</s>", Event);
            }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenOEEvent

        #region GenOUEvent

        private void GenOUEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            bool liveStyle = bl.Liveindicator;
            string betteam = bl.Betteam;
            string liveHomeScore = bl.Livehomescore.ToString();
            string liveAwayScore = bl.Liveawayscore.ToString();
            string liveScore = (!liveStyle)
                                   ? string.Empty
                                   : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            decimal hdp1 = bl.hdp1;
            string home = CacheRef.GetTeamName(bl.HomeID) + ((bl.isNeutral) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
            string away = CacheRef.GetTeamName(bl.AwayID);
            decimal odds = bl.Odds;
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);

            string choice = "";
            if (betteam == "h") choice = "OVER " + NumberFormat.DecFormat(Math.Abs(hdp1), 2) + liveScore;
            else choice = "UNDER " + NumberFormat.DecFormat(Math.Abs(hdp1), 2) + liveScore;
            if (isMP) choice += oddMP(odds);
            var Event = home + " -VS- " + away;

            if (bl.StatusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
                Event = string.Format("<s>{0}</s>", Event);
            }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenOUEvent

        #region GenCSEvent

        private void GenCSEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            string home = CacheRef.GetTeamName(bl.HomeID) + ((bl.isNeutral) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
            string away = CacheRef.GetTeamName(bl.AwayID);
            string betteam = bl.Betteam;
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);

            decimal odds = bl.Odds;

            string choice = "";
            choice = (betteam == "5:0" || betteam == "0:5") ? betteam + " UP" : betteam;
            if (bl.Bettype == 30)
            {
                choice = (betteam == "4:0" || betteam == "0:4") ? betteam + " UP" : betteam;
            }
            if (isMP) choice += oddMP(odds);

            var Event = home + " -VS- " + away;
            if (bl.StatusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
                Event = string.Format("<s>{0}</s>", Event);
            }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenCSEvent

        #region Gen1x2Event

        private void Gen1x2Event(ref string eventItem, BetlistRow bl, bool isMP)
        {
            bool liveStyle = bl.Liveindicator;
            string liveHomeScore = bl.Livehomescore.ToString();
            string liveAwayScore = bl.Liveawayscore.ToString();
            string liveScore = (!liveStyle)
                                   ? string.Empty
                                   : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            string home = CacheRef.GetTeamName(bl.HomeID) + ((bl.isNeutral) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
            string away = CacheRef.GetTeamName(bl.AwayID);
            decimal odds = bl.Odds;
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);

            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            string choice = "";
            if (bl.Bettype == 15)
                choice = "HT." + bl.Betteam;
            else
                choice = "FT." + bl.Betteam;

            if (isMP) choice += oddMP(odds);

            var Event = home + " -VS- " + away;
            if (bl.StatusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
                Event = string.Format("<s>{0}</s>", Event);
            }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice + liveScore);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion Gen1x2Event

        #region GenTGEvent

        private void GenTGEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            string home = CacheRef.GetTeamName(bl.HomeID) + ((bl.isNeutral) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
            string away = CacheRef.GetTeamName(bl.AwayID);
            decimal odds = bl.Odds;
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);

            string choice = bl.Betteam;
            if (isMP) choice += oddMP(odds);

            var Event = home + " -VS- " + away;
            if (bl.StatusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                sportypeName = string.Format("<s>{0}</s>", sportypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
                Event = string.Format("<s>{0}</s>", Event);
            }

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenTGEvent

        #region GenOutrightRow

        private void GenOutrightRow(ref string eventItem, BetlistRow bl)
        {
            string home = CacheRef.GetTeamName(bl.HomeID);
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);

            string choice = home;

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);

            TemplateHandler.RemoveBlock(ref eventItem, "event");
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenOutrightRow

        #region GenHorseracingRow

        private void GenHorseracingRow(ref string eventItem, BetlistRow bl)
        {
            string home = bl.MatchCode + " - " + CacheRef.GetHorseTeamName(bl.HomeID);
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            string race = bl.Race;

            string choice = home;
            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", "Race " + race);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenHorseracingRow

        #region GenFinancialRow

        private void GenFinancialRow(ref string eventItem, BetlistRow bl)
        {
            decimal hdp1 = bl.hdp1;
            string betteam = bl.Betteam;
            int bettype = bl.Bettype;

            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            string indexName = CacheRef.GetFncIndexName(bl.HomeID);

            string choice = GetFIBettypeName(betteam, bl.Bettype) + " <font color='#606060'>" + DNAide.Utils.FormatHandler.FormatNumber2C(hdp1, "E") + "</font>";

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", indexName);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        private bool IsFIOU(int bettype)
        {
            switch (bettype)
            {
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                    return true;
                default:
                    return false;
            }
        }

        private bool IsFIOE(int bettype)
        {
            switch (bettype)
            {
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                    return true;
                default:
                    return false;
            }
        }

        private string GetFIBettypeName(string betteam, int bettype)
        {
            if (IsFIOU(bettype))
            {
                return betteam == "a" ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblUnder") : ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOver");
            }
            else if (IsFIOE(bettype))
            {
                return betteam == "a" ? "EVEN" : "ODD";
            }
            return "";
        }

        #endregion GenFinancialRow

        #region GenNextGoalEvent

        private void GenNextGoalEvent(ref string eventItem, BetlistRow bl)
        {
            bool liveStyle = bl.Liveindicator;
            string liveHomeScore = bl.Livehomescore.ToString();
            string liveAwayScore = bl.Liveawayscore.ToString();

            string liveScore = (!liveStyle) ? string.Empty : " [" + liveHomeScore + "-" + liveAwayScore + "]";

            decimal hdp1 = bl.hdp1;
            decimal hdp2 = bl.hdp2;
            string betteam = bl.Betteam; ;
            string home = CacheRef.GetTeamName(bl.HomeID);
            string away = CacheRef.GetTeamName(bl.AwayID);
            int BetType = bl.Bettype;
            decimal odds = bl.Odds;
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string betstatus = bl.Betstatus;
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            bool isNeutral = bl.isNeutral;

            string choice = "";
            if (hdp1 > hdp2)
            {
                if (betteam == "h")
                {
                    choice = "<span class='bl_favorite'>" + home + " " + " <font color='#606060'>" + ((BetType != 20) ? "-" + NumberFormat.DecFormat(hdp1, 2) : "") + "</font> " + liveScore + "</span>";
                }
                else
                {
                    choice = "<span class='bl_underdog'>" + away + " " + " <font color='#606060'>" + ((BetType != 20) ? NumberFormat.DecFormat(hdp1, 2) : "") + "</font> " + liveScore + "</span>";
                }
            }
            else if (hdp1 < hdp2)
            {
                if (betteam == "h")
                {
                    choice = "<span class='bl_underdog'>" + home + " " + " <font color='#606060'>" + ((BetType != 20) ? NumberFormat.DecFormat(hdp2, 2) : "") + "</font> " + liveScore + "</span>";
                }
                else
                {
                    choice = "<span class='bl_favorite'>" + away + " " + " <font color='#606060'>" + ((BetType != 20) ? "-" + NumberFormat.DecFormat(hdp2, 2) : "") + "</font> " + liveScore + "</span>";
                }
            }
            else
            {
                if (betteam == "h")
                {
                    choice = "<span class='bl_underdog'>" + home + " " + liveScore + "</span>";
                }
                else
                {
                    choice = "<span class='bl_underdog'>" + away + " " + liveScore + "</span>";
                }
            }

            TemplateHandler.ReplaceBlock(ref eventItem, "limit-choice", choice);

            TemplateHandler.Assign(ref eventItem, "{bettype}", "NG");
            TemplateHandler.Assign(ref eventItem, "{event}", FirstGoal_LastGoal(home, away, betstatus, isNeutral));
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenNextGoalEvent

        #region GenMixParlay

        public string GenMixParlay(string refno)
        {
            string tplEventItem = WebUtils.LoadTemplate("BetList\\BetlistEvent1.htm");

            DataTable dtMP = new DataTable();

            var rs = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Betlist.BetList_SelMixParlay(refno);

            if (rs != null && rs.Tables.Count > 0)
                dtMP = rs.Tables[0];

            DataRowCollection rows = dtMP.Rows;
            string eventItem = "";
            string e = "";
            foreach (DataRow drMP in rows)
            {
                //input betlistrow
                BetlistRow blMP = InputMPBetList(drMP);
                e = tplEventItem;
                switch (blMP.Bettype)
                {
                    case 1:
                    case 7:
                    case 20:
                    case 21:
                    case 1201:
                        GenHdpEvent(ref e, blMP, true);
                        break;

                    case 12:
                    case 2:
                        GenOEEvent(ref e, blMP, true);
                        break;

                    case 3:
                    case 8:
                    case 1203:
                        GenOUEvent(ref e, blMP, true);
                        break;

                    case 4:
                    case 30:
                    case 14:
                    case 1204:
                        GenCSEvent(ref e, blMP, true);
                        break;

                    case 5:
                    case 15:
                    case 1205:
                        Gen1x2Event(ref e, blMP, true);
                        break;

                    case 6:
                    case 16:
                    case 1206:
                        GenTGEvent(ref e, blMP, true);
                        break;

                    case 13:
                        GenCleanSheetEvent(ref e, blMP, true);
                        break;

                    case 24:
                    case 1224:
                        GenDoubleChanceEvent(ref e, blMP, true);
                        break;

                    case 25:
                        GenDrawNoBetEvent(ref e, blMP, true);
                        break;

                    case 26:
                        GenBothOneNeitherEvent(ref e, blMP, true);
                        break;

                    case 27:
                        GenToWinToNilEvent(ref e, blMP, true);
                        break;

                    case 28:
                        GenThreeWayHandicap(ref e, blMP, true);
                        break;

                    case 121:
                    case 122:
                    case 123:
                        GenHomeAwayDrawNoBet(ref e, blMP, true);
                        break;
                }
                if (CastAide.AsInt32(drMP["liveindicator"]) == 1)
                {
                    TemplateHandler.Assign(ref e, "{liveball}", " style='background-color:#FFFFCC'");
                }
                else
                {
                    TemplateHandler.Assign(ref e, "{liveball}", "");
                }
                eventItem += e;
            }
            return eventItem;
        }

        public string GenMixParlay(DataTable dtMP)
        {
            string tplEventItem = WebUtils.LoadTemplate("BetList\\BetlistEvent1.htm");

            DataRowCollection rows = dtMP.Rows;
            string eventItem = "";
            string e = "";
            foreach (DataRow drMP in rows)
            {
                //input betlistrow
                BetlistRow blMP = InputMPBetList(drMP);
                e = tplEventItem;
                switch (blMP.Bettype)
                {
                    case 1:
                    case 7:
                    case 20:
                    case 21:
                    case 1201:
                        GenHdpEvent(ref e, blMP, true);

                        break;

                    case 12:
                    case 2:
                        GenOEEvent(ref e, blMP, true);
                        break;

                    case 3:
                    case 8:
                    case 1203:
                        GenOUEvent(ref e, blMP, true);
                        break;

                    case 4:
                    case 30:
                    case 14:
                    case 1204:
                        GenCSEvent(ref e, blMP, true);
                        break;

                    case 5:
                    case 15:
                    case 1205:
                        Gen1x2Event(ref e, blMP, true);
                        break;

                    case 6:
                    case 16:
                    case 1206:
                        GenTGEvent(ref e, blMP, true);
                        break;

                    case 13:
                        GenCleanSheetEvent(ref e, blMP, true);
                        break;

                    case 24:
                    case 1224:
                        GenDoubleChanceEvent(ref e, blMP, true);
                        break;

                    case 25:
                        GenDrawNoBetEvent(ref e, blMP, true);
                        break;

                    case 26:
                        GenBothOneNeitherEvent(ref e, blMP, true);
                        break;

                    case 27:
                        GenToWinToNilEvent(ref e, blMP, true);
                        break;

                    case 28:
                        GenThreeWayHandicap(ref e, blMP, true);
                        break;

                    case 121:
                    case 122:
                    case 123:
                        GenHomeAwayDrawNoBet(ref e, blMP, true);
                        break;
                }
                if (CastAide.AsInt32(drMP["liveindicator"]) == 1)
                {
                    TemplateHandler.Assign(ref e, "{liveball}", " style='background-color:#FFFFCC'");
                }
                else
                {
                    TemplateHandler.Assign(ref e, "{liveball}", "");
                }
                eventItem += e;
            }
            return eventItem;
        }

        #region GenMixParlay_WithHorizontalLine

        public string GenMixParlay_WithHorizontalLine(DataTable dtMP)
        {
            string tplEventItem = WebUtils.LoadTemplate("BetList\\BetlistEvent1.htm");

            DataRowCollection rows = dtMP.Rows;
            string eventItem = "";
            string e = "";
            foreach (DataRow drMP in rows)
            {
                //input betlistrow
                BetlistRow blMP = InputMPBetList(drMP);
                if (String.Compare(e, "") == 0)
                    e = tplEventItem; //the first row has no <HR> line
                else
                    e = "<HR>" + tplEventItem;
                switch (blMP.Bettype)
                {
                    case 1:
                    case 7:
                    case 20:
                    case 21:
                    case 1201:
                        GenHdpEvent(ref e, blMP, true);

                        break;

                    case 12:
                    case 2:
                        GenOEEvent(ref e, blMP, true);
                        break;

                    case 3:
                    case 8:
                    case 1203:
                        GenOUEvent(ref e, blMP, true);
                        break;

                    case 4:
                    case 30:
                    case 14:
                    case 1204:
                        GenCSEvent(ref e, blMP, true);
                        break;

                    case 5:
                    case 15:
                    case 1205:
                        Gen1x2Event(ref e, blMP, true);
                        break;

                    case 121:
                    case 122:
                    case 123:
                        GenHomeAwayDrawNoBet(ref e, blMP, true);
                        break;

                    case 6:
                    case 16:
                    case 1206:
                        GenTGEvent(ref e, blMP, true);
                        break;

                    case 13:
                        GenCleanSheetEvent(ref e, blMP, true);
                        break;

                    case 24:
                    case 1224:
                        GenDoubleChanceEvent(ref e, blMP, true);
                        break;

                    case 25:
                        GenDrawNoBetEvent(ref e, blMP, true);
                        break;

                    case 26:
                        GenBothOneNeitherEvent(ref e, blMP, true);
                        break;

                    case 27:
                        GenToWinToNilEvent(ref e, blMP, true);
                        break;

                    case 28:
                        GenThreeWayHandicap(ref e, blMP, true);
                        break;
                }
                if (CastAide.AsInt32(drMP["liveindicator"]) == 1)
                {
                    TemplateHandler.Assign(ref e, "{liveball}", " style='background-color:#FFFFCC'");
                }
                else
                {
                    TemplateHandler.Assign(ref e, "{liveball}", "");
                }
                eventItem += e;
            }
            return eventItem;
        }

        #endregion GenMixParlay_WithHorizontalLine

        #endregion GenMixParlay

        #region FirstGoal_LastGoal

        private string FirstGoal_LastGoal(string home, string away, string betstatus, bool isNeutral)
        {
            string fglg = WebUtils.LoadTemplate("BetList/FirstGoal_LastGoal.html");
            string eventstatus = betstatus.Trim();
            //string homeaway = "";
            if (eventstatus == "Refund")
            {
                TemplateHandler.RemoveBlock(ref fglg, "limit-FG-away");
                TemplateHandler.RemoveBlock(ref fglg, "limit-LG-away");
                TemplateHandler.RemoveBlock(ref fglg, "limit-FG-home");
                TemplateHandler.RemoveBlock(ref fglg, "limit-LG-home");
            }
            else if (eventstatus == "Refund FT")
            {
                string _betStatus = betstatus.Trim();
                string[] refundArr = _betStatus.Split(';');
                if (refundArr.Length == 2 && refundArr[1].ToString() != "")
                {
                    if (!string.IsNullOrEmpty(refundArr[1].ToString()) && refundArr[1].ToString().Contains(":"))
                    {
                        string[] bstas = refundArr[1].ToString().Split(':');
                        if (bstas.Length > 0 && string.Compare(bstas[0], "FL", true) == 0)
                        {
                            string _firstGoal = bstas[1].Split('-')[0];
                            string _lastGoal = bstas[1].Split('-')[1];
                            if (_firstGoal == "1" && _lastGoal == "1")
                            {
                                TemplateHandler.Assign(ref fglg, "clssFGHome", "FirstGoal");
                                TemplateHandler.Assign(ref fglg, "clssLGHome", "LastGoal");

                                TemplateHandler.RemoveBlock(ref fglg, "limit-FG-away");
                                TemplateHandler.RemoveBlock(ref fglg, "limit-LG-away");
                            }
                            else if (_firstGoal == "2" && _lastGoal == "2")
                            {
                                TemplateHandler.Assign(ref fglg, "clssFGAway", "FirstGoal");
                                TemplateHandler.Assign(ref fglg, "clssLGAway", "LastGoal");

                                TemplateHandler.RemoveBlock(ref fglg, "limit-FG-home");
                                TemplateHandler.RemoveBlock(ref fglg, "limit-LG-home");
                            }
                            else if (_firstGoal == "1" && _lastGoal == "2")
                            {
                                TemplateHandler.Assign(ref fglg, "clssFGHome", "FirstGoal");
                                TemplateHandler.RemoveBlock(ref fglg, "limit-LG-home");

                                TemplateHandler.Assign(ref fglg, "clssLGAway", "LastGoal");
                                TemplateHandler.RemoveBlock(ref fglg, "limit-FG-away");
                            }
                            else if (_firstGoal == "2" && _lastGoal == "1")
                            {
                                TemplateHandler.Assign(ref fglg, "clssLGHome", "LastGoal");
                                TemplateHandler.RemoveBlock(ref fglg, "limit-FG-home");

                                TemplateHandler.Assign(ref fglg, "clssFGAway", "FirstGoal");
                                TemplateHandler.RemoveBlock(ref fglg, "limit-LG-away");
                            }
                            else
                            {
                                TemplateHandler.RemoveBlock(ref fglg, "limit-FG-away");
                                TemplateHandler.RemoveBlock(ref fglg, "limit-LG-away");
                                TemplateHandler.RemoveBlock(ref fglg, "limit-FG-home");
                                TemplateHandler.RemoveBlock(ref fglg, "limit-LG-home");
                            }
                        }
                    }
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref fglg, "limit-FG-away");
                    TemplateHandler.RemoveBlock(ref fglg, "limit-LG-away");
                    TemplateHandler.RemoveBlock(ref fglg, "limit-FG-home");
                    TemplateHandler.RemoveBlock(ref fglg, "limit-LG-home");
                }
            }
            else
            {
                string _betStatus = betstatus.Trim();
                if (!string.IsNullOrEmpty(_betStatus) && _betStatus.Contains(":"))
                {
                    string[] bstas = _betStatus.Split(':');
                    if (bstas.Length > 0 && string.Compare(bstas[0], "FL", true) == 0)
                    {
                        string _firstGoal = bstas[1].Split('-')[0];
                        string _lastGoal = bstas[1].Split('-')[1];
                        if (_firstGoal == "1" && _lastGoal == "1")
                        {
                            TemplateHandler.Assign(ref fglg, "clssFGHome", "FirstGoal");
                            TemplateHandler.Assign(ref fglg, "clssLGHome", "LastGoal");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-FG-away");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-LG-away");
                        }
                        else if (_firstGoal == "2" && _lastGoal == "2")
                        {
                            TemplateHandler.Assign(ref fglg, "clssFGAway", "FirstGoal");
                            TemplateHandler.Assign(ref fglg, "clssLGAway", "LastGoal");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-FG-home");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-LG-home");
                        }
                        else if (_firstGoal == "1" && _lastGoal == "2")
                        {
                            TemplateHandler.Assign(ref fglg, "clssFGHome", "FirstGoal");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-LG-home");
                            TemplateHandler.Assign(ref fglg, "clssLGAway", "LastGoal");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-FG-away");
                        }
                        else if (_firstGoal == "2" && _lastGoal == "1")
                        {
                            TemplateHandler.Assign(ref fglg, "clssLGHome", "LastGoal");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-FG-home");

                            TemplateHandler.Assign(ref fglg, "clssFGAway", "FirstGoal");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-LG-away");
                        }
                        else
                        {
                            TemplateHandler.RemoveBlock(ref fglg, "limit-FG-away");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-LG-away");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-FG-home");
                            TemplateHandler.RemoveBlock(ref fglg, "limit-LG-home");
                        }
                    }
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref fglg, "limit-FG-away");
                    TemplateHandler.RemoveBlock(ref fglg, "limit-LG-away");
                    TemplateHandler.RemoveBlock(ref fglg, "limit-FG-home");
                    TemplateHandler.RemoveBlock(ref fglg, "limit-LG-home");
                }
            }
            TemplateHandler.Assign(ref fglg, "home", home + (isNeutral ? "(N)" : string.Empty));
            TemplateHandler.Assign(ref fglg, "away", away);
            return fglg.Replace("\r\n", "");
        }

        #endregion FirstGoal_LastGoal

        #region GenCasino

        private void GenCasino(ref string eventItem, int betType)
        {
            string choice = "";
            if (betType == 71 || betType == 113)
                choice = "Casino Games";
            else if (betType == 72 || betType == 114)
                choice = "Poker Games";
            else if (betType == 112)
                choice = "Casino Games" + " - " + "Transfer";

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", "");
            TemplateHandler.Assign(ref eventItem, "{event}", "");
            TemplateHandler.Assign(ref eventItem, "{sporttype}", "");
            TemplateHandler.Assign(ref eventItem, "{leaguename}", "");
        }

        #endregion GenCasino

        #region GenNumberGame (bettype = 81-> 90)

        protected void GenEvent_NumberGame(ref string eventItem, BetlistRow bl, bool isMP)
        {
            #region private varialbes

            var choice = string.Empty;
            var bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            var bettype = bl.Bettype;
            var betteam = bl.Betteam;
            var liveScore = bl.Liveindicator ? string.Format(" [{0}]", bl.BetID) : string.Empty;

            #endregion private varialbes

            #region check bettype & apply betty name

            if (bettype == 81 || bettype == 82 || bettype == 85)
            {
                var maxBall = (double.Parse(Bingo.Core.BingoSettings.MaxBall.ToString()) / 2);

                choice = betteam.ToLower() == "h" ? "OVER " : "UNDER ";
                choice += string.Format("<span class='bl-bettype'>{0}</span>", maxBall);
                choice += liveScore;
            }
            else if (bettype == 83 || bettype == 84 || bettype == 86)
            {
                choice = betteam.ToLower() == "h" ? "ODD " : "EVEN ";
                choice += liveScore;
            }
            else if (bettype == 87)
            {
                choice = betteam.ToLower() == "h" ? "HIGH " : "LOW ";
                choice += liveScore;
            }
            else if (bettype == 88)//warrior
                choice = betteam.ToLower() == "h" ? "2nd ball to win " : "3rd ball to win ";
            else if (bettype == 89)//next combo
            {
                #region number group - bettypes

                switch (betteam.ToLower())
                {
                    case "1:1":
                        choice = "Over/Odd ";
                        break;

                    case "1:2":
                        choice = "Over/Even ";
                        break;

                    case "2:1":
                        choice = "Under/Odd ";
                        break;

                    case "2:2":
                        choice = "Under/Even ";
                        break;

                    default:
                        choice = "Over/Odd ";
                        break;
                }

                #endregion number group - bettypes

                var maxBall = (double.Parse(Bingo.Core.BingoSettings.MaxBall.ToString()) / 2);
                choice += bl.Liveindicator ? string.Format("<span class='bl-bettype'>{0}</span>", maxBall) : string.Empty;
                choice += liveScore;
            }
            else if (bettype == 90)// number group
            {
                choice = Fanex.Tarus.BetList.Helper.ChoiceHelper.GetNumberWheelChoice(CastAide.AsInt32(bl.hdp1), CastAide.AsInt32(bl.Betteam));
                choice += liveScore;
            }

            #endregion check bettype & apply betty name

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);

            string bettypeFormat = string.Empty;
            switch (bettype)
            {
                case 87:
                case 88:
                case 89:
                case 90:
                    bettypeFormat = bettypeName;
                    break;

                case 81:
                case 82:
                case 83:
                case 84:
                    bettypeFormat = (!bl.Liveindicator ? string.Empty : "Next ") + bettypeName;
                    break;

                default:
                    bettypeFormat = (!bl.Liveindicator ? "FT. " : "Next ") + bettypeName;
                    break;
            }
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeFormat);

            TemplateHandler.Assign(ref eventItem, "{event}", string.Format("{0} {1}",
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNumberGameNo"), bl.MatchCode));

            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenNumberGame (bettype = 81-> 90)

        #region Soccer - new bettypes

        #region Double Chance (bettype = 24)

        protected void GenDoubleChanceEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            #region private variables

            bool liveStyle = bl.Liveindicator;
            var liveHomeScore = bl.Livehomescore;
            var liveAwayScore = bl.Liveawayscore;
            var betteam = bl.Betteam;
            var home = CacheRef.GetTeamName(bl.HomeID);
            var away = CacheRef.GetTeamName(bl.AwayID);
            var liveScore = (!liveStyle) ? string.Empty : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            var choice = string.Empty;
            var bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            var sporttypeName = CacheRef.GetSportName(bl.Sporttype);
            var leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            var statusId = bl.StatusId;
            var odds = bl.Odds;

            #endregion private variables

            #region assign value for choice parameter

            if (betteam.ToLower().Equals("1x"))
                choice = string.Format("{0} <span style='color: #000; font-weight: normal;'>or</span> Draw", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("2x"))
                choice = string.Format("{0} <span style='color: #000; font-weight: normal;'>or</span> Draw", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("12"))
                choice = string.Format("{0} <span style='color: #000; font-weight: normal;'>or</span> {1}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) choice += oddMP(odds);

            #endregion assign value for choice parameter

            #region assign template

            AssignDataForEventColumn(ref eventItem, choice, liveScore, bettypeName, sporttypeName, leagueName, home, away, statusId);

            #endregion assign template
        }

        #endregion Double Chance (bettype = 24)

        #region Draw no bet (bettype = 25)

        protected void GenDrawNoBetEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            #region private variables

            bool liveStyle = bl.Liveindicator;
            var liveHomeScore = bl.Livehomescore;
            var liveAwayScore = bl.Liveawayscore;
            var betteam = bl.Betteam;
            var home = CacheRef.GetTeamName(bl.HomeID);
            var away = CacheRef.GetTeamName(bl.AwayID);
            var choice = string.Empty;
            var bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            var sporttypeName = CacheRef.GetSportName(bl.Sporttype);
            var leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            var liveScore = (!liveStyle) ? string.Empty : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            var odds = bl.Odds;

            #endregion private variables

            #region assign value for choice parameter

            if (betteam.ToLower().Equals("h"))
                choice = string.Format("{0}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("a"))
                choice = string.Format("{0}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) choice += oddMP(odds);

            #endregion assign value for choice parameter

            #region assign template

            AssignDataForEventColumn(ref eventItem, choice, liveScore, bettypeName, sporttypeName, leagueName, home, away, bl.StatusId);

            #endregion assign template
        }

        #endregion Draw no bet (bettype = 25)

        #region Both/One/Neither team to score (bettype = 26)

        protected void GenBothOneNeitherEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            #region private variables

            bool liveStyle = bl.Liveindicator;
            var liveHomeScore = bl.Livehomescore;
            var liveAwayScore = bl.Liveawayscore;
            var betteam = bl.Betteam;
            var home = CacheRef.GetTeamName(bl.HomeID);
            var away = CacheRef.GetTeamName(bl.AwayID);
            var choice = string.Empty;
            var bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            var sporttypeName = CacheRef.GetSportName(bl.Sporttype);
            var leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            var liveScore = (!liveStyle) ? string.Empty : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            var odds = bl.Odds;

            #endregion private variables

            #region assign value for choice parameter

            if (betteam.ToLower().Equals("b"))
                choice = "Both";
            else if (betteam.ToLower().Equals("o"))
                choice = "One";
            else if (betteam.ToLower().Equals("n"))
                choice = "No Goal";

            if (isMP) choice += oddMP(odds);

            #endregion assign value for choice parameter

            #region assign template

            AssignDataForEventColumn(ref eventItem, choice, liveScore, bettypeName, sporttypeName, leagueName, home, away, bl.StatusId);

            #endregion assign template
        }

        #endregion Both/One/Neither team to score (bettype = 26)

        #region To win to nil (bettype = 27)

        protected void GenToWinToNilEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            #region private variables

            bool liveStyle = bl.Liveindicator;
            var liveHomeScore = bl.Livehomescore;
            var liveAwayScore = bl.Liveawayscore;
            var betteam = bl.Betteam;
            var home = CacheRef.GetTeamName(bl.HomeID);
            var away = CacheRef.GetTeamName(bl.AwayID);
            var choice = string.Empty;
            var bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            var sporttypeName = CacheRef.GetSportName(bl.Sporttype);
            var leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            var liveScore = (!liveStyle) ? string.Empty : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            var odds = bl.Odds;

            #endregion private variables

            #region assign value for choice parameter

            if (betteam.ToLower().Equals("h"))
                choice = string.Format("{0}", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));
            else if (betteam.ToLower().Equals("a"))
                choice = string.Format("{0}", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""));

            if (isMP) choice += oddMP(odds);

            #endregion assign value for choice parameter

            #region assign template

            AssignDataForEventColumn(ref eventItem, choice, liveScore, bettypeName, sporttypeName, leagueName, home, away, bl.StatusId);

            #endregion assign template
        }

        #endregion To win to nil (bettype = 27)

        #region Clean sheet (bettype = 13)

        protected void GenCleanSheetEvent(ref string eventItem, BetlistRow bl, bool isMP)
        {
            #region private variables

            bool liveStyle = bl.Liveindicator;
            var liveHomeScore = bl.Livehomescore;
            var liveAwayScore = bl.Liveawayscore;
            var betteam = bl.Betteam;
            var home = CacheRef.GetTeamName(bl.HomeID);
            var away = CacheRef.GetTeamName(bl.AwayID);
            var choice = string.Empty;
            var bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            var sporttypeName = CacheRef.GetSportName(bl.Sporttype);
            var leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            var statusId = bl.StatusId;
            var liveScore = (!liveStyle) ? string.Empty : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            decimal odds = bl.Odds;

            #endregion private variables

            #region assign value for choice parameter

            if (betteam.ToLower().Equals("hy"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "YES");
            else if (betteam.ToLower().Equals("hn"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "NO");
            else if (betteam.ToLower().Equals("ay"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "YES");
            else if (betteam.ToLower().Equals("an"))
                choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""), "NO");

            if (isMP) choice += oddMP(odds);

            #endregion assign value for choice parameter

            #region assign template

            AssignDataForEventColumn(ref eventItem, choice, liveScore, bettypeName, sporttypeName, leagueName, home, away, statusId);

            #endregion assign template
        }

        #endregion Clean sheet (bettype = 13)

        #region 3-way handicap (bettype = 28)

        protected void GenThreeWayHandicap(ref string eventItem, BetlistRow bl, bool isMP)
        {
            #region private variables

            bool liveStyle = bl.Liveindicator;
            var liveHomeScore = bl.Livehomescore;
            var liveAwayScore = bl.Liveawayscore;
            var betteam = bl.Betteam;
            var home = CacheRef.GetTeamName(bl.HomeID);
            var away = CacheRef.GetTeamName(bl.AwayID);
            var choice = string.Empty;
            var bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            var sporttypeName = CacheRef.GetSportName(bl.Sporttype);
            var leagueName = CacheRef.GetLeagueName(bl.LeagueID);
            var liveScore = (!liveStyle) ? string.Empty : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            var hdpResult = 0;
            var hdp1 = bl.hdp1;
            var hdp2 = bl.hdp2;
            var odds = bl.Odds;

            #endregion private variables

            #region assign value for choice parameter

            if (betteam.ToLower().Equals("1"))
            {
                hdpResult = (int)(hdp1 - hdp2 > 0 ? -(hdp1 - hdp2) : Math.Abs(hdp1 - hdp2));

                choice = string.Format("{0} <span style='color:#000;'>({1}{2})</span>",
                    home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    hdpResult > 0 ? "+" : string.Empty, hdpResult);
                /*choice = string.Format("H <span style='color:#000;'>({0}{1})</span>", (hdpResult > 0 ? "+" : string.Empty), hdpResult);*/
            }
            else if (betteam.ToLower().Equals("2"))
            {
                hdpResult = (int)(hdp2 - hdp1 > 0 ? -(hdp2 - hdp1) : Math.Abs(hdp2 - hdp1));
                choice = string.Format("{0} <span style='color:#000;'>({1}{2})</span>",
                    away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), ""),
                    hdpResult > 0 ? "+" : string.Empty, hdpResult);
                /*choice = string.Format("A <span style='color:#000;'>({0}{1})</span>", (hdpResult > 0 ? "+" : string.Empty), hdpResult);*/
            }
            else if (betteam.ToLower().Equals("x"))
                /*choice = string.Format("Draw <span style='color:#000;'>(<span style='color:#ff0000;'>{0}</span> +{1})</span>",
                    (hdp1 - hdp2 > 0 ? away.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), "") :
                    home.Replace(ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral"), "")),
                    (int)(Math.Abs(hdp1 - hdp2)));*/
                choice = string.Format("Draw <span style='color:#000'>(<span style='color:#ff0000;'>{0}</span> +{1})</span>", hdp1 - hdp2 > 0 ? "A" : "H", (int)(Math.Abs(hdp1 - hdp2)));
            if (isMP) choice += oddMP(odds);

            #endregion assign value for choice parameter

            #region assign template

            AssignDataForEventColumn(ref eventItem, choice, liveScore, bettypeName, sporttypeName, leagueName, home, away, bl.StatusId);

            #endregion assign template
        }

        #endregion 3-way handicap (bettype = 28)

        #region assign event column

        protected void AssignDataForEventColumn(ref string eventItem, string choice, string liveScore, string bettypeName,
            string sporttypeName, string leagueName, string home, string away, int statusId)
        {
            var Event = home + " -VS- " + away;
            if (statusId == 102)
            {
                choice = string.Format("<s>{0}</s>", choice);
                bettypeName = string.Format("<s>{0}</s>", bettypeName);
                sporttypeName = string.Format("<s>{0}</s>", sporttypeName);
                leagueName = string.Format("<s>{0}</s>", leagueName);
                Event = string.Format("<s>{0}</s>", Event);
            }

            TemplateHandler.Assign(ref eventItem, "{choice}", string.Format("{0} {1}", choice, liveScore));
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", home + " -VS- " + away);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sporttypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion assign event column

        #endregion Soccer - new bettypes

        #region BettypeWirror

        public void AssignWriror(ref string eventItem, BetlistRow bl)
        {
            int BetType = bl.Bettype;
            string betteam = bl.Betteam;

            string eventBettype = "Number Game No " + bl.MatchCode;
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);

            string choice = betteam == "h" ? "2nd ball win" : "3rd ball win";

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", eventBettype);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion BettypeWirror

        #region Bettype Next Combo

        public void AssignNextCombo(ref string eventItem, BetlistRow bl)
        {
            bool liveStyle = bl.Liveindicator;
            string liveHomeScore = bl.Livehomescore.ToString();
            string liveAwayScore = bl.Liveawayscore.ToString();
            int BetType = bl.Bettype;
            string betteam = bl.Betteam;
            string liveScore = (!liveStyle)
                                   ? string.Empty
                                   : " <span class='bl-bettype'>[" + bl.BetID + "]</span>";
            string choice = "";
            if (betteam == "1:1")
                choice = "Over/Odd";
            else if (betteam == "1:2")
                choice = "Over/Even";
            else if (betteam == "2:1")
                choice = "Under/Odd";
            else if (betteam == "2:2")
                choice = "Under/Even";
            choice += " " + (double.Parse(Bingo.Core.BingoSettings.MaxBall.ToString()) / 2) + " " + liveScore;
            string eventBettype = "Number Game No " + bl.MatchCode;
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", eventBettype);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion Bettype Next Combo

        #region P2PGames

        private void GenP2PGamesEvent(ref string eventItem)
        {
            TemplateHandler.Assign(ref eventItem, "{choice}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "P2PGames"));
            TemplateHandler.Assign(ref eventItem, "{bettype}", string.Empty);
            TemplateHandler.Assign(ref eventItem, "{event}", string.Empty);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", string.Empty);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", string.Empty);
            TemplateHandler.RemoveBlock(ref eventItem, "P2Premove");
        }

        #endregion P2PGames

        #region combination mp

        protected void GenCombinationMP(ref string eventItem, BetlistRow bl)
        {
            var strCombinationName = CacheRef.GetCombinationName(bl.Betteam);
            eventItem = "<a class='lnk' style='color:gray' href='javascript:void(\"\");' onclick='showCMP(event,\"" +
                               bl.RefNO + "\",\"" + 1 + "\");'><span style='color:blue'>System Parlay</span> - " +
                               strCombinationName + "</a>"
                               + "<div id='spEvent_" + bl.RefNO + bl.TransID + "' class='hide'></div>";
            eventItem += "<br/>";
            eventItem +=
                "<div align='right' style='float:right; cursor:pointer' href='javascript:void(\"\");' onclick='javascript:showDetailsCMP(\""
                + bl.RefNO + "\",\"" + Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype) + " - " + strCombinationName + "\", " + bl.TransID
                + ", " + bl.BetID
                +
                ");' onmouseover='javascript:cmpMouseOverDetailButton(event);' onmouseout='javascript:cmpMouseOutDetailButton(event);'>";
            eventItem += "<table border='0' width='60' cellpadding='0' cellspacing='0' style='border-collapse:collapse'>";
            eventItem += "<tr>";

            eventItem += "<td style='width:20px; border:0px none'>";
            eventItem += "<div style='width:14px; height:14px; background:url(\"Images/a_Icon.png\") no-repeat scroll 0 0 transparent'></div>";
            eventItem += "</td>";

            eventItem += "<td style='width:40px; border:0px none'>";
            eventItem += "<a style='color:#0094E7; font-weight:bold'>Details</a>";
            eventItem += "</td>";

            eventItem += "</tr>";
            eventItem += "</table></div>";
        }

        #endregion combination mp

        #region GenerateEvent

        public string GenerateEvent(string evt, BetlistRow bl)
        {
            switch (bl.Bettype)
            {
                case 1:
                case 7:
                case 124://FT 1x2 AH
                case 125://1st 1x2 AH
                case 20:
                case 1201:
                    GenHdpEvent(ref evt, bl, false);
                    break;

                case 12:
                case 2:
                    GenOEEvent(ref evt, bl, false);
                    break;

                case 3:
                case 8:
                case 1203:
                    GenOUEvent(ref evt, bl, false);
                    break;

                case 4:
                case 30:
                case 14:
                case 1204:
                    GenCSEvent(ref evt, bl, false);
                    break;

                case 5://FT 1x2
                case 1205://FT 1x2

                    Gen1x2Event(ref evt, bl, false);
                    break;

                case 15://1st 1x2

                    Gen1x2Event(ref evt, bl, false);
                    break;

                case 6:
                case 16:
                case 1206:
                    GenTGEvent(ref evt, bl, false);
                    break;

                case 10:
                    GenOutrightRow(ref evt, bl);
                    break;

                case 121:
                case 122:
                case 123:
                    GenHomeAwayDrawNoBet(ref evt, bl, false);
                    break;

                case 31:
                case 32:
                case 33:
                case 41:
                case 42:
                case 43:
                    GenHorseracingRow(ref evt, bl);
                    break;

                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                    GenFinancialRow(ref evt, bl);
                    break;

                case 9:
                    //GenMixParlay(ref evt, bl);
                    evt = "<div id='spEvent_" + bl.RefNO +
                          "' class='hide'></div><a class='lnk' href='javascript:void(\"\");' onclick='showMP(\"" +
                          bl.RefNO + "\");' id='hidden0'>Mix Parlay</a>";
                    break;

                case 22:
                    GenNextGoalEvent(ref evt, bl);
                    break;

                case 71:
                case 72:
                case 112:
                case 113:
                case 114:
                    GenCasino(ref evt, bl.Bettype);
                    break;

                case 81://Number Game
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 90:
                    GenEvent_NumberGame(ref evt, bl, false);
                    break;

                case 88:
                    AssignWriror(ref evt, bl);
                    break;

                case 89:
                    AssignNextCombo(ref evt, bl);
                    break;

                case 24:
                case 1224:
                    GenDoubleChanceEvent(ref evt, bl, false);
                    break;

                case 25:
                    GenDrawNoBetEvent(ref evt, bl, false);
                    break;

                case 26:
                    GenBothOneNeitherEvent(ref evt, bl, false);
                    break;

                case 27:
                    GenToWinToNilEvent(ref evt, bl, false);
                    break;

                case 13:
                    GenCleanSheetEvent(ref evt, bl, false);
                    break;

                case 28:
                    GenThreeWayHandicap(ref evt, bl, false);
                    break;

                case 1000:
                    GenP2PGamesEvent(ref evt);
                    break;

                case 29:
                    GenCombinationMP(ref evt, bl); ;
                    break;
            }
            return evt;
        }

        private void GenHomeAwayDrawNoBet(ref string eventItem, BetlistRow bl, bool isMP)
        {
            bool liveStyle = bl.Liveindicator;
            string liveHomeScore = bl.Livehomescore.ToString();
            string liveAwayScore = bl.Liveawayscore.ToString();
            string home = CacheRef.GetTeamName(bl.HomeID) + ((bl.isNeutral) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty);
            string away = CacheRef.GetTeamName(bl.AwayID);
            int BetType = bl.Bettype;
            string betteam = bl.Betteam;
            decimal hdp1 = bl.hdp1;
            decimal hdp2 = bl.hdp2;
            string liveScore = (!liveStyle)
                                   ? string.Empty
                                   : " <span class='bl-bettype'>[" + liveHomeScore + "-" + liveAwayScore + "]</span>";
            string choice = "";

            if (bl.Bettype == 121)
            {
                if (bl.Betteam == "x") choice = "Draw";
                else if (bl.Betteam == "a") choice = away;
            }
            else if (bl.Bettype == 122)
            {
                if (bl.Betteam == "h") choice = home;
                else if (bl.Betteam == "x") choice = "Draw";
            }
            else if (bl.Bettype == 123)
            {
                if (bl.Betteam == "h") choice = "Draw";
                else if (bl.Betteam == "a") choice = "No Draw";
            }

            decimal odds = bl.Odds;
            if (isMP) choice += oddMP(odds);
            string bettypeName = Fanex.Tarus.Infrastructure.Helper.SportHelper.Bettype.GetBettype(bl.Bettype);
            string sportypeName = CacheRef.GetSportName(bl.Sporttype);
            string leagueName = CacheRef.GetLeagueName(bl.LeagueID);

            choice += liveScore;
            var Event = home + " -VS- " + away;

            TemplateHandler.Assign(ref eventItem, "{choice}", choice);
            TemplateHandler.Assign(ref eventItem, "{bettype}", bettypeName);
            TemplateHandler.Assign(ref eventItem, "{event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", sportypeName);
            TemplateHandler.Assign(ref eventItem, "{leaguename}", leagueName);
        }

        #endregion GenerateEvent

        public string GenerateEventLiveCasino(string eventItem, BetlistRow blRow, System.Collections.Generic.List<Fanex.Tarus.LiveCasino.Entities.Betlist.BetlistEvent> betlistEvent)
        {
            string Event = "";
            for (int i = 0; i < betlistEvent.Count; i++)
            {
                Event += "<span style='color:Red; font-weight:bold'>" + betlistEvent[i].GameType + "</span>&nbsp;<span style='color:#86817B'>" + betlistEvent[i].Stake + "</span>&nbsp;@&nbsp;<span style='color:#7C9097'>" + betlistEvent[i].Odds + "</span>";
                Event += "<br/>";
            }
            TemplateHandler.Assign(ref eventItem, "{Event}", Event);
            TemplateHandler.Assign(ref eventItem, "{sporttype}", "Live Casino");
            TemplateHandler.Assign(ref eventItem, "{bettype}", CacheRef.GetBettype(blRow.Bettype));
            TemplateHandler.Assign(ref eventItem, "{showtime}", blRow.ShowTime);
            return eventItem;
        }
    }
}