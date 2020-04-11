namespace Fanex.Tarus.BetList.Business
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Core;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Tarus.Infrastructure.Caching;
    using Models;
    using Resources;

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
                    return BetListResources.EUR;
                case CurrencyContants.MYR:
                    return BetListResources.RM;
                case CurrencyContants.USD:
                    return BetListResources.USD;
                default:
                    return string.Empty;
            }
        }

        public static bool IsSpadegaming(int betTypeId)
            => BetTypeContants.FishingGod <= betTypeId && betTypeId <= BetTypeContants.TigerWarrior;

        public static bool IsVirtualSportBetType(int betTypeId)
        {
            return BetTypeContants.MinVirtualSportBetType <= betTypeId &&
                betTypeId <= BetTypeContants.MaxVirtualSportBetType;
        }

        public static bool IsGoldDeluxe(int betTypeId)
        {
            return betTypeId == BetTypeContants.GoldDeluxeBaccarat ||
                betTypeId == BetTypeContants.GoldDeluxeParlayBaccarat ||
                betTypeId == BetTypeContants.GoldDeluxeRoulette ||
                betTypeId == BetTypeContants.Sicbo ||
                betTypeId == BetTypeContants.Bidme;
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
            return (2301 <= betTypeId && betTypeId <= 2399) || (2901 <= betTypeId && betTypeId <= 2999);
        }

        public static bool IsAllBet(int betTypeId)
        {
            return (2401 <= betTypeId && betTypeId <= 2499);
        }

        public static bool IsSAGaming(int betTypeId)
        {
            return (3001 <= betTypeId && betTypeId <= 3099);
        }

        public static bool IsSexyGaming(int betTypeId)
        {
            return (3101 <= betTypeId && betTypeId <= 3199);
        }

        public static bool IsVoidbridge(int betTypeId)
        {
            return 4001 <= betTypeId && betTypeId <= 4100;
        }

        public static bool IsPP(int betTypeId)
        {
            return 4101 <= betTypeId && betTypeId <= 4300;
        }

        public static bool IsNG2NeedSuffix(int betTypeId)
        {
            return 2501 <= betTypeId && betTypeId <= 2699;
        }

        public static int GetNG2SportType(int betTypeId)
        {
            if (2501 <= betTypeId && betTypeId <= 2539)
            {
                return 213;
            }

            if (2541 <= betTypeId && betTypeId <= 2579)
            {
                return 214;
            }

            if (2581 <= betTypeId && betTypeId <= 2619)
            {
                return 215;
            }

            if (2621 <= betTypeId && betTypeId <= 2659)
            {
                return 216;
            }

            if (2661 <= betTypeId && betTypeId <= 2679)
            {
                return 217;
            }

            if (2681 <= betTypeId && betTypeId <= 2699)
            {
                return 218;
            }

            return 0;
        }

        public static bool IsVirtualSports2(int betTypeId)
        {
            return 2700 <= betTypeId && betTypeId <= 2899;
        }

        public static bool IsCardClub(int betTypeId)
        {
            return 1901 <= betTypeId && betTypeId <= 1999;
        }

        public static bool IsVirtualGames(int betTypeId)
        {
            return 4601 <= betTypeId && betTypeId <= 4900;
        }

        public static bool IsBBin(int betTypeId)
        {
            return 3300 == betTypeId || betTypeId == 21008 || betTypeId == 21009;
        }

        public static bool IsHappy5Bettype(int bettypeId)
            => bettypeId >= 8101 && bettypeId <= 8105;

        public static bool IsSabaclub(int betTypeId)
        {
            return 1611 <= betTypeId && betTypeId <= 1699;
        }

        public static bool IsTableGames(int betTypeId)
            => 4901 <= betTypeId && betTypeId <= 5000;

        #endregion StaticMethods
    }
}
