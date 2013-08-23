using System;
using System.Data;
using System.Web;
using IBCUtils;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

/// <summary>
/// CITS
/// </summary>
public partial class CreditTerm_TermController : PageBase
{
    public string str = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            DataTable dt = AccountCredit.Acc_CreditTermSelGroupHeadlineByGroupID(Convert.ToInt32(Request["groupid"])).ResultSet.Tables[0];
            str += "<script type='text/javascript'>";
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                string groupid = Request["groupid"];
                str += "var creditgrade = '" + dr["creditgrade"] + "';";
                str += "var pre = '" + FormatHandler.FormatNumber2(dr["preagreeamount"]) + "';";
                str += "var installamt = '" + FormatHandler.FormatNumber2(dr["installmentamount"]) + "';";
                str += "var installdate = '" + dr["installmentdate"] + "';";
                str += "var remark = '" + dr["accountremark"] + "';";
                str += "var groupid = '" + dr["groupid"] + "';";
                str += "var curr = '" + CacheUtils.GetExchangeName(DNAide.Core.CastAide.AsInt32(dr["curr"])) + "';";
                str += "var checkerid = '" + DNAide.Core.CastAide.AsInt32(dr["checkerid"], 0) + "';";
                string isghl = (Convert.ToBoolean(dr["isgroupheadline"])) ? "1" : "0";
                str += "var isgroupheadline = '" + isghl + "';";
            }
            else str += "var groupid = '0';";
            string check = (string.IsNullOrEmpty(Request["check"])) ? "0" : Request["check"];
            str += "parent.setvalue('" + check + "');";
            str += "</script>";
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }
}