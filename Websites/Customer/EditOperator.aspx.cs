using System;
using System.Data;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using DNAide.Data;


public partial class NewCustomer_EditOperator : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Literal1.Text = "Edit Operator";
        if (Session["AccId"] != null)
        {
            EditOperatorContentHtml.Text = GenEditInfo();
        }
    }
    private string GenEditInfo()
    {
        StringBuilder html = new StringBuilder();
        try
        {
            if (Request["custId"] == null) return "";
            string contentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/AddOperator.htm");
            DbResult ds = SelectCustomer.CustomerSelEditOperatorRpt(Convert.ToInt32(Request["custId"]));
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Hidden.htm", new string[] { "{name}", "{strValue}" }, new string[] { "operatorId", Request["custId"] }));            
            if (ds != null)
            {
                DataTable tbl = ds.ResultSet.Tables[0];
                TemplateHandler.Assign(ref contentTpl, "{title}", "Edit Operator");// + tbl.Rows[0]["username"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vUsername}", tbl.Rows[0]["username"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vFirstname}", tbl.Rows[0]["firstname"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vLastname}", tbl.Rows[0]["lastname"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vAddress}", tbl.Rows[0]["address"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vCity}", tbl.Rows[0]["city"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vState}", tbl.Rows[0]["state"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vCode}", tbl.Rows[0]["postal"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vPhone}", tbl.Rows[0]["phone"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vMobile}", tbl.Rows[0]["mobilephone"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vEmail}", tbl.Rows[0]["email"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vFax}", tbl.Rows[0]["fax"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vRole}", tbl.Rows[0]["roleid"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{vCountry}", tbl.Rows[0]["country"].ToString());
                TemplateHandler.Assign(ref contentTpl, "{txtuser}", "none");
                TemplateHandler.Assign(ref contentTpl, "{pwd-none}", "none");
                TemplateHandler.Assign(ref contentTpl, "{checkinput}", "");
                html.Append(contentTpl);
                
            }
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
}
