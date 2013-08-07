<%@ WebHandler Language="C#" Class="UpdateVirtualRateMain" %>

using System;
using System.Web;
using System.IO;
using System.Data;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.Web;
using DNAide.Data;
using DNAide.Core;


public class UpdateVirtualRateMain : IHttpHandler, IRequiresSessionState
{    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        try
        {
            if (CastAide.AsString(context.Request["bolUnLoadPage"])=="0")
            {
                int Index = CastAide.AsInt32(context.Request["Index"]);
                int errcode = 0;
                string errmsg = "";
                
                for (int i=1; i<=Index; i++)
                {
                    VirtualRate.UpdateVirtualRateIntoCustinfo(CastAide.AsString(context.Request["FileName"]), CastAide.AsInt32(context.Request["FileSize"]), i, ref errcode, ref errmsg);
                    if (errcode>0)
                    {
                        context.Response.Write(AlertMessage.GetMsg(errmsg, AlertMessage.MsgType.Error));
                        return;
                    }                    
                }
                VirtualRate.DelDataTmpExcel();
                context.Response.Write(AlertMessage.GetMsg("Updated successfully", AlertMessage.MsgType.Success));
            }
            else
                VirtualRate.DelDataTmpExcel();
        }
        catch (Exception ex)
        { 
            context.Response.Write(AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error));
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }      
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}
