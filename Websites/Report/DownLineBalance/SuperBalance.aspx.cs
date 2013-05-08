using System;
using System.Data;
using DNAide.Web;
using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using IBCUtils;
using DNAide.Core;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;
using SCCACHE = SharedCache.WinServiceCommon.Provider.Cache.IndexusDistributionCache;


using Sunplus.Core.LoggingLib;
/// <summary>
/// CITS
/// </summary>
public partial class SuperBalance : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request["IsExport"] == "1")
            {
                string db = (CastAide.AsString(Request["IsRep"], "0").Equals("1")) ? "rep" : "main";
                bool IsYesterday = CastAide.AsString(Request["IsYesterDay"], "0").Equals("1");
                string username = Request["username"];
                string firstletter = Request["letter"];
                string currency = Request["currency"];
                IServiceDownLineBalance balance = new ServiceDownLineBalance();
                string strData = balance.SuperBalance(db, IsYesterday, username, firstletter, currency);
                Sunplus.Accounting.Business.Report.Reports.ExportToExcel(strData, "Super-Balance");
                return;
            }

            #region
            int roleid = 4;
            int custid = 0;
            string isYesterday = "1";
            if (!string.IsNullOrEmpty(Request["custid"])) custid = Convert.ToInt32(Request["custid"]);
            if (!string.IsNullOrEmpty(Request["roleid"])) roleid = Convert.ToInt32(Request["roleid"]);
            isYesterday = Request["isYesterday"];
            isYesterday = (string.IsNullOrEmpty(isYesterday)) ? "1" : isYesterday;

            string tplDB = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Balance/BalanceForm.htm");
            string balanceDiff = TemplateHandler.CopyBlock(tplDB, "BalanceDiff");

            DataTable dt = null;
            dt = IBCProvider.ReportProvider.DownLineBalance().BalanceCompHistory(ReportUtils.BalanceUp.BalanceUpDay).ResultSet.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                //COMP_DIFF_STATEMENT = colorValue(DNAide.Core.CastAide.AsDecimal(dt.Rows[0]["Difference"]));
                TemplateHandler.Assign(ref balanceDiff, "{date}", ReportUtils.BalanceUp.BalanceUpDay.ToString("MMMM dd"));
                TemplateHandler.Assign(ref balanceDiff, "{diff}", colorValue(DNAide.Core.CastAide.AsDecimal(dt.Rows[0]["Difference"])));
                TemplateHandler.ReplaceBlock(ref tplDB, "BalanceDiff", balanceDiff);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref tplDB, "BalanceDiff");
            }

            //header.Text = "Super";

            if (isYesterday == "1")
            {
                TemplateHandler.Assign(ref tplDB, "{status-yestrans}", "checked='checked'");
                TemplateHandler.Assign(ref tplDB, "{status-alltrans}", "");
            }
            else
            {
                TemplateHandler.Assign(ref tplDB, "{status-yestrans}", "");
                TemplateHandler.Assign(ref tplDB, "{status-alltrans}", "checked='checked'");
            }

            TemplateHandler.Assign(ref tplDB, "{status-rep}", "checked='checked'");
            TemplateHandler.Assign(ref tplDB, "{status-main}", "");

            header.Text += tplDB;

            string strSearch = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/SearchReport.htm");
            TemplateHandler.RemoveBlock(ref strSearch, "status");
            TemplateHandler.Assign(ref strSearch, "{Keyword}", "CustId");
            TemplateHandler.Assign(ref strSearch, "{rowspan}", "3");
            TemplateHandler.Assign(ref strSearch, "{search-Val}", "All");
            TemplateHandler.Assign(ref strSearch, "{listAlphaValue}", AccCommon.GetListAlPhabet(""));
            TemplateHandler.Assign(ref strSearch, "{listCurrencyValue}", CustomerUtils.GetListCurrencyBySystem(AppSession.SYSTEM, false, true, false));
            TemplateHandler.Assign(ref strSearch, "{functionName}", "GetData");

            if (roleid >= 3)
                content.Text += strSearch;

            string script = "<script type='text/javascript'>";
            script += "var custid=" + custid + ";";
            script += "var roleid =" + roleid + ";";
            script += "var isYesterday =" + isYesterday + ";";
            if (custid > 0)
                script += "GetData();";
            script += "</script>";
            #endregion

            content.Text += script;
        }
        catch (InvalidOperationException invalidOEx)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(invalidOEx);
            
            RedirectDeny();
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            
        }
    }
    private string COMP_DIFF_STATEMENT
    {
        get
        {
            try
            {
                return CastAide.AsString(SCCACHE.SharedCache.Get("comp_diff_value"));
            }
            catch (Exception ex)
            {
                
                return string.Empty;
            }
        }
        set
        {
            SCCACHE.SharedCache.Add("comp_diff_value", value, DateTime.Now.AddMinutes(10));
        }
    }
    private string colorValue(decimal value)
    {
        if (value >= 0)
            return "<font color='blue'>" + FormatHandler.FormatNumber20(value) + "</font>";
        else return "<font color='red'>" + FormatHandler.FormatNumber20(value) + "</font>";
    }
    
}
