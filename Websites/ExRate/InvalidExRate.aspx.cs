using System;
using DNAide.Web;
using Sunplus.Accounting.Business.UI;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web.Controls;

public partial class InvalidExRate : PageBase
{
    protected override bool HasPermission()
    {
        return Functions.ExchangeRate.IsAllowExchangeRate && base.HasPermission();
    }

    protected string sSearch="";
    protected void Page_Load(object sender, EventArgs e)
    {
        InvalidExchangeRate.Text = "Invalid Ex.Rate"; 
        InvalidExRateListContentHtml.Text = ListInvalidExRate();
    }
  
    private string ListInvalidExRate()
    {
        string ListMain = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("ExRate/InvalidExRate.html");

        TemplateHandler.Assign(ref ListMain, "{FromDateCal}", Calendar.GenCalendarControl("fdate", "../", "fdateTrigger", DateTime.Now.ToShortDateString(), false, false, 0, 0, ""));//Phuong.Nguyen, show others day of next month
        TemplateHandler.Assign(ref ListMain, "{ToDateCal}", Calendar.GenCalendarControl("tdate", "../", "tdateTrigger", DateTime.Now.ToShortDateString(), true, false, 0, 0, ""));   //END
        sSearch = TemplateHandler.CopyBlock(ListMain, "SearchContent");
        TemplateHandler.RemoveBlock(ref ListMain, "SearchContent");
        TemplateHandler.ReplaceBlock(ref ListMain, "limit-content", GenerateHtml.GetContentInvalidExRate(DateTime.Now, DateTime.Now));
        return ListMain;      
    }    
 }
