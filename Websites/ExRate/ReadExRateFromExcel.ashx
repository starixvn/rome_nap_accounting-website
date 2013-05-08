<%@ WebHandler Language="C#" Class="ReadExRateFromExcel" %>

using System;
using System.Web;
using System.Web.SessionState;

using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.ExChangeRate;

using Sunplus.Accounting.Entities;

public class ReadExRateFromExcel : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        IExchangeRateService rate = CommonBootStrapper.Locator.GetInstance<IExchangeRateService>();
        ResultBO result=rate.ReadExRateFromExcel();
        context.Response.Write(result.ErrorCode.ToString() + "|" + result.ErrorMessage);
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}