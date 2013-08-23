using System;
using System.Collections;
using DNAide.Data;
using DNAide.Web;
using Sunplus.Accounting.DataAccess;

public partial class Report_rptCheckBet : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CheckBet.htm");
            string date = Request["fdate"];
            Hashtable p = new Hashtable();
            p.Add("winlostdate", date);

            DbResult result = DbHelper.Execute("Acc_checkhack2", p, DBType.ARCHIVE, 180);
            TemplateHandler.Assign(ref _s, "{title}", "CHECK BET for " + date);
            TemplateHandler.Assign(ref _s, "{checkTime}", DateTime.Now.ToString());
            TemplateHandler.Assign(ref _s, "{check1}", result.ResultSet.Tables[0].Rows[0]["check1"].ToString());
            TemplateHandler.Assign(ref _s, "{check2}", result.ResultSet.Tables[0].Rows[0]["check2"].ToString());
            this.sContent.Text = _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }
}