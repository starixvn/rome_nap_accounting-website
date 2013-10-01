using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;
using Fanex.Tarus.LiveCasino.Business.ExRate;
using Fanex.Tarus.LiveCasino.Entities.ExRate;
using Sunplus.Accounting.Business.PT.Bingo;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.TplCore.App_Templates.ExRate.App_Templates.ExRate;
using Sunplus.Accounting.DataAccess;
using Sunplus.Bingo.Core;

/// <summary>
/// Summary description for ExChangeRate
/// </summary>
namespace Sunplus.Accounting.Business.ExChangeRate
{
    using System.Linq;

    public class ExChangeRate
    {
        #region GenAddContent

        public string GenAddContent(string FunctionName)
        {
            ExRateAdd_Template ex = new ExRateAdd_Template();
            ex.submit = "Submit";
            ex.reset = "Reset";
            ex.Currency = string.Empty;
            ex.BaseRate = string.Empty;
            ex.Minbet = string.Empty;
            ex.MinbetLive = string.Empty;
            ex.GBPRate = string.Empty;
            ex.AUDRate = string.Empty;
            ex.ZARRate = string.Empty;
            ex.EURORate = string.Empty;
            ex.USDRate = string.Empty;
            ex.function = FunctionName;
            ex.exrateId = string.Empty;
            ex.Function_Reset = "ResetValue();";
            ex.ReadOnly = "";
            return ex.ToString();
        }

        #endregion GenAddContent

        #region GenListContent

        public string GenListContent()
        {
            ExRateList_Template ex = new ExRateList_Template();
            var rs = IBCProvider.ExChangeRate.ExchangeSelAccount();
            DataRow[] Rows = rs.ResultSet.Tables[0].Select();

            /* ex.Content.Clear();
             string odd_even = "";
             foreach (DataRow dr in Rows)
             {
                 int exid = CastAide.AsInt32(dr["exchangeid"]);
                 bool f = IsHideCurrencies(exid);
                 if (f) continue;
                 odd_even = (odd_even == "") ? "evenrow" : "";
                 ExRateList_Content_Block exRow = new ExRateList_Content_Block();
                 exRow.currency = CastAide.AsString(dr["currency"]);
                 exRow.BaseRate = CastAide.AsString(dr["actualrate"]);
                 exRow.MinBet = NumberFormat.DecFormat(dr["MinBet"],0);
                 exRow.MinBetLive = NumberFormat.DecFormat(dr["MinBetLive"], 0);
                 exRow.GBPRate = (Convert.IsDBNull(dr["GBPRate"]) ? "-" : dr["GBPRate"].ToString());
                 exRow.AUDRate = (Convert.IsDBNull(dr["AUDRate"]) ? "-" : dr["AUDRate"].ToString());
                 exRow.ZARRate = (Convert.IsDBNull(dr["ZARRate"]) ? "-" : dr["ZARRate"].ToString());
                 exRow.EURORate = (Convert.IsDBNull(dr["EURORate"]) ? "-" : dr["EURORate"].ToString());
                 exRow.USDRate = (Convert.IsDBNull(dr["US$Rate"]) ? "-" : dr["US$Rate"].ToString());
                 exRow.exrateId = exid.ToString();
                 if(!Permissions.GetHostPermission(Enumerations.Functions.ExRates.ExchangeRate))
                 {
                     exRow.edit_none = "none";
                 }
                 exRow.odd_even = odd_even;

                 ex.Content.Append(exRow);
             }
             return ex.Content.ToString();*/

            StringBuilder html = new StringBuilder();
            string tpl = WebUtils.LoadTemplateBySystem("ExRate/ExRateList1.htm");
            string tplContent = TemplateHandler.CopyBlock(tpl, "Content");

            string odd_even = "";
            var i = 1;
            foreach (DataRow dr in Rows)
            {
                int exid = CastAide.AsInt32(dr["exchangeid"]);
                bool f = IsHideCurrencies(exid);
                if (f) continue;
                odd_even = (odd_even == "") ? "evenrow" : "";

                string exRow = tplContent;
                TemplateHandler.Assign(ref exRow, "currency", CastAide.AsString(dr["currency"]));
                TemplateHandler.Assign(ref exRow, "standardCode", CastAide.AsString(dr["currencyCode"]));
                TemplateHandler.Assign(ref exRow, "BaseRate", CastAide.AsString(dr["actualrate"]));
                TemplateHandler.Assign(ref exRow, "MinBet", NumberFormat.DecFormat(dr["MinBet"], 0));
                TemplateHandler.Assign(ref exRow, "MinBetLive", NumberFormat.DecFormat(dr["MinBetLive"], 0));
                TemplateHandler.Assign(ref exRow, "GBPRate", (Convert.IsDBNull(dr["GBPRate"]) ? "-" : dr["GBPRate"].ToString()));
                TemplateHandler.Assign(ref exRow, "AUDRate", (Convert.IsDBNull(dr["AUDRate"]) ? "-" : dr["AUDRate"].ToString()));
                TemplateHandler.Assign(ref exRow, "ZARRate", (Convert.IsDBNull(dr["ZARRate"]) ? "-" : dr["ZARRate"].ToString()));
                TemplateHandler.Assign(ref exRow, "EURORate", (Convert.IsDBNull(dr["EURORate"]) ? "-" : dr["EURORate"].ToString()));
                TemplateHandler.Assign(ref exRow, "USDRate", (Convert.IsDBNull(dr["US$Rate"]) ? "-" : dr["US$Rate"].ToString()));
                TemplateHandler.Assign(ref exRow, "exrateId", exid);
                TemplateHandler.Assign(ref exRow, "description", CastAide.AsString(dr["Description"]));

                if (!Permissions.GetHostPermission(Enumerations.Functions.ExRates.ExchangeRate))
                {
                    TemplateHandler.Assign(ref exRow, "edit_none", "none");
                }
                TemplateHandler.Assign(ref exRow, "odd_even", odd_even);

                html.Append(exRow);
            }

            TemplateHandler.ReplaceBlock(ref tpl, "Content", html.ToString());

            return tpl;
        }

        #endregion GenListContent

        #region IsHideCurrencies

        public bool IsHideCurrencies(int exid)
        {
            return WebSettings.HiddenCurrencies.Split(',').Any(i => i == exid.ToString());
        }

        #endregion IsHideCurrencies

        #region InsertExChangeRate

        public string InsertExChangeRate(string actualrate, string currency, string minbet, string minbetlive,
            string GBPRate, string AUDRate, string ZARRate, string EURORate, string USDRate, string Code, string Description, string MultipliedBy)
        {
            string msg = "";
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                hash.Add("actualrate", actualrate);
                hash.Add("currency", currency.Trim());
                hash.Add("minbet", minbet);
                hash.Add("minbetlive", minbetlive);

                if (GBPRate.Length != 0)
                    hash.Add("gbprate", GBPRate);

                if (AUDRate.Length != 0)
                    hash.Add("audrate", AUDRate);

                if (ZARRate.Length != 0)
                    hash.Add("zarrate", ZARRate);

                if (EURORate.Length != 0)
                    hash.Add("eurorate", EURORate);

                if (USDRate.Length != 0)
                    hash.Add("us$rate", USDRate);

                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("exchangeid", 0);

                hash.Add("code", Code.Trim());
                hash.Add("description", Description);
                hash.Add("multipliedby", Convert.ToInt32(MultipliedBy.Trim()));

                int errcode = 0;
                string errmsg = "";
                int exchangeid = 0;

                IBCProvider.ExChangeRate.ExchangeIns(hash, ref errcode, ref errmsg, ref exchangeid);

                msg = errmsg;
                int exid = exchangeid;
                if (msg.Length == 0)
                {
                    CasinoLibrary.SBAgent.BasicInfo info = new CasinoLibrary.SBAgent.BasicInfo();
                    bool isSuc = info.UpdateExchange(Globals.CASINO_GROUP1, exid, currency, CastAide.AsDouble(actualrate));

                    if (BingoSettings.BingoGame.Enabled)
                    {
                        var bingoManager = new BingoManager();
                        bingoManager.UpdateExchangeRate(BingoSettings.BingoGame.CodeGameNo, exid, currency,
                                                        CastAide.AsDouble(actualrate), AppSession.AccId,
                                                        AppSession.AccUserName, HttpContext.Current.Request.UserHostAddress);
                    }
                    if (WebSettings.EnableLiveCasino)
                    {
                        var liveCs = new ExRateManager();
                        ExRateBO exRateBo = new ExRateBO()
                                                {
                                                    ExchangeId = exid,
                                                    Currency = currency,
                                                    Actualrate = actualrate,
                                                    ModifyBy = AppSession.AccId,
                                                    ModifyUserName = AppSession.AccUserName,
                                                    FromIp = HttpContext.Current.Request.UserHostAddress,
                                                };
                        liveCs.UpdateExchange(exRateBo);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return msg;
        }

        #endregion InsertExChangeRate

        #region EditExchangeRate

        public string EditExchangeRate(int exid)
        {
            /* ExRateAdd_Template ex = new ExRateAdd_Template();
             ex.function = "UpdateEditExChangeRate();";
             ex.Function_Reset = "ResetValueWhenEdit();";
             ex.ReadOnly = "readonly='true'";
             DbResult rs = IBCProvider.ExChangeRate.ExchangeSelByID(exid);

             DataTable dt = rs.ResultSet.Tables[0];
             string html = "";
             if (dt.Rows.Count == 0)
             {
                 html = GenAddContent("UpdateEditExChangeRate();");
             }
             else
             {
                 foreach (DataRow dr in dt.Rows)
                 {
                     ex.submit = "Submit";
                     ex.reset = "Reset";
                     ex.Currency = CastAide.AsString(dr["currency"]);
                     ex.BaseRate = CastAide.AsString(dr["actualrate"]);
                     ex.Minbet = CastAide.AsString(dr["minbet"]);
                     ex.MinbetLive = CastAide.AsString(dr["minbetlive"]);
                     ex.GBPRate = CastAide.AsString(dr["GBPRate"]);
                     ex.AUDRate = CastAide.AsString(dr["AUDRate"]);
                     ex.ZARRate = CastAide.AsString(dr["ZARRate"]);
                     ex.EURORate = CastAide.AsString(dr["EURORate"]);
                     ex.USDRate = CastAide.AsString(dr["US$Rate"]);
                   //  ex.Code = CastAide.AsString(dr["Code"]);
                     ex.exrateId = exid.ToString();
                 }
                 html = ex.ToString();
             }

             return html;*/

            string tpl = WebUtils.LoadTemplateBySystem("ExRate/ExRateAdd.htm");
            TemplateHandler.Assign(ref tpl, "function", "UpdateEditExChangeRate();");
            TemplateHandler.Assign(ref tpl, "Function_Reset", "ResetValueWhenEdit();");
            TemplateHandler.Assign(ref tpl, "ReadOnly", "readonly='true'");
            DbResult rs = IBCProvider.ExChangeRate.ExchangeSelByID(exid);
            DataTable dt = rs.ResultSet.Tables[0];
            string html = "";
            if (dt.Rows.Count == 0)
            {
                tpl = GenAddContent("UpdateEditExChangeRate();");
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TemplateHandler.Assign(ref tpl, "submit", "Submit");
                    TemplateHandler.Assign(ref tpl, "reset", "Reset");
                    TemplateHandler.Assign(ref tpl, "Currency", CastAide.AsString(dr["currency"]));
                    TemplateHandler.Assign(ref tpl, "BaseRate", CastAide.AsString(dr["actualrate"]));
                    TemplateHandler.Assign(ref tpl, "Minbet", CastAide.AsString(dr["minbet"]));
                    TemplateHandler.Assign(ref tpl, "MinbetLive", CastAide.AsString(dr["minbetlive"]));
                    TemplateHandler.Assign(ref tpl, "GBPRate", CastAide.AsString(dr["GBPRate"]));
                    TemplateHandler.Assign(ref tpl, "AUDRate", CastAide.AsString(dr["AUDRate"]));
                    TemplateHandler.Assign(ref tpl, "ZARRate", CastAide.AsString(dr["ZARRate"]));
                    TemplateHandler.Assign(ref tpl, "EURORate", CastAide.AsString(dr["EURORate"]));
                    TemplateHandler.Assign(ref tpl, "USDRate", CastAide.AsString(dr["US$Rate"]));
                    TemplateHandler.Assign(ref tpl, "exrateId", exid);
                    TemplateHandler.Assign(ref tpl, "Code", CastAide.AsString(dr["Code"]));
                }
            }
            return tpl;
        }

        public List<string> EditExchangeRate1(int exid)
        {
            List<string> list = new List<string>();
            ExRateAdd_Template ex = new ExRateAdd_Template();
            ex.function = "UpdateEditExChangeRate();";
            ex.Function_Reset = "ResetValueWhenEdit();";
            ex.ReadOnly = "readonly='true'";
            DbResult rs = IBCProvider.ExChangeRate.ExchangeSelByID(exid);

            DataTable dt = rs.ResultSet.Tables[0];
            string html = "";
            if (dt.Rows.Count == 0)
            {
                html = GenAddContent("UpdateEditExChangeRate();");
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ex.submit = "Submit";
                    ex.reset = "Reset";
                    ex.Currency = CastAide.AsString(dr["currency"]);
                    list.Add(ex.Currency);
                    ex.BaseRate = CastAide.AsString(dr["actualrate"]);
                    list.Add(ex.BaseRate);
                    ex.Minbet = CastAide.AsString(dr["minbet"]);
                    list.Add(ex.Minbet);
                    ex.MinbetLive = CastAide.AsString(dr["minbetlive"]);
                    list.Add(ex.MinbetLive);
                    ex.GBPRate = CastAide.AsString(dr["GBPRate"]);
                    list.Add(ex.GBPRate);
                    ex.AUDRate = CastAide.AsString(dr["AUDRate"]);
                    list.Add(ex.AUDRate);
                    ex.ZARRate = CastAide.AsString(dr["ZARRate"]);
                    list.Add(ex.ZARRate);
                    ex.EURORate = CastAide.AsString(dr["EURORate"]);
                    list.Add(ex.EURORate);
                    ex.USDRate = CastAide.AsString(dr["US$Rate"]);
                    list.Add(ex.USDRate);
                    ex.Code = CastAide.AsString(dr["Code"]);
                    list.Add(ex.Code);
                    list.Add(CastAide.AsString(dr["Description"]));
                    list.Add(CastAide.AsString(dr["MultipliedBy"]));
                    ex.exrateId = exid.ToString();
                }
                html = ex.ToString();
            }

            return list;
        }

        #endregion EditExchangeRate

        #region ResetEditExchangeRate

        public string ResetEditExchangeRate(int exid)
        {
            DbResult rs = IBCProvider.ExChangeRate.ExchangeSelByID(exid);
            string html = "";
            DataTable dt = rs.ResultSet.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                html = CastAide.AsString(dr["actualrate"]) + "|";
                html += CastAide.AsString(dr["currency"]) + "|";
                html += CastAide.AsString(dr["minbet"]) + "|";
                html += CastAide.AsString(dr["minbetlive"]) + "|";
                html += CastAide.AsString(dr["GBPRate"]) + "|";
                html += CastAide.AsString(dr["AUDRate"]) + "|";
                html += CastAide.AsString(dr["ZARRate"]) + "|";
                html += CastAide.AsString(dr["EURORate"]) + "|";
                html += CastAide.AsString(dr["US$Rate"]) + "|";
                html += CastAide.AsString(dr["Description"]) + "|";
                html += CastAide.AsString(dr["MultipliedBy"]);
            }
            return html;
        }

        #endregion ResetEditExchangeRate

        #region UpdateEditExChangeRate

        public string UpdateEditExChangeRate(int exid, string actualrate, string currency, string minbet, string minbetlive, string GBPRate, string AUDRate, string ZARRate, string EURORate, string USDRate, string Description, string MultipliedBy, string Code)
        {
            string msg = "";
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                hash.Add("exchangeid", exid);
                hash.Add("actualrate", actualrate);
                hash.Add("currency", currency);
                hash.Add("minbet", minbet);
                hash.Add("minbetlive", minbetlive);

                if (GBPRate.Length != 0)
                    hash.Add("gbprate", GBPRate);

                if (AUDRate.Length != 0)
                    hash.Add("audrate", AUDRate);

                if (ZARRate.Length != 0)
                    hash.Add("zarrate", ZARRate);

                if (EURORate.Length != 0)
                    hash.Add("eurorate", EURORate);

                if (USDRate.Length != 0)
                    hash.Add("us$rate", USDRate);

                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("description", Description);
                hash.Add("multipliedby", Convert.ToInt32(MultipliedBy.Trim()));

                DbResult rs;
                rs = IBCProvider.ExChangeRate.ExchangeSelByID(exid);// get info to send email
                DataRow[] Rows = rs.ResultSet.Tables[0].Select();

                int errcode = 0;
                string errmsg = "";

                IBCProvider.ExChangeRate.ExchangeUpd(hash, ref errcode, ref errmsg);
                msg = errmsg;

                if (msg.Length == 0)
                {
                    CasinoLibrary.SBAgent.BasicInfo info = new CasinoLibrary.SBAgent.BasicInfo();
                    bool isSuc = info.UpdateExchange(Globals.CASINO_GROUP1, exid, currency, CastAide.AsDouble(actualrate));

                    if (BingoSettings.BingoGame.Enabled)
                    {
                        var bingoManager = new BingoManager();
                        bingoManager.UpdateExchangeRate(BingoSettings.BingoGame.CodeGameNo, exid, currency,
                                                        CastAide.AsDouble(actualrate), AppSession.AccId,
                                                        AppSession.AccUserName, HttpContext.Current.Request.UserHostAddress);
                    }
                    if (WebSettings.EnableLiveCasino)
                    {
                        var liveCs = new ExRateManager();
                        ExRateBO exRateBo = new ExRateBO()
                        {
                            ExchangeId = exid,
                            Currency = currency,
                            Actualrate = actualrate,
                            ModifyBy = AppSession.AccId,
                            ModifyUserName = AppSession.AccUserName,
                            FromIp = HttpContext.Current.Request.UserHostAddress,
                        };
                        liveCs.UpdateExchange(exRateBo);
                    }

                    string tplSubject = "Exchange Rate was changed";
                    SendEmail_Template mail = new SendEmail_Template();
                    foreach (DataRow dr in Rows)
                    {
                        mail.username = AppSession.Username;
                        mail.ExchangeID = exid.ToString();

                        //if (actualrate.ToString() != dr["actualrate"].ToString())

                        mail.BaseRate = actualrate.ToString();
                        mail.oldBaseRate = dr["actualrate"].ToString();
                        if (actualrate.ToString() != dr["actualrate"].ToString())
                            mail.BaseRate = changeColor(actualrate.ToString());

                        mail.minbet = dr["minbet"].ToString();
                        mail.minbetlive = dr["minbetlive"].ToString();
                        if (Convert.IsDBNull(dr["GBPRate"]) && GBPRate.Length == 0)
                            mail.GBPRate.Clear();
                        else
                        {
                            mail.GBPRate.newGBPRate = GBPRate;
                            mail.GBPRate.oldGBPRate = dr["GBPRate"].ToString();
                            if (GBPRate != dr["GBPRate"].ToString())
                                mail.GBPRate.newGBPRate = changeColor(GBPRate);
                        }

                        if (Convert.IsDBNull(dr["AUDRate"]) && AUDRate.Length == 0)
                            mail.AUDRate.Clear();
                        else
                        {
                            mail.AUDRate.newAUDRate = AUDRate;
                            mail.AUDRate.oldAUDRate = dr["AUDRate"].ToString();
                            if (AUDRate != dr["AUDRate"].ToString())
                                mail.AUDRate.newAUDRate = changeColor(AUDRate);
                        }

                        if (Convert.IsDBNull(dr["ZARRate"]) && ZARRate.Length == 0)
                            mail.ZARRate.Clear();
                        else
                        {
                            mail.ZARRate.newZARRate = ZARRate;
                            mail.ZARRate.oldZARRate = dr["ZARRate"].ToString();
                            if (ZARRate != dr["ZARRate"].ToString())
                                mail.ZARRate.newZARRate = changeColor(ZARRate);
                        }

                        if (Convert.IsDBNull(dr["EURORate"]) && EURORate.Length == 0)
                            mail.EURORate.Clear();
                        else
                        {
                            mail.EURORate.newEURORate = EURORate;
                            mail.EURORate.oldEURORate = dr["EURORate"].ToString();
                            if (EURORate != dr["EURORate"].ToString())
                                mail.EURORate.newEURORate = changeColor(EURORate);
                        }

                        if (Convert.IsDBNull(dr["US$Rate"]) && USDRate.Length == 0)
                            mail.USDRate.Clear();
                        else
                        {
                            mail.USDRate.newUSDRate = USDRate;
                            mail.USDRate.oldUSDRate = dr["US$Rate"].ToString();
                            if (USDRate != dr["US$Rate"].ToString())
                                mail.USDRate.newUSDRate = changeColor(USDRate);
                        }

                        mail.IP = HttpContext.Current.Request.UserHostAddress;
                        mail.date = DateTime.Now.ToString();
                    }

                    //bool f = DNAide.Utils.SendMailService.SendMail2(MailSection.MailServer, MailSection.ToMailExchangeRate, MailSection.FromMail, MailSection.CCMailExchangeRate, MailSection.BCCMailExchangeRate, tplSubject, mail.ToString(), MailSection.MailPwd);
                    SendMailExtension.AccSendMail(MailSection.HostName.Gmail, MailSection.ToMailExchangeRate,
                                                  MailSection.CCMailExchangeRate, MailSection.BCCMailExchangeRate,
                                                  tplSubject, mail.ToString());
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                //bool f = DNAide.Utils.SendMailService.SendMail2(MailSetting.MailServer, MailSetting.ToMailExchangeRate, MailSetting.FromMail, MailSetting.CCMailExchangeRate, MailSetting.BCCMailExchangeRate, "Have a problem when updating exchange rate", ex.ToString(), MailSetting.MailPwd);
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return msg;
        }

        #endregion UpdateEditExChangeRate

        private string changeColor(string value)
        {
            return "<span style='color:blue'>" + value + "</span>";
        }
    }
}
