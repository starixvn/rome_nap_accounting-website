using System;
using System.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Presentation;

/// <summary>
/// CITS
/// </summary>
public partial class CreditTerm_Creditterm_Group_Feed : PageBase
{

    public string strContent = "";
    public string strfilter = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        strfilter = CreditTermHTML.GetContentCreditTermFilter(HttpContext.Current);
        strContent = CreditTermHTML.GetContentCreditTerm(HttpContext.Current);
    }
}
