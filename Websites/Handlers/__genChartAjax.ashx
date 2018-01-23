<%@ WebHandler Language="C#" Class="genChartAjax" %>

using System;
using System.Web;
using System.Web.SessionState;
using IBCUtils.Web;

public class genChartAjax : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        string content = "";
        string value = "";
        string value1 = "";
        value += "[[1,145],[2,245],[3,445],[4,745],[5,245]]";
        value1 += "[[1,45],[2,85],[3,345],[4,45],[5,345]]";
        content +="<div onclick=\"DrawChart('linechart'," +  value + "," + value1 + ");\">";
        content +="Draw chart</div>";
        content +="<div id=\"linechart\" style=\"width: 500px; height: 300px\">";
        content += "</div>";
        context.Response.Write(content);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}