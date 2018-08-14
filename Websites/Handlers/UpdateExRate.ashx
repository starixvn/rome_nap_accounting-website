<%@ WebHandler Language="C#" Class="UpdateExRate" %>

using System;
using System.Web;
using Sunplus.Accounting.Business.ExChangeRate;
using Sunplus.Accounting.DataAccess;
using System.Web.SessionState;
using Fanex.CustomerSetting.Module.Core.Web;
using DNAide.Core;
using System.Collections.Generic;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Configurations;

using SCCACHE = Fanex.Tarus.Infrastructure.Caching.CacheHelper;


public class UpdateExRate : IHttpHandler,IRequiresSessionState {

    public void ProcessRequest (HttpContext context)
    {
        context.Response.ContentType = "text/html";

        try
        {
            string errMsg = "";
            int userId = Convert.ToInt32(context.Session["AccId"]);
            string actualRate = CastAide.AsString(context.Request["BaseRate"]);
            string currency = context.Request["Currency"];
            string minbet = CastAide.AsString(context.Request["Minbet"]);
            string minbetlive = CastAide.AsString(context.Request["MinbetLive"]);           
            string Code = CastAide.AsString(context.Request["Code"]);
            string Description = CastAide.AsString(context.Request["Description"]);
            string MultipliedBy = CastAide.AsString(context.Request["MultipliedBy"]);

            int exRateId = 0;
            ExChangeRate exRate = new ExChangeRate();

            if (context.Request["type"] == "insert")
            {

                errMsg = exRate.InsertExChangeRate(actualRate, currency, minbet, minbetlive, Code, Description, MultipliedBy);
                if (errMsg.Length > 0)
                    context.Response.Write("no|" + errMsg);
                else
                {
                    System.Threading.Thread.Sleep(3000);
                    Dictionary <int, string> exchange;
                    exchange = IBCProvider.Utils.GetExchange();
                    SCCACHE.Set("exchange", exchange, DateTime.Now.AddHours(5));
                    //context.Response.Write("ok|"+ Resources.Language.lblInsertExSuccessful );
                    context.Response.Write("ok|Insert exchange rate successfully!");
                    //context.Response.Write("ok|" + exRate.GenListContent());
                }


            }
            else if (context.Request["type"] == "update")
            {
                try
                {
                    exRateId = CastAide.AsInt32(context.Request["exrateId"]);
                    Fanex.Tarus.Customer.Business.NG2.ISyncCurrencyService syncCurrencyService = new Fanex.Tarus.Customer.Business.NG2.SyncCurrencyService();
                    var currencies = syncCurrencyService.SyncCurrencies().Currencies;

                    errMsg = exRate.UpdateEditExChangeRate(exRateId, actualRate, currency, minbet, minbetlive, Description, MultipliedBy);
                    if (errMsg.Length == 0)
                    {
                        var allowedCurrency = false;
                        var format = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
                        foreach (var item in currencies)
                        {
                            double rate = 0;
                            double.TryParse(actualRate, System.Globalization.NumberStyles.Float, format,  out rate);
                            if (item.CurrencyId == exRateId && rate != item.BaseRate)
                            {
                                allowedCurrency = true;
                                break;
                            }
                        }

                        if (allowedCurrency)
                        {
                            ONEbook.NG2.API.Interface.IThirdPartyServices service = new ONEbook.NG2.API.Services();
                            var request = new ONEbook.NG2.API.DTO.SyncExchangeRate.Request();

                            request.Auth.SiteId = 2000;
                            request.Call.CurrencyArray.Add(new ONEbook.NG2.API.DTO.SyncExchangeRate.CurrencyInfo { CurrencyId = exRateId, ExchangeRate = Fanex.CustomerSetting.Module.Core.Utils.ToDecimal(actualRate) });
                            request.UserId = AppSession.CurrentSession.UserId;

                            var result = service.SyncExchangeRates(request);
                            if (!result.Result.IsSuccess)
                            {
                                // Send email notify IT to sync manually.
                                var to = System.Configuration.ConfigurationManager.AppSettings["ToMailNG2"];
                                var cc = System.Configuration.ConfigurationManager.AppSettings["CCMailNG2"];
                                var subject = Resources.MiniTemplate.UpdateExtRateFailedNg2Title;
                                subject = string.Format(subject, /*Fanex.Tarus.Infrastructure.Caching.CacheRef.GetProductName(23)*/"NG2");
                                var body = Resources.MiniTemplate.UpdateExtRateFailedNg2Email;
                                body = string.Format(body, exRateId, currency, actualRate);

                                SendMailExtension.AccSendMail(MailSection.HostName.Gmail, to, cc, "IT Team", subject, body);
                            }
                        }

                        System.Threading.Thread.Sleep(3000);
                        context.Response.Write("ok|Update exchange rate successfully!");//exRate.GenListContent());
                        return;
                    }
                    else
                    {
                        if (errMsg == "You are not authorized on this function")
                        {
                            context.Response.Write("500|" + errMsg);
                        }
                        else
                        {
                            context.Response.Write("no|" + errMsg);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    errMsg = ex.Message;
                    context.Response.Write("no|" + errMsg);
                }
            }

            else if (context.Request["type"] == "reset")
            {
                exRateId = CastAide.AsInt32(context.Request["exrateId"]);
                context.Response.Write( exRate.ResetEditExchangeRate(exRateId));
            }

        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            context.Response.Write("no|" + ex.Message);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}
