using System;
using System.Collections;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.Entities.Customer;

/// <summary>
/// Summary description for Customer
/// </summary>
namespace Sunplus.Accounting.Business.Customer
{
    public class CustomerUtils
    {
        public CustomerUtils()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static string GetListSite(bool isAddNewSuper, string system)
        {
            var html = new StringBuilder();
            try
            {
                DbResult ds = SelectCustomer.SiteSelRpt(system);
                if (ds != null)
                {
                    DataTable tbl = ds.ResultSet.Tables[0];
                    if (isAddNewSuper)
                        html.Append("<option value=\"\">Select</option>");
                    bool enableDeposit = WebSettings.IsApplySplitingDepositeCustomer;
                    foreach (DataRow dr in tbl.Rows)
                    {
                        if (CastAide.AsBoolean(dr["isLicensee"]) && enableDeposit) continue;
                        html.Append("<option value=\"" + dr["site"] + "\">" + dr["site"] + "</option>");
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        /// <summary>
        /// Get currency list by System
        /// </summary>
        /// <param name="system"></param>
        /// <param name="checkHorseRacing"></param>
        /// <param name="isSearchFilter"></param>
        /// <param name="isAddNewSuper"></param>
        /// <returns></returns>
        public static string GetListCurrencyBySystem(string system, bool checkHorseRacing, bool isSearchFilter, bool isAddNewSuper)
        {
            var html = new StringBuilder();
            try
            {
                DbResult ds = SelectCustomer.ExchangeSelRpt(system);
                if (ds != null)
                {
                    DataTable tbl = ds.ResultSet.Tables[0];
                    EnumerableRowCollection<DataRow> currencies = tbl.AsEnumerable();

                    if (isSearchFilter)
                        html.Append("<option value=''>All</option>");
                    if (isAddNewSuper)
                        html.Append("<option value=''>Select</option>");
                    if (checkHorseRacing)
                    {
                        foreach (DataRow dr in currencies)
                        {
                            html.Append("<option value=\"" + dr["exchangeid"] + "_" + dr["HRAllowed"] + "\">" +
                                        dr["currency"] + "</option>");
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in currencies)
                        {
                            html.Append("<option value=\"" + dr["exchangeid"] + "\">" + dr["currency"] + "</option>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string GetListGroup(double maxValue, double currentVal)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                int groupValue = Convert.ToInt32(maxValue * 10000);
                if (currentVal > maxValue)
                {
                    html.Append("<option>" + currentVal + "</option>");
                }
                while (groupValue >= 0)
                {
                    double selValue = Convert.ToDouble(groupValue) / 10000;
                    if (selValue == currentVal)
                    {
                        html.Append("<option selected='true'>" + selValue + "</option>");
                    }
                    else
                        html.Append("<option>" + selValue + "</option>");
                    groupValue = groupValue - 5;
                }
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string GetListGroup(double maxValue)
        {
            return GetListGroup(maxValue, 0);
        }

        //phuong.nguyen
        //Overload with interval parameter
        public static string GetListGroup(double maxValue, double currentVal, int interval)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                int groupValue = Convert.ToInt32(maxValue * 10000);
                if (currentVal > maxValue)
                {
                    html.Append("<option>" + currentVal + "</option>");
                }
                while (groupValue >= 0)
                {
                    double selValue = Convert.ToDouble(groupValue) / 10000;
                    if (selValue == currentVal)
                    {
                        html.Append("<option selected='true'>" + selValue + "</option>");
                    }
                    else
                        html.Append("<option>" + selValue + "</option>");
                    groupValue = groupValue - interval;
                }
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string GetListOtherComm(double maxValue, double currentVal)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                //int groupValue = Convert.ToInt32(maxValue * 100000);
                int groupValue = Convert.ToInt32(maxValue * 10000);
                if (currentVal > maxValue)
                {
                    html.Append("<option>" + currentVal + "</option>");
                }
                while (groupValue >= 0)
                {
                    //double selValue = Convert.ToDouble(groupValue) / 100000;
                    double selValue = Convert.ToDouble(groupValue) / 10000;
                    if (currentVal == selValue)
                        html.Append("<option selected='true'>" + selValue + "</option>");
                    else
                        html.Append("<option>" + selValue + "</option>");
                    //groupValue = groupValue - 25;
                    groupValue = groupValue - 5;
                }
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string GetListOtherComm(double maxValue, double currentVal, int interval)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                //int groupValue = Convert.ToInt32(maxValue * 100000);
                int groupValue = Convert.ToInt32(maxValue * 10000);
                if (currentVal > maxValue)
                {
                    html.Append("<option>" + currentVal + "</option>");
                }
                while (groupValue >= 0)
                {
                    //double selValue = Convert.ToDouble(groupValue) / 100000;
                    double selValue = Convert.ToDouble(groupValue) / 10000;
                    if (currentVal == selValue)
                        html.Append("<option selected='true'>" + selValue + "</option>");
                    else
                        html.Append("<option>" + selValue + "</option>");
                    //groupValue = groupValue - 25;
                    groupValue = groupValue - interval;
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return html.ToString();
        }

        public static string GetListOtherComm(double maxValue)
        {
            return GetListOtherComm(maxValue, 0);
        }

        public static string GetListPositionTaking(double fromValue, double toValue)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                int intToValue = Convert.ToInt32(toValue * 100);
                int intFromValue = Convert.ToInt32(fromValue * 100);
                while (intToValue >= intFromValue)
                {
                    double ptValue = Convert.ToDouble(intToValue) / 100;
                    html.Append("<option value=\"" + ptValue + "\">" + ptValue + "</option>");
                    intToValue = intToValue - 1;
                }
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string GetListPositionTaking(double fromValue, double toValue, double currentValue)
        {
            StringBuilder html = new StringBuilder();
            string content = string.Empty;
            try
            {
                bool isInRange = false;
                int intToValue = Convert.ToInt32(toValue * 100);
                int intFromValue = Convert.ToInt32(fromValue * 100);
                while (intToValue >= intFromValue)
                {
                    double ptValue = Convert.ToDouble(intToValue) / 100;
                    if (ptValue == currentValue)
                    {
                        isInRange = true;
                        html.Append("<option selected=\"true\" value=\"" + ptValue + "\">" + ptValue + "</option>");
                    }
                    else
                    {
                        html.Append("<option value=\"" + ptValue + "\">" + ptValue + "</option>");
                    }

                    intToValue = intToValue - 1;
                }
                if (!isInRange)
                {
                    content += "<option selected=\"true\" value=\"" + currentValue + "\">" + currentValue + "</option>";
                }
                content += html.ToString();
            }
            catch (Exception ex)
            {
            }
            return content;
        }

        /// <summary>
        /// Header of SoccerPT (agent->acc)
        /// </summary>
        /// <param name="isMember"></param>
        /// <returns></returns>
        public static string LoadSoccerPTHeader(bool isMember)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                #region
                /*
            string content = WebUtils.LoadTemplate("Customer/SoccerPositionTakingHeader.htm");
            TemplateHandler.Assign(ref content, "{handicap}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Handicap"));
            TemplateHandler.Assign(ref content, "{overunder}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OverUnder"));
            TemplateHandler.Assign(ref content, "{1sthdp}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "FirstHdp"));
            TemplateHandler.Assign(ref content, "{1stou}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "FirstOU"));
            TemplateHandler.Assign(ref content, "{oddeven}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OddEven"));
            TemplateHandler.Assign(ref content, "{1x2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "str1x2"));
            TemplateHandler.Assign(ref content, "{correctscore}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CorrectScore"));
            TemplateHandler.Assign(ref content, "{totalgoal}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TotalGoal"));
            TemplateHandler.Assign(ref content, "{mixparlay}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MixParlay"));
            TemplateHandler.Assign(ref content, "{outright}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Outright"));
            if (isMember)//showAutoPT
            {
                TemplateHandler.Assign(ref content, "{autopresetposition}", "Auto Super Preset Position");
                TemplateHandler.Assign(ref content, "{auto-none}", "");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref content, "super-auto-pt");
            }
           // html.Append(content);
            */
                #endregion

                string strHtml = WebUtils.LoadTemplate("Customer/agent/SBettypeHeader.htm");
                int col = 2;
                if (isMember) col = 1;
                TemplateHandler.Assign(ref strHtml, "{col}", col);
                TemplateHandler.Assign(ref strHtml, "{hdp-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "HandicapNextGoal"));
                TemplateHandler.Assign(ref strHtml, "{ou-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OverUnder"));
                TemplateHandler.Assign(ref strHtml, "{1sthdp-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "FirstHdp"));
                TemplateHandler.Assign(ref strHtml, "{1stou-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "FirstOU"));
                TemplateHandler.Assign(ref strHtml, "{oe-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OddEven"));
                TemplateHandler.Assign(ref strHtml, "{1x2-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "str1x2"));
                TemplateHandler.Assign(ref strHtml, "{cs-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CorrectScore"));
                TemplateHandler.Assign(ref strHtml, "{tg-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TotalGoal"));
                TemplateHandler.Assign(ref strHtml, "{mp-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MixParlay"));
                TemplateHandler.Assign(ref strHtml, "{or-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Outright"));
                html.Append(strHtml);
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string LoadSoccerPTHeader_v2(bool showAutoPT, bool editMixMaxPT, string sporttype, DataRow dr, string prefix)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string content = WebUtils.LoadTemplate("Customer/v2/SoccerPositionTakingHeader.htm");
                string BetSetting = WebUtils.LoadTemplate("Customer/v2/BetSetting.htm");

                TemplateHandler.Assign(ref content, "{handicap}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "HandicapNextGoal"));
                TemplateHandler.Assign(ref content, "{overunder}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OverUnder"));
                TemplateHandler.Assign(ref content, "{1sthdp}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "FirstHdp"));
                TemplateHandler.Assign(ref content, "{1stou}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "FirstOU"));
                TemplateHandler.Assign(ref content, "{oddeven}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OddEven"));
                TemplateHandler.Assign(ref content, "{1x2}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "str1x2"));
                TemplateHandler.Assign(ref content, "{correctscore}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CorrectScore"));
                TemplateHandler.Assign(ref content, "{totalgoal}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TotalGoal"));
                TemplateHandler.Assign(ref content, "{mixparlay}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MixParlay"));
                TemplateHandler.Assign(ref content, "{outright}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Outright"));
                TemplateHandler.Assign(ref content, "{PositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));

                TemplateHandler.Assign(ref content, "{cols}", (prefix == "s") ? "1" : "2");

                if (editMixMaxPT)
                {
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    TemplateHandler.RemoveBlock(ref content, "min-max-pt");
                    TemplateHandler.Assign(ref content, "{width}", (prefix == "s") ? "950" : "100%");
                    TemplateHandler.Assign(ref content, "{pwidth}", (prefix == "s") ? "" : "15%");
                }
                else
                {
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));
                    TemplateHandler.Assign(ref content, "{BetSetting}", AddBetSetting(BetSetting, sporttype, dr));
                    TemplateHandler.Assign(ref content, "{width}", "950");
                    TemplateHandler.Assign(ref content, "{pwidth}", "15%");
                }

                if (showAutoPT)
                {
                    TemplateHandler.Assign(ref content, "{autopresetposition}", "Auto Super Preset Position");
                    TemplateHandler.Assign(ref content, "{auto-none}", "");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref content, "super-auto-pt");
                }
                html.Append(content);
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string LoadBasketballPTHeader_v2(bool showAutoPT, bool editMixMaxPT, string sporttype, DataRow dr, string prefix)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string content = WebUtils.LoadTemplate("Customer/v2/BasketballPositionTakingHeader.htm");
                string BetSetting = WebUtils.LoadTemplate("Customer/v2/BetSetting.htm");

                TemplateHandler.Assign(ref content, "{handicap}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Handicap"));
                TemplateHandler.Assign(ref content, "{overunder}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OverUnder"));
                TemplateHandler.Assign(ref content, "{oddeven}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OddEven"));
                TemplateHandler.Assign(ref content, "{moneyline}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMoneyLine"));
                TemplateHandler.Assign(ref content, "{mixparlay}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MixParlay"));
                TemplateHandler.Assign(ref content, "{BetSetting}", AddBetSetting(BetSetting, sporttype, dr));
                TemplateHandler.Assign(ref content, "{PositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));

                //Kiet Mai - remove mixpalay when sporttype = 2 (basketball)
                /*if (sporttype == "2") //baskball
                {
                    TemplateHandler.Assign(ref content, "{cols}", "7");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref content, "no-mixparlay");
                    TemplateHandler.Assign(ref content, "{cols}", "6");
                }*/
                TemplateHandler.RemoveBlock(ref content, "no-mixparlay");
                TemplateHandler.Assign(ref content, "{cols}", "6");

                if (editMixMaxPT)
                {
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    TemplateHandler.RemoveBlock(ref content, "min-max-pt");
                    if (prefix == "s")
                    {
                        TemplateHandler.RemoveBlock(ref content, "super-min");
                        TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                    }
                    else TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                }
                else
                {
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));
                    TemplateHandler.Assign(ref content, "{BetSetting}", AddBetSetting(BetSetting, sporttype, dr));
                    TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                }

                if (showAutoPT)
                {
                    TemplateHandler.Assign(ref content, "{autopresetposition}", "Auto Super Preset Position");
                    TemplateHandler.Assign(ref content, "{auto-none}", "");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref content, "super-auto-pt");
                }
                html.Append(content);
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string LoadBaseballPTHeader_v2(bool showAutoPT, bool editMixMaxPT, string sporttype, DataRow dr, string prefix)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string content = WebUtils.LoadTemplate("Customer/v2/BaseballPositionTakingHeader.htm");
                string BetSetting = WebUtils.LoadTemplate("Customer/v2/BetSetting.htm");

                TemplateHandler.Assign(ref BetSetting, "{sporttype}", sporttype);

                TemplateHandler.Assign(ref content, "{handicap}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Handicap"));
                TemplateHandler.Assign(ref content, "{overunder}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OverUnder"));
                TemplateHandler.Assign(ref content, "{moneyline}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMoneyLine"));
                TemplateHandler.Assign(ref content, "{BetSetting}", AddBetSetting(BetSetting, sporttype, dr));

                TemplateHandler.Assign(ref content, "{PositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));

                if (editMixMaxPT)
                {
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    TemplateHandler.RemoveBlock(ref content, "min-max-pt");
                    if (prefix == "s")
                    {
                        TemplateHandler.RemoveBlock(ref content, "super-min");
                        TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                    }
                    else TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                }
                else
                {
                    TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));
                }

                if (showAutoPT)
                {
                    TemplateHandler.Assign(ref content, "{autopresetposition}", "Auto Super Preset Position");
                    TemplateHandler.Assign(ref content, "{auto-none}", "");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref content, "super-auto-pt");
                }
                html.Append(content);
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string LoadFinalcialPTHeader_v2(bool showAutoPT, bool editMixMaxPT, string sporttype, DataRow dr, string prefix)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string content = WebUtils.LoadTemplate("Customer/v2/FinalcialPositionTakingHeader.htm");
                string BetSetting = WebUtils.LoadTemplate("Customer/v2/BetSetting.htm");

                TemplateHandler.Assign(ref BetSetting, "{sporttype}", sporttype);

                TemplateHandler.Assign(ref content, "{overunder}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OverUnder"));
                TemplateHandler.Assign(ref content, "{oddeven}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OddEven"));
                TemplateHandler.Assign(ref content, "{BetSetting}", AddBetSetting(BetSetting, sporttype, dr));

                TemplateHandler.Assign(ref content, "{PositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));

                if (editMixMaxPT)
                {
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    TemplateHandler.RemoveBlock(ref content, "min-max-pt");
                    if (prefix == "s")
                    {
                        TemplateHandler.RemoveBlock(ref content, "super-min");
                        TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                    }
                    else TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                }
                else
                {
                    TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));
                }

                if (showAutoPT)
                {
                    TemplateHandler.Assign(ref content, "{autopresetposition}", "Auto Super Preset Position");
                    TemplateHandler.Assign(ref content, "{auto-none}", "");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref content, "super-auto-pt");
                }
                html.Append(content);
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string LoadHorseRacingPTHeader_v2(bool showAutoPT, bool editMixMaxPT, string sporttype, DataRow dr, string prefix)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string content = WebUtils.LoadTemplate("Customer/v2/HorseRacingPositionTakingHeader.htm");
                string BetSetting = WebUtils.LoadTemplate("Customer/v2/BetSetting.htm");

                TemplateHandler.Assign(ref BetSetting, "{sporttype}", sporttype);

                TemplateHandler.Assign(ref content, "{overunder}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OverUnder"));
                TemplateHandler.Assign(ref content, "{BetSetting}", AddBetSetting(BetSetting, sporttype, dr));

                if (editMixMaxPT)
                {
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    TemplateHandler.RemoveBlock(ref content, "min-max-pt");
                    if (prefix == "s")
                    {
                        TemplateHandler.RemoveBlock(ref content, "super-min");
                        TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                    }
                    else TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                }
                else
                {
                    TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));
                }
                html.Append(content);
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string LoadMotosportsPTHeader_v2(bool showAutoPT, bool editMixMaxPT, string sporttype, DataRow dr, string prefix)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string content = WebUtils.LoadTemplate("Customer/v2/MotosportsPositionTakingHeader.htm");
                string BetSetting = WebUtils.LoadTemplate("Customer/v2/BetSetting.htm");

                TemplateHandler.Assign(ref content, "{handicap}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Handicap"));
                TemplateHandler.Assign(ref content, "{moneyline}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMoneyLine"));
                TemplateHandler.Assign(ref content, "{BetSetting}", AddBetSetting(BetSetting, sporttype, dr));

                TemplateHandler.Assign(ref content, "{PositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));

                if (editMixMaxPT)
                {
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "PositionTaking"));
                    TemplateHandler.RemoveBlock(ref content, "min-max-pt");
                    if (prefix == "s")
                    {
                        TemplateHandler.RemoveBlock(ref content, "super-min");
                        TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                    }
                    else TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                }
                else
                {
                    TemplateHandler.Assign(ref content, "{wPT}", "wPT70");
                    TemplateHandler.Assign(ref content, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));
                    TemplateHandler.Assign(ref content, "{BetSetting}", AddBetSetting(BetSetting, sporttype, dr));
                }

                if (showAutoPT)
                {
                    TemplateHandler.Assign(ref content, "{autopresetposition}", "Auto Super Preset Position");
                    TemplateHandler.Assign(ref content, "{auto-none}", "");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref content, "super-auto-pt");
                }
                html.Append(content);
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static string AddBetSetting(string tpl, string sporttype, DataRow dr)
        {
            try
            {
                TemplateHandler.Assign(ref tpl, "{sporttype}", sporttype);
                TemplateHandler.Assign(ref tpl, "{minbet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMinBet"));
                TemplateHandler.Assign(ref tpl, "{maxbet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxBet"));
                TemplateHandler.Assign(ref tpl, "{maxpermatch-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMaxPerMatch"));
                TemplateHandler.Assign(ref tpl, "{bet-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBetSetting"));

                double minbet = 0, maxbet = 0, maxpermatch = 0;
                if (dr != null)
                {
                    if (sporttype == "1")
                    {
                        minbet = FormatHandler.GetDouble(dr["minbet"]);
                        maxbet = FormatHandler.GetDouble(dr["maxbet"]);
                        maxpermatch = FormatHandler.GetDouble(dr["maxpermatch"]);
                    }
                    else
                    {
                        minbet = FormatHandler.GetDouble(dr["minbetv" + sporttype]);
                        maxbet = FormatHandler.GetDouble(dr["maxbetv" + sporttype]);
                        maxpermatch = FormatHandler.GetDouble(dr["maxpermatch" + sporttype]);
                    }
                }

                TemplateHandler.Assign(ref tpl, "{minbet" + sporttype + "-val}", FormatHandler.FormatNumber1(minbet));
                TemplateHandler.Assign(ref tpl, "{maxbet" + sporttype + "-val}", FormatHandler.FormatNumber1(maxbet));
                TemplateHandler.Assign(ref tpl, "{maxpermatch" + sporttype + "-val}", FormatHandler.FormatNumber1(maxpermatch));

                TemplateHandler.Assign(ref tpl, "{mbMin}", "hide");
                TemplateHandler.Assign(ref tpl, "{mbMax}", "hide");
                TemplateHandler.Assign(ref tpl, "{mpmMin}", "hide");
                TemplateHandler.Assign(ref tpl, "{mpmMax}", "hide");
                if (sporttype == "201")
                {
                    tpl += HtmlHelper.GenHidden("hiddenMinbet201", FormatHandler.FormatNumber1(minbet));
                    tpl += HtmlHelper.GenHidden("hiddenMaxbet201", FormatHandler.FormatNumber1(maxbet));
                    tpl += HtmlHelper.GenHidden("hiddenMaxpermatch201", FormatHandler.FormatNumber1(maxpermatch));
                }
                return tpl;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string LoadBasketballPTHeader()
        {
            StringBuilder html = new StringBuilder();
            try
            {
                string content = WebUtils.LoadTemplate("Customer/BasketballPositionTakingHeader.htm");
                TemplateHandler.Assign(ref content, "{handicap}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Handicap"));
                TemplateHandler.Assign(ref content, "{overunder}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OverUnder"));
                TemplateHandler.Assign(ref content, "{oddeven}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OddEven"));
                html.Append(content);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return html.ToString();
        }

        public static void LoadTitleCommon(ref string content)
        {
            try
            {
                TemplateHandler.Assign(ref content, "{username}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UserName"));
                TemplateHandler.Assign(ref content, "{password}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Password"));
                TemplateHandler.Assign(ref content, "{firstname}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "FirstName"));
                TemplateHandler.Assign(ref content, "{lastname}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "LastName"));
                TemplateHandler.Assign(ref content, "{phone}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Phone"));
                TemplateHandler.Assign(ref content, "{mobile}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Mobile"));
                TemplateHandler.Assign(ref content, "{commission}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission"));
                TemplateHandler.Assign(ref content, "{groupa}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupA"));
                TemplateHandler.Assign(ref content, "{soccer}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Soccer"));
                TemplateHandler.Assign(ref content, "{basketball}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MinBet"));
                TemplateHandler.Assign(ref content, "{minbet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MinBet"));
                TemplateHandler.Assign(ref content, "{minbetb}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MinBetb"));
                TemplateHandler.Assign(ref content, "{maxbet}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MaxBet"));
                TemplateHandler.Assign(ref content, "{maxbetb}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MaxBetb"));
                TemplateHandler.Assign(ref content, "{maxpetmatch}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MaxPerMatch"));
                TemplateHandler.Assign(ref content, "{maxpetmatchb}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MaxPerMatchb"));
                TemplateHandler.Assign(ref content, "{soccerPositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "SoccerPositionTaking"));
                TemplateHandler.Assign(ref content, "{groupb}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupB"));
                TemplateHandler.Assign(ref content, "{groupc}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupC"));
                TemplateHandler.Assign(ref content, "{groupd}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupD"));
                TemplateHandler.Assign(ref content, "{othercomm}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OtherComm"));
                TemplateHandler.Assign(ref content, "{basketballPositionTaking}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "BasketballPositionTaking"));
            }
            catch (Exception ex)
            {
            }
        }

        public static string AssignMultipleHidden(Hashtable p)
        {
            StringBuilder html = new StringBuilder();
            string strHiddenTpl = WebUtils.LoadTemplate("Hidden.htm");
            try
            {
                IDictionaryEnumerator Enumulator = p.GetEnumerator();
                while (Enumulator.MoveNext())
                {
                    string content = strHiddenTpl;
                    TemplateHandler.Assign(ref content, "{name}", Enumulator.Key.ToString());
                    TemplateHandler.Assign(ref content, "{strValue}", Enumulator.Value.ToString());
                    html.Append(content + "\n");
                }
            }
            catch (Exception ex)
            {
            }
            return html.ToString();
        }

        public static void DisplayLimitMinMaxBet(ref string strContent, DataTable tbl)
        {
            try
            {
                TemplateHandler.Assign(ref strContent, "{vminbetmax}", FormatHandler.FormatNumber1(tbl.Rows[0]["minbet_max"]));
                if (FormatHandler.GetDecimal(tbl.Rows[0]["minbet_max"]) == 0)
                {
                    TemplateHandler.Assign(ref strContent, "{m-max-none}", "none");
                }
                TemplateHandler.Assign(ref strContent, "{vminbetmaxb}", FormatHandler.FormatNumber1(tbl.Rows[0]["minbetb_max"]));
                if (FormatHandler.GetDecimal(tbl.Rows[0]["minbetb_max"]) == 0)
                {
                    TemplateHandler.Assign(ref strContent, "{m-maxb-none}", "none");
                }
                TemplateHandler.Assign(ref strContent, "{vmaxbetmin}", FormatHandler.FormatNumber1(tbl.Rows[0]["maxbet_min"]));
                if (FormatHandler.GetDecimal(tbl.Rows[0]["maxbet_min"]) == 0)
                {
                    TemplateHandler.Assign(ref strContent, "{m-min-none}", "none");
                }
                TemplateHandler.Assign(ref strContent, "{vmaxbetminb}", FormatHandler.FormatNumber1(tbl.Rows[0]["maxbetb_min"]));
                if (FormatHandler.GetDecimal(tbl.Rows[0]["maxbetb_min"]) == 0)
                {
                    TemplateHandler.Assign(ref strContent, "{m-minb-none}", "none");
                }
                TemplateHandler.Assign(ref strContent, "{vmaxpermin}", FormatHandler.FormatNumber1(tbl.Rows[0]["maxpermatch_min"]));
                if (FormatHandler.GetDecimal(tbl.Rows[0]["maxpermatch_min"]) == 0)
                {
                    TemplateHandler.Assign(ref strContent, "{per-min-none}", "none");
                }
                TemplateHandler.Assign(ref strContent, "{vmaxperminb}", FormatHandler.FormatNumber1(tbl.Rows[0]["maxpermatchb_min"]));
                if (FormatHandler.GetDecimal(tbl.Rows[0]["maxpermatchb_min"]) == 0)
                {
                    TemplateHandler.Assign(ref strContent, "{per-minb-none}", "none");
                }

                TemplateHandler.Assign(ref strContent, "{vmaxpermax}", FormatHandler.FormatNumber1(tbl.Rows[0]["maxpermatch_max"]));
                if (FormatHandler.GetDecimal(tbl.Rows[0]["maxpermatch_max"]) == 0)
                {
                    TemplateHandler.Assign(ref strContent, "{per-max-none}", "none");
                }
                TemplateHandler.Assign(ref strContent, "{vmaxpermaxb}", FormatHandler.FormatNumber1(tbl.Rows[0]["maxpermatchb_max"]));
                if (FormatHandler.GetDecimal(tbl.Rows[0]["maxpermatchb_max"]) == 0)
                {
                    TemplateHandler.Assign(ref strContent, "{per-maxb-none}", "none");
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// This function used for
        /// </summary>
        /// <param name="tpl"></param>
        /// <param name="isEditMin"></param>
        public static void GetSportTitle(ref string tpl, bool isEditMin)
        {
            TemplateHandler.Assign(ref tpl, "{soccer-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSoccer"));
            TemplateHandler.Assign(ref tpl, "{basket-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBasketball"));
            TemplateHandler.Assign(ref tpl, "{football-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFootball"));
            TemplateHandler.Assign(ref tpl, "{tennis-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTennis"));
            TemplateHandler.Assign(ref tpl, "{baseball-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblBaseball"));
            TemplateHandler.Assign(ref tpl, "{golf-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblGolf"));
            TemplateHandler.Assign(ref tpl, "{moto-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMotosports"));
            //mixed sport
            TemplateHandler.Assign(ref tpl, "{MixedSports-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMixedSports"));

            TemplateHandler.Assign(ref tpl, "{othersports-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOthersports"));
            if (isEditMin)
            {
                TemplateHandler.Assign(ref tpl, "{copy-all-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyAllPTfromHdpMin"));
                TemplateHandler.Assign(ref tpl, "{copy-basket-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyBasketHdpMin"));
            }
            else
            {
                TemplateHandler.Assign(ref tpl, "{copy-all-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyAllPTfromHdp"));
                TemplateHandler.Assign(ref tpl, "{copy-basket-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblCopyBasketHdp"));
            }

            TemplateHandler.Assign(ref tpl, "{financial-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFinancial"));
            TemplateHandler.Assign(ref tpl, "{horseracing-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblHorseRacing"));
        }

        public static void GetCredit(DataRow dr, ref string tpl, bool isEdit)
        {
            if (isEdit)
            {
                TemplateHandler.Assign(ref tpl, "{credit-val}", FormatHandler.FormatNumber1(dr["credit"]));
                TemplateHandler.Assign(ref tpl, "{mpmMin}", "minmaxbet");
                TemplateHandler.Assign(ref tpl, "{credit-min}", FormatHandler.FormatNumber1(dr["Credit_Min"]));
            }
            else
            {
                TemplateHandler.Assign(ref tpl, "{credit-val}", "0");
                TemplateHandler.Assign(ref tpl, "{mpmMin}", "hidden");
            }

            TemplateHandler.Assign(ref tpl, "{mpmMax}", "minmaxbet");
            TemplateHandler.Assign(ref tpl, "{credit-max}", FormatHandler.FormatNumber1(dr["Credit_Max"]));
        }

        public static string[] GetListNumber(string currentVal, bool isMember)
        {
            string[] result = { "", "" };
            try
            {
                StringBuilder htmlNo1 = new StringBuilder();
                StringBuilder htmlNo2 = new StringBuilder();
                StringBuilder htmlNo3 = new StringBuilder();
                //''ascii 48-->57, 65-->90, 97-->122
                string no1 = currentVal.Substring(0, 1).ToUpper();
                string no2 = currentVal.Substring(1, 1).ToUpper();
                string no3 = no2;
                string strNextNo1 = no1;
                string strNextNo2 = "";
                string strNextNo3 = "";
                if (isMember)
                {
                    no3 = currentVal.Substring(2, 1).ToUpper();
                    strNextNo2 = no2;
                    result = new string[] { "", "", "" };
                }
                string[] arr = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                                   "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                                   "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                                   "U", "V", "W", "X", "Y", "Z"};
                int index = 0;
                if (isMember)
                {
                    if (no3 == arr[arr.Length - 1])
                    {
                        strNextNo3 = arr[0];
                        if (no2 == arr[arr.Length - 1])
                        {
                            strNextNo2 = arr[0];
                            for (int i = 0; i < arr.Length; i++)
                            {
                                if (no1 == arr[i])
                                {
                                    index = i;
                                    break;
                                }
                            }
                            //index = arr.IndexOf(arr, no1);
                            strNextNo1 = arr[index + 1];
                        }
                        else
                        {
                            //index = arr.IndexOf(arr, no2);
                            for (int i = 0; i < arr.Length; i++)
                            {
                                if (no2 == arr[i])
                                {
                                    index = i;
                                    break;
                                }
                            }
                            strNextNo2 = arr[index + 1];
                        }
                    }
                    else
                    {
                        //index = arr.IndexOf(arr, no3);
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (no3 == arr[i])
                            {
                                index = i;
                                break;
                            }
                        }
                        strNextNo3 = arr[index + 1];
                    }
                }
                else
                {
                    if (no2 == arr[arr.Length - 1])
                    {
                        strNextNo2 = arr[0];
                        //index = arr.IndexOf(arr, no1);
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (no1 == arr[i])
                            {
                                index = i;
                                break;
                            }
                        }
                        strNextNo1 = arr[index + 1];
                    }
                    else
                    {
                        //index = arr.IndexOf(arr, no2);
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (no2 == arr[i])
                            {
                                index = i;
                                break;
                            }
                        }
                        strNextNo2 = arr[index + 1];
                    }
                }

                for (int i = 0; i < arr.Length; i++) //For i As Integer = 0 To arr.Length - 1 Step 1
                {
                    if (strNextNo2 == arr[i])
                        htmlNo2.Append("<option selected='true' value='" + arr[i] + "'>" + arr[i] + "</option>");
                    else
                        htmlNo2.Append("<option value='" + arr[i] + "'>" + arr[i] + "</option>");

                    if (strNextNo1 == arr[i])
                        htmlNo1.Append("<option selected='true' value='" + arr[i] + "'>" + arr[i] + "</option>");
                    else
                        htmlNo1.Append("<option value='" + arr[i] + "'>" + arr[i] + "</option>");

                    if (isMember)
                    {
                        if (strNextNo3 == arr[i])
                            htmlNo3.Append("<option selected='true' value='" + arr[i] + "'>" + arr[i] + "</option>");
                        else
                            htmlNo3.Append("<option value='" + arr[i] + "'>" + arr[i] + "</option>");
                    }
                }
                result[0] = htmlNo1.ToString();
                result[1] = htmlNo2.ToString();
                if (isMember)
                    result[2] = htmlNo3.ToString();
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="strHtml"></param>
        /// <param name="isMult"></param>
        public static void GetTitleInfo(ref string strHtml, bool isMult)
        {
            TemplateHandler.Assign(ref strHtml, "{uname-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "UserName"));
            TemplateHandler.Assign(ref strHtml, "{pwd-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Password"));
            TemplateHandler.Assign(ref strHtml, "{fname-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "FirstName"));
            TemplateHandler.Assign(ref strHtml, "{lname-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "LastName"));
            TemplateHandler.Assign(ref strHtml, "{phone-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Phone"));
            TemplateHandler.Assign(ref strHtml, "{mobile-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Mobile"));
            TemplateHandler.Assign(ref strHtml, "{fax-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFax"));
            TemplateHandler.Assign(ref strHtml, "{closed-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblStatusClosed"));
            string str = String.Empty;
            string strSS = String.Empty;
            if (isMult)
            {
                str = "<input type='checkbox' id='chkUpdComm' onclick='AllowUpdate(this,false);' name='chkUpdComm'/>";
                strSS = "<input type='checkbox' id='chkUpdStatus' onclick='AllowUpdate(this,true);' name='chkUpdStatus'/>";
            }
            TemplateHandler.Assign(ref strHtml, "{status-text}", strSS + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Status"));
            TemplateHandler.Assign(ref strHtml, "{commission-text}", str + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Commission"));
            TemplateHandler.Assign(ref strHtml, "{commissionHorseRacing-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CommissionHR"));
            TemplateHandler.Assign(ref strHtml, "{commissionFinancial-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CommissionFinancial"));

            TemplateHandler.Assign(ref strHtml, "{groupa-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupA"));
            TemplateHandler.Assign(ref strHtml, "{groupb-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupB"));
            TemplateHandler.Assign(ref strHtml, "{groupc-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupC"));
            TemplateHandler.Assign(ref strHtml, "{groupd-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "GroupD"));
            TemplateHandler.Assign(ref strHtml, "{discountcs-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "OtherComm"));

            TemplateHandler.Assign(ref strHtml, "{discountcs31-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CommissionUK"));
            TemplateHandler.Assign(ref strHtml, "{discountcs32-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblOthersMeeting"));

            TemplateHandler.Assign(ref strHtml, "{discountcsFC-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CommFinancial"));
            if (!WebSettings.Disable1x2Comm)
            {
                TemplateHandler.Assign(ref strHtml, "{discountcs1x2-text}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lbl1x2Comm"));
            }
        }

        /// <summary>
        /// this function used for Get Limit Min Max Bet(agent-->acc)
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="tpl"></param>
        /// <param name="sportTypeId"></param>
        public static void GetLimitMinMaxBet(DataRow dr, ref string tpl, string sportTypeId)
        {
            try
            {
                string minbetMin;
                string maxbetMax;
                string maxpermatchMin;
                string maxpermatchMax;

                if (sportTypeId == String.Empty)//Then 'this case for soccer
                {
                    minbetMin = FormatHandler.FormatNumber1(dr["MinBet" + sportTypeId + "_Max"]);
                    maxbetMax = FormatHandler.FormatNumber1(dr["MaxBet" + sportTypeId + "_Min"]);
                    maxpermatchMin = FormatHandler.FormatNumber1(dr["MaxPerMatch" + sportTypeId + "_Min"]);
                    maxpermatchMax = FormatHandler.FormatNumber1(dr["MaxPerMatch" + sportTypeId + "_Max"]);
                    sportTypeId = "1";
                }
                else
                {
                    minbetMin = FormatHandler.FormatNumber1(dr["MinBetv" + sportTypeId + "_Max"]);
                    maxbetMax = FormatHandler.FormatNumber1(dr["MaxBetv" + sportTypeId + "_Min"]);
                    maxpermatchMin = FormatHandler.FormatNumber1(dr["MaxPerMatchv" + sportTypeId + "_Min"]);
                    maxpermatchMax = FormatHandler.FormatNumber1(dr["MaxPerMatchv" + sportTypeId + "_Max"]);
                }
                TemplateHandler.Assign(ref tpl, "{minbet-min-" + sportTypeId + "}", minbetMin);
                TemplateHandler.Assign(ref tpl, "{maxbet-max-" + sportTypeId + "}", maxbetMax);
                TemplateHandler.Assign(ref tpl, "{maxpermatch-min-" + sportTypeId + "}", maxpermatchMin);
                TemplateHandler.Assign(ref tpl, "{maxpermatch-max-" + sportTypeId + "}", maxpermatchMax);
                if (minbetMin == "0")
                    TemplateHandler.Assign(ref tpl, "{mbMin}", "hidden");
                else
                    TemplateHandler.Assign(ref tpl, "{mbMin}", "minmaxbet");

                if (maxbetMax == "0")
                    TemplateHandler.Assign(ref tpl, "{mbMax}", "hidden");
                else
                    TemplateHandler.Assign(ref tpl, "{mbMax}", "minmaxbet");

                if (maxpermatchMin == "0")
                    TemplateHandler.Assign(ref tpl, "{mpmMin}", "hidden");
                else
                    TemplateHandler.Assign(ref tpl, "{mpmMin}", "minmaxbet");

                if (maxpermatchMax == "0")
                    TemplateHandler.Assign(ref tpl, "{mpmMax}", "hidden");
                else
                    TemplateHandler.Assign(ref tpl, "{mpmMax}", "minmaxbet");
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        //this function is used to assign value for Min Bet, Max Bet, Max Per Match in Adding, Multiple Editing case(agent->acc)
        public static void GetMinMaxAddValue(ref string tpl, int sportTypeId, DataRow dr)
        {
            if (sportTypeId == 1)
            {
                TemplateHandler.Assign(ref tpl, "minbet" + sportTypeId.ToString() + "-val", DataFormat.FormatNumber1(dr["minbet_max"]));
                TemplateHandler.Assign(ref tpl, "maxbet" + sportTypeId.ToString() + "-val", DataFormat.FormatNumber1(dr["maxbet_min"]));
                TemplateHandler.Assign(ref tpl, "maxpermatch" + sportTypeId.ToString() + "-val", DataFormat.FormatNumber1(dr["maxpermatch_max"]));
            }

            else
            {
                TemplateHandler.Assign(ref tpl, "minbet" + sportTypeId.ToString() + "-val", DataFormat.FormatNumber1(dr["minbetv" + sportTypeId.ToString() + "_max"]));
                TemplateHandler.Assign(ref tpl, "maxbet" + sportTypeId.ToString() + "-val", DataFormat.FormatNumber1(dr["maxbetv" + sportTypeId.ToString() + "_min"]));
                TemplateHandler.Assign(ref tpl, "maxpermatch" + sportTypeId.ToString() + "-val", DataFormat.FormatNumber1(dr["maxpermatchv" + sportTypeId.ToString() + "_max"]));
            }
        }

        public static void GetMinMaxAddValue(ref string tpl, int sportTypeId, string strReplace)
        {
            TemplateHandler.Assign(ref tpl, "minbet" + sportTypeId + "-val", strReplace);
            TemplateHandler.Assign(ref tpl, "maxbet" + sportTypeId + "-val", strReplace);
            TemplateHandler.Assign(ref tpl, "maxpermatch" + sportTypeId + "-val", strReplace);
        }

        //this function is used to assign value for Min Bet, Max Bet, Max Per Match in Single Editing case
        public static void GetMinMaxEditValue(ref string tpl, int sportTypeId, DataRow dr)
        {
            TemplateHandler.Assign(ref tpl, "minbet" + sportTypeId + "-val", DataFormat.FormatNumber1(dr["minbetv" + sportTypeId]));
            TemplateHandler.Assign(ref tpl, "maxbet" + sportTypeId + "-val", DataFormat.FormatNumber1(dr["maxbetv" + sportTypeId]));
            TemplateHandler.Assign(ref tpl, "maxpermatch" + sportTypeId + "-val", DataFormat.FormatNumber1(dr["maxpermatchv" + sportTypeId]));
        }

        // this function to assign value for Balance transfer Condition (in Add New Master page)(agent->acc)
        public static void GetTranferTime(ref string tpl, Boolean isEdit)
        {
            if (!isEdit)
            {
                TemplateHandler.RemoveBlock(ref tpl, "none-add");
                TemplateHandler.Assign(ref tpl, "daily-chk", "checked");
                TemplateHandler.Assign(ref tpl, "weekly-chk", "");
                TemplateHandler.Assign(ref tpl, "mon-chk", "");
                TemplateHandler.Assign(ref tpl, "tue-chk", "");
                TemplateHandler.Assign(ref tpl, "wed-chk", "");
                TemplateHandler.Assign(ref tpl, "thurs-chk", "");
                TemplateHandler.Assign(ref tpl, "fri-chk", "");
                TemplateHandler.Assign(ref tpl, "sat-chk", "");
                TemplateHandler.Assign(ref tpl, "sun-chk", "");
                TemplateHandler.Assign(ref tpl, "padd-left", "pad33 l");
            }
            {
                TemplateHandler.Assign(ref tpl, "submit-text", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Submit"));
                TemplateHandler.Assign(ref tpl, "padd-left", "left");
            }
            TemplateHandler.Assign(ref tpl, "acc-balance-tranfer", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAccBalanceTranCondition"));
            TemplateHandler.Assign(ref tpl, "daily", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDaily"));
            TemplateHandler.Assign(ref tpl, "weekly", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWeekly"));
            TemplateHandler.Assign(ref tpl, "mon", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblMon"));
            TemplateHandler.Assign(ref tpl, "tue", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTue"));
            TemplateHandler.Assign(ref tpl, "wed", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWed"));
            TemplateHandler.Assign(ref tpl, "thurs", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblThurs"));
            TemplateHandler.Assign(ref tpl, "fri", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblFri"));
            TemplateHandler.Assign(ref tpl, "sat", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSat"));
            TemplateHandler.Assign(ref tpl, "sun", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSun"));
        }

        public static string GetVirtualRate(object virtualRate)
        {
            double virRate = 0;
            virRate = FormatHandler.GetDouble(virtualRate) * 100;
            string strVirRate = string.Empty;
            if (virRate < 0)
            {
                strVirRate = virRate.ToString();
            }
            else if (virRate > 0)
            {
                strVirRate = "+" + virRate.ToString();
            }
            else
            {
                strVirRate = "--";
            }
            return strVirRate;
        }

        public static bool IsTransferDaily(string custId)
        {
            DbResult rs = Select4Edit.TransferOptionSel(custId);
            if (rs != null)
            {
                string arrChar = DNAide.Core.CastAide.AsString(rs.Outputs["transferoption"]);
                if (arrChar != "")
                {
                    arrChar = arrChar.Substring(arrChar.Length - 1);
                    if (arrChar == "0")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Modified by: Lewin
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static CustDetail GetCustomer(string username)
        {
            CustDetail custDetail = null;
            if (!string.IsNullOrEmpty(username))
            {
                var rs = SelectCustomer.CustSelByUsername(username, AppSession.SYSTEM);
                if (rs != null)
                {
                    if (rs != null && rs.Tables.Count > 0)
                        if (rs.Tables[0].Rows.Count > 0)
                        {
                            custDetail = new CustDetail
                                             {
                                                 CustId = CastAide.AsInt32(rs.Tables[0].Rows[0]["custid"]),
                                                 RoleId = CastAide.AsInt32(rs.Tables[0].Rows[0]["roleid"])
                                             };
                        }
                }
            }

            return custDetail;
        }

        #region Check Permission for Accout
        //Tho Nguyen

        public static bool CheckPermission(int custid, string funcName)
        {
            try
            {
                DbResult rs = DataAccess.IBCDLL.Accounting.UserControl.CheckHostPermission(custid, funcName);
                if (CastAide.AsInt32(rs.Outputs["errcode"]) != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return false;
            }
        }

        #endregion
    }
}