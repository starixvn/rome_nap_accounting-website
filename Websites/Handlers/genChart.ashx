<%@ WebHandler Language="C#" Class="genChart" %>

using System.Web;
using System.Web.SessionState;
using DNAide.Web;

public class genChart : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string iniChart="";
        string dataitem = "";
        string dataitems="";
        iniChart = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Chart/initchart.htm");

        /*dataitem = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Chart/dataitem.htm");
        TemplateHandler.Assign(ref dataitem, "{Values}", "[0,45],[1,95],[2,45],[3,945],[4,845],[5,445]");
        TemplateHandler.Assign(ref dataitem, "{DataName}", "Line 1");

        dataitems += dataitem;
        TemplateHandler.Assign(ref iniChart, "{DataItems}", dataitems);
        TemplateHandler.Assign(ref iniChart, "{XAxisValues}", "[0, \"\"],[1,\"Good\"],[2,\"All\"],[3,\"Avg Month\"],[4,\"Great\"],[5,\"Best\"],[6,\" \"]");*/
        context.Response.Write(iniChart);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
