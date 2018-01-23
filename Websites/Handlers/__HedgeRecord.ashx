<%@ WebHandler Language="C#" Class="HedgeRecord" %>

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

public class HedgeRecord : BaseHandler
{
    public override void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        try
        {
            ReportBase report = new rptHedgeRecord();
            Hashtable hash = new Hashtable();
            DateTime fdate = CastAide.AsDateTime(context.Request["fdate"], DateTime.Today);
            DateTime tdate = CastAide.AsDateTime(context.Request["tdate"], DateTime.Today);
            hash.Add("fdate", FormatHandler.ResetStartDate(fdate));
            hash.Add("tdate", FormatHandler.ResetEndDate(tdate));
            context.Response.Write(report.RenderHTML(hash));
        }
        catch (Exception ex)
        {
           GotoErroPage(context);
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
