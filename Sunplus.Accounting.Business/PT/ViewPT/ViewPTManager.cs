using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CasinoLibrary.SBAgent;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using Fanex.Tarus.LiveCasino.Business.Setting;
using Microsoft.Practices.Unity;
using Sunplus.Accounting.Business.PositionTaking;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.AdoNet;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
using Sunplus.Accounting.Entities.PositionTaking.Racing;
using Sunplus.Bingo.Core;
using Sunplus.Bingo.Entities;
using Sunplus.Bingo.Services;
using Bettype = Sunplus.Accounting.Entities.Customer.PT.ViewPT.Bettype;
using Constants = Sunplus.Accounting.Core.Common.Constants;
using Result = Sunplus.Bingo.Entities.Result;
using Sport = Sunplus.Accounting.Entities.Customer.PT.ViewPT.Sport;

namespace Sunplus.Accounting.Business.PT.ViewPT
{
    public class ViewPTManager:IViewPtManager
    {
        private static string _ibcAutoPt = "v{0}_AutoIBC_pt_{1}";
        private static string _ibcPt = "ibc_pt_hr";
        private static string _ibcPtForTote = "{0}_ibc_pt_{1}";
        private static string _ibcPtForToteOnMember = "v{0}_ibc_pt_{1}_{2}";
        private static string _comGroupA = "ga{0}";
        private static string _comGroupB = "gb{0}";
        private static string _comGroupC = "gc{0}";
        private static string _comGroupD = "gd{0}";
        private static string _comOther = "othercomm{0}";
        private static string _com1x2 = "comm_5_{0}";
        private static string _comFi = "v201_comm_3_{0}";
        private static string _comP2PGame = "v253_comm_1_{0}";
        private static string _comP2PPrivateGame = "v253_comm_2_{0}";
        private static string _comGameComm = "v161_comm_81_{0}";
        private static string _comHorse = "v{0}_comm_{1}_{2}";
        private static string _enableToteKey = "enabletotestatus_{0}_{1}";
        private static string _enableHorseKey = "enablehorsestatus_{0}";
        private static string _currencyallowforeachtoteKey = "currencyallowforeachtotekey{0}_{1}";
        private static string _currency = "currency";
        private static string _sportGroupStatusSessionKey = "sportgroupstatussessionkey{0}";
        private static string _lcsPt = "{0}positiontaking";
        public SportDTO GetPtSportType(string userName)
        {
            var result = new SportDTO();
            try
            {
                result.SportsList = new List<Sport>();
                using (var dr = SelectCustomer.GetPTSportType())
                {
                    Sport children;
                    var racebookChildren = new List<Sport>();

                    #region get parent sport

                    while (dr.Read())
                    {
                        var sportId = CastAide.AsInt32(dr["sporttype"].ToString());
                        var bettype = GetPtBetType(sportId);
                        if (sportId == Constants.Sport.HorseRacing || sportId == Constants.Sport.Greyhound ||
                            sportId == Constants.Sport.Harness)
                        {
                            if (!GetRacbookStatus(sportId))
                                continue;
                            children = new Sport()
                                           {
                                               SportId = sportId,
                                               SportName = dr["sportname"].ToString(),
                                               Bettype = bettype
                                           };
                            racebookChildren.Add(children);
                        }
                        else
                        {
                            if (sportId == Constants.Sport.NumberGame && !BingoSettings.NumberGame.Enabled) continue;
                            var sport = new Sport()
                                            {
                                                SportId = sportId,
                                                SportName = dr["sportname"].ToString(),
                                                Bettype = bettype
                                            };
                            result.SportsList.Add(sport);
                        }
                    }

                    #endregion

                    #region racebook to sport list

                    if (racebookChildren != null)
                    {
                        var racebook = new Sport()
                                           {
                                               SportId = -1,
                                               SportName =
                                                   ResourceExtension.GetResourceValue(
                                                       ResourceExtension.Resources.Language, "HR"),
                                               Children = racebookChildren
                                           };
                        result.SportsList.Add(racebook);
                    }

                    #endregion

                    #region add Casino, Bingo, Mixed sport

                    GetSportsOutOfList(ref result, userName);

                    #endregion
                }
            }
            catch(Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public BettypeDTO GetPtBetType(int sportId)
        {
            var result = new BettypeDTO {BettypeList = new List<Bettype>()};
            try
            {
                if(sportId == Constants.Sport.LiveCasino)
                    return result;
                var racingSetting = RacingSection.Instance;
                var selectHorse =
                    (from currentHorse in
                         racingSetting.Racings.Cast<RacingElement>().Where(
                             currentHorse => currentHorse.Id == sportId.ToString())
                     select currentHorse);

                using (var dr = SelectCustomer.GetPTBetType(sportId))
                {
                    #region for Racebook

                    if (selectHorse.Count() > 0)
                    {

                        while (dr.Read())
                        {
                            var currentBettypeId = CastAide.AsInt32(dr["bettype"].ToString());
                            var currentBettypeName = string.Empty;
                            var toteEnableStatus = false;
                            switch (currentBettypeId)
                            {
                                case (int) BettypeInfo.Bettype.SAToteWin:
                                case (int) BettypeInfo.Bettype.SATotePlace:
                                    currentBettypeName = dr["typenamee"].ToString();
                                    toteEnableStatus = GetToteStatus(sportId, (int) BettypeInfo.Bettype.SAToteWin);
                                    break;
                                case (int) BettypeInfo.Bettype.UKToteWin:
                                case (int) BettypeInfo.Bettype.UKTotePlace:
                                    currentBettypeName = dr["typenamee"].ToString();
                                    toteEnableStatus = GetToteStatus(sportId, (int) BettypeInfo.Bettype.UKToteWin);
                                    break;
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                case (int) BettypeInfo.Bettype.AUTotePlace:
                                    currentBettypeName = currentBettypeId == (int) BettypeInfo.Bettype.AUToteWin
                                                             ? ResourceExtension.GetResourceValue(
                                                                 ResourceExtension.Resources.Language, "lblAUTote51")
                                                             : ResourceExtension.GetResourceValue(
                                                                 ResourceExtension.Resources.Language, "lblAUTote52");
                                    toteEnableStatus = GetToteStatus(sportId, (int) BettypeInfo.Bettype.AUToteWin);
                                    break;
                                default:
                                    break;
                            }

                            if (!toteEnableStatus) continue;
                            var bettype = new Bettype()
                                              {
                                                  BettypeId = currentBettypeId,
                                                  BettypeName = currentBettypeName
                                              };
                            result.BettypeList.Add(bettype);
                        }

                    }
                        #endregion

                    #region all sports
                    else
                    {
                        while (dr.Read())
                        {
                            #region remove mix parlay on Other sports
                            var bettypeId = CastAide.AsInt32(dr["bettype"].ToString());

                            if (sportId == Constants.Sport.OtherSports &&
                                bettypeId == Constants.BetType.MixParlay)
                                continue;

                            #endregion
                            if (sportId == Constants.Sport.NumberGame && !CheckNGBettypeStatusBySport(bettypeId))
                                continue;
                            

                            var bettype = new Bettype()
                                              {
                                                  BettypeId = bettypeId,
                                                  BettypeName = dr["typenamee"].ToString()
                                              };
                            result.BettypeList.Add(bettype);
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public ViewPtDTO ViewPositionTaking(string userName, int sportId, int bettypeId, bool isLive)
        {
            var result = new ViewPtDTO();
            try
            {
                if (sportId == Constants.Sport.Casino || sportId == Constants.Sport.BingoGame)
                    return CasinoPositionTaking(userName, sportId, bettypeId);
                if (sportId == Constants.Sport.LiveCasino)
                    return LiveCasinoPositionTaking(userName);

                var data = SelectCustomer.GetPT(userName, sportId == 999 ? Constants.Sport.OtherSports : sportId,
                                                sportId == 999 ? Constants.BetType.MixParlay : bettypeId, isLive, false);
                if (CastAide.AsInt32(data.Outputs["errcode"], -1) != 0)
                {
                    result.ErrorMessage = data.Outputs["errmsg"].ToString();
                }
                else if (data.ResultSet.Tables.Count > 0)
                {
                    result.IsCasino = false;
                    var drCustInfo = data.ResultSet.Tables[0].Rows[0];
                    var roleId = CastAide.AsInt32(drCustInfo["roleid"], 1);

                    #region assign Customer Info

                    result.CustomerInfo = GetCustomerRelation(data);

                    #endregion

                    #region get PT info

                    #region racebook only, get auto ibc & actual

                    if (sportId == Constants.Sport.HorseRacing || sportId == Constants.Sport.Greyhound ||
                        sportId == Constants.Sport.Harness)
                    {
                        var racebook = GetIbcPtForRacebook(data, roleId, sportId, bettypeId);
                        result.IBCPt = racebook;
                    }
                    result.PotionTakingDetail = PositionTakingDetail(data, roleId, sportId);

                    #endregion

                    #endregion
                }
                else
                {
                    result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                             "thereisnodata");
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public ViewCommissionDTO ViewCommission(string userName)
        {
            var result = new ViewCommissionDTO();
            try
            {
                var data = SelectCustomer.GetPT(userName, 1, 1, false, true);
                if (CastAide.AsInt32(data.Outputs["errcode"], -1) != 0)
                {
                    result.ErrorMessage = data.Outputs["errmsg"].ToString();
                }
                else if (data.ResultSet.Tables.Count > 0)
                {
                    var drCustInfo = data.ResultSet.Tables[0].Rows[0];
                    var roleId = CastAide.AsInt32(drCustInfo["roleid"], 1);
                    var rComm = data.ResultSet.Tables[2].Rows[0];

                    result.Comm = new List<Commission>();

                    #region assign Customer Info

                    result.CustomerInfo = GetCustomerRelation(data);

                    #endregion

                    #region assign Comm

                    #region define group

                    var groupA = new AllowViewSport()
                                     {
                                         Status = true,
                                         Name =
                                             ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                "GroupA")
                                     };
                    var groupB = new AllowViewSport()
                                     {
                                         Status = true,
                                         Name = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                   "GroupB")
                                     };
                    var groupC = new AllowViewSport()
                                     {
                                         Status = true,
                                         Name = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                   "GroupC")
                                     };
                    var groupD = new AllowViewSport()
                                     {
                                         Status = true,
                                         Name = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                   "GroupD")
                                     };
                    var other = new AllowViewSport()
                                    {
                                        Status = true,
                                        Name = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                  "lblOther")
                                    };
                    var finance = new AllowViewSport()
                                      {
                                          Status = true,
                                          Name =
                                              ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                 "lblFIComm")
                                      };
                    var gameComm = new AllowViewSport()
                                       {
                                           Status = BingoSettings.NumberGame.Enabled,
                                           Name =
                                               ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                  "lblGameComm")
                                       };
                   
                    //Enable P2P
                    var p2pStatus = PokerSection.Instance[PokerSection.PokerCollections.CustomerSetting]["visibleP2PStatus"];

                    var p2pGameComm = new AllowViewSport()
                    {
                        Status = CastAide.AsBoolean(p2pStatus.Value),
                        Name =
                            ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                               "lblP2PGameComm")
                    };

                    var p2pPrivateGameComm = new AllowViewSport()
                    {
                        Status = CastAide.AsBoolean(p2pStatus.Value),
                        Name =
                            ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                               "lblP2PPrivateGameComm")
                    };


                    var onexTwo = new AllowViewSport()
                                      {
                                          Status = !WebSettings.Disable1x2Comm,
                                          Name =
                                              ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                                 "lbl1x2")
                                      };
                    var liveCasino = new AllowViewSport()
                                         {
                                             Status = WebSettings.EnableLiveCasino,
                                             Name =
                                                 ResourceExtension.GetResourceValue(
                                                     ResourceExtension.Resources.Language,
                                                     "liveCasinoTitle")
                                         };

                    #endregion

                    #region super

                    if (roleId <= (int) CustInfo.Roles.Super)
                    {
                        #region assign new data

                        #region Horse Racing

                        var superRacingInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.HorseRacing);
                        var totalRacingMember = superRacingInfo != null && superRacingInfo.AllowViewTotes != null
                                                    ? superRacingInfo.AllowViewTotes.Count
                                                    : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(superRacingInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.SAToteWin:
                                    superRacingInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 151, (int) BettypeInfo.Bettype.SAToteWin, 4)],
                                            4);
                                    break;
                                case (int) BettypeInfo.Bettype.UKToteWin:
                                    superRacingInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 151, (int) BettypeInfo.Bettype.UKToteWin, 4)],
                                            4);
                                    break;
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    superRacingInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 151, (int) BettypeInfo.Bettype.AUToteWin, 4)],
                                            4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #region Greyhound

                        var superGreyhoundInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.Greyhound);
                        totalRacingMember = superGreyhoundInfo != null && superGreyhoundInfo.AllowViewTotes != null
                                                ? superGreyhoundInfo.AllowViewTotes.Count
                                                : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(superGreyhoundInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.UKToteWin:
                                    superGreyhoundInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 152, (int) BettypeInfo.Bettype.UKToteWin, 4)],
                                            4);
                                    break;
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    superGreyhoundInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 152, (int) BettypeInfo.Bettype.AUToteWin, 4)],
                                            4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #region Harness

                        var superHarnessInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.Harness);
                        totalRacingMember = superHarnessInfo != null && superHarnessInfo.AllowViewTotes != null
                                                ? superHarnessInfo.AllowViewTotes.Count
                                                : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(superHarnessInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    superHarnessInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 153, (int) BettypeInfo.Bettype.AUToteWin, 4)],
                                            4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #endregion

                        #region assign data to object

                        var superCom = new Commission()
                                           {
                                               RoleName = CustInfo.Roles.Super.ToString(),
                                               GroupA =
                                                   new AllowViewSport()
                                                       {
                                                           Status = groupA.Status,
                                                           Name = groupA.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(
                                                                   rComm[string.Format(_comGroupA, 4)],
                                                                   4)
                                                       },
                                               GroupB =
                                                   new AllowViewSport()
                                                       {
                                                           Status = groupB.Status,
                                                           Name = groupB.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(
                                                                   rComm[string.Format(_comGroupB, 4)],
                                                                   4)
                                                       },
                                               GroupC =
                                                   new AllowViewSport()
                                                       {
                                                           Status = groupC.Status,
                                                           Name = groupC.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(
                                                                   rComm[string.Format(_comGroupC, 4)],
                                                                   4)
                                                       },
                                               GroupD =
                                                   new AllowViewSport()
                                                       {
                                                           Status = groupD.Status,
                                                           Name = groupD.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(
                                                                   rComm[string.Format(_comGroupD, 4)],
                                                                   4)
                                                       },
                                               Other =
                                                   new AllowViewSport()
                                                       {
                                                           Status = other.Status,
                                                           Name = other.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(
                                                                   rComm[string.Format(_comOther, 4)], 4)
                                                       },
                                               OneXTwo =
                                                   new AllowViewSport()
                                                       {
                                                           Status = onexTwo.Status,
                                                           Name = onexTwo.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(rComm[string.Format(_com1x2, 4)],
                                                                                      4)
                                                       },

                                               HorseRacing = superRacingInfo,
                                               Greyhound = superGreyhoundInfo,
                                               Harness = superHarnessInfo,

                                               FiComm =
                                                   new AllowViewSport()
                                                       {
                                                           Status = finance.Status,
                                                           Name = finance.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(rComm[string.Format(_comFi, 4)], 4)
                                                       },
                                               GameComm =
                                                   new AllowViewSport()
                                                       {
                                                           Status = gameComm.Status,
                                                           Name = gameComm.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(
                                                                   rComm[string.Format(_comGameComm, 4)], 4)
                                                       },
                                               P2PGameComm = new AllowViewSport()
                                               {
                                                   Status = p2pGameComm.Status,
                                                   Name = p2pGameComm.Name,
                                                   Comm =
                                                       NumberFormat.DecFormat(
                                                           rComm[string.Format(_comP2PGame,4)], 4)
                                               },
                                               P2PPrivateGameComm = new AllowViewSport()
                                               {
                                                   Status = p2pPrivateGameComm.Status,
                                                   Name = p2pPrivateGameComm.Name,
                                                   Comm =
                                                       NumberFormat.DecFormat(
                                                           rComm[string.Format(_comP2PPrivateGame,4)], 4)
                                               },
                                               LiveCasinoComm = new AllowViewSport()
                                                                    {
                                                                        Status = liveCasino.Status,
                                                                        Name = liveCasino.Name,
                                                                        Comm =
                                                                            NumberFormat.DecFormat(
                                                                                rComm[string.Format(_comGameComm, 4)], 4)
                                                                    }
                                           };
                        result.Comm.Add(superCom);

                        #endregion
                    }

                    #endregion

                    #region master

                    if (roleId <= (int) CustInfo.Roles.Master)
                    {
                        #region assign new data

                        #region Horse Racing

                        var masterRacingInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.HorseRacing);
                        var totalRacingMember = masterRacingInfo != null && masterRacingInfo.AllowViewTotes != null
                                                    ? masterRacingInfo.AllowViewTotes.Count
                                                    : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(masterRacingInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.SAToteWin:
                                    masterRacingInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 151, (int) BettypeInfo.Bettype.SAToteWin, 3)],
                                            4);
                                    break;
                                case (int) BettypeInfo.Bettype.UKToteWin:
                                    masterRacingInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 151, (int) BettypeInfo.Bettype.UKToteWin, 3)],
                                            4);
                                    break;
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    masterRacingInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 151, (int) BettypeInfo.Bettype.AUToteWin, 3)],
                                            4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #region Greyhound

                        var masterGreyhoundInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.Greyhound);
                        totalRacingMember = masterGreyhoundInfo != null && masterGreyhoundInfo.AllowViewTotes != null
                                                ? masterGreyhoundInfo.AllowViewTotes.Count
                                                : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(masterGreyhoundInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.UKToteWin:
                                    masterGreyhoundInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 152, (int) BettypeInfo.Bettype.UKToteWin, 3)],
                                            4);
                                    break;
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    masterGreyhoundInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 152, (int) BettypeInfo.Bettype.AUToteWin, 3)],
                                            4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #region Harness

                        var masterHarnessInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.Harness);
                        totalRacingMember = masterHarnessInfo != null && masterHarnessInfo.AllowViewTotes != null
                                                ? masterHarnessInfo.AllowViewTotes.Count
                                                : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(masterHarnessInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    masterHarnessInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 153, (int) BettypeInfo.Bettype.AUToteWin, 3)],
                                            4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #endregion

                        #region assign data to object

                        var masterCom = new Commission()
                                            {
                                                RoleName = CustInfo.Roles.Master.ToString(),
                                                GroupA =
                                                    new AllowViewSport()
                                                        {
                                                            Status = groupA.Status,
                                                            Name = groupA.Name,
                                                            Comm =
                                                                NumberFormat.DecFormat(
                                                                    rComm[string.Format(_comGroupA, 3)], 4)
                                                        },
                                                GroupB =
                                                    new AllowViewSport()
                                                        {
                                                            Status = groupB.Status,
                                                            Name = groupB.Name,
                                                            Comm =
                                                                NumberFormat.DecFormat(
                                                                    rComm[string.Format(_comGroupB, 3)], 4)
                                                        },
                                                GroupC =
                                                    new AllowViewSport()
                                                        {
                                                            Status = groupC.Status,
                                                            Name = groupC.Name,
                                                            Comm =
                                                                NumberFormat.DecFormat(
                                                                    rComm[string.Format(_comGroupC, 3)], 4)
                                                        },
                                                GroupD =
                                                    new AllowViewSport()
                                                        {
                                                            Status = groupD.Status,
                                                            Name = groupD.Name,
                                                            Comm =
                                                                NumberFormat.DecFormat(
                                                                    rComm[string.Format(_comGroupD, 3)], 4)
                                                        },
                                                Other =
                                                    new AllowViewSport()
                                                        {
                                                            Status = other.Status,
                                                            Name = other.Name,
                                                            Comm =
                                                                NumberFormat.DecFormat(rComm[string.Format(_comOther, 3)],
                                                                                       4)
                                                        },
                                                OneXTwo =
                                                    new AllowViewSport()
                                                        {
                                                            Status = onexTwo.Status,
                                                            Name = onexTwo.Name,
                                                            Comm =
                                                                NumberFormat.DecFormat(rComm[string.Format(_com1x2, 3)], 4)
                                                        },

                                                HorseRacing = masterRacingInfo,
                                                Greyhound = masterGreyhoundInfo,
                                                Harness = masterHarnessInfo,

                                                FiComm =
                                                    new AllowViewSport()
                                                        {
                                                            Status = finance.Status,
                                                            Name = finance.Name,
                                                            Comm =
                                                                NumberFormat.DecFormat(rComm[string.Format(_comFi, 3)], 4)
                                                        },
                                                P2PGameComm = new AllowViewSport()
                                                {
                                                    Status = p2pGameComm.Status,
                                                    Name = p2pGameComm.Name,
                                                    Comm =
                                                        NumberFormat.DecFormat(
                                                            rComm[string.Format(_comP2PGame, 3)], 4)
                                                },
                                                P2PPrivateGameComm = new AllowViewSport()
                                                {
                                                    Status = p2pPrivateGameComm.Status,
                                                    Name = p2pPrivateGameComm.Name,
                                                    Comm =
                                                        NumberFormat.DecFormat(
                                                            rComm[string.Format(_comP2PPrivateGame, 3)], 4)
                                                },
                                                GameComm =
                                                    new AllowViewSport()
                                                        {
                                                            Status = gameComm.Status,
                                                            Name = gameComm.Name,
                                                            Comm =
                                                                NumberFormat.DecFormat(
                                                                    rComm[string.Format(_comGameComm, 3)], 4)
                                                        },
                                                        LiveCasinoComm = new AllowViewSport()
                                                                             {
                                                                                 Status = liveCasino.Status,
                                                                                 Name = liveCasino.Name,
                                                                                 Comm =
                                                                                     NumberFormat.DecFormat(
                                                                                         rComm[string.Format(_comGameComm, 3)], 4)
                                                                             }
                                            };
                        result.Comm.Add(masterCom);

                        #endregion
                    }

                    #endregion

                    #region agent

                    if (roleId <= (int) CustInfo.Roles.Agent)
                    {
                        #region assign new data

                        #region Horse Racing

                        var agentRacingStatus = GetSportGroupStatus(drCustInfo, Constants.Sport.HorseRacing);
                        var totalRacingMember = agentRacingStatus != null && agentRacingStatus.AllowViewTotes != null
                                                    ? agentRacingStatus.AllowViewTotes.Count
                                                    : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(agentRacingStatus.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.SAToteWin:
                                    agentRacingStatus.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 151, (int) BettypeInfo.Bettype.SAToteWin, 2)],
                                            4);
                                    break;
                                case (int) BettypeInfo.Bettype.UKToteWin:
                                    agentRacingStatus.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 151, (int) BettypeInfo.Bettype.UKToteWin, 2)],
                                            4);
                                    break;
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    agentRacingStatus.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 151, (int) BettypeInfo.Bettype.AUToteWin, 2)],
                                            4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #region Greyhound

                        var agentGreyhoundInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.Greyhound);
                        totalRacingMember = agentGreyhoundInfo != null && agentGreyhoundInfo.AllowViewTotes != null
                                                ? agentGreyhoundInfo.AllowViewTotes.Count
                                                : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(agentGreyhoundInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.UKToteWin:
                                    agentGreyhoundInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 152, (int) BettypeInfo.Bettype.UKToteWin, 2)],
                                            4);
                                    break;
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    agentGreyhoundInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 152, (int) BettypeInfo.Bettype.AUToteWin, 2)],
                                            4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #region Harness

                        var agentHarnessInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.Harness);
                        totalRacingMember = agentHarnessInfo != null && agentHarnessInfo.AllowViewTotes != null
                                                ? agentHarnessInfo.AllowViewTotes.Count
                                                : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(agentHarnessInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    agentHarnessInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[string.Format(_comHorse, 153, (int) BettypeInfo.Bettype.AUToteWin, 2)],
                                            4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #endregion

                        #region assign data to object

                        var agentCom = new Commission()
                                           {
                                               RoleName = CustInfo.Roles.Agent.ToString(),
                                               GroupA =
                                                   new AllowViewSport()
                                                       {
                                                           Status = groupA.Status,
                                                           Name = groupA.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(rComm[string.Format(_comGroupA, 2)],
                                                                                      4)
                                                       },
                                               GroupB =
                                                   new AllowViewSport()
                                                       {
                                                           Status = groupB.Status,
                                                           Name = groupB.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(rComm[string.Format(_comGroupB, 2)],
                                                                                      4)
                                                       },
                                               GroupC =
                                                   new AllowViewSport()
                                                       {
                                                           Status = groupC.Status,
                                                           Name = groupC.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(rComm[string.Format(_comGroupC, 2)],
                                                                                      4)
                                                       },
                                               GroupD =
                                                   new AllowViewSport()
                                                       {
                                                           Status = groupD.Status,
                                                           Name = groupD.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(rComm[string.Format(_comGroupD, 2)],
                                                                                      4)
                                                       },
                                               Other =
                                                   new AllowViewSport()
                                                       {
                                                           Status = other.Status,
                                                           Name = other.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(rComm[string.Format(_comOther, 2)], 4)
                                                       },
                                               OneXTwo =
                                                   new AllowViewSport()
                                                       {
                                                           Status = onexTwo.Status,
                                                           Name = onexTwo.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(rComm[string.Format(_com1x2, 2)], 4)
                                                       },

                                               HorseRacing = agentRacingStatus,
                                               Greyhound = agentGreyhoundInfo,
                                               Harness = agentHarnessInfo,

                                               FiComm =
                                                   new AllowViewSport()
                                                       {
                                                           Status = finance.Status,
                                                           Name = finance.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(rComm[string.Format(_comFi, 2)], 4)
                                                       },
                                               P2PGameComm = new AllowViewSport()
                                               {
                                                   Status = p2pGameComm.Status,
                                                   Name = p2pGameComm.Name,
                                                   Comm =
                                                       NumberFormat.DecFormat(
                                                           rComm[string.Format(_comP2PGame, 2)], 4)
                                               },
                                               P2PPrivateGameComm = new AllowViewSport()
                                               {
                                                   Status = p2pPrivateGameComm.Status,
                                                   Name = p2pPrivateGameComm.Name,
                                                   Comm =
                                                       NumberFormat.DecFormat(
                                                           rComm[string.Format(_comP2PPrivateGame,2)], 4)
                                               },
                                               GameComm =
                                                   new AllowViewSport()
                                                       {
                                                           Status = gameComm.Status,
                                                           Name = gameComm.Name,
                                                           Comm =
                                                               NumberFormat.DecFormat(
                                                                   rComm[string.Format(_comGameComm, 2)], 4)
                                                       },
                                                       LiveCasinoComm = new AllowViewSport()
                                                                            {
                                                                                Status = liveCasino.Status,
                                                                                Name = liveCasino.Name,
                                                                                Comm =
                                                                                    NumberFormat.DecFormat(
                                                                                        rComm[string.Format(_comGameComm, 2)], 4)
                                                                            }
                                           };
                        result.Comm.Add(agentCom);

                        #endregion
                    }

                    #endregion

                    #region member

                    if (roleId == (int) CustInfo.Roles.Member)
                    {
                        #region assign new data

                        var group = rComm["ugroup"].ToString().ToLower();
                        if (group != "") group = group.Substring(group.Length - 1, 1);
                        var memberComm = NumberFormat.DecFormat(rComm["comm"], 4);

                        #region Horse Racing

                        var memberRacingInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.HorseRacing);
                        var totalRacingMember = memberRacingInfo != null && memberRacingInfo.AllowViewTotes != null
                                                    ? memberRacingInfo.AllowViewTotes.Count
                                                    : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(memberRacingInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.SAToteWin:
                                    memberRacingInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[
                                                string.Format("v{0}_comm_{1}", Constants.Sport.HorseRacing,
                                                              (int) BettypeInfo.Bettype.SAToteWin)], 4);
                                    break;
                                case (int) BettypeInfo.Bettype.UKToteWin:
                                    memberRacingInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[
                                                string.Format("v{0}_comm_{1}", Constants.Sport.HorseRacing,
                                                              (int) BettypeInfo.Bettype.UKToteWin)], 4);
                                    break;
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    memberRacingInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[
                                                string.Format("v{0}_comm_{1}", Constants.Sport.HorseRacing,
                                                              (int) BettypeInfo.Bettype.AUToteWin)], 4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #region Greyhound

                        var memberGreyhoundInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.Greyhound);
                        totalRacingMember = memberGreyhoundInfo != null && memberGreyhoundInfo.AllowViewTotes != null
                                                ? memberGreyhoundInfo.AllowViewTotes.Count
                                                : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(memberGreyhoundInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.UKToteWin:
                                    memberGreyhoundInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[
                                                string.Format("v{0}_comm_{1}", Constants.Sport.Greyhound,
                                                              (int) BettypeInfo.Bettype.UKToteWin)], 4);
                                    break;
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    memberGreyhoundInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[
                                                string.Format("v{0}_comm_{1}", Constants.Sport.Greyhound,
                                                              (int) BettypeInfo.Bettype.AUToteWin)], 4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #region Harness

                        var memberHarnessInfo = GetSportGroupStatus(drCustInfo, Constants.Sport.Harness);
                        totalRacingMember = memberHarnessInfo != null && memberHarnessInfo.AllowViewTotes != null
                                                ? memberHarnessInfo.AllowViewTotes.Count
                                                : 0;
                        for (var i = 0; i < totalRacingMember; i++)
                        {
                            switch (CastAide.AsInt32(memberHarnessInfo.AllowViewTotes[i].Id))
                            {
                                case (int) BettypeInfo.Bettype.AUToteWin:
                                    memberHarnessInfo.AllowViewTotes[i].Comm =
                                        NumberFormat.DecFormat(
                                            rComm[
                                                string.Format("v{0}_comm_{1}", Constants.Sport.Harness,
                                                              (int) BettypeInfo.Bettype.AUToteWin)], 4);
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #endregion

                        #region assign data to object

                        var memberCom = new Commission()
                                            {
                                                RoleName = CustInfo.Roles.Member.ToString(),
                                                GroupA =
                                                    new AllowViewSport()
                                                        {
                                                            Status = groupA.Status,
                                                            Name = groupA.Name,
                                                            Comm = group.Equals("a") ? memberComm : string.Empty
                                                        },
                                                GroupB =
                                                    new AllowViewSport()
                                                        {
                                                            Status = groupB.Status,
                                                            Name = groupB.Name,
                                                            Comm = group.Equals("b") ? memberComm : string.Empty
                                                        },
                                                GroupC =
                                                    new AllowViewSport()
                                                        {
                                                            Status = groupC.Status,
                                                            Name = groupC.Name,
                                                            Comm = group.Equals("c") ? memberComm : string.Empty
                                                        },
                                                GroupD =
                                                    new AllowViewSport()
                                                        {
                                                            Status = groupD.Status,
                                                            Name = groupD.Name,
                                                            Comm = group.Equals("d") ? memberComm : string.Empty
                                                        },
                                                Other =
                                                    new AllowViewSport()
                                                        {
                                                            Status = other.Status,
                                                            Name = other.Name,
                                                            Comm = NumberFormat.DecFormat(rComm["othercomm"], 4)
                                                        },
                                                OneXTwo =
                                                    new AllowViewSport()
                                                        {
                                                            Status = onexTwo.Status,
                                                            Name = onexTwo.Name,
                                                            Comm = NumberFormat.DecFormat(rComm["comm_5"], 4)
                                                        },

                                                HorseRacing = memberRacingInfo,
                                                Greyhound = memberGreyhoundInfo,
                                                Harness = memberHarnessInfo,

                                                FiComm =
                                                    new AllowViewSport()
                                                        {
                                                            Status = finance.Status,
                                                            Name = finance.Name,
                                                            Comm = NumberFormat.DecFormat(rComm["v201_comm_3"], 4)
                                                        },
                                                P2PGameComm = new AllowViewSport()
                                                {
                                                    Status = p2pGameComm.Status,
                                                    Name = p2pGameComm.Name,
                                                    Comm =
                                                        NumberFormat.DecFormat(
                                                            rComm["v253_Comm_1"], 4)
                                                },
                                                P2PPrivateGameComm = new AllowViewSport()
                                                {
                                                    Status = p2pPrivateGameComm.Status,
                                                    Name = p2pPrivateGameComm.Name,
                                                    Comm =
                                                        NumberFormat.DecFormat(
                                                            rComm["v253_Comm_2"], 4)
                                                },
                                                GameComm =
                                                    new AllowViewSport()
                                                        {
                                                            Status = gameComm.Status,
                                                            Name = gameComm.Name,
                                                            Comm = NumberFormat.DecFormat(rComm["v161_comm_81"], 4)
                                                        },
                                                        LiveCasinoComm = new AllowViewSport()
                                                                             {
                                                                                 Status = liveCasino.Status,
                                                                                 Name = liveCasino.Name,
                                                                                 Comm = NumberFormat.DecFormat(rComm["v161_comm_81"], 4)
                                                                             }
                                            };
                        result.Comm.Add(memberCom);

                        #endregion
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                             "thereisnodata");
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        #region private methods
        private void GetSportsOutOfList(ref SportDTO sports, string userName)
        {
            BettypeDTO bettype;
            try
            {
                if (!WebSettings.disableCS)
                {
                    //Sunplus.Accounting.Business.Customer.GetCustomerInfo
                    var custInfo = new CustomerDAO().GetCustomerInfo(userName);
                    if (custInfo != null && custInfo.Rows.Count > 0)
                    {
                        var rngSiteName = new CustomerDAO().GetRNGSiteNameByCustId(CastAide.AsInt32(custInfo.Rows[0]["custid"]));
                        bettype = GetListCasinoGameKindGameType(rngSiteName);
                        var sport = new Sport()
                        {
                            SportId = Constants.Sport.Casino,
                            SportName = "Casino",
                            Bettype = bettype
                        };
                        sports.SportsList.Add(sport);
                    }
                }
            }
            catch(Exception ex){
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            if (BingoSettings.BingoGame.Enabled)
            {
                bettype = GetListBingoGameKindGameType();
                var sport = new Sport()
                {
                    SportId = Constants.Sport.BingoGame,
                    SportName = "Bingo",
                    Bettype = bettype
                };
                sports.SportsList.Add(sport);
            }
            #region Parlay Mix Sport

            var parlayMixsport = new Sport()
                                     {
                                         SportId = 999,
                                         SportName = ResourceExtension.GetResourceValue(
                                             ResourceExtension.Resources.Language, "lblMixedSports"),
                                         Bettype = new BettypeDTO()
                                     };
            sports.SportsList.Add(parlayMixsport);
            #endregion

            #region live casino
            ISettingManager liveCS = new SettingManager();

            if (WebSettings.EnableLiveCasino)
            {
                
                var sport = new Sport()
                {
                    SportId = Constants.Sport.LiveCasino,
                    SportName = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "liveCasinoTitle"),
                    Bettype = new BettypeDTO()
                };
                sports.SportsList.Add(sport);
            }
            #endregion
            #region All Sports
            var allSports = new Sport()
            {
                SportId = 0,
                SportName = "..:: ALL ::..",
                Bettype = GetPtBetType(0)
            };
            sports.SportsList.Add(allSports);
            #endregion
        }

        private static BettypeDTO GetListBingoGameKindGameType()
        {
            var bettypes = new BettypeDTO {BettypeList = new List<Bettype>()};

            #region get game kind and game type from db
            var customerService = BingoContainer.Current.Resolve<ICustomerService>();
            IResult result = new Result();
            var tblGameKind = customerService.GetListGameKind(BingoSettings.BingoGame.CodeGameNo, ref  result);

            if (tblGameKind != null && tblGameKind.Count > 0)
            {
                
                //Load Game Type
                foreach (GameKind dr in tblGameKind)
                {
                    var tblGameType = customerService.GetListGameType(dr.ID, ref result);
                    if (tblGameType != null && tblGameType.Count > 0)
                    {
                        var gameKind = new Bettype()
                                           {
                                               BettypeId = (-CastAide.AsInt32(dr.ID.ToString())),
                                               BettypeName = string.Format("Group - {0}", dr.Name)
                                           };
                        bettypes.BettypeList.Add(gameKind);
                        foreach (GameType dr2 in tblGameType)
                        {
                            var gameType = new Bettype()
                                               {
                                                   BettypeId = CastAide.AsInt32(dr2.ID.ToString()),
                                                   BettypeName = dr2.Name
                                               };
                            bettypes.BettypeList.Add(gameType);
                        }
                    }
                }
            }
            #endregion

            #region add All Game Kind and All Game Type

            var allGameKind = new Bettype()
                                  {
                                      BettypeId = -1,
                                      BettypeName = ".: ALL - Game Kind :."
                                  };
            bettypes.BettypeList.Add(allGameKind);
            var allGameType = new Bettype()
            {
                BettypeId = 0,
                BettypeName = ".: ALL - Game Type :."
            };
            bettypes.BettypeList.Add(allGameType);

            #endregion

            return bettypes;
        }

        private static BettypeDTO GetListCasinoGameKindGameType(string rngSiteName)
        {
            var bettypes = new BettypeDTO();
            bettypes.BettypeList = new List<Bettype>();

            #region get game kind and game type from db
            var bi = new BasicInfo();
            bi.SiteName = rngSiteName;
            var tblGameKind = bi.GetGameKind("01");
            if (tblGameKind != null && tblGameKind.Rows.Count != 0)
            {
                //Load Game Type
                foreach (DataRow dr in tblGameKind.Rows)
                {
                    var tblGameType = bi.GetGameType(CastAide.AsInt32(dr["CodeID"]));
                    if (tblGameType != null && tblGameType.Rows.Count != 0)
                    {
                        var gameKind = new Bettype()
                        {
                            BettypeId = (-CastAide.AsInt32(dr["CodeID"].ToString())),
                            BettypeName = string.Format("Group - {0}", dr["CodeName"].ToString())
                        };
                        bettypes.BettypeList.Add(gameKind);

                        foreach (DataRow dr2 in tblGameType.Rows)
                        {
                            var gameType = new Bettype()
                            {
                                BettypeId = CastAide.AsInt32(dr2["GameTypeID"].ToString()),
                                BettypeName = dr2["GameTypeName"].ToString()
                            };
                            bettypes.BettypeList.Add(gameType);
                        }
                    }
                }
            }
            #endregion

            #region add All Game Kind and All Game Type

            var allGameKind = new Bettype()
            {
                BettypeId = -1,
                BettypeName = ".: ALL - Game Kind :."
            };
            bettypes.BettypeList.Add(allGameKind);
            var allGameType = new Bettype()
            {
                BettypeId = 0,
                BettypeName = ".: ALL - Game Type :."
            };
            bettypes.BettypeList.Add(allGameType);

            #endregion

            return bettypes;
        }

        private static RacebookIbcPt GetIbcPtForRacebook(DbResult data, int roleId, int sportId, int bettypeId)
        {
            var result = new RacebookIbcPt();
            if (data != null && data.ResultSet.Tables.Count > 0)
            {
                var racebook = data.ResultSet.Tables[2];
                var ibcAutoPt = data.ResultSet.Tables[1];
                result.IbcPtForRacebook = new List<IbcAutoPtForRacebook>();

                #region pt info
                var ibcPtOnRacebook = new IbcAutoPtForRacebook();
                var ibcActualPtOnMember = new ActualIbcPtOnMember();

                #region check valid currency
                var ptInfo = data.ResultSet.Tables[0].Rows[0];
                var currencyOfCustomer = ptInfo.Table.Columns.Contains(_currency) ? CastAide.AsInt32(ptInfo[_currency]) : 1;
                var currencyAllowForTote = GetCurrencyAllowForEachTote(sportId, GetParentToteId(bettypeId));
                var currencyAllowViewPt = Racing.RacingManager.CheckAllowCurrency(currencyOfCustomer, currencyAllowForTote, true);
                
                #endregion

                switch (bettypeId)
                {
                    #region for each bettype
                    #region SA tote
                    case (int)BettypeInfo.Bettype.SAToteWin:
                        //if (!currencyAllowViewPt)
                        //    break;
                        ibcPtOnRacebook.IbcAutoPt = currencyAllowViewPt ?  NumberFormat.DecFormat(racebook.Rows[0][string.Format(_ibcAutoPt, sportId, (int)BettypeInfo.Bettype.SAToteWin)], 2) : "N/A";
                        ibcPtOnRacebook.ToteId = sportId;
                        ibcPtOnRacebook.ToteName = GetToteName((int)BettypeInfo.Bettype.SAToteWin);
                        if (roleId == (int)CustInfo.Roles.Member)
                        {
                            ibcActualPtOnMember.Win = currencyAllowViewPt ? NumberFormat.DecFormat(ibcAutoPt.Rows[0][_ibcPt], 2) : "N/A";
                            ibcPtOnRacebook.ActualIbcPtOnMember = ibcActualPtOnMember;
                        }
                        result.IbcPtForRacebook.Add(ibcPtOnRacebook);
                        break;
                    case (int)BettypeInfo.Bettype.SATotePlace:
                        //if (!currencyAllowViewPt)
                        //    break;
                        ibcPtOnRacebook.IbcAutoPt =currencyAllowViewPt ? NumberFormat.DecFormat(racebook.Rows[0][string.Format(_ibcAutoPt, sportId, (int)BettypeInfo.Bettype.SAToteWin)], 2) : "N/A";
                        ibcPtOnRacebook.ToteId = sportId;
                        ibcPtOnRacebook.ToteName = GetToteName((int)BettypeInfo.Bettype.SAToteWin);
                        if (roleId == (int)CustInfo.Roles.Member)
                        {
                            ibcActualPtOnMember.Place = currencyAllowViewPt ? NumberFormat.DecFormat(ibcAutoPt.Rows[0][_ibcPt], 2) : "N/A";
                            ibcPtOnRacebook.ActualIbcPtOnMember = ibcActualPtOnMember;
                        }
                        result.IbcPtForRacebook.Add(ibcPtOnRacebook);
                        break;
                    #endregion
                    #region UK tote
                    case (int)BettypeInfo.Bettype.UKToteWin:
                        //if (!currencyAllowViewPt)
                        //    break;
                        ibcPtOnRacebook.IbcAutoPt =currencyAllowViewPt ?  NumberFormat.DecFormat(racebook.Rows[0][string.Format(_ibcAutoPt, sportId, (int)BettypeInfo.Bettype.UKToteWin)], 2) : "N/A";
                        ibcPtOnRacebook.ToteId = sportId;
                        ibcPtOnRacebook.ToteName = GetToteName((int)BettypeInfo.Bettype.UKToteWin);
                        if (roleId == (int)CustInfo.Roles.Member)
                        {
                            ibcActualPtOnMember.Win =currencyAllowViewPt ? NumberFormat.DecFormat(ibcAutoPt.Rows[0][_ibcPt], 2) : "N/A";
                            ibcPtOnRacebook.ActualIbcPtOnMember = ibcActualPtOnMember;
                        }
                        result.IbcPtForRacebook.Add(ibcPtOnRacebook);
                        break;
                    case (int)BettypeInfo.Bettype.UKTotePlace:
                        //if (!currencyAllowViewPt)
                        //    break;
                        ibcPtOnRacebook.IbcAutoPt = currencyAllowViewPt ? NumberFormat.DecFormat(racebook.Rows[0][string.Format(_ibcAutoPt, sportId, (int)BettypeInfo.Bettype.UKToteWin)], 2) : "N/A";
                        ibcPtOnRacebook.ToteId = sportId;
                        ibcPtOnRacebook.ToteName = GetToteName((int)BettypeInfo.Bettype.UKToteWin);
                        if (roleId == (int)CustInfo.Roles.Member)
                        {
                            ibcActualPtOnMember.Place = currencyAllowViewPt ? NumberFormat.DecFormat(ibcAutoPt.Rows[0][_ibcPt], 2) : "N/A";
                            ibcPtOnRacebook.ActualIbcPtOnMember = ibcActualPtOnMember;
                        }
                        result.IbcPtForRacebook.Add(ibcPtOnRacebook);
                        break;
                    #endregion
                    #region AU tote
                    case (int)BettypeInfo.Bettype.AUToteWin:
                        //if (!currencyAllowViewPt)
                        //    break;
                        ibcPtOnRacebook.IbcAutoPt = currencyAllowViewPt ? NumberFormat.DecFormat(racebook.Rows[0][string.Format(_ibcAutoPt, sportId, (int)BettypeInfo.Bettype.AUToteWin)], 2) : "N/A";
                        ibcPtOnRacebook.ToteName = GetToteName((int)BettypeInfo.Bettype.AUToteWin);
                        if (roleId == (int)CustInfo.Roles.Member)
                        {
                            ibcActualPtOnMember.Win =currencyAllowViewPt ? NumberFormat.DecFormat(ibcAutoPt.Rows[0][_ibcPt], 2) : "N/A";
                            ibcPtOnRacebook.ActualIbcPtOnMember = ibcActualPtOnMember;
                        }
                        result.IbcPtForRacebook.Add(ibcPtOnRacebook);
                        break;
                    case (int)BettypeInfo.Bettype.AUTotePlace:
                        //if (!currencyAllowViewPt)
                        //    break;
                        ibcPtOnRacebook.IbcAutoPt =currencyAllowViewPt ?  NumberFormat.DecFormat(racebook.Rows[0][string.Format(_ibcAutoPt, sportId, (int)BettypeInfo.Bettype.AUToteWin)], 2) : "N/A";
                        ibcPtOnRacebook.ToteId = sportId;
                        ibcPtOnRacebook.ToteName = GetToteName((int)BettypeInfo.Bettype.AUToteWin);
                        if (roleId == (int)CustInfo.Roles.Member)
                        {
                            ibcActualPtOnMember.Place = currencyAllowViewPt ? NumberFormat.DecFormat(ibcAutoPt.Rows[0][_ibcPt], 2) : "N/A";
                            ibcPtOnRacebook.ActualIbcPtOnMember = ibcActualPtOnMember;
                        }
                        result.IbcPtForRacebook.Add(ibcPtOnRacebook);
                        break;
                    #endregion
                    #endregion
                    #region get all bettype
                    case 0:
                        foreach (DataRow row in ibcAutoPt.Rows)
                        {
                            bool enableTote;
                            switch (CastAide.AsInt32(row["bettype"]))
                            {
                                #region SA tote
                                #region WIN
                                case (int)BettypeInfo.Bettype.SAToteWin:
                                    enableTote = GetToteStatus(sportId, (int)BettypeInfo.Bettype.SAToteWin);
                                    currencyAllowForTote = GetCurrencyAllowForEachTote(sportId, (int)BettypeInfo.Bettype.SAToteWin);
                                    currencyAllowViewPt = Racing.RacingManager.CheckAllowCurrency(currencyOfCustomer, currencyAllowForTote, enableTote);
                                    //if (!currencyAllowViewPt)
                                    //    break;                                    
                                    if (enableTote)
                                    {
                                        var ibcPtSaWinCurrentValue = (from r in ibcAutoPt.AsEnumerable()
                                                                        where
                                                                            r.Field<int>("bettype") ==
                                                                            (int)BettypeInfo.Bettype.SAToteWin
                                                                        select r.Field<decimal>(_ibcPt)).First();

                                        ibcPtOnRacebook.IbcAutoPt = currencyAllowViewPt ?  NumberFormat.DecFormat(racebook.Rows[0][string.Format(_ibcAutoPt, sportId, (int)BettypeInfo.Bettype.SAToteWin)], 2) : "N/A";
                                        ibcPtOnRacebook.ToteId = sportId;
                                        ibcPtOnRacebook.ToteName = GetToteName((int)BettypeInfo.Bettype.SAToteWin);
                                        if (roleId == (int)CustInfo.Roles.Member)
                                        {
                                            ibcActualPtOnMember.Win =currencyAllowViewPt ? NumberFormat.DecFormat(ibcPtSaWinCurrentValue, 2) : "N/A";
                                            ibcActualPtOnMember.EWin =currencyAllowViewPt ?
                                                NumberFormat.DecFormat(
                                                    racebook.Rows[0][
                                                        string.Format(_ibcPtForToteOnMember, sportId,
                                                        (int) BettypeInfo.Bettype.SAToteWin, 33)], 2) : "N/A";
                                        }
                                    }
                                    break;
                                #endregion
                                #region PLACE
                                case (int)BettypeInfo.Bettype.SATotePlace:
                                    //if (!currencyAllowViewPt)
                                    //    break;

                                    enableTote = GetToteStatus(sportId, (int)BettypeInfo.Bettype.SAToteWin);
                                    if (enableTote)
                                    {
                                        var ibcPtSaPlaceCurrentValue = (from r in ibcAutoPt.AsEnumerable()
                                                                        where
                                                                            r.Field<int>("bettype") ==
                                                                            (int)BettypeInfo.Bettype.SATotePlace
                                                                        select r.Field<decimal>(_ibcPt)).First();
                                        if (roleId == (int)CustInfo.Roles.Member)
                                        {
                                            ibcActualPtOnMember.Place = currencyAllowViewPt ? NumberFormat.DecFormat(
                                                ibcPtSaPlaceCurrentValue, 2) : "N/A";
                                            ibcActualPtOnMember.EPlace =currencyAllowViewPt ?
                                                NumberFormat.DecFormat(
                                                    racebook.Rows[0][
                                                        string.Format(_ibcPtForToteOnMember, sportId,
                                                        (int) BettypeInfo.Bettype.SATotePlace, 33)], 2) : "N/A";
                                            ibcPtOnRacebook.ActualIbcPtOnMember = ibcActualPtOnMember;
                                        }
                                        result.IbcPtForRacebook.Add(ibcPtOnRacebook);
                                    }
                                    break;
                                #endregion
                                #endregion
                                #region UK tote
                                #region WIN
                                case (int)BettypeInfo.Bettype.UKToteWin:
                                    enableTote = GetToteStatus(sportId, (int)BettypeInfo.Bettype.UKToteWin);
                                    currencyAllowForTote = GetCurrencyAllowForEachTote(sportId, (int)BettypeInfo.Bettype.UKToteWin);
                                    currencyAllowViewPt = Racing.RacingManager.CheckAllowCurrency(currencyOfCustomer, currencyAllowForTote, enableTote);
                                    //if (!currencyAllowViewPt)
                                    //    break;                                    

                                    if (enableTote)
                                    {
                                        ibcPtOnRacebook = new IbcAutoPtForRacebook();
                                        ibcActualPtOnMember = new ActualIbcPtOnMember();
                                        var ibcPtUkWinCurrentValue = (from r in ibcAutoPt.AsEnumerable()
                                                                        where
                                                                            r.Field<int>("bettype") ==
                                                                            (int)BettypeInfo.Bettype.UKToteWin
                                                                        select r.Field<decimal>(_ibcPt)).First();

                                        ibcPtOnRacebook.IbcAutoPt = currencyAllowViewPt ?  NumberFormat.DecFormat(racebook.Rows[0][string.Format(_ibcAutoPt, sportId, (int)BettypeInfo.Bettype.UKToteWin)], 2) : "N/A";
                                        ibcPtOnRacebook.ToteId = sportId;
                                        ibcPtOnRacebook.ToteName = GetToteName((int)BettypeInfo.Bettype.UKToteWin);
                                        if (roleId == (int)CustInfo.Roles.Member)
                                        {
                                            ibcActualPtOnMember.Win = currencyAllowViewPt ? NumberFormat.DecFormat(ibcPtUkWinCurrentValue, 2) : "N/A";
                                            ibcActualPtOnMember.EWin = currencyAllowViewPt ?
                                                NumberFormat.DecFormat(
                                                    racebook.Rows[0][
                                                        string.Format(_ibcPtForToteOnMember, sportId,
                                                        (int) BettypeInfo.Bettype.UKToteWin, 43)], 2) : "N/A";
                                        }
                                    }
                                    break;
                                #endregion
                                #region PLACE
                                case (int)BettypeInfo.Bettype.UKTotePlace:
                                    //if (!currencyAllowViewPt)
                                    //    break;
                                    enableTote = GetToteStatus(sportId, (int)BettypeInfo.Bettype.UKToteWin);
                                    if (enableTote)
                                    {
                                        var ibcPtUkPlaceCurrentValue = (from r in ibcAutoPt.AsEnumerable()
                                                                        where
                                                                            r.Field<int>("bettype") ==
                                                                            (int)BettypeInfo.Bettype.UKTotePlace
                                                                        select r.Field<decimal>(_ibcPt)).First();
                                        if (roleId == (int)CustInfo.Roles.Member)
                                        {
                                            ibcActualPtOnMember.Place = currencyAllowViewPt ? NumberFormat.DecFormat(
                                                ibcPtUkPlaceCurrentValue, 2) : "N/A";
                                            ibcActualPtOnMember.EPlace =currencyAllowViewPt ?
                                                NumberFormat.DecFormat(
                                                    racebook.Rows[0][
                                                        string.Format(_ibcPtForToteOnMember, sportId,
                                                        (int) BettypeInfo.Bettype.UKTotePlace, 43)], 2) : "N/A";

                                            ibcPtOnRacebook.ActualIbcPtOnMember = ibcActualPtOnMember;
                                        }
                                        result.IbcPtForRacebook.Add(ibcPtOnRacebook);
                                    }
                                    break;
                                #endregion
                                #endregion
                                #region AU tote
                                #region WIN
                                case (int)BettypeInfo.Bettype.AUToteWin:
                                    enableTote = GetToteStatus(sportId, (int)BettypeInfo.Bettype.AUToteWin);
                                    currencyAllowForTote = GetCurrencyAllowForEachTote(sportId, (int)BettypeInfo.Bettype.AUToteWin);
                                    currencyAllowViewPt = Racing.RacingManager.CheckAllowCurrency(currencyOfCustomer, currencyAllowForTote, enableTote);
                                    //if (!currencyAllowViewPt)
                                    //    break;
                                    
                                    if (enableTote)
                                    {
                                        ibcPtOnRacebook = new IbcAutoPtForRacebook();
                                        ibcActualPtOnMember = new ActualIbcPtOnMember();
                                        var ibcPtAuWinCurrentValue = (from r in ibcAutoPt.AsEnumerable()
                                                                        where
                                                                            r.Field<int>("bettype") ==
                                                                            (int)BettypeInfo.Bettype.AUToteWin
                                                                        select r.Field<decimal>(_ibcPt)).First();

                                        ibcPtOnRacebook.IbcAutoPt = currencyAllowViewPt ?  NumberFormat.DecFormat(racebook.Rows[0][string.Format(_ibcAutoPt, sportId, (int)BettypeInfo.Bettype.AUToteWin)], 2) : "N/A";
                                        ibcPtOnRacebook.ToteId = sportId;
                                        ibcPtOnRacebook.ToteName = GetToteName((int)BettypeInfo.Bettype.AUToteWin);
                                        if (roleId == (int)CustInfo.Roles.Member)
                                        {
                                            ibcActualPtOnMember.Win = currencyAllowViewPt ? NumberFormat.DecFormat(ibcPtAuWinCurrentValue, 2) : "N/A";
                                            ibcActualPtOnMember.EWin =currencyAllowViewPt ?
                                                NumberFormat.DecFormat(
                                                    racebook.Rows[0][
                                                        string.Format(_ibcPtForToteOnMember, sportId,
                                                        (int) BettypeInfo.Bettype.AUToteWin, 53)], 2) : "N/A";
                                        }

                                    }
                                    break;
                                #endregion
                                #region PLACE
                                case (int)BettypeInfo.Bettype.AUTotePlace:
                                    //if (!currencyAllowViewPt)
                                    //    break;

                                    enableTote = GetToteStatus(sportId, (int)BettypeInfo.Bettype.AUToteWin);
                                    if (enableTote)
                                    {
                                        var ibcPtAuPlaceCurrentValue = (from r in ibcAutoPt.AsEnumerable()
                                                                        where
                                                                            r.Field<int>("bettype") ==
                                                                            (int)BettypeInfo.Bettype.AUTotePlace
                                                                        select r.Field<decimal>(_ibcPt)).First();
                                        if (roleId == (int)CustInfo.Roles.Member)
                                        {
                                            ibcActualPtOnMember.Place = currencyAllowViewPt ? NumberFormat.DecFormat(
                                                ibcPtAuPlaceCurrentValue, 2) : "N/A";
                                            ibcActualPtOnMember.EPlace = currencyAllowViewPt ?
                                                NumberFormat.DecFormat(
                                                    racebook.Rows[0][
                                                        string.Format(_ibcPtForToteOnMember, sportId,
                                                        (int) BettypeInfo.Bettype.AUTotePlace, 53)], 2) : "N/A";

                                            ibcPtOnRacebook.ActualIbcPtOnMember = ibcActualPtOnMember;
                                        }
                                        result.IbcPtForRacebook.Add(ibcPtOnRacebook);
                                    }
                                    break;
                                #endregion
                                #endregion
                            }
                        }

                        break;
                    #endregion
                    default:
                        break;
                }
                
                #endregion
            }
            return result;
        }

        private static List<PositionTakingDTO> PositionTakingDetail(DbResult data, int roleId, int sportId)
        {
            var ptDetail = new List<PositionTakingDTO>();
            if (data != null && data.ResultSet.Tables.Count > 0)
            {
                var ptData = data.ResultSet.Tables[1];
                var ptInfo = data.ResultSet.Tables[0].Rows[0];
                var currencyOfCustomer = ptInfo.Table.Columns.Contains(_currency) ? CastAide.AsInt32(ptInfo[_currency]) : 1;
                
                foreach (DataRow dr in ptData.Rows)
                {
                    var ptCurrent = new PositionTakingDTO();
                    var betType = CastAide.AsInt32(dr["bettype"]);
                    var sportType = CastAide.AsInt32(dr["sporttype"].ToString());
                    var allowViewCurrentTote = false;
                    var currencyAllowForTote = string.Empty;
                    var currencyAllowViewPt = false;

                    if (sportType == Constants.Sport.OtherSports && betType == Constants.BetType.MixParlay && sportId == Constants.Sport.OtherSports)
                        continue;
                    if(sportId == Constants.Sport.NumberGame && !CheckBettypeSatusForNumberGame(betType, CastAide.AsBoolean(dr["islive"])))
                        continue;

                    #region assign sport info
                    ptCurrent.LiveGame = (CastAide.AsBoolean(dr["islive"])) ? "LIVE" : string.Empty;
                    ptCurrent.SportName = ((sportType == Constants.Sport.OtherSports &&
                                            betType == Constants.BetType.MixParlay)
                                               ? ResourceExtension.GetResourceValue(
                                                   ResourceExtension.Resources.Language, "lblMixedSports")
                                               : dr["sportname"].ToString());

                    ptCurrent.SportType = string.Format("{0}", sportType);
                    ptCurrent.Bettype = betType.ToString();

                    if ((sportType == Constants.Sport.HorseRacing ||
                         sportType == Constants.Sport.Greyhound
                         || sportType == Constants.Sport.Harness))
                    {
                        
                       allowViewCurrentTote = GetToteStatus(sportType, GetParentToteId(betType));

                       #region check currency
                       currencyAllowForTote = GetCurrencyAllowForEachTote(sportType, GetParentToteId(betType));
                       currencyAllowViewPt = Racing.RacingManager.CheckAllowCurrency(currencyOfCustomer, currencyAllowForTote, allowViewCurrentTote);
                       #endregion

                        #region check allow view current tote
                        
                        if (!allowViewCurrentTote)
                            continue;
                        #endregion

                        if (betType == Constants.BetType.Win || betType == Constants.BetType.Place ||
                            betType == Constants.BetType.WinFOdds || betType == Constants.BetType.PlaceFOdds ||
                            betType == Constants.BetType.FiveMinOU || betType == Constants.BetType.FifteenMinOU)
                        {
                            ptCurrent.TypeName = string.Format("{0} - {1}",
                                                               GetToteName(betType),
                                                               HRPositionTakingManager.GetBettypeName(betType));
                        }
                    }
                    else
                        ptCurrent.TypeName = dr["typename"].ToString();

                    ptCurrent.PermistionEnableAutoMaster =
                        Permissions.IsEnabledAutoMaster(!CastAide.AsBoolean(ptInfo["disableautompt"], false));
                    ptCurrent.EnableAutoMaster = !WebSettings.disableAutoM;
                    ptCurrent.DisableFlagCss = (!ptCurrent.PermistionEnableAutoMaster ? "disableFlag" : "OpenFlag");
                    ptCurrent.DisableAutoCss = (!ptCurrent.PermistionEnableAutoMaster ? "disColorMPT" : "");
                    #endregion

                    #region assign pt detail                    

                    if(sportId == Constants.Sport.HorseRacing || sportId == Constants.Sport.Greyhound || sportId == Constants.Sport.Harness)
                    {
                        #region member

                        if (roleId < (int)CustInfo.Roles.Agent)
                        {
                            var memberPt = new PositionTakingInfo()
                            {
                                Super = currencyAllowViewPt ? NumberFormat.DecFormat(dr["s1"], 2) : "N/A",
                                Master = currencyAllowViewPt ? NumberFormat.DecFormat(dr["m1"], 2) : "N/A",
                                Agent = currencyAllowViewPt ? NumberFormat.DecFormat(dr["a1"], 2) : "N/A",
                                SuperAuto = currencyAllowViewPt ? NumberFormat.DecFormat(dr["u1"], 2) : "N/A",
                                SuperIsAuto = 
                                    (CastAide.AsBoolean(dr["i1"], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MasterAuto = currencyAllowViewPt ? NumberFormat.DecFormat(dr["um1"], 2) : "N/A",
                                MasterIsAuto =
                                    (CastAide.AsBoolean(dr["im1"], false))
                                        ? " checked='checked'"
                                        : string.Empty
                            };
                            ptCurrent.Member = memberPt;
                        }

                        #endregion

                        #region agent

                        if (roleId < (int)CustInfo.Roles.Master)
                        {
                            var agentPt = new PositionTakingInfo()
                            {
                                Master = currencyAllowViewPt ? NumberFormat.DecFormat(dr["m2"], 2) : "N/A",
                                Agent = currencyAllowViewPt ? NumberFormat.DecFormat(dr["a2"], 2) : "N/A",
                                SuperAuto = currencyAllowViewPt ? NumberFormat.DecFormat(dr["um2"], 2) : "N/A",
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr["im2"], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MasterAuto = currencyAllowViewPt ? NumberFormat.DecFormat(dr["um2"], 2) : "N/A",
                                MasterIsAuto =
                                    (CastAide.AsBoolean(dr["im2"], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MinPt = currencyAllowViewPt ? NumberFormat.DecFormat(dr["n2"], 2) : "N/A",
                                MaxPt = currencyAllowViewPt ? NumberFormat.DecFormat(dr["l2"], 2) : "N/A"
                            };
                            ptCurrent.Agent = agentPt;
                        }

                        #endregion

                        #region master

                        if (roleId < (int)CustInfo.Roles.Super)
                        {
                            var masterPt = new PositionTakingInfo()
                            {
                                Super = currencyAllowViewPt ? NumberFormat.DecFormat(dr["s3"], 2) : "N/A",
                                Master = currencyAllowViewPt ? NumberFormat.DecFormat(dr["m3"], 2) : "N/A",
                                SuperAuto = currencyAllowViewPt ? NumberFormat.DecFormat(dr["u3"], 2) : "N/A",
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr["i3"], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MinPt = currencyAllowViewPt ? NumberFormat.DecFormat(dr["n3"], 2) : "N/A",
                                MaxPt = currencyAllowViewPt ? NumberFormat.DecFormat(dr["l3"], 2) : "N/A"
                            };
                            ptCurrent.Master = masterPt;
                        }

                        #endregion

                        #region super

                        if (roleId <= (int)CustInfo.Roles.Super)
                        {
                            var superPt = new PositionTakingInfo()
                            {
                                Super = currencyAllowViewPt ? NumberFormat.DecFormat(dr["s4"], 2) : "N/A",
                                Master = currencyAllowViewPt ? NumberFormat.DecFormat(dr["m4"], 2) : "N/A",
                                Agent = currencyAllowViewPt ? NumberFormat.DecFormat(dr["a4"], 2) : "N/A",
                                MinPt = currencyAllowViewPt ? NumberFormat.DecFormat(dr["n4"], 2) : "N/A",
                                MaxPt = currencyAllowViewPt ? NumberFormat.DecFormat(dr["l4"], 2) : "N/A"
                            };
                            ptCurrent.Super = superPt;
                        }

                        #endregion
                    }
                    else
                    {
                        #region member

                        if (roleId < (int)CustInfo.Roles.Agent)
                        {
                            var memberPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr["s1"], 2),
                                Master = NumberFormat.DecFormat(dr["m1"], 2),
                                Agent = NumberFormat.DecFormat(dr["a1"], 2),
                                SuperAuto = NumberFormat.DecFormat(dr["u1"], 2),
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr["i1"], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MasterAuto = NumberFormat.DecFormat(dr["um1"], 2),
                                MasterIsAuto =
                                    (CastAide.AsBoolean(dr["im1"], false))
                                        ? " checked='checked'"
                                        : string.Empty
                            };
                            ptCurrent.Member = memberPt;
                        }

                        #endregion

                        #region agent

                        if (roleId < (int)CustInfo.Roles.Master)
                        {
                            var agentPt = new PositionTakingInfo()
                            {
                                Master = NumberFormat.DecFormat(dr["m2"], 2),
                                Agent = NumberFormat.DecFormat(dr["a2"], 2),
                                SuperAuto = NumberFormat.DecFormat(dr["um2"], 2),
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr["im2"], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MasterAuto = NumberFormat.DecFormat(dr["um2"], 2),
                                MasterIsAuto =
                                    (CastAide.AsBoolean(dr["im2"], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MinPt = NumberFormat.DecFormat(dr["n2"], 2),
                                MaxPt = NumberFormat.DecFormat(dr["l2"], 2)
                            };
                            ptCurrent.Agent = agentPt;
                        }

                        #endregion

                        #region master

                        if (roleId < (int)CustInfo.Roles.Super)
                        {
                            var masterPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr["s3"], 2),
                                Master = NumberFormat.DecFormat(dr["m3"], 2),
                                SuperAuto = NumberFormat.DecFormat(dr["u3"], 2),
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr["i3"], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MinPt = NumberFormat.DecFormat(dr["n3"], 2),
                                MaxPt = NumberFormat.DecFormat(dr["l3"], 2)
                            };
                            ptCurrent.Master = masterPt;
                        }

                        #endregion

                        #region super

                        if (roleId <= (int)CustInfo.Roles.Super)
                        {
                            var superPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr["s4"], 2),
                                Master = NumberFormat.DecFormat(dr["m4"], 2),
                                Agent = NumberFormat.DecFormat(dr["a4"], 2),
                                MinPt = NumberFormat.DecFormat(dr["n4"], 2),
                                MaxPt = NumberFormat.DecFormat(dr["l4"], 2)
                            };
                            ptCurrent.Super = superPt;
                        }

                        #endregion
                    }

                    

                    #endregion

                    ptDetail.Add(ptCurrent);
                }
            }
            return ptDetail;
        }

        private static string GetRacebookName(int sportId)
        {
            var sportName = string.Empty;
            switch(sportId)
            {
                case Constants.Sport.HorseRacing:
                    sportName = ResourceExtension.GetResourceValue(
                            ResourceExtension.Resources.Language, "HR");
                    break;
                case Constants.Sport.Greyhound:
                    sportName = ResourceExtension.GetResourceValue(
                            ResourceExtension.Resources.Language, "Greyhound");
                    break;
                case Constants.Sport.Harness:
                    sportName = ResourceExtension.GetResourceValue(
                            ResourceExtension.Resources.Language, "Harness");
                    break;
                default:
                    break;
            }

            return sportName;
        }

        private static string GetToteName(int bettype)
        {
            var toteName = string.Empty;
            switch(bettype)
            {
                case (int)BettypeInfo.Bettype.SAToteWin:
                case (int)BettypeInfo.Bettype.SATotePlace:
                    toteName = ResourceExtension.GetResourceValue(
                            ResourceExtension.Resources.Language, "lblTote");
                    break;
                case (int)BettypeInfo.Bettype.UKToteWin:
                case (int)BettypeInfo.Bettype.UKTotePlace:
                    toteName = ResourceExtension.GetResourceValue(
                            ResourceExtension.Resources.Language, "lblOpenTote");
                    break;
                case (int)BettypeInfo.Bettype.AUToteWin:
                case (int)BettypeInfo.Bettype.AUTotePlace:
                    toteName = ResourceExtension.GetResourceValue(
                            ResourceExtension.Resources.Language, "lblAUTote");
                    break;
                default:
                    break;
            }
            return toteName;
        }

        private static string GetHorseBettypeName(int bettype)
        {
            switch (bettype)
            {
                case (int)BettypeInfo.Bettype.SAToteWin:
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWin");
                case (int)BettypeInfo.Bettype.UKToteWin:
                    return string.Format("{0}.{1}",
                                         ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                            "lblWin"),
                                         ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                            "lblOpenTote"));
                case (int)BettypeInfo.Bettype.AUToteWin:
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAUTote51");

                case (int)BettypeInfo.Bettype.SATotePlace:
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlace");
                case (int)BettypeInfo.Bettype.UKTotePlace:
                    return string.Format("{0}.{1}",
                                         ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                            "lblPlace"),
                                         ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                            "lblOpenTote"));
                case (int)BettypeInfo.Bettype.AUTotePlace:
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAUTote52");
                default:
                    return string.Empty;
            }
        }

        private static ViewPtDTO CasinoPositionTaking(string userName, int sportId, int bettypeId)
        {
            var result = new ViewPtDTO();
            try
            {
                var dtPt = new DataTable();

                #region get data

                switch (sportId)
                {
                    case Constants.Sport.Casino:
                        dtPt = bettypeId < 0
                                   ? SelectCustomer.GetCSPT(userName, bettypeId*(-1), 0)
                                   : SelectCustomer.GetCSPT(userName, 0, bettypeId);
                        break;
                    case Constants.Sport.BingoGame:
                        var customerService = BingoContainer.Current.Resolve<ICustomerService>();
                        dtPt = bettypeId < 0
                                   ? customerService.GetCustomerPt(userName, bettypeId*(-1), 0)
                                   : customerService.GetCustomerPt(userName, 0, bettypeId);
                        break;
                }

                #endregion

                #region assign data to ViewPtDTO

                if (dtPt != null && dtPt.Rows.Count > 0)
                {
                    result.IsCasino = true;
                    var drInfo = dtPt.Rows[dtPt.Rows.Count - 1];
                    var roleId = CastAide.AsInt32(drInfo["roleid"], 1);
                    result.CustomerInfo = new CustomerRelation {Upline = new List<CustomerInfo>()};
                    result.PotionTakingDetail = new List<PositionTakingDTO>();

                    #region assign Customer Info

                    #region agent

                    if (roleId < (int) CustInfo.Roles.Agent)
                    {
                        var agentInfo = new CustomerInfo()
                                            {
                                                CustId = CastAide.AsInt32(dtPt.Rows[2]["custid"].ToString()),
                                                UserName = dtPt.Rows[2]["username"].ToString(),
                                                RoleId = (int) CustInfo.Roles.Agent,
                                                RoleName = ResourceExtension.GetResourceValue(
                                                    ResourceExtension.Resources.Language, "Agent")
                                            };
                        result.CustomerInfo.Upline.Add(agentInfo);
                    }

                    #endregion

                    #region master

                    if (roleId < (int) CustInfo.Roles.Master)
                    {
                        var masterInfo = new CustomerInfo()
                                             {
                                                 CustId = CastAide.AsInt32(dtPt.Rows[1]["custid"].ToString()),
                                                 UserName = dtPt.Rows[1]["username"].ToString(),
                                                 RoleId = (int) CustInfo.Roles.Master,
                                                 RoleName = ResourceExtension.GetResourceValue(
                                                     ResourceExtension.Resources.Language, "Master")
                                             };
                        result.CustomerInfo.Upline.Add(masterInfo);
                    }

                    #endregion

                    #region super

                    if (roleId < (int) CustInfo.Roles.Super)
                    {
                        var superInfo = new CustomerInfo()
                                            {
                                                CustId = CastAide.AsInt32(dtPt.Rows[0]["custid"].ToString()),
                                                UserName = dtPt.Rows[0]["username"].ToString(),
                                                RoleId = (int) CustInfo.Roles.Super,
                                                RoleName = ResourceExtension.GetResourceValue(
                                                    ResourceExtension.Resources.Language, "Super")
                                            };
                        result.CustomerInfo.Upline.Add(superInfo);
                    }

                    #endregion

                    #region current user level

                    result.CustomerInfo.Customer = new CustomerInfo()
                                                       {
                                                           CustId = CastAide.AsInt32(drInfo["custid"].ToString()),
                                                           UserName = drInfo["username"].ToString(),
                                                           RoleId = (int) CustInfo.Roles.Member
                                                       };

                    #endregion

                    #endregion

                    #region assign PT

                    var ptCurrent = new PositionTakingDTO();
                    foreach (DataRow dr in dtPt.Rows)
                    {
                        int currentRoleId = CastAide.AsInt16(dr["roleid"].ToString());

                        #region super

                        if (currentRoleId == (int) CustInfo.Roles.Super)
                        {
                            ptCurrent.SportName = sportId == Constants.Sport.Casino ? "Casino" : "Bingo";
                            ptCurrent.SportType = dr["gamekind"].ToString();
                            ptCurrent.TypeName = dr["gametype"].ToString();

                            var superPt = new PositionTakingInfo()
                                              {
                                                  Super = NumberFormat.DecFormat(dr["sPT"], 2),
                                                  Master = NumberFormat.DecFormat(dr["mPT"], 2),
                                                  Agent = NumberFormat.DecFormat(dr["aPT"], 2),
                                                  MinPt = NumberFormat.DecFormat(dr["MinPT"], 2),
                                                  SuperAuto = NumberFormat.DecFormat(dr["AutoPT"], 2),
                                                  SuperIsAuto =
                                                      (CastAide.AsBoolean(dr["isAutoPT"], false))
                                                          ? " checked='checked'"
                                                          : string.Empty,
                                                  Com = IBCUtils.FormatHandler.FormatNumber4(dr["RollingRate"])
                                              };
                            ptCurrent.Super = superPt;
                        }

                        #endregion

                        #region master

                        if (currentRoleId == (int) CustInfo.Roles.Master)
                        {
                            var masterPt = new PositionTakingInfo()
                                               {
                                                   Super = NumberFormat.DecFormat(dr["sPT"], 2),
                                                   Master = NumberFormat.DecFormat(dr["mPT"], 2),
                                                   Agent = NumberFormat.DecFormat(dr["aPT"], 2),
                                                   MinPt = NumberFormat.DecFormat(dr["MinPT"], 2),
                                                   SuperAuto = NumberFormat.DecFormat(dr["AutoPT"], 2),
                                                   SuperIsAuto =
                                                       (CastAide.AsBoolean(dr["isAutoPT"], false))
                                                           ? " checked='checked'"
                                                           : string.Empty,
                                                   Com = IBCUtils.FormatHandler.FormatNumber4(dr["RollingRate"])
                                               };
                            ptCurrent.Master = masterPt;
                        }

                        #endregion

                        #region Agent

                        if (currentRoleId == (int) CustInfo.Roles.Agent)
                        {
                            var agentPt = new PositionTakingInfo()
                                              {
                                                  Super = NumberFormat.DecFormat(dr["sPT"], 2),
                                                  Master = NumberFormat.DecFormat(dr["mPT"], 2),
                                                  Agent = NumberFormat.DecFormat(dr["aPT"], 2),
                                                  MinPt = NumberFormat.DecFormat(dr["MinPT"], 2),
                                                  SuperAuto = NumberFormat.DecFormat(dr["AutoPT"], 2),
                                                  SuperIsAuto =
                                                      (CastAide.AsBoolean(dr["isAutoPT"], false))
                                                          ? " checked='checked'"
                                                          : string.Empty,
                                                  Com = IBCUtils.FormatHandler.FormatNumber4(dr["RollingRate"])
                                              };
                            ptCurrent.Agent = agentPt;
                        }

                        #endregion

                        #region member

                        if (currentRoleId == (int) CustInfo.Roles.Member)
                        {
                            var memberPt = new PositionTakingInfo()
                                               {
                                                   Super = NumberFormat.DecFormat(dr["sPT"], 2),
                                                   Master = NumberFormat.DecFormat(dr["mPT"], 2),
                                                   Agent = NumberFormat.DecFormat(dr["aPT"], 2),
                                                   MinPt = NumberFormat.DecFormat(dr["MinPT"], 2),
                                                   SuperAuto = NumberFormat.DecFormat(dr["AutoPT"], 2),
                                                   SuperIsAuto =
                                                       (CastAide.AsBoolean(dr["isAutoPT"], false))
                                                           ? " checked='checked'"
                                                           : string.Empty,
                                                   Com = IBCUtils.FormatHandler.FormatNumber4(dr["RollingRate"])
                                               };
                            ptCurrent.Member = memberPt;
                        }

                        #endregion

                        if (roleId != currentRoleId) continue;
                        result.PotionTakingDetail.Add(ptCurrent);
                        ptCurrent = new PositionTakingDTO();
                    }

                    #endregion
                }
                else
                {
                    result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                             "thereisnodata");
                }

                #endregion
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        private static ViewPtDTO LiveCasinoPositionTaking(string userName)
        {
            var result = new ViewPtDTO();
            try
            {
                #region get data
                var dtPt = SelectCustomer.GetLiveCasinoPt(userName);
                #endregion

                #region assign data to ViewPtDTO
                
                ISettingManager LiveCS = new SettingManager();

                if (dtPt != null && dtPt.ResultSet.Tables.Count > 1)
                {
                    var dtInfo = dtPt.ResultSet.Tables[0];
                    var masterDisableAutoPt = dtPt.ResultSet.Tables[1].Rows.Count > 0 ? CastAide.AsBoolean(dtPt.ResultSet.Tables[1].Rows[0]["DisableAutoMPT"], false) : false;
                    var data = new DataTable();
                    DataView dv = dtInfo.DefaultView;
                    dv.Sort = "roleid desc";
                    data = dv.ToTable();

                    result.IsCasino = false;
                    result.IsLiveCasino = true;
                    var drInfo = data.Rows[dtInfo.Rows.Count - 1];
                    var roleId = CastAide.AsInt32(drInfo["roleid"], 1);
                    result.CustomerInfo = new CustomerRelation { Upline = new List<CustomerInfo>() };
                    result.PotionTakingDetail = new List<PositionTakingDTO>();
                    //var masterDisableAutoPt =
                    #region assign Customer Info

                    #region agent

                    if (roleId < (int)CustInfo.Roles.Agent)
                    {
                        var agentInfo = new CustomerInfo()
                        {
                            CustId = CastAide.AsInt32(data.Rows[2]["custid"].ToString()),
                            UserName = data.Rows[2]["username"].ToString(),
                            RoleId = (int)CustInfo.Roles.Agent,
                            RoleName = ResourceExtension.GetResourceValue(
                                ResourceExtension.Resources.Language, "Agent")
                        };
                        result.CustomerInfo.Upline.Add(agentInfo);
                    }

                    #endregion

                    #region master

                    if (roleId < (int)CustInfo.Roles.Master)
                    {
                        var masterInfo = new CustomerInfo()
                        {
                            CustId = CastAide.AsInt32(data.Rows[1]["custid"].ToString()),
                            UserName = data.Rows[1]["username"].ToString(),
                            RoleId = (int)CustInfo.Roles.Master,
                            RoleName = ResourceExtension.GetResourceValue(
                                ResourceExtension.Resources.Language, "Master")
                        };
                        result.CustomerInfo.Upline.Add(masterInfo);
                    }

                    #endregion

                    #region super

                    if (roleId < (int)CustInfo.Roles.Super)
                    {
                        var superInfo = new CustomerInfo()
                        {
                            CustId = CastAide.AsInt32(data.Rows[0]["custid"].ToString()),
                            UserName = data.Rows[0]["username"].ToString(),
                            RoleId = (int)CustInfo.Roles.Super,
                            RoleName = ResourceExtension.GetResourceValue(
                                ResourceExtension.Resources.Language, "Super")
                        };
                        result.CustomerInfo.Upline.Add(superInfo);
                    }

                    #endregion

                    #region current user level

                    result.CustomerInfo.Customer = new CustomerInfo()
                    {
                        CustId = CastAide.AsInt32(drInfo["custid"].ToString()),
                        UserName = drInfo["username"].ToString(),
                        RoleId = (int)CustInfo.Roles.Member
                    };

                    #endregion

                    #endregion

                    #region assign PT

                    var ptCurrent = new PositionTakingDTO();
                    foreach (DataRow dr in data.Rows)
                    {
                        int currentRoleId = CastAide.AsInt16(dr["roleid"].ToString());
                        ptCurrent.PermistionEnableAutoMaster =
                            Permissions.IsEnabledAutoMaster(!masterDisableAutoPt);
                        ptCurrent.EnableAutoMaster = !WebSettings.disableAutoM;
                        ptCurrent.DisableFlagCss = (!ptCurrent.PermistionEnableAutoMaster ? "disableFlag" : "OpenFlag");
                        ptCurrent.DisableAutoCss = (!ptCurrent.PermistionEnableAutoMaster ? "disColorMPT" : "");

                        #region super

                        if (currentRoleId == (int)CustInfo.Roles.Super)
                        {
                            ptCurrent.SportName = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "liveCasinoTitle");
                            ptCurrent.SportType = string.Empty;
                            ptCurrent.TypeName = string.Empty;

                            var superPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "s")], 2),
                                Master = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "m")], 2),
                                Agent = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "a")], 2),
                                MinPt = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "min")], 2),
                                Com = IBCUtils.FormatHandler.FormatNumber4(dr["commission"])
                            };
                            ptCurrent.Super = superPt;
                        }

                        #endregion

                        #region master

                        if (currentRoleId == (int)CustInfo.Roles.Master)
                        {
                            var masterPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "s")], 2),
                                Master = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "m")], 2),
                                MinPt = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "min")], 2),
                                SuperAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "auto")], 2),
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isauto")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                Com = IBCUtils.FormatHandler.FormatNumber4(dr["commission"])
                            };
                            ptCurrent.Master = masterPt;
                        }

                        #endregion

                        #region Agent

                        if (currentRoleId == (int)CustInfo.Roles.Agent)
                        {
                            var agentPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "s")], 2),
                                Master = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "m")], 2),
                                Agent = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "a")], 2),
                                MinPt = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "min")], 2),
                                SuperAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "auto")], 2),
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isauto")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MasterAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "autom")], 2),
                                MasterIsAuto = (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isautom")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                Com = IBCUtils.FormatHandler.FormatNumber4(dr["commission"])
                            };
                            ptCurrent.Agent = agentPt;
                        }

                        #endregion

                        #region member

                        if (currentRoleId == (int)CustInfo.Roles.Member)
                        {
                            var memberPt = new PositionTakingInfo()
                            {
                                Super = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "s")], 2),
                                Master = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "m")], 2),
                                Agent = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "a")], 2),
                                SuperAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "auto")], 2),
                                SuperIsAuto =
                                    (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isauto")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                MasterAuto = NumberFormat.DecFormat(dr[string.Format(_lcsPt, "autom")], 2),
                                MasterIsAuto = (CastAide.AsBoolean(dr[string.Format(_lcsPt, "isautom")], false))
                                        ? " checked='checked'"
                                        : string.Empty,
                                Com = IBCUtils.FormatHandler.FormatNumber4(dr["commission"])
                            };
                            ptCurrent.Member = memberPt;
                        }

                        #endregion

                        if (roleId != currentRoleId) continue;
                        result.PotionTakingDetail.Add(ptCurrent);
                        ptCurrent = new PositionTakingDTO();
                    }

                    #endregion
                }
                else
                {
                    result.ErrorMessage = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language,
                                                                             "thereisnodata");
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        private static CustomerRelation GetCustomerRelation(DbResult data)
        {
            var customerRelation = new CustomerRelation { Upline = new List<CustomerInfo>() };
            #region assign Customer Info
            var drCustInfo = data.ResultSet.Tables[0].Rows[0];
            var roleId = CastAide.AsInt32(drCustInfo["roleid"], 1);

            #region agent
            if (roleId < (int)CustInfo.Roles.Agent)
            {
                var agentInfo = new CustomerInfo()
                {
                    CustId = CastAide.AsInt32(drCustInfo["AgID"].ToString()),
                    UserName = drCustInfo["Ag"].ToString(),
                    RoleId = (int)CustInfo.Roles.Agent,
                    RoleName = ResourceExtension.GetResourceValue(
                     ResourceExtension.Resources.Language, "Agent")
                };
                customerRelation.Upline.Add(agentInfo);
                
            }
            #endregion
            #region master
            if (roleId < (int)CustInfo.Roles.Master)
            {
                var masterInfo = new CustomerInfo()
                {
                    CustId = CastAide.AsInt32(drCustInfo["MAgID"].ToString()),
                    UserName = drCustInfo["MAg"].ToString(),
                    RoleId = (int)CustInfo.Roles.Master,
                    RoleName = ResourceExtension.GetResourceValue(
                                         ResourceExtension.Resources.Language, "Master")
                };
                customerRelation.Upline.Add(masterInfo);
            }
            #endregion
            #region super
            if (roleId < (int)CustInfo.Roles.Super)
            {
                var superInfo = new CustomerInfo()
                {
                    CustId = CastAide.AsInt32(drCustInfo["SAgID"].ToString()),
                    UserName = drCustInfo["SAg"].ToString(),
                    RoleId = (int)CustInfo.Roles.Super,
                    RoleName = ResourceExtension.GetResourceValue(
                                         ResourceExtension.Resources.Language, "Super")
                };
                customerRelation.Upline.Add(superInfo);
            }
            #endregion
            #region member
            customerRelation.Customer = new CustomerInfo()
            {
                CustId = CastAide.AsInt32(drCustInfo["custid"].ToString()),
                UserName = drCustInfo["username"].ToString(),
                RoleId = (int)CustInfo.Roles.Member
            };
            #endregion
            #endregion
            return customerRelation;
        }

        private static bool GetRacbookStatus(int sportId)
        {
            var status = false;
            
            var selectHorse =
                (from currentHorse in
                     RacingSection.Instance.Racings.Cast<RacingElement>().Where(
                         currentTote => currentTote.Id == sportId.ToString())
                 select currentHorse);
            if (selectHorse != null && selectHorse.Count() > 0)            
                status = selectHorse.ToList()[0].Active;
            
            return status;
        }

        private static bool GetToteStatus(int sportId, int toteId)
        {
            var status = false;

            var racingSetting = RacingSection.Instance;
            var selectHorse =
                (from currentHorse in
                     racingSetting.Racings.Cast<RacingElement>().Where(
                         currentHorse => currentHorse.Id == sportId.ToString())
                 select currentHorse);

            if (selectHorse== null || selectHorse.ToList()[0].Active == false)
                return false;

            var selectTote = (from currentTote in
                                          selectHorse.ToList()[0].Totes.Cast<ToteElement>().Where(
                                              currentTote => currentTote.Id == string.Format("{0}", toteId))
                                      select currentTote);
            if (selectTote != null && selectTote.Count() > 0)            
                status = selectTote.ToList()[0].Active;
            
            return status;
        }

        private static string GetCurrencyAllowForEachTote(int sportId, int toteId)
        {
            var allowlist = string.Empty;

            var racingSetting = RacingSection.Instance;
            var selectHorse =
                (from currentHorse in
                     racingSetting.Racings.Cast<RacingElement>().Where(
                         currentHorse => currentHorse.Id == sportId.ToString())
                 select currentHorse);

            if (selectHorse != null && selectHorse.Count() > 0)
            {
                var selectTote = (from currentTote in
                                      selectHorse.ToList()[0].Totes.Cast<ToteElement>().Where(
                                          currentTote => currentTote.Id == string.Format("{0}", toteId))
                                  select currentTote);

                if (selectTote.Count() > 0)                
                    allowlist = selectTote.ToList()[0].Currency;
                
            }
            return allowlist;
        }
        
        private static int GetParentToteId(int toteId)
        {
            var parentId = 0;
            switch (toteId)
            {
                case (int)BettypeInfo.Bettype.SAToteWin:
                case (int)BettypeInfo.Bettype.SATotePlace:
                    parentId = (int)BettypeInfo.Bettype.SAToteWin;
                    break;
                case (int)BettypeInfo.Bettype.UKToteWin:
                case (int)BettypeInfo.Bettype.UKTotePlace:
                    parentId = (int)BettypeInfo.Bettype.UKToteWin;
                    break;
                case (int)BettypeInfo.Bettype.AUToteWin:
                case (int)BettypeInfo.Bettype.AUTotePlace:
                    parentId = (int)BettypeInfo.Bettype.AUToteWin;
                    break;
                default:
                    break;
            }

            return parentId;
        }

        private static AllowViewSport GetSportGroupStatus(DataRow data, int sportId)
        {
            var result = new AllowViewSport();

            var isEnableSport = GetRacbookStatus(sportId);
            var currencyOfCustomer = data.Table.Columns.Contains(_currency) ? CastAide.AsInt32(data[_currency]) : 1;
            result.Name = GetRacebookName(sportId);
            result.Status = isEnableSport;

            if (isEnableSport)
            {
                result.AllowViewTotes = new List<AllowViewSport>();
                #region get racebook info from config
                var racingSetting = RacingSection.Instance;
                var selectHorse =
                    (from currentHorse in
                         racingSetting.Racings.Cast<RacingElement>().Where(
                             currentHorse => currentHorse.Id == sportId.ToString())
                     select currentHorse);

                if(selectHorse != null && selectHorse.Count() > 0)
                {
                    foreach (ToteElement toteElement in selectHorse.ToList()[0].Totes)
                    {
                        if (toteElement != null)
                        {
                            var selectTote = (from currentTote in
                                                  selectHorse.ToList()[0].Totes.Cast<ToteElement>().Where(
                                                      currentTote => currentTote.Id == string.Format("{0}", toteElement.Id))
                                              select currentTote);
                            if (selectTote != null && selectTote.Count() > 0)
                            {

                                #region check currency
                                var enableToteStatus = GetToteStatus(sportId, CastAide.AsInt32(toteElement.Id));
                                var currencyAllowForTote = GetCurrencyAllowForEachTote(sportId, GetParentToteId(CastAide.AsInt32(toteElement.Id)));
                                var currencyAllowViewPt = Racing.RacingManager.CheckAllowCurrency(currencyOfCustomer, currencyAllowForTote, enableToteStatus);                                
                                #endregion
                                if (enableToteStatus && currencyAllowViewPt)
                                {
                                    var currentToteInfo = new AllowViewSport
                                                              {
                                                                  Id = toteElement.Id,
                                                                  Name =
                                                                      GetToteName(CastAide.AsInt32(toteElement.Id)),
                                                                  Status = true
                                                              };

                                    result.AllowViewTotes.Add(currentToteInfo);
                                }
                            }
                        }
                    }
                    
                }
                
                #endregion
                
            }
            return result;
        }

        private static bool CheckBettypeSatusForNumberGame(int bettype, bool isLive) 
        {
            var status = false;
            var bettypes = NumberGameSection.Instance[NumberGameSection.NumberGameCollectionType.Bettype];
            var ball = new NumberGameElement();

            if (isLive)
                ball = bettypes[NumberGameSection.BallCollectionType.Liveball.ToString()];
            else
                ball = bettypes[NumberGameSection.BallCollectionType.Deadball.ToString()];

            if (ball != null)
            {
                var query = (from q in ball.Settings.Cast<NumberGameElement>().Where(q => q.Id == bettype && q.IsSetting) select q);
                if (query != null)
                    status = query.First().Active;
            }

            return status;
        }

        private static bool CheckNGBettypeStatusBySport(int bettype)
        {
            
            var status = false;
            try
            {
                #region private variables
                var bettypes = NumberGameSection.Instance[NumberGameSection.NumberGameCollectionType.Bettype];
                var deadball = bettypes[NumberGameSection.BallCollectionType.Deadball.ToString()];
                var liveball = bettypes[NumberGameSection.BallCollectionType.Liveball.ToString()];

                var deadQuery = (from q in deadball.Settings.Cast<NumberGameElement>().Where(q => q.Id == bettype && q.IsSetting) select q);
                var liveQuery = (from q in liveball.Settings.Cast<NumberGameElement>().Where(q => q.Id == bettype && q.IsSetting) select q);
                #endregion

                if (deadQuery != null && liveQuery != null 
                    && deadQuery.Count<NumberGameElement>() > 0 && liveQuery.Count<NumberGameElement>() > 0)
                {
                    if (deadQuery.First().Active || liveQuery.First().Active)
                        status = true;
                }
                else
                {
                    if (deadQuery != null && deadQuery.Count<NumberGameElement>()> 0)
                        status = deadQuery.First().Active;
                    else if (liveQuery != null && liveQuery.Count<NumberGameElement>() > 0)
                        status = liveQuery.First().Active;
                }
            }
            catch { }
            return status;
        }
        #endregion
    }
}
