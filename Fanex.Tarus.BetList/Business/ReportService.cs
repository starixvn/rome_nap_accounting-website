namespace Fanex.Tarus.BetList.Business
{
    using Constants;
    using Core;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Tarus.Infrastructure.Caching;
    using Models;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public static class ReportService
    {
        public const string CacheIPVN = "Fanex.BetList.Accounting.Services.Ipvn";
        public const string BetTrans14SPSuffix = "_14";
        public const string WarehouseSPSuffix = "_WH";

        #region StaticProperties

        public static int BetlistMaxTotalRow =>
            string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Betlist:MaxTotalRow"]) ?
                    10000 : int.Parse(ConfigurationManager.AppSettings["Betlist:MaxTotalRow"]);

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
            => BetTypeContants.MinSpadeGamingBettype <= betTypeId && betTypeId <= BetTypeContants.MaxSpadeGamingBettype;

        public static bool IsVirtualSportBetType(int betTypeId)
        {
            return BetTypeContants.MinVirtualSportBetType <= betTypeId &&
                betTypeId <= BetTypeContants.MaxVirtualSportBetType;
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
            return (3101 <= betTypeId && betTypeId <= 3150);
        }

        public static bool IsGTCasino(int betTypeId)
        {
            return (3201 <= betTypeId && betTypeId <= 3299);
        }

        public static bool IsVoidbridge(int betTypeId)
        {
            return 4001 <= betTypeId && betTypeId <= 4100 ||
                5800 <= betTypeId && betTypeId <= 5999;
        }

        public static bool IsPP(int betTypeId)
        {
            return (4101 <= betTypeId && betTypeId <= 4300) ||
                (6701 <= betTypeId && betTypeId <= 7000);
        }
        public static bool IsVirtualSports3(int betTypeId)
            => 5302 <= betTypeId && betTypeId <= 5399;

        public static bool IsVirtualSports2(int betTypeId)
        {
            return 2700 <= betTypeId && betTypeId <= 2899;
        }

        public static bool IsVirtualSports4(int betTypeId)
            => 10400 <= betTypeId && betTypeId <= 10499;

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

        public static bool IsFGG(int betTypeId)
        {
            return 8202 <= betTypeId && betTypeId <= 8299;
        }

        public static bool IsSealEntertainment(int betTypeId)
        {
            return 8900 <= betTypeId && betTypeId <= 8999;
        }

        public static bool IsMGRNG(int betTypeId)
        {
            return 8400 <= betTypeId && betTypeId <= 8699;
        }

        public static bool IsJili(int betTypeId)
        {
            return 5100 <= betTypeId && betTypeId <= 5299;
        }

        public static bool IsYeeBet(int betTypeId)
        {
            return 3151 <= betTypeId && betTypeId <= 3200;
        }

        public static int GetSportFromBetTypeId(int betTypeId)
        {
            if (5601 <= betTypeId && betTypeId <= 5699)
                return SportContants.IonCasino;
            if (1021 <= betTypeId && betTypeId <= 1099)
                return SportContants.KenoLottery;
            if (6001 <= betTypeId && betTypeId <= 6300)
                return SportContants.PGSoft;
            if (3401 <= betTypeId && betTypeId <= 3499)
                return SportContants.WM;
            if (3501 <= betTypeId && betTypeId <= 3599)
                return SportContants.BigGame;
            if (3611 <= betTypeId && betTypeId <= 3620)
                return SportContants.SportsLottery;
            if (3801 <= betTypeId && betTypeId <= 3899)
                return SportContants.VGaming;
            if (6401 <= betTypeId && betTypeId <= 6700)
                return SportContants.Joker;            
            if (5401 <= betTypeId && betTypeId <= 5499)
                return SportContants.MaxGame;
            if (5500 <= betTypeId && betTypeId <= 5599)
                return SportContants.RNGWar;
            if (3701 <= betTypeId && betTypeId <= 3799)
                return SportContants.PPLiveCasino;
            if (7001 <= betTypeId && betTypeId <= 7300)
                return SportContants.Habanero;
            if (7301 <= betTypeId && betTypeId <= 7600)
                return SportContants.CG;
            if (7701 <= betTypeId && betTypeId <= 7799)
                return SportContants.AdvantPlay;
            if (7801 <= betTypeId && betTypeId <= 7899)
                return SportContants.AdvantPlayMini;
            if (8700 <= betTypeId && betTypeId <= 8709)
                return SportContants.PlayerTips;
            if (10300 <= betTypeId && betTypeId <= 10399)
                return SportContants.WE;
            if (7601 <= betTypeId && betTypeId <= 7700)
                return SportContants.Live22;
            if (10000 <= betTypeId && betTypeId <= 10299)
                return SportContants.UUSlots;
            if (10801 <= betTypeId && betTypeId <= 11000)
                return SportContants.SabaxD;
            if (11001 <= betTypeId && betTypeId <= 11019)
                return SportContants.SabaCoins;
            if (11100 <= betTypeId && betTypeId <= 11299)
                return SportContants.FastSpin;
            if (11300 <= betTypeId && betTypeId <= 11399)
                return SportContants.FACHAI;
            if (10450 <= betTypeId && betTypeId <= 10750)
                return SportContants.Nextspin;
            return 0;
        }


        #endregion StaticMethods
    }
}