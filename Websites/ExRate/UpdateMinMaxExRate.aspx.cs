using System;
using System.Data;
using System.Web;
using DNAide.Web;
using DNAide.Data;
using System.Text;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;

public partial class UpdateMinMaxExRate : PageBase
{
    protected override bool HasPermission()
    {
        return Functions.ExchangeRate.IsAllowExchangeRate && base.HasPermission();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        string content = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("ExRate/UpdateMinMaxExRate.html");
        string status = Request["status"].ToString();
        int fromid = string.IsNullOrEmpty(Request["frmid"]) ? 0 : CastAide.AsInt32(Request["frmid"]);
        int toid = string.IsNullOrEmpty(Request["toid"]) ? 0 : CastAide.AsInt32(Request["toid"]);
        DbResult dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Balance.ExchangeSelCurrency();

        TemplateHandler.Assign(ref content, "optionFromcur", ExChangeSel(fromid, dr));
        TemplateHandler.Assign(ref content, "optionTocur", ExChangeSel(toid, dr));

        if (status == "Edit")
        {
            TemplateHandler.Assign(ref content, "minvalue", Request["min"]);
            TemplateHandler.Assign(ref content, "maxvalue", Request["max"]);
            TemplateHandler.Assign(ref content, "disabled", "disabled");
        }
        else
        {
            TemplateHandler.Assign(ref content, "minvalue", "-1.00");
            TemplateHandler.Assign(ref content, "maxvalue", "1.00");
            TemplateHandler.Assign(ref content, "disabled", string.Empty);
        }
        TemplateHandler.Assign(ref content, "status", status);
        sContent.Text = content;
    }
    private string ExChangeSel(int id, DbResult datadr)
    {
        StringBuilder OptionContent = new StringBuilder();
        if (datadr.ResultSet.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in datadr.ResultSet.Tables[0].Rows)
            {
                string strOption = "<option value='{CurrencyId}' {selected}>{CurrencyName}</option>";
                if (id != 0 && id == CastAide.AsInt32(dr["exchangeid"]))
                {
                    TemplateHandler.Assign(ref strOption, "selected", "selected");
                }
                else
                {
                    TemplateHandler.Assign(ref strOption, "selected", string.Empty);
                }
                TemplateHandler.Assign(ref strOption, "CurrencyId", dr["exchangeid"]);
                TemplateHandler.Assign(ref strOption, "CurrencyName", dr["currency"]);
                OptionContent.Append(strOption);
            }
        }
        return OptionContent.ToString();
    }
}
