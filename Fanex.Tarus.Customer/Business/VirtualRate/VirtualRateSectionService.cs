// --------------------------------------------------------------------------
// <copyright file="VirtualRateSectionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
        /// <summary>
        /// Gets the bet types type list.
        /// </summary>
        /// <param name="sportId">The sport identifier.</param>
        /// <returns>
        /// The bet types type list.
        /// </returns>
        public static List<BetTypeModel> BetTypeList(int sportId)
        {
            var result = new List<BetTypeModel>();
            var virtualRateBetTypes = VirtualRateSection.Instance.Sports[SportNameKey(sportId)].BetTypes;
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
        /// Gets the bet types type index list.
        /// </summary>
        /// <param name="sportId">The sport identifier.</param>
        /// <returns>
        /// The bet types type index list.
        /// </returns>
        public static Dictionary<int, int> BetTypeIndexList(int sportId)
        {
            var result = new Dictionary<int, int>();
            var virtualRateBetTypes = VirtualRateSection.Instance.Sports[SportNameKey(sportId)].BetTypes;
            var index = 0;
            foreach (VirtualRateElement betType in virtualRateBetTypes)
            {
                if (!betType.Active)
                {
                    continue;
                }

                result.Add(Converter.ToInt(betType.Id), index);
                index++;
            }

            return result;
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
        /// Gets the bet types type list for view log.
        /// </summary>
        /// <param name="sportId">The sport identifier.</param>
        /// <returns>
        /// The bet types type list for view log.
        /// </returns>
        public static List<BetTypeModel> BetTypeListViewLog(int sportId)
        {
            var result = new List<BetTypeModel>();
            var virtualRateBetTypes = VirtualRateSection.Instance.Sports[SportNameKey(sportId)].BetTypes;
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
                result.Add(betTypeModel);

                if (!string.IsNullOrEmpty(betType.MultiBetTypes))
                {
                    var betTypeIdList = betType.MultiBetTypes.Split(new char[] { ',' });
                    var betTypeIdLength = betTypeIdList.Length;
                    for (var i = 0; i < betTypeIdLength; i++)
                    {
                        var betTypeId = Converter.ToInt(betTypeIdList[i]);
                        result.Add(new BetTypeModel
                        {
                            Id = betTypeId,
                            Name = ShortBetTypeName(betTypeId)
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the bet types type index list for view log.
        /// </summary>
        /// <param name="sportId">The sport identifier.</param>
        /// <returns>
        /// The bet types type index list for view log.
        /// </returns>
        public static Dictionary<int, int> BetTypeIndexListViewLog(int sportId)
        {
            var result = new Dictionary<int, int>();
            var virtualRateBetTypes = VirtualRateSection.Instance.Sports[SportNameKey(sportId)].BetTypes;
            var index = 0;
            foreach (VirtualRateElement betType in virtualRateBetTypes)
            {
                if (!betType.Active)
                {
                    continue;
                }

                result.Add(Converter.ToInt(betType.Id), index);
                index++;

                if (!string.IsNullOrEmpty(betType.MultiBetTypes))
                {
                    var betTypeIdList = betType.MultiBetTypes.Split(new char[] { ',' });
                    var betTypeIdLength = betTypeIdList.Length;
                    for (var i = 0; i < betTypeIdLength; i++)
                    {
                        result.Add(Converter.ToInt(betTypeIdList[i]), index);
                        index++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the short name of the bet type.
        /// </summary>
        /// <param name="betType">Type of the bet.</param>
        /// <returns>The short name of the bet type.</returns>
        private static string ShortBetTypeName(int betType)
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
                default:
                    return CacheRef.GetBettype(betType);
            }
        }

        /// <summary>
        /// Gets the sport name key.
        /// </summary>
        /// <param name="sportId">The sport identifier.</param>
        /// <returns>The sport name key.</returns>
        private static string SportNameKey(int sportId)
        {
            switch (sportId)
            {
                case Sports.Soccer:
                    return "Soccer";
                case Sports.OtherSports:
                    return "Other Sports";
                default:
                    return string.Empty;
            }
        }
    }
}
