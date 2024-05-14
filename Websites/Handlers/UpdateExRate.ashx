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
using Sunplus.Accounting.Entities;
using System.Text;

using SCCACHE = Fanex.Tarus.Infrastructure.Caching.CacheHelper;


public class UpdateExRate : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";

        try
        {
            string errMsg = "";
            string actualRate = CastAide.AsString(context.Request["BaseRate"]);
            string currency = context.Request["Currency"];
            string minbet = CastAide.AsString(context.Request["Minbet"]);
            string minbetlive = CastAide.AsString(context.Request["MinbetLive"]);
            string Code = CastAide.AsString(context.Request["Code"]);
            string Description = CastAide.AsString(context.Request["Description"]);
            string MultipliedBy = CastAide.AsString(context.Request["MultipliedBy"]);
            string OfferSite = CastAide.AsString(context.Request["OfferSite"]);
            string CurrencyType = CastAide.AsString(context.Request["CurrencyType"]);
            string CoinMarketCapID = CurrencyType == "1" ? context.Request["CoinMarketCapID"] : null;
            bool IsActive = CastAide.AsBoolean(context.Request["IsActive"]);

            int exRateId = 0;
            ExChangeRate exRate = new ExChangeRate();

            if (context.Request["type"] == "insert")
            {

                errMsg = exRate.InsertExChangeRate(actualRate, currency, minbet, minbetlive, Code, Description, MultipliedBy, OfferSite, CurrencyType, CoinMarketCapID);
                if (errMsg.Length > 0)
                    context.Response.Write("no|" + errMsg);
                else
                {
                    System.Threading.Thread.Sleep(3000);
                    Dictionary<int, string> exchange;
                    exchange = IBCProvider.Utils.GetExchange();
                    SCCACHE.Set("exchange", exchange, DateTime.Now.AddHours(5));
                    context.Response.Write("ok|Insert exchange rate successfully!");
                }


            }
            else if (context.Request["type"] == "update")
            {
                try
                {
                    exRateId = CastAide.AsInt32(context.Request["exrateId"]);

                    errMsg = exRate.UpdateEditExChangeRate(exRateId, actualRate, currency, minbet, minbetlive, Description, MultipliedBy, Code, OfferSite, CurrencyType, CoinMarketCapID, IsActive);

                    if (errMsg.Length == 0)
                    {
                        context.Response.Write("ok|Update exchange rate successfully!");
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
                context.Response.Write(exRate.ResetEditExchangeRate(exRateId));
            }
            else if (context.Request["type"] == "editbaserate")
            {
                var result = exRate.EditBaseRate_UpdateBaseRate(context.Request["updateParams"]);

                if (result.ErrorCode == 0)
                {
                    context.Response.Write("ok|The base rates have been updated successfully.");
                }
                else if (result.ErrorCode == 2)
                {
                    context.Response.Write("2|" + result.ErrorMessage);
                }
                else
                {
                    context.Response.Write("no|" + result.ErrorMessage);
                }
            }

        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            context.Response.Write("no|" + ex.Message);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}