using System;
using System.Web.UI;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class Customer_EditMaxExposure : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Title = "Edit Max Exposure";
            if (Session["AccId"] != null)
            {
                if (Request["custId"] == null) return;
                int custID = CastAide.AsInt32(Request["custId"], -1);
                string editTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/MaxExposure.htm");
                decimal matchLimit = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.GetMatchLimit(custID);
                TemplateHandler.Assign(ref editTpl, "{ex-Val}", DNAide.Utils.NumberFormat.DecFormat((matchLimit*100), 0));
                string strTitle = Request["type"] + ": " + Request["uname"];
                TemplateHandler.Assign(ref editTpl, "{title-user}", strTitle);
                editTpl += Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Hidden.htm", new string[] {"{name}", "{strValue}"},
                                                                new string[] {"custId", custID.ToString()});
                editTpl += Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Hidden.htm", new string[] {"{name}", "{strValue}"},
                                                                new string[] {"set", Request["set"]});
                EditMaxExContentHtml.Text = editTpl;
            }
        }
        catch(InvalidOperationException exception)
        {
            RedirectDeny();
        }
        catch (Exception)
        {

            throw;
        }
        
    }
}
