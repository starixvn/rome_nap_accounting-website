<%@ WebHandler Language="C#" Class="UploadVirtualRate" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Web.SessionState;
using Sunplus.Accounting.Business.ExportStatement;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.Web;
using DNAide.Data;
using DNAide.Core;
using System.Text;
using Sunplus.Accounting.DataAccess.IBCDLL;
using Sunplus.Accounting.Core.Extensions;

public class UploadVirtualRate : BaseHandler
{

    public override void ProcessRequest (HttpContext context) {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.ContentType = "text/html";

        IServiceExportStatement serviceExportStatement = new ServiceExportStatement();

        string mode = context.Request["mode"];
        mode = mode ?? "0";
        try
        {
            switch (mode)
            {
                case "0":
                    string sss = serviceExportStatement.ReadUsers();
                    context.Response.Write(sss);
                    break;
                case "1":
                    var validUses = context.Request["userslistvalid"].Replace("&amp;", "&");
                    context.Response.Write(
                        serviceExportStatement.AllStatement_UsersList(validUses,
                                                                      context.Request["fdate"], context.Request["tdate"]));
                    break;
            }
        }
        catch (System.Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            GotoErroPage(context);
        }

    }






    public bool IsReusable {
        get {
            return false;
        }
    }

}
