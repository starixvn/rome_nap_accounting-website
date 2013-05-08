using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL.Report;
using Sunplus.Accounting.Core.Common;

public partial class ExcelData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Set the appropriate ContentType.
        string path = "";
        string name = Request["path"];


        string user = "[" + AppSession.Username + "]";            

        Response.ClearContent();
        int startindex=name.LastIndexOf(@"\")+1;
        int endindex=name.Length - startindex;
        Response.AddHeader("content-disposition", "attachment;filename=" + name.Substring(startindex,endindex).Replace(user, ""));
        Response.ContentType = "Application/x-msexcel";


        
        Response.WriteFile(name);
        Response.End();
    }
}
