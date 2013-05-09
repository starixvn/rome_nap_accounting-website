// -----------------------------------------------------------------------
// <copyright file="SportingSolutionsService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.BetList.Business.SportingSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DNAide.Core;

   
    public class SportingSolutionsService: ISportingSolutionsService
    {
        public string GetChoice(int bettypeId, string resourceId, string betteam, int hdp1, int hdp2, string home, string away)
        {
            string choice = string.Empty;
           // var bettypeId = CastAide.AsInt32(resourceId.Substring(3,4));
            string Set = string.Empty, Game = string.Empty;
            if (bettypeId > 1309 && resourceId.Length >= 3)
            {
                Set = resourceId.Substring(1, 2);
            }
            if (bettypeId > 1323 && resourceId.Length >= 5)
            {
                Game = resourceId.Substring(3, 2);
            }
            switch (bettypeId)
            {
                case 1301://Match Winner
                case 1311:// Set x Winner
                case 1319:// Set x Race to y Games
                case 1324:// Set x Gmae y Winner
                case 1332:// Set x Game y Point z Winner
                case 1303:// Set Handicap
                case 1308:// Match Handicap Games 2-Way
                case 1316:// Set x Game Handicap
                case 1326: // Set x Game Y Point Handicap
                    choice = betteam == "h" ? home : away;
                    break;
                case 1302:// Match Correct Scorce
                    choice = GenMatchCorrectScore(betteam);;
                    break;
                case 1305:// Match Total Games Odd/Even
                case 1318:// Set x Total Games Odd/Even
                case 1331: // Set x Game Y Total Points Odd/Even
                    choice =  betteam == "h" ? "Odd" : "Even";
                    break;
                case 1306:// Match Total Games 2-Way
                case 1304:// Total Set
                case 1310:// Player x Match Total Games 2-Way
                case 1312:// Set x Total Games 2-Way
                case 1323:// Set x Total Deuce Games 2- Way
                case 1329: // Set x Game Y Total Points 2 -Way
                    choice = (betteam == "h" ? "Over " : "Under ") + hdp1;
                    break;
                case 1307://Match Total Games 3-Way
                case 1313:// Set x Total Games 3-Way
                case 1330:// Set x Game Y Total Points 3- Way
                    choice = GenMatchTotalGames3Way(betteam, hdp1, hdp2);
                    break;
                case 1309://Match Handicap Games 3-way
                    choice = GenMatchHandicapGame3Way(betteam, home, away);
                    break;
                case 1314:// Set x will there be a Tiebreak?
                case 1327:// Set x Game Y Duece Betting
                    choice = betteam == "h" ? "Yes" : "NO";
                    break;
                case 1315:// Set x Playerto win on a Tiebreak
                    choice = GenPlayerWinOnTiebreak(betteam, home, away);
                    break;
                case 1317:// Set x Correct Score
                    choice = GenCorrectScoreSet(betteam);
                    break;
                case 1320:// Set x Score after 2 Games
                    choice = GenSetScoreAfter2Games(betteam);
                    break;
                case 1321:// Set x score after 4 Games
                    choice = GenSetScoreAfter4Games(betteam);
                    break;
                case 1322: // Set x score after 6 Games
                    choice = GenSetScoreAfter6Games(betteam);
                    break;
                case 1325:
                    choice = GenSetXGameYCorrectScore(betteam);
                    break;
                case 1328:// Set x Game Y Total Points Exact
                    choice = GenSetXGameYTotalPointsExact(betteam);
                    break;
                case 1333: // Set x Game y & y+1 Winner
                    choice = GenSetXGameY1Winner(betteam, home, away);
                    break;
                case 1334: // Set X Game y & y+1 Points
                    choice = GenSetXGameY1Points(betteam, hdp1);
                    break;

            }
            return choice;
        }

        private string GenMatchCorrectScore(string betteam)
        {
            string choice = string.Empty;
            switch (betteam)
            {
                case "21":
                    choice = "2:1";
                    break;
                case "12":
                    choice = "1:2";
                    break;
                case "20":
                    choice = "2:0";
                    break;
                case "02":
                    choice = "0:2";
                    break;
                case "30":
                    choice = "3:0";
                    break;
                case "03":
                    choice = "0:3";
                    break;
                case "31":
                    choice = "3:1";
                    break;
                case "13":
                    choice = "1:3";
                    break;
                case "32":
                    choice = "3:2";
                    break;
                case "23":
                    choice = "2:3";
                    break;
            }
            return choice;
        }
        private string GenSetXGameY1Points(string betteam, int hdp1)
        {
            string choice = string.Empty;
            switch (betteam)
            {
                case "1":
                    choice = "Under ";
                    break;
                case "2":
                    choice = "Over ";
                    break;
                case "x":
                    choice = "exactly ";
                    break;
            }
            return choice + hdp1;
        }

        private string GenSetXGameY1Winner(string betteam, string home, string away)
        {
            string choice = string.Empty;
            switch (betteam)
            {
                case "1":
                    choice = home;
                    break;
                case "2":
                    choice = away;
                    break;
                case "x":
                    choice = "neither";
                    break;
            }
            return choice;
        }

        private string GenSetXGameYTotalPointsExact(string betteam)
        {
             string choice = string.Empty;
             switch (betteam)
             {
                 case "01":
                     choice = "4 points";
                     break;
                 case "02":
                     choice = "5 points";
                     break;
                 case "03":
                     choice = "6 points";
                     break;
                 case "04":
                     choice = "7 UP points";
                     break;
             }
             return choice;
        }

        private string GenSetXGameYCorrectScore(string betteam)
        {
            string choice = string.Empty;
            switch (betteam)
            {
                case "01":
                    choice = "0:1";
                    break;
                case "02":
                    choice = "0:2";
                    break;
                case "03":
                    choice = "0:3";
                    break;
                case "04":
                    choice = "0:4";
                    break;
                case "10":
                    choice = "1:0";
                    break;
                case "20":
                    choice = "2:0";
                    break;
                case "30":
                    choice = "3:0";
                    break;
                case "40":
                    choice = "4:0";
                    break;
            }
            return choice;
        }

        private string GenSetScoreAfter6Games(string betteam)
        {
            string choice = string.Empty;
            switch (betteam)
            {
                case "60":
                    choice = "6:0";
                    break;
                case "51":
                    choice = "5:1";
                    break;
                case "42":
                    choice = "4:2";
                    break;
                case "33":
                    choice = "3:3";
                    break;
                case "24":
                    choice = "2:4";
                    break;
                case "15":
                    choice = "1:5";
                    break;
                case "06":
                    choice = "0:6";
                    break;
            }
            return choice;
        }

        private string GenSetScoreAfter4Games(string betteam)
        {
            string choice = string.Empty;
            switch (betteam)
            {
                case "40":
                    choice = "4:0";
                    break;
                case "31":
                    choice = "3:1";
                    break;
                case "22":
                    choice = "2:2";
                    break;
                case "13":
                    choice = "1:3";
                    break;
                case "04":
                    choice = "0:4";
                    break;
            }
            return choice;
        }

        private string GenSetScoreAfter2Games(string betteam)
        {
            string choice = string.Empty;
            switch (betteam)
            {
                case "20":
                    choice = "2:0";
                    break;
                case "11":
                    choice = "1:1";
                    break;
                case "02":
                    choice = "0:2";
                    break;
            }
            return choice;

        }

        private string GenCorrectScoreSet(string betteam)
        {
            string choice = string.Empty;
            switch (betteam)
            {
                case "60":
                    choice = "6:0";
                    break;
                case "06":
                    choice = "0:6";
                    break;
                case "61":
                    choice = "6:1";
                    break;
                case "16":
                    choice = "1:6";
                    break;
                case "62":
                    choice = "6:2";
                    break;
                case "26":
                    choice = "2:6";
                    break;
                case "63":
                    choice = "6:3";
                    break;
                case "36":
                    choice = "3:6";
                    break;
                case "64":
                    choice = "6:4";
                    break;
                case "46":
                    choice = "4:6";
                    break;
                case "75":
                    choice = "7:5";
                    break;
                case "57":
                    choice = "5:7";
                    break;
                case "76":
                    choice = "7:6";
                    break;
                case "67":
                    choice = "6:7";
                    break;
            }
            return choice;
        }

        private string GenPlayerWinOnTiebreak(string betteam, string home, string away)
        {
            string choice = string.Empty;
            if (betteam == "1")
                choice = home;
            else if (betteam == "2")
                choice = away;
            else if (betteam == "x")
                choice = "No Tiebreak";
            return choice;
        }

        private string GenMatchHandicapGame3Way(string betteam, string home, string away)
        {
            string choice = string.Empty;
            if (betteam == "x")
                choice = "Draw";
            else if (betteam == "1")
                choice = home;
            else if (betteam == "2")
                choice = away;
            return choice;
        }

        private string GenMatchTotalGames3Way(string betteam, int hdp1, int hdp2)
        {
            string choice = string.Empty;
            if (betteam == "1")
            {
                choice = "Over " + hdp1;
            }
            else if (betteam == "x")
            {
                choice = "Between " + hdp2 + " and " + hdp1;
            }
            else if (betteam == "2")
            {
                choice = "Under " + hdp2;
            }
            return choice;
        }

       
    }
}
