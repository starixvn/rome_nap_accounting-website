<%@ WebHandler Language="C#" Class="DailyCreatedCustomers" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Web.SessionState;
using IBCUtils.Web;
using DNAide.Data;
using DNAide.Core;
using System.Text;
using Sunplus.Accounting.DataAccess.IBCDLL;
using BLL.Report;
using System.Collections; 
using IBCUtils;

public class DailyCreatedCustomers : IHttpHandler, IRequiresSessionState
{ 
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        ReportBase report = new rptDailyCreatedCustomer();
        Hashtable hash = new Hashtable();
        if (CastAide.AsString(context.Request["ViewDL"]) != "1") // Search
        {
            DateTime fdate = CastAide.AsDateTime(context.Request["fdate"], DateTime.Today);
            DateTime tdate = CastAide.AsDateTime(context.Request["tdate"], DateTime.Today);            
            hash.Add("fromdate", FormatHandler.ResetStartDate(fdate));
            hash.Add("todate", FormatHandler.ResetEndDate(tdate));
            context.Response.Write(report.CreateHeader(hash));
        }
        else // View DownLine
        {
            DateTime date = CastAide.AsDateTime(context.Request["date"], DateTime.Today);
            int  roleid = CastAide.AsInt16(context.Request["roleid"]);
            hash.Add("date", date);
            hash.Add("roleid", roleid);
            context.Response.Write(report.RenderHTML(hash));
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