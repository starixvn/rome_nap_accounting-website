using System;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.Report;

public partial class Report_Racing_RacingGroupDaily : PageBase
{
    //public string strDate = "";
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string FDay = "";
    protected string TDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        FDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        TDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);        
    }
}