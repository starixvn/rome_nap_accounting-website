// --------------------------------------------------------------------------
// <copyright file="ScoreInformationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.ScoreInformation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using DNAide.Core;
    using Fanex.Tarus.BetList.Business.SportingSolutions;
    using Fanex.Tarus.BetList.Constants;
    using Fanex.Tarus.BetList.Criteria.ScoreInformation;
    using Fanex.Tarus.BetList.Criteria.SportingSolutions;
    using Fanex.Tarus.BetList.DataAccess.ScoreInformation;
    using Fanex.Tarus.BetList.Entities.ScoreInformation;
    using Fanex.Tarus.BetList.Resources;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Constants;
    using Sunplus.Accounting.Core.Extensions;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ScoreInformationService : IScoreInformationService
    {
        private readonly IScoreInformationDataAccess _repository;

        private readonly ISportingSolutionsService _sportingSolutionsService;

        public ScoreInformationService()
            : this(null, null)
        {
        }

        public ScoreInformationService(IScoreInformationDataAccess repository,
            ISportingSolutionsService sportingSolutionsService)
        {
            this._repository = repository ?? new ScoreInformationDataAccess();
            this._sportingSolutionsService = sportingSolutionsService ?? new SportingSolutionsService();
        }

        public ScoreInfo GetScoreInfo(GetScoreInformationCriteria criteria)
        {
            try
            {
                ScoreInfo result = null;
                switch (criteria.SportType)
                {
                    case Sports.Soccer:
                        result = this.GetSoccerInfo(criteria);
                        break;
                    case Sports.Basketball:
                    case Sports.Netball:
                    case Sports.Football:
                    case Sports.Rugby:
                        result = this.GetBasketballInfo(criteria);
                        break;
                    case Sports.IceHockey:
                    case Sports.FieldHockey:
                        result = this.GetHockeySportInfo(criteria);
                        break;
                    case Sports.Tennis:
                        result = this.GetTennisInfo(criteria);
                        break;
                    case Sports.Billard:
                    case Sports.Golf:
                    case Sports.Motorsports:
                    case Sports.WinterSports:
                    case Sports.Cycling:
                    case Sports.Fencing:
                    case Sports.Judo:
                    case Sports.ModernPentathlon:
                    case Sports.Rowing:
                    case Sports.Sailing:
                    case Sports.Shooting:
                    case Sports.Taekwondo:
                    case Sports.Triathlon:
                    case Sports.Wrestling:
                        result = this.GetBillardInfo(criteria);
                        break;
                    case Sports.Baseball:
                        result = this.GetBaseballInfo(criteria);
                        break;
                    case Sports.Badminton:
                    case Sports.Volleyball:
                    case Sports.Squash:
                        result = this.GetBadmintonInfo(criteria);
                        break;
                    case Sports.NumberGame:
                        result = this.GetNumberGameInfo(criteria);
                        break;
                    case Sports.VirtualSoccer:
                        result = this.GetVirtualSoccerInfo(criteria);
                        break;
                    default:
                        result = this.GetCommonSportInfo(criteria);
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        private TwoTimesMatchInfo GetSoccerInfo(GetScoreInformationCriteria criteria)
        {
            Hashtable outputParam = new Hashtable();
            var dataTable = this._repository.GetScoreInformation(criteria, ref outputParam);
            if (dataTable == null || dataTable.Count == 0)
            {
                return null;
            }

            var rawData = dataTable.First();
            DateTime eventDate = CastAide.AsDateTime(rawData.eventdate);
            TwoTimesMatchInfo result = new TwoTimesMatchInfo
            {
                HomeTeamId = CastAide.AsInt32(rawData.homeID),
                AwayTeamId = CastAide.AsInt32(rawData.awayID),
                IsNeutral = CastAide.AsBoolean(rawData.IsNeutral),

                HomeHTScore = CastAide.AsString(rawData.hthomescore),
                HomeFinalScore = CastAide.AsString(rawData.finalhomescore),
                AwayHTScore = CastAide.AsString(rawData.htawayscore),
                AwayFinalScore = CastAide.AsString(rawData.finalawayscore),
                EventStatus = CastAide.AsString(rawData.eventstatus),
                TraderName = CastAide.AsString(outputParam["tradername"]),
                EventTime = eventDate.ToString("MM/dd/yyyy"),
                IsPending = CastAide.AsBoolean(rawData.IsPending)
            };

            result.HomeTeamName = CacheRef.GetTeamName(result.HomeTeamId);
            result.AwayTeamName = CacheRef.GetTeamName(result.AwayTeamId);
            if (string.IsNullOrEmpty(result.TraderName))
            {
                result.TraderName = BetListResources.Unassigned;
            }

            string betStatus = CastAide.AsString(rawData.betstatus);
            bool isRefund = CastAide.AsBoolean(rawData.IsRefund);
            bool isCalRefund = CastAide.AsBoolean(rawData.IsCalRefund);

            if (!string.IsNullOrEmpty(betStatus))
            {
                result.CheckCancellation(betStatus);
                if (!isRefund &&
                    !isCalRefund)
                {
                    string[] arrBetStatus = betStatus.Split(':');
                    string ht1stScoreTeam = arrBetStatus[1].Split('-')[0];
                    string final1stScoreTeam = arrBetStatus[1].Split('-')[1];
                    switch (ht1stScoreTeam)
                    {
                        case BetListContants.SCORE_INFOMATION_HOME_TEAM_ID:
                            result.HT1stScoreTeam = result.HomeTeamId;
                            break;
                        case BetListContants.SCORE_INFOMATION_AWAY_TEAM_ID:
                            result.HT1stScoreTeam = result.AwayTeamId;
                            break;
                    }

                    switch (final1stScoreTeam)
                    {
                        case BetListContants.SCORE_INFOMATION_HOME_TEAM_ID:
                            result.Final1stScoreTeam = result.HomeTeamId;
                            break;
                        case BetListContants.SCORE_INFOMATION_AWAY_TEAM_ID:
                            result.Final1stScoreTeam = result.AwayTeamId;
                            break;
                    }
                }
            }

            return result;
        }

        private BasketballInfo GetBasketballInfo(GetScoreInformationCriteria criteria)
        {
            Hashtable outputParam = new Hashtable();
            var dataTable = this._repository.GetScoreInformation(criteria, ref outputParam);
            if (dataTable == null || dataTable.Count == 0)
            {
                return null;
            }

            var rawData = dataTable.First();
            DateTime eventDate = CastAide.AsDateTime(rawData.eventdate);
            BasketballInfo result = new BasketballInfo
            {
                HomeTeamId = CastAide.AsInt32(rawData.homeID),
                AwayTeamId = CastAide.AsInt32(rawData.awayID),
                IsNeutral = CastAide.AsBoolean(rawData.IsNeutral),

                HomeHTScore = CastAide.AsString(rawData.hthomescore),
                HomeFinalScore = CastAide.AsString(rawData.finalhomescore),
                AwayHTScore = CastAide.AsString(rawData.htawayscore),
                AwayFinalScore = CastAide.AsString(rawData.finalawayscore),
                EventStatus = CastAide.AsString(rawData.eventstatus),
                TraderName = CastAide.AsString(outputParam["tradername"]),
                EventTime = eventDate.ToString("MM/dd/yyyy"),
                IsPending = CastAide.AsBoolean(rawData.IsPending),

                HomeScores = new List<string>(),
                AwayScores = new List<string>()
            };

            result.HomeTeamName = CacheRef.GetTeamName(result.HomeTeamId);
            result.AwayTeamName = CacheRef.GetTeamName(result.AwayTeamId);
            if (string.IsNullOrEmpty(result.TraderName))
            {
                result.TraderName = BetListResources.Unassigned;
            }

            string betStatus = CastAide.AsString(rawData.betstatus);
            bool isRefund = CastAide.AsBoolean(rawData.IsRefund);
            bool isCalRefund = CastAide.AsBoolean(rawData.IsCalRefund);
            int i = 0;
            var numberOfSets = BetListContants.SCORE_INFOMATION_SETS_OF_BASKETBALL;
            if (!string.IsNullOrEmpty(betStatus))
            {
                result.CheckCancellation(betStatus);
                if (!isRefund &&
                    !isCalRefund)
                {
                    string[] arrBetStatus = betStatus.Split(',');
                    var statusLength = arrBetStatus.Length;
                    for (; i < statusLength && i < numberOfSets; i++)
                    {
                        var scoreSet = arrBetStatus[i].Trim().Split('-');
                        int homeSet = CastAide.AsInt32(scoreSet[0]);
                        int awaySet = CastAide.AsInt32(scoreSet[1]);
                        result.HomeScores.Add(homeSet.ToString());
                        result.AwayScores.Add(awaySet.ToString());
                    }
                }
            }

            for (; i < numberOfSets; i++)
            {
                result.HomeScores.Add(string.Empty);
                result.AwayScores.Add(string.Empty);
            }

            return result;
        }

        private BasketballInfo GetHockeySportInfo(GetScoreInformationCriteria criteria)
        {
            Hashtable outputParam = new Hashtable();
            var dataTable = this._repository.GetScoreInformation(criteria, ref outputParam);
            if (dataTable == null || dataTable.Count == 0)
            {
                return null;
            }

            var rawData = dataTable.First();
            DateTime eventDate = CastAide.AsDateTime(rawData.eventdate);
            BasketballInfo result = new BasketballInfo
            {
                HomeTeamId = CastAide.AsInt32(rawData.homeID),
                AwayTeamId = CastAide.AsInt32(rawData.awayID),
                IsNeutral = CastAide.AsBoolean(rawData.IsNeutral),

                HomeFinalScore = CastAide.AsString(rawData.finalhomescore),
                AwayFinalScore = CastAide.AsString(rawData.finalawayscore),
                EventStatus = CastAide.AsString(rawData.eventstatus),
                TraderName = CastAide.AsString(outputParam["tradername"]),
                EventTime = eventDate.ToString("MM/dd/yyyy"),
                IsPending = CastAide.AsBoolean(rawData.IsPending),

                HomeScores = new List<string>(),
                AwayScores = new List<string>()
            };

            result.HomeTeamName = CacheRef.GetTeamName(result.HomeTeamId);
            result.AwayTeamName = CacheRef.GetTeamName(result.AwayTeamId);
            if (string.IsNullOrEmpty(result.TraderName))
            {
                result.TraderName = BetListResources.Unassigned;
            }

            string betStatus = CastAide.AsString(rawData.betstatus);
            bool isRefund = CastAide.AsBoolean(rawData.IsRefund);
            bool isCalRefund = CastAide.AsBoolean(rawData.IsCalRefund);
            int i = 0;
            var numberOfSets = BetListContants.SCORE_INFOMATION_SETS_OF_HOCKEY_SPORT;
            if (!string.IsNullOrEmpty(betStatus))
            {
                result.CheckCancellation(betStatus);
                if (!isRefund &&
                    !isCalRefund)
                {
                    string[] arrBetStatus = betStatus.Split(',');
                    var statusLength = arrBetStatus.Length;
                    for (; i < statusLength && i < numberOfSets; i++)
                    {
                        var scoreSet = arrBetStatus[i].Trim().Split('-');
                        int homeSet = CastAide.AsInt32(scoreSet[0]);
                        int awaySet = CastAide.AsInt32(scoreSet[1]);
                        result.HomeScores.Add(homeSet.ToString());
                        result.AwayScores.Add(awaySet.ToString());
                    }
                }
            }

            for (; i < numberOfSets; i++)
            {
                result.HomeScores.Add(string.Empty);
                result.AwayScores.Add(string.Empty);
            }

            return result;
        }

        private TennisInfo GetTennisInfo(GetScoreInformationCriteria criteria)
        {
            Hashtable outputParam = new Hashtable();
            var dataTable = this._repository.GetScoreInformation(criteria, ref outputParam);
            if (dataTable == null || dataTable.Count == 0)
            {
                return null;
            }

            var rawData = dataTable.First();
            DateTime eventDate = CastAide.AsDateTime(rawData.eventdate);
            TennisInfo result = new TennisInfo
            {
                HomeTeamId = CastAide.AsInt32(rawData.homeID),
                AwayTeamId = CastAide.AsInt32(rawData.awayID),
                IsNeutral = CastAide.AsBoolean(rawData.IsNeutral),

                HomeFinalScore = CastAide.AsString(rawData.finalhomescore),
                AwayFinalScore = CastAide.AsString(rawData.finalawayscore),
                EventStatus = CastAide.AsString(rawData.eventstatus),
                TraderName = CastAide.AsString(outputParam["tradername"]),
                EventTime = eventDate.ToString("MM/dd/yyyy"),
                IsPending = CastAide.AsBoolean(rawData.IsPending),

                HomeScores = new List<string>(),
                AwayScores = new List<string>(),
                HomeTotalGamesScore_Str = string.Empty,
                AwayTotalGamesScore_Str = string.Empty
            };

            result.HomeTeamName = CacheRef.GetTeamName(result.HomeTeamId);
            result.AwayTeamName = CacheRef.GetTeamName(result.AwayTeamId);
            if (string.IsNullOrEmpty(result.TraderName))
            {
                result.TraderName = BetListResources.Unassigned;
            }

            string betStatus = CastAide.AsString(rawData.betstatus);
            bool isRefund = CastAide.AsBoolean(rawData.IsRefund);
            bool isCalRefund = CastAide.AsBoolean(rawData.IsCalRefund);
            int i = 0;
            var numberOfSets = BetListContants.SCORE_INFOMATION_SETS_OF_TENNIS;
            if (!string.IsNullOrEmpty(betStatus))
            {
                result.CheckCancellation(betStatus);
                if (!isRefund &&
                    !isCalRefund)
                {
                    string[] arrBetStatus = betStatus.Split(',');
                    var statusLength = arrBetStatus.Length;
                    for (; i < statusLength && i < numberOfSets; i++)
                    {
                        var scoreSet = arrBetStatus[i].Trim().Split('-');
                        int homeSet = CastAide.AsInt32(scoreSet[0]);
                        int awaySet = CastAide.AsInt32(scoreSet[1]);
                        result.HomeScores.Add(homeSet.ToString());
                        result.AwayScores.Add(awaySet.ToString());

                        result.HomeTotalGamesScore += homeSet;
                        result.AwayTotalGamesScore += awaySet;
                    }
                }
            }

            for (; i < numberOfSets; i++)
            {
                result.HomeScores.Add(string.Empty);
                result.AwayScores.Add(string.Empty);
            }

            if (result.HomeTotalGamesScore > 0 || result.AwayTotalGamesScore > 0)
            {
                result.HomeTotalGamesScore_Str = result.HomeTotalGamesScore.ToString();
                result.AwayTotalGamesScore_Str = result.AwayTotalGamesScore.ToString();
            }

            if (!string.IsNullOrEmpty(criteria.BetId) &&
                BetType.StartSetX_BetTypeRange <= criteria.Bettype &&
                criteria.Bettype <= BetType.EndSetX_BetTypeRange)
            {
                string betId = criteria.BetId;
                string setId = CastAide.AsInt32(betId.Substring(1, 2)).ToString();

                result.SetXInformation = this._sportingSolutionsService.Get_SetXContent(
                     new GetSportingSolutionsResultCriteria
                     {
                         Matchid = criteria.MatchID,
                         Resourceid = setId
                     });
            }

            return result;
        }

        private ScoreInfo GetBillardInfo(GetScoreInformationCriteria criteria)
        {
            Hashtable outputParam = new Hashtable();
            var dataTable = this._repository.GetScoreInformation(criteria, ref outputParam);
            if (dataTable == null || dataTable.Count == 0)
            {
                return null;
            }

            var rawData = dataTable.First();
            DateTime eventDate = CastAide.AsDateTime(rawData.eventdate);
            ScoreInfo result = new ScoreInfo
            {
                HomeTeamId = CastAide.AsInt32(rawData.homeID),
                AwayTeamId = CastAide.AsInt32(rawData.awayID),
                IsNeutral = CastAide.AsBoolean(rawData.IsNeutral),

                HomeFinalScore = CastAide.AsString(rawData.finalhomescore),
                AwayFinalScore = CastAide.AsString(rawData.finalawayscore),
                EventStatus = CastAide.AsString(rawData.eventstatus),
                TraderName = CastAide.AsString(outputParam["tradername"]),
                EventTime = eventDate.ToString("MM/dd/yyyy"),
                IsPending = CastAide.AsBoolean(rawData.IsPending)
            };

            result.HomeTeamName = CacheRef.GetTeamName(result.HomeTeamId);
            result.AwayTeamName = CacheRef.GetTeamName(result.AwayTeamId);
            if (string.IsNullOrEmpty(result.TraderName))
            {
                result.TraderName = BetListResources.Unassigned;
            }

            string betStatus = CastAide.AsString(rawData.betstatus);
            bool isRefund = CastAide.AsBoolean(rawData.IsRefund);
            bool isCalRefund = CastAide.AsBoolean(rawData.IsCalRefund);
            if (!string.IsNullOrEmpty(betStatus))
            {
                result.CheckCancellation(betStatus);
            }

            return result;
        }

        private BaseballInfo GetBaseballInfo(GetScoreInformationCriteria criteria)
        {
            Hashtable outputParam = new Hashtable();
            var dataTable = this._repository.GetScoreInformation(criteria, ref outputParam);
            if (dataTable == null || dataTable.Count == 0)
            {
                return null;
            }

            var rawData = dataTable.First();
            DateTime eventDate = CastAide.AsDateTime(rawData.eventdate);
            BaseballInfo result = new BaseballInfo
            {
                HomeTeamId = CastAide.AsInt32(rawData.homeID),
                AwayTeamId = CastAide.AsInt32(rawData.awayID),
                IsNeutral = CastAide.AsBoolean(rawData.IsNeutral),

                HomeHTScore = CastAide.AsString(rawData.hthomescore),
                HomeFinalScore = CastAide.AsString(rawData.finalhomescore),
                AwayHTScore = CastAide.AsString(rawData.htawayscore),
                AwayFinalScore = CastAide.AsString(rawData.finalawayscore),
                EventStatus = CastAide.AsString(rawData.eventstatus),
                TraderName = CastAide.AsString(outputParam["tradername"]),
                EventTime = eventDate.ToString("MM/dd/yyyy"),
                IsPending = CastAide.AsBoolean(rawData.IsPending),

                HomeScores = new List<string>(),
                AwayScores = new List<string>(),
                HomeEIScore = string.Empty,
                AwayEIScore = string.Empty
            };

            result.HomeTeamName = CacheRef.GetTeamName(result.HomeTeamId);
            result.AwayTeamName = CacheRef.GetTeamName(result.AwayTeamId);
            if (string.IsNullOrEmpty(result.TraderName))
            {
                result.TraderName = BetListResources.Unassigned;
            }

            string betStatus = CastAide.AsString(rawData.betstatus);
            bool isRefund = CastAide.AsBoolean(rawData.IsRefund);
            bool isCalRefund = CastAide.AsBoolean(rawData.IsCalRefund);
            int i = 0;
            var numberOfSets = BetListContants.SCORE_INFOMATION_SETS_OF_BASEBALL;
            if (!string.IsNullOrEmpty(betStatus))
            {
                result.CheckCancellation(betStatus);
                if (!isRefund &&
                    !isCalRefund)
                {
                    string[] arrBetStatus = betStatus.Split(',');
                    var statusLength = arrBetStatus.Length;
                    for (; i < statusLength && i < numberOfSets; i++)
                    {
                        var scoreSet = arrBetStatus[i].Trim().Split('-');
                        int homeSet = CastAide.AsInt32(scoreSet[0]);
                        int awaySet = CastAide.AsInt32(scoreSet[1]);
                        result.HomeScores.Add(homeSet.ToString());
                        result.AwayScores.Add(awaySet.ToString());
                    }

                    if (arrBetStatus.Length > numberOfSets)
                    {
                        var scoreSet = arrBetStatus[numberOfSets].Trim().Split('-');
                        int homeSet = CastAide.AsInt32(scoreSet[0]);
                        int awaySet = CastAide.AsInt32(scoreSet[1]);

                        result.HomeEIScore = homeSet.ToString();
                        result.AwayEIScore = awaySet.ToString();
                    }
                }
            }

            for (; i < numberOfSets; i++)
            {
                result.HomeScores.Add(string.Empty);
                result.AwayScores.Add(string.Empty);
            }

            return result;
        }

        private BadmintonInfo GetBadmintonInfo(GetScoreInformationCriteria criteria)
        {
            Hashtable outputParam = new Hashtable();
            var dataTable = this._repository.GetScoreInformation(criteria, ref outputParam);
            if (dataTable == null || dataTable.Count == 0)
            {
                return null;
            }

            var rawData = dataTable.First();
            DateTime eventDate = CastAide.AsDateTime(rawData.eventdate);
            BadmintonInfo result = new BadmintonInfo
            {
                HomeTeamId = CastAide.AsInt32(rawData.homeID),
                AwayTeamId = CastAide.AsInt32(rawData.awayID),
                IsNeutral = CastAide.AsBoolean(rawData.IsNeutral),

                HomeFinalScore = CastAide.AsString(rawData.finalhomescore),
                AwayFinalScore = CastAide.AsString(rawData.finalawayscore),
                EventStatus = CastAide.AsString(rawData.eventstatus),
                TraderName = CastAide.AsString(outputParam["tradername"]),
                EventTime = eventDate.ToString("MM/dd/yyyy"),
                IsPending = CastAide.AsBoolean(rawData.IsPending),

                HomeScores = new List<string>(),
                AwayScores = new List<string>(),
                HomeTotalGameScore_Str = string.Empty,
                AwayTotalGameScore_Str = string.Empty
            };

            result.HomeTeamName = CacheRef.GetTeamName(result.HomeTeamId);
            result.AwayTeamName = CacheRef.GetTeamName(result.AwayTeamId);
            if (string.IsNullOrEmpty(result.TraderName))
            {
                result.TraderName = BetListResources.Unassigned;
            }

            string betStatus = CastAide.AsString(rawData.betstatus);
            bool isRefund = CastAide.AsBoolean(rawData.IsRefund);
            bool isCalRefund = CastAide.AsBoolean(rawData.IsCalRefund);
            int i = 0;
            var numberOfSets = BetListContants.SCORE_INFOMATION_SETS_OF_BADMINTON;
            if (!string.IsNullOrEmpty(betStatus))
            {
                result.CheckCancellation(betStatus);
                if (!isRefund &&
                    !isCalRefund)
                {
                    string[] arrBetStatus = betStatus.Split(',');
                    var statusLength = arrBetStatus.Length;
                    for (; i < statusLength && i < numberOfSets; i++)
                    {
                        var scoreSet = arrBetStatus[i].Trim().Split('-');
                        int homeSet = CastAide.AsInt32(scoreSet[0]);
                        int awaySet = CastAide.AsInt32(scoreSet[1]);

                        result.HomeTotalGameScore += homeSet;
                        result.AwayTotalGameScore += awaySet;
                        result.HomeScores.Add(homeSet.ToString());
                        result.AwayScores.Add(awaySet.ToString());
                    }
                }
            }

            for (; i < numberOfSets; i++)
            {
                result.HomeScores.Add(string.Empty);
                result.AwayScores.Add(string.Empty);
            }

            if (result.HomeTotalGameScore > 0 || result.AwayTotalGameScore > 0)
            {
                result.HomeTotalGameScore_Str = result.HomeTotalGameScore.ToString();
                result.AwayTotalGameScore_Str = result.AwayTotalGameScore.ToString();
            }

            return result;
        }

        private TwoTimesMatchInfo GetCommonSportInfo(GetScoreInformationCriteria criteria)
        {
            Hashtable outputParam = new Hashtable();
            var dataTable = this._repository.GetScoreInformation(criteria, ref outputParam);
            if (dataTable == null || dataTable.Count == 0)
            {
                return null;
            }

            var rawData = dataTable.First();
            DateTime eventDate = CastAide.AsDateTime(rawData.eventdate);
            TwoTimesMatchInfo result = new TwoTimesMatchInfo
            {
                HomeTeamId = CastAide.AsInt32(rawData.homeID),
                AwayTeamId = CastAide.AsInt32(rawData.awayID),
                IsNeutral = CastAide.AsBoolean(rawData.IsNeutral),

                HomeHTScore = CastAide.AsString(rawData.hthomescore),
                HomeFinalScore = CastAide.AsString(rawData.finalhomescore),
                AwayHTScore = CastAide.AsString(rawData.htawayscore),
                AwayFinalScore = CastAide.AsString(rawData.finalawayscore),
                EventStatus = CastAide.AsString(rawData.eventstatus),
                TraderName = CastAide.AsString(outputParam["tradername"]),
                EventTime = eventDate.ToString("MM/dd/yyyy"),
                IsPending = CastAide.AsBoolean(rawData.IsPending)
            };

            result.HomeTeamName = CacheRef.GetTeamName(result.HomeTeamId);
            result.AwayTeamName = CacheRef.GetTeamName(result.AwayTeamId);
            if (string.IsNullOrEmpty(result.TraderName))
            {
                result.TraderName = BetListResources.Unassigned;
            }

            return result;
        }

        private TwoTimesMatchInfo GetNumberGameInfo(GetScoreInformationCriteria criteria)
        {
            return null;
        }

        private TwoTimesMatchInfo GetVirtualSoccerInfo(GetScoreInformationCriteria criteria)
        {
            Hashtable outputParam = new Hashtable();
            var dataTable = this._repository.GetScoreInformation(criteria, ref outputParam);
            if (dataTable == null || dataTable.Count == 0)
            {
                return null;
            }

            var rawData = dataTable.First();
            TwoTimesMatchInfo result = new TwoTimesMatchInfo
            {
                HomeTeamId = CastAide.AsInt32(rawData.homeID),
                AwayTeamId = CastAide.AsInt32(rawData.awayID),
                IsNeutral = CastAide.AsBoolean(rawData.IsNeutral),

                HomeHTScore = CastAide.AsString(rawData.hthomescore),
                HomeFinalScore = CastAide.AsString(rawData.finalhomescore),
                AwayHTScore = CastAide.AsString(rawData.htawayscore),
                AwayFinalScore = CastAide.AsString(rawData.finalawayscore),
                EventStatus = CastAide.AsString(rawData.eventstatus),
                TraderName = CastAide.AsString(outputParam["tradername"]),
                EventTime = CastAide.AsString(rawData.eventdate),
                IsPending = CastAide.AsBoolean(rawData.IsPending)
            };

            result.HomeTeamName = CacheRef.GetTeamName(result.HomeTeamId);
            result.AwayTeamName = CacheRef.GetTeamName(result.AwayTeamId);
            if (string.IsNullOrEmpty(result.TraderName))
            {
                result.TraderName = BetListResources.Unassigned;
            }

            string betStatus = CastAide.AsString(rawData.betstatus);
            bool isRefund = CastAide.AsBoolean(rawData.IsRefund);
            bool isCalRefund = CastAide.AsBoolean(rawData.IsCalRefund);

            if (!string.IsNullOrEmpty(betStatus))
            {
                result.CheckCancellation(betStatus);
                if (!isRefund &&
                    !isCalRefund)
                {
                    string[] arrBetStatus = betStatus.Split(':');
                    string ht1stScoreTeam = arrBetStatus[1].Split('-')[0];
                    string final1stScoreTeam = arrBetStatus[1].Split('-')[1];
                    switch (ht1stScoreTeam)
                    {
                        case BetListContants.SCORE_INFOMATION_HOME_TEAM_ID:
                            result.HT1stScoreTeam = result.HomeTeamId;
                            break;
                        case BetListContants.SCORE_INFOMATION_AWAY_TEAM_ID:
                            result.HT1stScoreTeam = result.AwayTeamId;
                            break;
                    }

                    switch (final1stScoreTeam)
                    {
                        case BetListContants.SCORE_INFOMATION_HOME_TEAM_ID:
                            result.Final1stScoreTeam = result.HomeTeamId;
                            break;
                        case BetListContants.SCORE_INFOMATION_AWAY_TEAM_ID:
                            result.Final1stScoreTeam = result.AwayTeamId;
                            break;
                    }
                }
            }

            return result;
        }
    }
}
