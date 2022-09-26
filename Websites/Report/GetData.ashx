<%@ WebHandler Language="C#" Class="GetData" %>

using DNAide.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Presentation;
using System;
using System.Web;
using System.Web.SessionState;

public class GetData : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        JObject o = new JObject();
        context.Response.ContentType = "text/html";

        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        string mode = context.Request["mode"];
        mode = (string.IsNullOrEmpty(mode)) ? "0" : mode;

        DateTime fdate = CastAide.AsDateTime(context.Request["fdate"]);
        DateTime tdate = CastAide.AsDateTime(context.Request["tdate"]);

        switch (mode)
        {
            case "9":// Credit Term
                string groupMaxLoss = "N/A";
                o.Add(new JProperty("CreditTerm", CreditTermHTML.GetContentCreditTerm(context, ref groupMaxLoss)));
                o.Add(new JProperty("CreditTermFilter", CreditTermHTML.GetContentCreditTermFilter(context, groupMaxLoss))); // fix bug 1791
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;

            case "23": //Get FromDate, ToDate
                AccCommon.GetDatetimeOption(CastAide.AsString(context.Request["date"]), ref fdate, ref tdate);
                context.Response.Write(fdate.ToString("MM/dd/yyyy") + "|" + tdate.ToString("MM/dd/yyyy"));
                break;

            default:
                o.Add(new JProperty("Error", "Error"));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
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