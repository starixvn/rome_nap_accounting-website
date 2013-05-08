using System;
using DNAide.Web;
using Sunplus.Accounting.DataAccess.Report;

public partial class Report_HRWLDetails : PageBase
{
    public string param = "";
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string MinDay = "";
    protected string FDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;

        string fdate = Request["fdate"];
        if (fdate == null) fdate = Request["wldate"];
        if (fdate == null) fdate = ToDay;
        //fdate = ReportHTML.ValidDate(fdate);

        param = "wldate=" + fdate + "&WL=WL" + "&bettype=Casino&type=Direct";

        //headerDatetime.Text = Sunplus.Accounting.Business.Report.Reports.GetDateTimeSingle(fdate, "Winloss Date", string.Empty);       

        #region Header
        int displaydays = 0;
        TimeSpan sp = DateTime.Today - new DateTime(2001, 01, 01);
        displaydays = sp.Days;
        ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        MinDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddMonths(-3));
        FDay = string.Format("{0:MM/dd/yyyy}", fdate);

        string evt = "SetEvtGetdate()";
        string strDateTimeSingle = Sunplus.Accounting.Business.Report.Reports.GetDateTimeSingle(fdate, "Date", string.Empty, displaydays, "../", string.Empty, evt);

        if (Request["type"] == "Direct")
            TemplateHandler.Assign(ref strDateTimeSingle, "strDispday", "");
        else
            TemplateHandler.Assign(ref strDateTimeSingle, "strDispday", "none");

        string db = Request["db"];
        if (db == null) db = "Rep";
        if (db == "Rep")
        {
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Rep", "checked=checked");
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Main", "");
        }
        else
        {
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Rep", "");
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Main", "checked=checked");
        }
        TemplateHandler.Assign(ref strDateTimeSingle, "db", db);
        TemplateHandler.RemoveBlock(ref strDateTimeSingle, "path");

        headerDatetime.Text = strDateTimeSingle;

        #endregion
    }
}
