<%@ WebHandler Language="C#" Class="GetDataPunter" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using BLL.Report.Punter;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;

using Newtonsoft.Json;

public class GetDataPunter : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string type = context.Request["type"];
        IServiceAgencyAnalysis objPunter = new ServiceAgencyAnalysis();
        string html = "";
        string username = "", fromdate = "", todate = "", toprow = "", orderby = "", ordertype = "", CustId = "";
        string fMonth = "", fYear = "", tMonth = "", tYear = "", Statistic = "" ;
        try 
        {
            fMonth = context.Request["fMonth"];
            fYear = context.Request["fYear"];
            tMonth = context.Request["tMonth"];
            tYear = context.Request["tYear"];
            Statistic = context.Request["Statistic"];
            username = context.Request["username"];
            
            switch (type)
            {
                case "DirectCustomerReport":
                    html = objPunter.DirectCustomerReport(fMonth, fYear, tMonth, tYear, Statistic, username);
                    break;
                case "GenerateChartByCustId":
                    CustId = context.Request["custid"];
                    GenerateChartAgencyAnalysis chart = new GenerateChartAgencyAnalysis();
                    html = chart.GenerateChartByCustId(CustId, username, fMonth, fYear, tMonth, tYear);
                        
                    break;
            }
            
        }
        catch (System.Exception ex)
        {

            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
          
            var h = new Hashtable { { "data", AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error) } };
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
