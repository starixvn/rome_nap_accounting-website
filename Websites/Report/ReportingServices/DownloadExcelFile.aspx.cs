using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL.Report;

public partial class Report_ReportingServices_DownloadExcelFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Set the appropriate ContentType.
        string path = "~/Report/ReportingServices/Data/";
        string name = Request["path"].Replace("---", "/").Replace("--", "_");
        Response.ClearContent();
        int startindex = name.LastIndexOf("/") + 1;
        int endindex = name.Length - startindex;
        Response.AddHeader("content-disposition", "attachment;filename=" + name.Substring(startindex, endindex) + ".xls");
        Response.ContentType = "Application/x-msexcel";
        //Get the physical path to the file.

        //path = "D:/Data";
        ReportingServices rpt = new ReportingServices();
        path = rpt.GetPath;
        string FilePath = path + name + ".xls";
        //string FilePath = MapPath(path + name + ".xls");
        //Write the file directly to the HTTP content output stream.
        Response.WriteFile(FilePath);
        Response.End();
    }
}
