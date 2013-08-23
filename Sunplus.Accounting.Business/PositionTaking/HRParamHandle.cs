using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using DNAide.Core;

using Sunplus.Accounting.Core.Configurations;



/// <summary>
/// Summary description for FinanceManager
/// </summary>
namespace Sunplus.Accounting.Business.PositionTaking
{
    public partial class HRPositionTakingManager
    {
        #region Add Params

        public static void AddParamsCopyPreset(HttpRequest r, List<int> bettype, ref Hashtable paramsHR)
        {
            try
            {
                int tmpbettype = 0;
                foreach (int t in bettype)
                {
                    if (WebSettings.DisableHRPT_OpenTote && !IsToteBettype(t))
                    {
                        tmpbettype = t - BetTypeInterval;
                        if (r[IS_COPY_PRESET + "_" + tmpbettype] != null)
                            paramsHR.Add(IS_COPY_PRESET + "_" + t, r[IS_COPY_PRESET + "_" + tmpbettype]);
                    }
                    else
                    {
                        if (r[IS_COPY_PRESET + "_" + t] != null)
                            paramsHR.Add(IS_COPY_PRESET + "_" + t, r[IS_COPY_PRESET + "_" + t]);
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void SuperAddParamsPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsHR)
        {
            try
            {
                int tmpbettype = 0;
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (WebSettings.DisableHRPT_OpenTote && !IsToteBettype(t))
                    {
                        tmpbettype = t - BetTypeInterval;
                        if (r[prefix + IBCPT + "_" + tmpbettype] != null)
                            paramsHR.Add(prefix + IBCPT.ToLower() + "_" + t, r[prefix + IBCPT + "_" + tmpbettype]);
                        if (r[prefix + SPT + "_" + tmpbettype] != null)
                            paramsHR.Add(prefix + SPT + "_" + t, r[prefix + SPT + "_" + tmpbettype]);
                        if (r[prefix + MPT + "_" + tmpbettype] != null)
                            paramsHR.Add(prefix + MPT + "_" + t, r[prefix + MPT + "_" + tmpbettype]);
                        if (r[prefix + APT + "_" + tmpbettype] != null)
                            paramsHR.Add(prefix + APT + "_" + t, r[prefix + APT + "_" + tmpbettype]);
                    }
                    else
                    {
                        if (r[prefix + IBCPT + "_" + t] != null)
                            paramsHR.Add(prefix + IBCPT.ToLower() + "_" + t, r[prefix + IBCPT + "_" + t]);
                        if (r[prefix + SPT + "_" + t] != null)
                            paramsHR.Add(prefix + SPT + "_" + t, r[prefix + SPT + "_" + t]);
                        if (r[prefix + MPT + "_" + t] != null)
                            paramsHR.Add(prefix + MPT + "_" + t, r[prefix + MPT + "_" + t]);
                        if (r[prefix + APT + "_" + t] != null)
                            paramsHR.Add(prefix + APT + "_" + t, r[prefix + APT + "_" + t]);
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void MasterAddParamsPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsHR)
        {
            try
            {
                int tmpbettype = 0;
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                string isauto = "";
                string isauto1 = "";
                foreach (int t in bettype)
                {
                    if (WebSettings.DisableHRPT_OpenTote && !IsToteBettype(t))
                    {
                        tmpbettype = t - BetTypeInterval;
                        if (r[prefix + SPT + "_" + tmpbettype] != null)
                            paramsHR.Add(prefix + SPT + "_" + t, r[prefix + SPT + "_" + tmpbettype]);
                        if (r[prefix + MPT + "_" + tmpbettype] != null)
                            paramsHR.Add(prefix + MPT + "_" + t, r[prefix + MPT + "_" + tmpbettype]);
                        if (r[Auto + "_" + prefix + SPT + "_" + tmpbettype] != null)
                            paramsHR.Add(Auto + "_" + prefix + SPT + "_" + t,
                                         r[Auto + "_" + prefix + SPT + "_" + tmpbettype]);
                        isauto = STR_IsAuto + "_" + prefix + SPT + "_" + t;
                        isauto1 = STR_IsAuto + "_" + prefix + SPT + "_" + tmpbettype;
                        if (r[isauto1] != null)
                        {
                            paramsHR.Add(isauto, r[isauto1]);
                            if (r[isauto1] == "true")
                            {
                                paramsHR[prefix + SPT + "_" + t] = 0;
                            }
                            else
                            {
                                paramsHR[Auto + "_" + prefix + SPT + "_" + t] = 0;
                            }
                        }
                    }
                    else
                    {
                        if (r[prefix + SPT + "_" + t] != null)
                            paramsHR.Add(prefix + SPT + "_" + t, r[prefix + SPT + "_" + t]);
                        if (r[prefix + MPT + "_" + t] != null)
                            paramsHR.Add(prefix + MPT + "_" + t, r[prefix + MPT + "_" + t]);
                        if (r[Auto + "_" + prefix + SPT + "_" + t] != null)
                            paramsHR.Add(Auto + "_" + prefix + SPT + "_" + t, r[Auto + "_" + prefix + SPT + "_" + t]);
                        isauto = STR_IsAuto + "_" + prefix + SPT + "_" + t;
                        if (r[isauto] != null)
                        {
                            paramsHR.Add(isauto, r[STR_IsAuto + "_" + prefix + SPT + "_" + t]);
                            if (r[isauto] == "true")
                            {
                                paramsHR[prefix + SPT + "_" + t] = 0;
                            }
                            else
                            {
                                paramsHR[Auto + "_" + prefix + SPT + "_" + t] = 0;
                            }
                        }
                    }
                }
                if (r["newspositiontaking"] != null) paramsHR.Add("newspositiontaking", r["newspositiontaking"]);
                if (r["updateallmembers"] != null)
                    paramsHR.Add("updateallmembers", CastAide.AsBoolean(CastAide.AsInt16(r["updateallmembers"])));
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AgentAddParamsPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsHR)
        {
            try
            {
                int tmpbettype = 0;
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (WebSettings.DisableHRPT_OpenTote && !IsToteBettype(t))
                    {
                        tmpbettype = t - BetTypeInterval;
                    }
                    else
                    {
                        tmpbettype = t;
                        //if (r[prefix + APT + "_" + t.ToString()] != null) paramsHR.Add(prefix + APT + "_" + t.ToString(), r[prefix + APT + "_" + t.ToString()]);
                        //if (r[prefix + MPT + "_" + t.ToString()] != null) paramsHR.Add(prefix + MPT + "_" + t.ToString(), r[prefix + MPT + "_" + t.ToString()]);
                        //if (r[Auto + "_" + prefix + MPT + "_" + t.ToString()] != null) paramsHR.Add(Auto + "_" + prefix + MPT + "_" + t.ToString(), r[Auto + "_" + prefix + MPT + "_" + t.ToString()]);
                        //if (r[STR_IsAuto + "_" + prefix + MPT + "_" + t.ToString()] != null) paramsHR.Add(STR_IsAuto + "_" + prefix + MPT + "_" + t.ToString(), r[STR_IsAuto + "_" + prefix + MPT + "_" + t.ToString()]);
                    }
                    if (r[prefix + APT + "_" + tmpbettype] != null)
                        paramsHR.Add(prefix + APT + "_" + t, r[prefix + APT + "_" + tmpbettype]);
                    if (r[prefix + MPT + "_" + tmpbettype] != null)
                        paramsHR.Add(prefix + MPT + "_" + t, r[prefix + MPT + "_" + tmpbettype]);
                    if (r[Auto + "_" + prefix + MPT + "_" + tmpbettype] != null)
                        paramsHR.Add(Auto + "_" + prefix + MPT + "_" + t,
                                     r[Auto + "_" + prefix + MPT + "_" + tmpbettype]);
                    //if (r[STR_IsAuto + "_" + prefix + MPT + "_" + tmpbettype.ToString()] != null) paramsHR.Add(STR_IsAuto + "_" + prefix + MPT + "_" + t.ToString(), r[STR_IsAuto + "_" + prefix + MPT + "_" + tmpbettype.ToString()]);
                    string isauto = STR_IsAuto + Delimiter + prefix + MPT + Delimiter + tmpbettype;

                    if (r[isauto] != null)
                    {
                        paramsHR.Add(STR_IsAuto + Delimiter + prefix + MPT + Delimiter + t, r[isauto]);
                        if (r[isauto] == "true")
                        {
                            paramsHR[prefix + MPT + Delimiter + t] = 0;
                        }
                        else
                        {
                            paramsHR[Auto + Delimiter + prefix + MPT + Delimiter + t] = 0;
                        }
                    }
                }
                if (r["newspositiontaking"] != null) paramsHR.Add("newmpositiontaking", r["newspositiontaking"]);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void MemberAddParamsPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsHR)
        {
            try
            {
                int tmpbettype = 0;
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (WebSettings.DisableHRPT_OpenTote && !IsToteBettype(t))
                    {
                        tmpbettype = t - BetTypeInterval;
                        if (r[prefix + APT + "_" + tmpbettype] != null)
                            paramsHR.Add(prefix + APT + "_" + t, r[prefix + APT + "_" + tmpbettype]);
                    }
                    else
                    {
                        if (r[prefix + APT + "_" + t] != null)
                            paramsHR.Add(prefix + APT + "_" + t, r[prefix + APT + "_" + t]);
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsMinMaxBet(HttpRequest r, int sporttype, ref Hashtable paramsHR)
        {
            try
            {
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                if (r[MINBET + prefix + "_31"] != null) paramsHR.Add(MINBET + "_31", r[MINBET + prefix + "_31"]);
                if (r[MAXBET + prefix + "_31"] != null) paramsHR.Add(MAXBET + "_31", r[MAXBET + prefix + "_31"]);
                if (r[MaxPerMatch + prefix + "_31"] != null)
                    paramsHR.Add(MaxPerMatch + "_31", r[MaxPerMatch + prefix + "_31"]);

                if (r[MINBET + prefix + "_41"] != null) paramsHR.Add(MINBET + "_41", r[MINBET + prefix + "_41"]);
                if (r[MAXBET + prefix + "_41"] != null) paramsHR.Add(MAXBET + "_41", r[MAXBET + prefix + "_41"]);
                if (r[MaxPerMatch + prefix + "_41"] != null)
                    paramsHR.Add(MaxPerMatch + "_41", r[MaxPerMatch + prefix + "_41"]);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsDisabled(HttpRequest r, ref Hashtable paramsHR)
        {
            try
            {
                if (r[ISDISABLED] != null) paramsHR.Add(ISDISABLED_Update, r[ISDISABLED]);
                if (r[ISDOWNLINEDISABLED] != null) paramsHR.Add(ISDOWNLINEDISABLED_Update, r[ISDOWNLINEDISABLED]);

                if (r[ISDISABLEDOpenTote] != null) paramsHR.Add(ISDISABLEDOpenTote_Update, r[ISDISABLEDOpenTote]);
                if (r[ISDOWNLINEDISABLEDOpenTote] != null)
                    paramsHR.Add(ISDOWNLINEDISABLEDOpenTote_Update, r[ISDOWNLINEDISABLEDOpenTote]);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsComm(HttpRequest r, int sporttype, ref Hashtable paramsHR)
        {
            try
            {
                string comm31 = (sporttype > 1) ? "v" + sporttype + DISCOUNT + "_31" : "";
                string comm41 = (sporttype > 1) ? "v" + sporttype + DISCOUNT + "_41" : "";
                if (r[comm31] != null) paramsHR.Add(comm31, r[comm31]);
                if (WebSettings.DisableHRPT_OpenTote)
                {
                    if (r[comm31] != null) paramsHR.Add(comm41, r[comm31]);
                }
                else
                {
                    if (r[comm41] != null) paramsHR.Add(comm41, r[comm41]);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsMinMaxPT(HttpRequest r, int sporttype, List<int> bettype, ref Hashtable paramsHR)
        {
            try
            {
                int tmpbettype = 0;
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (WebSettings.DisableHRPT_OpenTote && !IsToteBettype(t))
                    {
                        tmpbettype = t - BetTypeInterval;
                        if (r[prefix + MINPT + Delimiter + tmpbettype] != null)
                            paramsHR.Add(MINPT + prefix + PT + Delimiter + t, r[prefix + MINPT + Delimiter + tmpbettype]);
                        if (r[prefix + MAXPT + Delimiter + tmpbettype] != null)
                            paramsHR.Add(MAXPT + prefix + PT + Delimiter + t, r[prefix + MAXPT + Delimiter + tmpbettype]);
                    }
                    else
                    {
                        if (r[prefix + MINPT + Delimiter + t] != null)
                            paramsHR.Add(MINPT + prefix + PT + Delimiter + t, r[prefix + MINPT + Delimiter + t]);
                        if (r[prefix + MAXPT + Delimiter + t] != null)
                            paramsHR.Add(MAXPT + prefix + PT + Delimiter + t, r[prefix + MAXPT + Delimiter + t]);
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsPTMasterOnMember(HttpRequest r, int sporttype, List<int> bettype,
                                                     ref Hashtable paramsHR)
        {
            try
            {
                int tmpbettype = 0;
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (WebSettings.DisableHRPT_OpenTote && !IsToteBettype(t))
                    {
                        tmpbettype = t - BetTypeInterval;
                    }
                    else
                    {
                        tmpbettype = t;
                        //if (r[prefix + MPT + Delimiter + t.ToString()] != null) paramsHR.Add(prefix + MPT + Delimiter + t.ToString(), r[prefix + MPT + Delimiter + t.ToString()]);
                    }
                    if (r[prefix + MPT + Delimiter + tmpbettype] != null)
                        paramsHR.Add(prefix + MPT + Delimiter + t, r[prefix + MPT + Delimiter + tmpbettype]);

                    if (r[prefix + MPT + Delimiter + Auto + Delimiter + tmpbettype] != null)
                        paramsHR.Add(Auto + Delimiter + prefix + MPT + Delimiter + "mem" + Delimiter + t,
                                     r[prefix + MPT + Delimiter + Auto + Delimiter + tmpbettype]);
                    string isauto = STR_IsAuto + Delimiter + prefix + MPT + Delimiter + tmpbettype;

                    if (r[isauto] != null)
                    {
                        paramsHR.Add(STR_IsAuto + Delimiter + prefix + MPT + Delimiter + MEM + Delimiter + t, r[isauto]);
                        if (r[isauto] == "true")
                        {
                            paramsHR[prefix + MPT + Delimiter + t] = 0;
                        }
                        else
                        {
                            paramsHR[Auto + Delimiter + prefix + MPT + Delimiter + "mem" + Delimiter + t] = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static void AddParamsPTSuperOnMember(HttpRequest r, int sporttype, List<int> bettype,
                                                    ref Hashtable paramsHR)
        {
            try
            {
                int tmpbettype = 0;
                string prefix = "";
                prefix = (sporttype > 1) ? "v" + sporttype : "";
                foreach (int t in bettype)
                {
                    if (WebSettings.DisableHRPT_OpenTote && !IsToteBettype(t))
                    {
                        tmpbettype = t - BetTypeInterval;
                        if (r[prefix + SPT + Delimiter + tmpbettype] != null)
                            paramsHR.Add(prefix + SPT + Delimiter + t, r[prefix + SPT + Delimiter + tmpbettype]);
                        if (r[prefix + SPT + Delimiter + Auto + Delimiter + tmpbettype] != null)
                            paramsHR.Add(Auto + Delimiter + prefix + SPT + Delimiter + t,
                                         r[prefix + SPT + Delimiter + Auto + Delimiter + tmpbettype]);
                        if (r[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + tmpbettype] != null)
                            paramsHR.Add(STR_IsAuto + Delimiter + prefix + SPT + Delimiter + t,
                                         r[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + tmpbettype]);
                    }
                    else
                    {
                        if (r[prefix + SPT + Delimiter + t] != null)
                            paramsHR.Add(prefix + SPT + Delimiter + t, r[prefix + SPT + Delimiter + t]);
                        if (r[prefix + SPT + Delimiter + Auto + Delimiter + t] != null)
                            paramsHR.Add(Auto + Delimiter + prefix + SPT + Delimiter + t,
                                         r[prefix + SPT + Delimiter + Auto + Delimiter + t]);
                        if (r[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + t] != null)
                            paramsHR.Add(STR_IsAuto + Delimiter + prefix + SPT + Delimiter + t,
                                         r[STR_IsAuto + Delimiter + prefix + SPT + Delimiter + t]);
                    }
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        #endregion Add Params
    }
}