// --------------------------------------------------------------------------
// <copyright file="VirtualRateSectionService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.VirtualRate
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Configurations;
    using Fanex.Tarus.Infrastructure.Constants;

    /// <summary>
    /// Virtual Rate Section Service.
    /// </summary>
    public static class VirtualRateSectionService
    {
        static VirtualRateSectionService()
        {
            VirtualRateSports = VirtualRateSection.Instance.Sports;
            ValueRange = VirtualRateSection.Instance.ValueRange[0].ValueRange;
        }

        public const string UploadVirtualFunction = "Upload Virtual Rate";

        public static VirtualRateCollection VirtualRateSports { get; set; }

        public static List<int> ValueRange { get; private set; }

        /// <summary>
        /// Gets the sport list.
        /// </summary>
        /// <returns>The sport list.</returns>
        public static List<SportVirtualRateModel> SportList()
        {
            List<SportVirtualRateModel> result = new List<SportVirtualRateModel>();
            var sportLength = VirtualRateSports.Count;
            for (var i = 0; i < sportLength; i++)
            {
                var sport = VirtualRateSports[i];
                var sportId = sport.Id;
                var sportVirtualRateModel = new SportVirtualRateModel
                {
                    Id = sportId,
                    Name = Sports.SportName(sportId),
                    Notes = sport.Notes,
                    SubSportList = new List<SportVirtualRateModel>(),
                    BetTypeList = sport.BetTypeOrder.Split(',').Select(bettype => new BetTypeModel()
                    {
                        Id = Converter.ToInt(bettype)
                    }).ToList(),
                    BetTypeOrder = sport.BetTypeOrder
                };

                if (sport.SubSports != null && sport.SubSports.Count > 0)
                {
                    var groupLength = sport.SubSports.Count;
                    sportVirtualRateModel.SubSportList = new List<SportVirtualRateModel>();
                    for (var j = 0; j < groupLength; j++)
                    {
                        var sportGroup = sport.SubSports[j];
                        var subSportList = sportGroup.SportList.Split(new char[] { ',' })
                            .Select(s => new BaseModel
                            {
                                Id = Converter.ToInt(s),
                                Name = CacheRef.GetSportName(s)
                            })
                            .ToList();
                        var subSportLength = subSportList.Count;
                        for (var k = 0; k < subSportLength; k++)
                        {
                            var subSport = subSportList[k];
                            sportVirtualRateModel.SubSportList.Add(new SportVirtualRateModel
                            {
                                Id = subSport.Id,
                                Name = subSport.Name,
                                BetTypeList = new List<BetTypeModel>()
                            });
                        }
                    }
                }

                result.Add(sportVirtualRateModel);
            }

            return result;
        }

        /// <summary>
        /// Gets the sports dictionary list.
        /// </summary>
        /// <returns>The sports dictionary list.</returns>
        public static Dictionary<int, SportPositionModel> SportDictionaryList()
        {
            Dictionary<int, SportPositionModel> result = new Dictionary<int, SportPositionModel>();
            var sportLength = VirtualRateSports.Count;
            for (var i = 0; i < sportLength; i++)
            {
                var sport = VirtualRateSports[i];
                if (sport.SubSports != null)
                {
                    var subSportListLength = sport.SubSports.Count;
                    var subSportIndex = 0;

                    if (subSportListLength > 0)
                    {
                        for (var j = 0; j < subSportListLength; j++)
                        {
                            var sportGroup = sport.SubSports[j];
                            var subSportList = sportGroup.SportList.Split(new char[] { ',' })
                                .Select(s => new BaseModel
                                {
                                    Id = Converter.ToInt(s),
                                    Name = CacheRef.GetSportName(s)
                                })
                                .ToList();
                            var subSportLength = subSportList.Count;
                            for (var k = 0; k < subSportLength; k++)
                            {
                                var sportId = subSportList[k].Id;
                                if (!result.ContainsKey(sportId))
                                {
                                    result.Add(
                                        subSportList[k].Id,
                                        new SportPositionModel
                                        {
                                            SportIndex = i,
                                            IsSubSport = true,
                                            SubSportIndex = subSportIndex
                                        });
                                    subSportIndex++;
                                }
                            }
                        }
                    }
                    else
                    {
                        result.Add(
                            sport.Id,
                            new SportPositionModel
                            {
                                SportIndex = i
                            });
                    }
                }
            }

            return result;
        }

        public static Dictionary<int, int> BetTypeIndexListByFunctionName(string functionName)
        {
            Dictionary<int, int> betTypeIndexList = new Dictionary<int, int>();
            var virtualRateBetTypes = VirtualRateSection.Instance.Functions[functionName].BetTypes;
            var index = 0;
            foreach (VirtualRateElement betType in virtualRateBetTypes)
            {
                if (!betType.Active)
                {
                    continue;
                }

                if (!betTypeIndexList.ContainsKey(betType.Id))
                {
                    betTypeIndexList.Add(betType.Id, index);
                    index++;
                }

                if (!string.IsNullOrEmpty(betType.MultiBetTypes))
                {
                    var betTypeIdList = betType.MultiBetTypes.Split(new char[] { ',' });
                    foreach (var betTypeId in betTypeIdList)
                    {
                        var id = Converter.ToInt(betTypeId);
                        if (!betTypeIndexList.ContainsKey(id))
                        {
                            betTypeIndexList.Add(id, index);
                            index++;
                        }
                    }
                }
            }

            return betTypeIndexList;
        }

        public static List<VirtualRateModel> DefaultVirtualRateList(int betTypeCount)
        {
            List<VirtualRateModel> result = new List<VirtualRateModel>();
            for (var i = 0; i < betTypeCount; i++)
            {
                result.Add(new VirtualRateModel());
            }

            return result;
        }

        /// <summary>
        /// Gets the bet types type list by function name.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>
        /// The bet types type list by function name.
        /// </returns>
        public static List<BetTypeModel> BetTypeListByFunctionName(string functionName)
        {
            var result = new List<BetTypeModel>();
            var virtualRateBetTypes = VirtualRateSection.Instance.Functions[functionName].BetTypes;
            foreach (VirtualRateElement betType in virtualRateBetTypes)
            {
                if (!betType.Active)
                {
                    continue;
                }

                var betTypeModel = new BetTypeModel
                {
                    Id = Converter.ToInt(betType.Id)
                };

                betTypeModel.Name = ShortBetTypeName(betTypeModel.Id);
                if (!string.IsNullOrEmpty(betType.MultiBetTypes))
                {
                    var betTypeIdList = betType.MultiBetTypes.Split(new char[] { ',' });
                    betTypeModel.MultiBetTypes = betTypeIdList.Select(id => Converter.ToInt(id)).ToArray();
                    betTypeModel.MultiBetTypeNames = betTypeModel.MultiBetTypes.Select(id => ShortBetTypeName(id)).ToArray();
                }

                result.Add(betTypeModel);
            }

            return result;
        }

        /// <summary>
        /// Gets the short name of the bet type.
        /// </summary>
        /// <param name="betType">Type of the bet.</param>
        /// <returns>The short name of the bet type.</returns>
        public static string ShortBetTypeName(int betType)
        {
            switch (betType)
            {
                case BetType.Handicap:
                    return Customer_Labels.HDP;
                case BetType.OverUnder:
                    return Customer_Labels.OU;
                case BetType.OddEven:
                    return Customer_Labels.OE;
                case BetType.FirstHandicap:
                    return Customer_Labels._1stHDP;
                case BetType.FirstOverUnder:
                    return Customer_Labels._1stOU;
                case BetType.FirstOddEven:
                    return Customer_Labels._1stOE;
                case BetType.OneXTwo:
                    return Customer_Labels._1X2;
                default:
                    return CacheRef.GetBettype(betType);
            }
        }

        /// <summary>
        /// Gets the sport name key.
        /// </summary>
        /// <param name="sportId">The sport identifier.</param>
        /// <returns>The sport name key.</returns>
        public static string SportNameKey(int sportId)
        {
            switch (sportId)
            {
                case Sports.Soccer:
                    return "Soccer";
                case Sports.Tennis:
                    return "Tennis";
                case Sports.Basketball:
                    return "Basketball";
                case Sports.OtherSports:
                    return "Other Sports";
                default:
                    return CacheRef.GetSportName(sportId);
            }
        }

        /// <summary>
        /// Gets the dictionary of sport identifier from name .
        /// </summary>
        /// <returns>The dictionary of sport identifier from name .</returns>
        public static Dictionary<string, int> SportIdFromNameDictionary()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            var sportLength = VirtualRateSports.Count;
            for (var i = 0; i < sportLength; i++)
            {
                var sport = VirtualRateSports[i];
                var sportName = string.Empty;
                if (sport.SubSports != null && sport.SubSports.Count > 0)
                {
                    var groupLength = sport.SubSports.Count;
                    for (var j = 0; j < groupLength; j++)
                    {
                        var sportGroup = sport.SubSports[j];
                        var subSportList = sportGroup.SportList.Split(new char[] { ',' })
                            .Select(s => new BaseModel
                            {
                                Id = Converter.ToInt(s),
                                Name = CacheRef.GetSportName(s)
                            })
                            .ToList();
                        var subSportLength = subSportList.Count;
                        for (var k = 0; k < subSportLength; k++)
                        {
                            var subSport = subSportList[k];
                            sportName = CacheRef.GetSportName(subSport.Id);
                            if (!result.ContainsKey(sportName))
                            {
                                result.Add(sportName, subSport.Id);
                            }
                        }
                    }
                }
                else
                {
                    sportName = CacheRef.GetSportName(sport.Id);
                    if (!result.ContainsKey(sportName))
                    {
                        result.Add(sportName, sport.Id);
                    }
                }
            }

            if (result.ContainsKey("OtherSports"))
            {
                result.Add("Other Sports", result["OtherSports"]);
            }

            return result;
        }
    }
}
