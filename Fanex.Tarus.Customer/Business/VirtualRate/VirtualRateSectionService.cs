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
        /// <summary>
        /// Gets the bet types type list.
        /// </summary>
        /// <returns>The bet types type list.</returns>
        public static List<BaseModel> BetTypeList()
        {
            var result = new List<BaseModel>();
            var virtualRateBetTypes = VirtualRateSection.Instance.BetTypes;
            foreach (VirtualRateElement betType in virtualRateBetTypes)
            {
                if (betType.Active)
                {
                    var betTypeModel = new BaseModel
                    {
                        Id = Converter.ToInt(betType.Id)
                    };

                    betTypeModel.Name = ShortBetTypeName(betTypeModel.Id);
                    result.Add(betTypeModel);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the bet types type index list.
        /// </summary>
        /// <returns>The bet types type index list.</returns>
        public static Dictionary<int, int> BetTypeIndexList()
        {
            var result = new Dictionary<int, int>();
            var virtualRateBetTypes = VirtualRateSection.Instance.BetTypes;
            var index = 0;
            foreach (VirtualRateElement betType in virtualRateBetTypes)
            {
                if (betType.Active)
                {
                    result.Add(Converter.ToInt(betType.Id), index);
                }

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
    }
}
