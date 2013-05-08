<%@ WebHandler Language="C#" Class="OpenToteSettlement" %>


using System.Text;
using System.Web;
using System.Web.SessionState;
using System;

public class OpenToteSettlement : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        string type = context.Request["type"];
        IServiceOpenToteSettlement obj = new ServiceOpenToteSettlement();
        StringBuilder html = new StringBuilder();
        string winlostdate = "", db = "", partnertype = "", poolcurrency = "";
        bool IsTest = false;
        switch (type)
        {
            case "LoadHeader":
                winlostdate = context.Request["winlostdate"];
                if (winlostdate == string.Empty)
                {
                    winlostdate = DateTime.Now.ToString("MM/dd/yyyy");
                }
                
                db = context.Request["db"];
                html.Append(obj.LoadHeader(winlostdate, db));
                break;
            case "WinlostHR_OpenTote":
                winlostdate = context.Request["winlostdate"];
                if (winlostdate == string.Empty)
                {
                    winlostdate = DateTime.Now.ToString("MM/dd/yyyy");
                }

                db = context.Request["db"];
                IsTest = context.Request["IsTest"] == "1";
                html.Append(obj.WinlostHR_OpenTote(winlostdate, db, IsTest));
                break;
            case "WinlostHR_OpenTote_Details":
                winlostdate = context.Request["winlostdate"];
                if (winlostdate == string.Empty)
                {
                    winlostdate = DateTime.Now.ToString("MM/dd/yyyy");
                }
                
                partnertype = context.Request["partnertype"];
                poolcurrency = context.Request["poolcurrency"];
                db = context.Request["db"];
                IsTest = context.Request["IsTest"] == "1";
                html.Append(obj.WinlostHR_OpenTote_Details(winlostdate, partnertype, poolcurrency, db, IsTest));
                break;
        }

        context.Response.Write(html.ToString());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}