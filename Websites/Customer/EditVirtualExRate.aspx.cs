using System;
using System.Data;
using DNAide.Data;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils;



public partial class Customer_EditVirtualExRate : PageBase
{

    public string strArrExRate = string.Empty;
    public int current = 0;    
    protected string isCustInfo = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            isCustInfo = (Request["iscustinfo"] != null) ? "true" : "false";
        }
        Page.Title = "Edit Virtual Rate";
        if (Session["AccId"] != null)
        {
            if (Request["custId"] == null) return;
            editVirtualRateContent.Text = GetVirtualRate();
        }
    }
    private string GetVirtualRate()
    {
        string editTpl = string.Empty;
        try
        {
            editTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/EditVirtualRate.htm");
            string strTitle = Request["type"] + ": " + Request["uname"];
            TemplateHandler.Assign(ref editTpl, "{title-user}", strTitle);

            //DataResult ds = Select4Edit.VirtualRateSel(Request["custid"]);
            DbResult ds = Select4Edit.VirtualRateSel(Request["custid"]);
            string val = string.Empty;
            decimal actRate = 0;
            string strVal = string.Empty;
            if (ds != null)
            {
                //DataRow row = ds.ResultsSet.Tables[0].Rows[0];
                DataRow row = ds.ResultSet.Tables[0].Rows[0];
                actRate = FormatHandler.GetDecimal(row["actualrate"]);
                int min = Convert.ToInt32(FormatHandler.GetDecimal(row["Min_virtualMultiplier"]) * 100);
                int max = Convert.ToInt32(FormatHandler.GetDecimal(row["Max_virtualMultiplier"]) * 100);
                current = Convert.ToInt32(FormatHandler.GetDecimal(row["virtualMultiplier"]) * 100);
                decimal exRate = 0;
                int index = 0;
                strArrExRate = "var arrExRate=[";
                while (max >= min)
                {
                    exRate = ((max + 100) * actRate) / 100;
                    if (max > 0)
                    {
                        strVal = "+" + max + " (" + FormatHandler.FormatNumber2(exRate) + ")";
                    }
                    else
                    {
                        strVal = max + " (" + FormatHandler.FormatNumber2(exRate) + ")";
                    }
                    if (max == current)
                    {
                        strArrExRate += max + ",";
                        val += "<option selected>" + strVal + "</option>";
                    }
                    else
                    {
                        strArrExRate += max + ",";
                        val += "<option>" + strVal + "</option>";
                    }
                    if (max <= 10 && max > 5)
                        max -= 1;
                    else max -= 5;
                    index++;
                }
                strArrExRate = strArrExRate.Substring(0, strArrExRate.Length - 1) + "]";
            }
            TemplateHandler.Assign(ref editTpl, "{actual-rate-val}", FormatHandler.FormatNumber4(actRate));
            TemplateHandler.Assign(ref editTpl, "{virtual-rate-val}", val);
            editTpl += Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Hidden.htm", new string[] { "{name}", "{strValue}" }, new string[] { "custId", Request["custId"] });
            editTpl += Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Hidden.htm", new string[] { "{name}", "{strValue}" }, new string[] { "CustName", Request["uname"] });
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return editTpl;
    }
}
