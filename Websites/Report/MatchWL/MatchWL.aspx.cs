using System;
using System.Web;
using System.Text;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using IBCUtils;
using DNAide.Core;
using DNAide.Web;
using System.Threading;
using Sunplus.Accounting.DataAccess.Report;

public partial class Report_MatchWL : PageBase
{
    protected override bool HasPermission()
    {
        return Functions.Report.AllowViewReport2A && base.HasPermission();
    }

    public string script = "";

    public StringBuilder strMatchWL = new StringBuilder(), strWLbyBettype = new StringBuilder(), strContent = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Request["IsExport"] == "1")
        {
            ExcelFile excelFile = new ExcelFile();
            var report2a = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.MatchWL.IMatchWL2aService>();
            bool hasData = false;
            excelFile = report2a.ExportMatchWL(ref hasData);
            if(hasData)
            {
                var fdate = Request["fdate"] ?? DateTime.Now.ToShortDateString();
                string filename = string.Format("MatchWL-{0}", fdate);
                Response.ContentType = "Application/x-msexcel";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".xls");
                excelFile.SaveXls(Response.OutputStream);
            }
        }
        else
        {
            Title = Sunplus.Accounting.Business.Report.Reports.Title;

            string fdate = "", type = "", db = "";
            string tplHeader = "";
            fdate = Request.Form["fdate"] ?? Request.QueryString["fdate"];

            #region Header
            int displaydays = 0;
            TimeSpan sp = DateTime.Today - new DateTime(2006, 01, 01);
            displaydays = sp.Days;

            string evt = "";
            string strDateTimeSingle = Sunplus.Accounting.Business.Report.Reports.GetDateTimeSingleMaxDays(fdate, "Date", string.Empty, displaydays,7, "../../", string.Empty, evt);
            TemplateHandler.RemoveBlock(ref strDateTimeSingle, "path");
            headerDatetime.InnerHtml = strDateTimeSingle;
        }
        #endregion


    }

     
}
