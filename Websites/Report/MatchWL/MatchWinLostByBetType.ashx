<%@ WebHandler Language="C#" Class="MatchWinLostByBetType" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Web.SessionState;
using IBCUtils.Web;
using DNAide.Data;
using DNAide.Core;
using System.Text;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL;
using BLL.Report;
using System.Collections;

public class MatchWinLostByBetType : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        try
        {
            context.Response.ContentType = "text/html";
            ReportBase report = new BLL.Report.MatchWL.MatchWLByBetTypeManager();
            Hashtable hash = new Hashtable();
            DateTime fdate = CastAide.AsDateTime(context.Request["fdate"], DateTime.Today);
            DateTime tdate = CastAide.AsDateTime(context.Request["tdate"], DateTime.Today);
            hash.Add("fdate", fdate);
            hash.Add("tdate", tdate);
            context.Response.Write(report.RenderHTML(hash));
        }
        catch (InvalidOperationException invalidOEx)
        {
            LogExtensions.ExceptionHandler(invalidOEx);
            HttpContext.Current.Server.Transfer(Sunplus.Accounting.Core.Common.Constants.Page.AccessDenied);
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
