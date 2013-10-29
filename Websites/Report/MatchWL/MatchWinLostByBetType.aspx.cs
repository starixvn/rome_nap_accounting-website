using System;
using System.Collections;
using BLL.Report;
using DNAide.Core;

public partial class MatchWinLostByBetType : PageBase
{
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string MinDay = "";
    protected string FDay = "";
    protected string TDay = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DateTime fdate = CastAide.AsDateTime(Request["fdate"], DateTime.Now);
            DateTime tdate = CastAide.AsDateTime(Request["tdate"], DateTime.Now);
            ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
            MinDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddMonths(-3));
            FDay = string.Format("{0:MM/dd/yyyy}", fdate);
            TDay = string.Format("{0:MM/dd/yyyy}", tdate);
            ReportBase report = new BLL.Report.MatchWL.MatchWLByBetTypeManager();
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
        catch (InvalidOperationException invalidOEx)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(invalidOEx);
            RedirectDeny();
        }
    }
}