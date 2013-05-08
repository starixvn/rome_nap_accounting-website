using System;
using System.Collections;
using DNAide.Core;
using BLL.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using BLL.Report.MatchWL;

/// <summary>
/// CITS
/// </summary>
public partial class MatchWinLostByBetType : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime fdate = CastAide.AsDateTime(Request["fdate"], DateTime.Now);
        DateTime tdate = CastAide.AsDateTime(Request["tdate"], DateTime.Now);
        ReportBase report = new MatchWLSingleManager();
        Hashtable hash = new Hashtable();
        hash.Add("fdate", fdate);
        hash.Add("tdate", tdate);
        if (CastAide.AsInt32(Request["IsExport"]) == 1)
        {
            string fileName = "MatchWinLostByBetType";
            hash.Add("FileName", fileName);
            report.Export2Excel(hash);
        }
        else
        {
            content.Text = report.CreateHeader(hash);
        }        
    }    
}
    

