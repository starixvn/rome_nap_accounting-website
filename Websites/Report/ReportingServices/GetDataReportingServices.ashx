<%@ WebHandler Language="C#" Class="GetDataReportingServices" %>

using System;
using System.Web;

public class GetDataReportingServices : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/html";
        context.Response.ClearContent();
        
        context.Response.ContentType = "Application/x-msexcel";
        string mode = context.Request["mode"];
        mode = string.IsNullOrEmpty(mode) ? "0" : mode;
        BLL.Report.ReportingServices report = new BLL.Report.ReportingServices();
        string path = context.Request["path"];
        context.Response.AddHeader("content-disposition", "attachment;filename=" + path);
        switch (mode)
        {
            case "0":                
                if (path != null)
                    path = path.Replace("^", "\\");
                else
                    path = "";

                
                string html = report.GetFilesDownLoad(path, true);
                context.Response.Write(html);
                break;
            case "1":
                {
                    //report.DownLoadFile(path);
                   
                    //Get the physical path to the file.
                    //string FilePath = System.Web.UI.Page.  MapPath("ListstaffforLuckyDraw2009.xls");
                    //Write the file directly to the HTTP content output stream.
                    context.Response.WriteFile(path);
                    context.Response.End();
                }
                break;
        }
        
    
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}