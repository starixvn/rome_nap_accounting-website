<%@ WebHandler Language="C#" Class="GetDataLeagueWL" %>

using System;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

public class GetDataLeagueWL : IHttpHandler, IRequiresSessionState
{

    JObject o = new JObject();
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string mode = context.Request["mode"];
        mode = (string.IsNullOrEmpty(mode)) ? "0" : mode;
        Hashtable hash = new Hashtable();
        string Content = "";
        switch (mode)
        {
            case "1": // League WL by sport
                hash = new Hashtable();
                hash.Add("db", context.Request["db"]);
                hash.Add("wldate", context.Request["wldate"]);
                BLL.Report.ReportBase LeagueWL = new BLL.Report.LeagueWL.LeagueWLSporttype();
                bool isback = context.Request["isback"] == "1";
                /*if (isback)
                {
                    Content = LeagueWL.GetCachedReport(LeagueWL.KeyCaching);
                    if (string.IsNullOrEmpty(Content))
                    {
                        Content = LeagueWL.RenderHTML(hash).Replace("{border}", "0");
                    }
                    Content = Content.Replace("{border}", "0");
                }
                else
                {
                    Content = LeagueWL.RenderHTML(hash).Replace("{border}", "0");
                }*/
                Content = LeagueWL.RenderHTML(hash).Replace("{border}", "0");
                o.Add(new JProperty("sData", Content));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
            case "2": // League WL by sporttype and leagueid
                hash = new Hashtable();
                hash.Add("db", context.Request["db"]);
                hash.Add("wldate", context.Request["wldate"]);
                hash.Add("sporttype", context.Request["sporttype"]);
                hash.Add("leagueid", context.Request["leagueid"]);
                BLL.Report.ReportBase LeagueWLBettype = new BLL.Report.LeagueWL.LeagueWLBettype();
                o.Add(new JProperty("sData", LeagueWLBettype.RenderHTML(hash).Replace("{border}", "0")));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
            case "3": // League WL details by leagueid, sporttype, bettype, liveindicator, IsHedge
                hash = new Hashtable();
                hash.Add("db", context.Request["db"]);
                hash.Add("wldate", context.Request["wldate"]);
                hash.Add("leagueid", context.Request["leagueid"]);
                hash.Add("sporttype", context.Request["sporttype"]);
                hash.Add("bettype", context.Request["bettype"]);
                hash.Add("liveindicator", context.Request["liveindicator"]);
                hash.Add("IsHedge", context.Request["IsHedge"]);
                BLL.Report.ReportBase LeagueWLDetails = new BLL.Report.LeagueWL.LeagueWLDetails();
                o.Add(new JProperty("sData", LeagueWLDetails.RenderHTML(hash).Replace("{border}", "0")));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;
            default:
                o.Add(new JProperty("Error", "Error"));
                context.Response.Write(JsonConvert.SerializeObject(o));
                break;                
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}