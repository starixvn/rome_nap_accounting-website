// --------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.BetList.Accounting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Accounting.Constants;
    using Fanex.BetList.Accounting.Core;
    using Fanex.BetList.Accounting.Models;
    using Fanex.BetList.Accounting.Resources;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Tarus.Infrastructure.Caching;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ReportService
    {
        public const string CacheIPVN = "Fanex.BetList.Accounting.Services.Ipvn";
        public const string BetTrans14SPSuffix = "_14";
        public const string WarehouseSPSuffix = "_WH";

        #region StaticProperties

        public static BalanceUp BalanceUp
        {
            get
            {
                BalanceUp balanceUp = null;
                try
                {
                    using (ObjectDb objDb = new ObjectDb("Core_ReportService_GetBalanceUpDay"))
                    {
                        balanceUp = objDb.QueryEntity<BalanceUp>();
                    }
                }
                catch (Exception ex)
                {
                    LoggingHandler.Log(ex);
                }

                return balanceUp;
            }
        }

        public static List<IPVNBean> IPVN
        {
            get
            {
                var ipVnList = CacheHelper.Get<List<IPVNBean>>(CacheIPVN);
                if (ipVnList == null)
                {
                    try
                    {
                        ipVnList = new List<IPVNBean>();
                        using (ObjectDb objDb = new ObjectDb("BetList_GetIPVN"))
                        {
                            using (var reader = objDb.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ipVnList.Add(new IPVNBean
                                    {
                                        IPFrom = reader.ToDouble("IPFrom"),
                                        IPTo = reader.ToDouble("IPTo")
                                    });
                                }
                            }
                        }

                        CacheHelper.Set<List<IPVNBean>>(CacheIPVN, ipVnList);
                    }
                    catch (Exception ex)
                    {
                        LoggingHandler.Log(ex);
                    }
                }

                return ipVnList;
            }
        }

        #endregion StaticProperties

        #region StaticMethods

        /// <summary>
        /// Gets the store mapping suffix.
        /// </summary>
        /// <param name="reportDate">The report date.</param>
        /// <returns>The store mapping suffix.</returns>
        public static string GetStoreMappingSuffix(DateTime reportDate)
        {
            if (reportDate > BalanceUp.MoveBetsDay)
            {
                return string.Empty;
            }
            else if (reportDate >= BalanceUp.BalanceUpDay)
            {
                return BetTrans14SPSuffix;
            }
            else
            {
                return WarehouseSPSuffix;
            }
        }

        /// <summary>
        /// Gets the name of the currency.
        /// </summary>
        /// <param name="currency">The currency.</param>
        /// <returns>The name of the currency.</returns>
        public static string GetCurrencyName(CurrencyContants currency)
        {
            switch (currency)
            {
                case CurrencyContants.EUR:
                    return Labels.EUR;
                case CurrencyContants.MYR:
                    return Labels.RM;
                case CurrencyContants.USD:
                    return Labels.USD;
                default:
                    return string.Empty;
            }
        }

        public static bool IsVirtualSportBetType(int betTypeId)
        {
            return BetTypeContants.MinVirtualSportBetType <= betTypeId &&
                betTypeId <= BetTypeContants.MaxVirtualSportBetType;
        }

        public static bool IsGoldDeluxe(int betTypeId)
        {
            return betTypeId == BetTypeContants.GoldDeluxeBaccarat ||
                betTypeId == BetTypeContants.GoldDeluxeParlayBaccarat ||
                betTypeId == BetTypeContants.GoldDeluxeRoulette;
        }

        public static bool IsMicroGaming(int betTypeId)
        {
            return BetTypeContants.MicroGamingBaccarat <= betTypeId &&
                betTypeId <= BetTypeContants.MicroGamingEndBetType;
        }

        public static bool IsAGCasino(int betTypeId)
        {
            return 1801 <= betTypeId && betTypeId <= 1804;
        }

        public static bool IsRNGGame_RNGCasino(int betTypeId)
        {
            return 2101 <= betTypeId && betTypeId <= 2199;
        }

        public static bool IsMiniGame_RNGCasino(int betTypeId)
        {
            return 2201 <= betTypeId && betTypeId <= 2299;
        }

        public static bool IsMobile_RNGCasino(int betTypeId)
        {
            return 2301 <= betTypeId && betTypeId <= 2399;
        }

        public static bool IsAllBet(int betTypeId)
        {
            return 2401 <= betTypeId && betTypeId <= 2499;
        }

        public static bool IsVoidbridge(int betTypeId)
        {
            return 4001 <= betTypeId && betTypeId <= 7999;
        }

        public static bool IsNG2NeedSuffix(int betTypeId)
        {
            var betTypes = new int[] { 2504, 2505, 2506, 2538, 2539, 2540, 2561, 2562, 2563, 2580, 2581, 2582, 2610, 2611, 2612, 2640, 2641, 2642, 2661, 2662, 2663, 2671, 2672, 2673 };
            return betTypes.Contains(betTypeId);
        }

        public static int GetNG2SportType(int betTypeId)
        {
            if (2501 <= betTypeId && betTypeId <= 2529)
            {
                return 213;
            }

            if (2531 <= betTypeId && betTypeId <= 2559)
            {
                return 214;
            }

            if (2561 <= betTypeId && betTypeId <= 2569)
            {
                return 215;
            }

            if (2571 <= betTypeId && betTypeId <= 2599)
            {
                return 216;
            }

            if (2601 <= betTypeId && betTypeId <= 2629)
            {
                return 217;
            }

            if (2631 <= betTypeId && betTypeId <= 2659)
            {
                return 217;
            }

            if (2661 <= betTypeId && betTypeId <= 2669)
            {
                return 219;
            }

            if (2671 <= betTypeId && betTypeId <= 2689)
            {
                return 220;
            }

            return 23;
        }
        #endregion StaticMethods
    }
}