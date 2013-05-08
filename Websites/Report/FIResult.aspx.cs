using System;
using DNAide.Web;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class Report_FIResult : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string matchcode = Request.QueryString["matchcode"];
        string checktime = Request.QueryString["checktime"];
        string hdp2 = DNAide.Utils.FormatHandler.FormatNumber2C(CastAide.AsDecimal(Request.QueryString["hdp2"]), "E");
        lFIResult.Text = GenHTMLFIResult(matchcode, checktime, hdp2);

    }

    private string GenHTMLFIResult(string matchcode, string checktime, string hdp2)
    {
        string htmresult = "";
        htmresult = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/FIResult.htm");
        TemplateHandler.Assign(ref htmresult, "{lblMeetingName}", matchcode);
        TemplateHandler.Assign(ref htmresult, "{lblTime}", "Time");
        TemplateHandler.Assign(ref htmresult, "{lblResult}", "Result");
        TemplateHandler.RemoveBlock(ref htmresult, "noinfo");
        TemplateHandler.Assign(ref htmresult, "{checktime}", checktime);
        TemplateHandler.Assign(ref htmresult, "{realindex}",hdp2);
       
        return htmresult;
    }
}
