<%@ WebHandler Language="C#" Class="UpdateExRate" %>

using System;
using System.Web;
using Sunplus.Accounting.Business.ExChangeRate;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Collections;
using System.Web.SessionState;
using IBCUtils;

using BLL;
using DNAide.Core;
using DNAide.Web;
using System.Collections.Generic;

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
            string GBPRate = CastAide.AsString(context.Request["GBPRate"]);
            string AUDRate = CastAide.AsString(context.Request["AUDRate"]);
            string ZARRate = CastAide.AsString(context.Request["ZARRate"]);
            string EURORate = CastAide.AsString(context.Request["EURORate"]);
            string USDRate = CastAide.AsString(context.Request["USDRate"]);
            string Code = CastAide.AsString(context.Request["Code"]);
            string Description = CastAide.AsString(context.Request["Description"]);
            string MultipliedBy = CastAide.AsString(context.Request["MultipliedBy"]);

            int exRateId = 0;
            ExChangeRate exRate = new ExChangeRate();

            if (context.Request["type"] == "insert")
            {

                errMsg = exRate.InsertExChangeRate(actualRate, currency, minbet, minbetlive, GBPRate, AUDRate, ZARRate, EURORate, USDRate, Code, Description, MultipliedBy);
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
                    errMsg = exRate.UpdateEditExChangeRate(exRateId, actualRate, currency, minbet, minbetlive, GBPRate, AUDRate, ZARRate, EURORate, USDRate, Description, MultipliedBy, Code);
                    if (errMsg.Length == 0)
                    {
                        var service = new ONEbook.NG2.API.Services();
                        var request = new ONEbook.NG2.API.DTO.Request();

                        request.Auth.SiteId = "2000";
                        request.Call.Currencies.Add(new KeyValuePair<int, decimal>(exRateId, Fanex.CustomerSetting.Module.Core.Utils.ToDecimal(actualRate)));
                        request.Call.UserId = AppSession.AccId;

                        service.SyncExchangeRates(request);

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
