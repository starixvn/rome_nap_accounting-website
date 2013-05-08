<%@ WebHandler Language="C#" Class="GetDataPunter" %>

using System;
using System.Collections;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;


public class GetDataPunter : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string type = context.Request["type"];
        IServicePunterAnalysis objPunter = new ServicePunterAnalysis();
        string html = "";
        string username = "", fromdate = "", todate = "", toprow = "", orderby = "", ordertype = "", CustId = "", Sporttype = "", bettype="", leagueid="";
        string fMonth = "", fYear = "", tMonth = "", tYear = "", leaguename ="";
        try 
        {
            switch (type)
            {
                case "month":
                    fMonth = context.Request["fMonth"];
                    fYear = context.Request["fYear"];
                    html = objPunter.GeneratePeriodMonthly(fMonth, fYear);
                    break;
                case "annal":
                    fYear = context.Request["fYear"];
                    html = objPunter.GeneratePeriodAnnual(fYear);
                    break;
                case "longrange":
                    fMonth = context.Request["fMonth"];
                    fYear = context.Request["fYear"];
                    tMonth = context.Request["tMonth"];
                    tYear = context.Request["tYear"];
                    html = objPunter.GeneratePeriodLongRange(fMonth, tMonth, fYear, tYear);
                    break;
                case "SelTopPunterSummary":
                    username = context.Request["username"];
                    fromdate = context.Request["fromdate"];
                    todate = context.Request["todate"];
                    toprow = context.Request["toprow"];
                    orderby = context.Request["orderby"];
                    ordertype = context.Request["ordertype"];
                    html = objPunter.SelTopPunterSummary(username, fromdate, todate, toprow, orderby, ordertype);
                    break;
                case "SelTopPunterSummaryExportOnly":
                    username = context.Request["username"];
                    fromdate = context.Request["fromdate"];
                    todate = context.Request["todate"];
                    toprow = context.Request["toprow"];
                    orderby = context.Request["orderby"];
                    ordertype = context.Request["ordertype"];
                    objPunter.SelTopPunterSummaryExportOnly(username, fromdate, todate, toprow, orderby, ordertype);
                    break;
                case "SelTopPunterByCustId":
                    username = context.Request["username"];
                    fromdate = context.Request["fromdate"];
                    todate = context.Request["todate"];
                    CustId = context.Request["custid"];
                    orderby = context.Request["orderby"];
                    ordertype = context.Request["ordertype"];
                    html = objPunter.SelTopPunterByCustId(CustId, username, fromdate, todate, orderby, ordertype);
                    break;
                case "SelTopPunterBySporttype":
                    username = context.Request["username"];
                    fromdate = context.Request["fromdate"];
                    todate = context.Request["todate"];
                    CustId = context.Request["custid"];
                    Sporttype = context.Request["sporttype"];
                    orderby = context.Request["orderby"];
                    ordertype = context.Request["ordertype"];
                    html = objPunter.SelTopPunterBySporttype(CustId, username, Sporttype, fromdate, todate, orderby, ordertype);
                    break;
                case "SelTopPunterByBettype":
                    username = context.Request["username"];
                    fromdate = context.Request["fromdate"];
                    todate = context.Request["todate"];
                    CustId = context.Request["custid"];
                    Sporttype = context.Request["sporttype"];
                    bettype = context.Request["bettype"];
                    orderby = context.Request["orderby"];
                    ordertype = context.Request["ordertype"];
                    html = objPunter.SelTopPunterByBettype(CustId, username, Sporttype, bettype, fromdate, todate, orderby, ordertype);
                    break;
                case "SelSports":
                    fromdate = context.Request["fromdate"];
                    todate = context.Request["todate"];
                    html = objPunter.SelSports(fromdate, todate, "1");
                    break;
                case "SelLeagues":
                    fromdate = context.Request["fromdate"];
                    todate = context.Request["todate"];
                    Sporttype = context.Request["sporttype"];
                    html = objPunter.SelLeagues(fromdate, todate, Sporttype);
                    break;
                case "SelTopPunterSummaryByLeagues":
                    leagueid = context.Request["leagueid"];
                    fromdate = context.Request["fromdate"];
                    todate = context.Request["todate"];
                    toprow = context.Request["toprow"];
                    orderby = context.Request["orderby"];
                    ordertype = context.Request["ordertype"];
                    leaguename = context.Request["leaguename"];
                    html = objPunter.SelTopPunterSummaryByLeague(leagueid, leaguename, fromdate, todate, toprow, orderby, ordertype);
                    break;
                case "SelTopPunterByLeagueCustId":
                    username = context.Request["username"];
                    fromdate = context.Request["fromdate"];
                    todate = context.Request["todate"];
                    CustId = context.Request["custid"];
                    orderby = context.Request["orderby"];
                    ordertype = context.Request["ordertype"];
                    leagueid = context.Request["leagueid"];
                    Sporttype = context.Request["sporttype"];
                    html = objPunter.SelTopPunterByLeagueCustId(CustId, leagueid, username, Sporttype, fromdate, todate, orderby, ordertype);
                    break;
            }
        }
        catch (System.Exception ex)
        {

           Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            var h = new Hashtable {{"data", AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error)}};
            html = JsonUtil.ToJson(h);
            
           
        }
    
        
        context.Response.Write(html);
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
