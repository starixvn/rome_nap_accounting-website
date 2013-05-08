using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Web;
using DNAide.Utils;
using DNAide.Collections;
using DNAide.Data;
using DNAide.Core;
using System.Data;
using Sunplus.Accounting.Core.Cache;

public partial class Report_ViewFinanceResult : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string main = String.Empty;
        long transId = CastAide.AsInt64(Request["transid"], 0);
        if (transId > 0)
        {
            string tplFinance = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("BetList/Financial.htm");
            TemplateHandler.Assign(ref tplFinance, "lblTime", "Time");
            TemplateHandler.Assign(ref tplFinance, "lblResult", "Results");
            TemplateHandler.Assign(ref tplFinance, "lblSportName", "Financials");

            DbResult dbResult = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Betlist.BetList_SelMatchResultFinance(transId);
            DataTable tblFIResult = dbResult.ResultSet.Tables[0];
            if (tblFIResult.Rows.Count > 0)
            {
                //get data
                DataRow row = tblFIResult.Rows[0];
                string marketName = CacheRef.GetLeagueName(row["leagueid"]);
                //string indexName = CastAide.AsString(row["indexName"]);
                string indexName = CacheRef.GetFncIndexName(row["homeid"]);
                string settleTime = CastAide.AsString(row["settleTime"]);
                decimal result = CastAide.AsDecimal(row["hdp2"]);

                //fill data                
                
                TemplateHandler.Assign(ref tplFinance, "marketname", marketName + (String.IsNullOrEmpty(marketName) ? "" : " - "));
                TemplateHandler.Assign(ref tplFinance, "indexname", indexName);
                TemplateHandler.Assign(ref tplFinance, "date", settleTime);
                TemplateHandler.Assign(ref tplFinance, "resultfi", FormatHandler.FormatNumber4(result, ""));
            }
            else
            {
                tplFinance = "<center>" + Resources.Language.thereisnodata + "</center>";
            }

            main += tplFinance;
        }
        this.lResult.Text = main;

    }
}
