using System;
using System.Collections;
using DNAide.Core;
using BLL.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

/// <summary>
/// CITS
/// </summary>
public partial class DeleteBetReport : PageBase
{

    public DateTime fdate = DateTime.Today;
    protected void Page_Load(object sender, EventArgs e)
    {
        header.Text = "Delete Bet";
        ReportBase report = new rptDeleteBet();
        Hashtable hash = new Hashtable();
        string fdate = ReportHTML.ValidDate(Request["fdate"]);
        hash.Add("wldate", fdate);
        hash.Add("fdate", fdate);
        if (CastAide.AsInt32(Request["IsExport"]) == 1)
        {
            string fileName = "DeleteBet_" + fdate;
            hash.Add("FileName", fileName);
            report.Export2Excel(hash);
        }
        else
        {            
            SearchStandard.Text = report.CreateHeader(hash);
            content.Text = report.RenderHTML(hash);
        }
    }    
}
