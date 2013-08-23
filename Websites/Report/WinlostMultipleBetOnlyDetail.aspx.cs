using System;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

public partial class Report_WinlostMultipleBetOnlyDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder html = new StringBuilder();
        html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WinLostMultipleBetOnlyDetailAllHeader.htm", new string[] { "{refno}" }, new string[] { Resources.Language.RefNo + ": " + Request["refno"] }));
        string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
        TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
        TemplateHandler.Assign(ref norow, "{tdno}", "1");
        TemplateHandler.Assign(ref norow, "{nodata}", Resources.Language.thereisnodata);
        long refno = 0;
        string lang = "E";
        if (!string.IsNullOrEmpty(Request["refno"])) refno = Convert.ToInt64(Request["refno"]);
        if (!string.IsNullOrEmpty(Request["lang"])) lang = Request["lang"];
        DataTable dt = AccountReport.AccBetsSelBetCheckM(refno.ToString()).ResultSet.Tables[0];
        try
        {
            if (dt.Rows.Count > 0)
            {
                double hdp1 = 0, hdp2 = 0, hdp = 0;
                string betteam = "";
                int bettype = 0;
                string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostMultipleBetOnlyDetailAllRow.htm");
                string odd_even = "";
                foreach (DataRow dr in dt.Rows)
                {
                    StringBuilder str = new StringBuilder();
                    string row = rows.Clone().ToString();
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    dr["odds"] = Convert.IsDBNull(dr["odds"]) ? 0 : dr["odds"];
                    string strHome = CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty), strAway = CacheRef.GetTeamName(dr["awayid"]);
                    //str.Append("<b>" + dr["home"] + "-vs-" + dr["away"] + "</b><br/><font color=red>" + dr["typenamee"] + "</font>&nbsp;");
                    str.Append("<b>" + strHome + "-vs-" + strAway + "</b><br/><font color=red>" + CacheRef.GetBettype(dr["bettype"]) + "</font>&nbsp;");
                    bettype = Convert.ToInt32(dr["bettype"]);
                    betteam = Convert.ToString(dr["betteam"]);
                    hdp1 = FormatHandler.GetDouble(dr["hdp1"]);
                    hdp2 = FormatHandler.GetDouble(dr["hdp2"]);

                    #region bettype

                    switch (bettype)
                    {
                        case 1:
                            if (betteam == "h")
                            {
                                str.Append(strHome);
                                hdp = hdp2 - hdp1;
                                str.Append(" @ " + FormatHandler.FormatNumber2(hdp));
                            }
                            else if (betteam == "a")
                            {
                                str.Append(strAway);
                                hdp = hdp1 - hdp2;
                                str.Append(" @ " + FormatHandler.FormatNumber2(hdp));
                            }
                            str.Append("/" + FormatHandler.FormatNumber2(dr["odds"]) + "<br/>");
                            break;

                        case 2:
                            if (betteam == "h")
                            {
                                str.Append(Resources.Language.Odd.ToUpper() + "@");
                            }
                            else if (betteam == "a")
                            {
                                str.Append(Resources.Language.Even.ToUpper() + "@");
                            }
                            str.Append("/" + FormatHandler.FormatNumber2(dr["odds"]) + "<br/>");
                            break;

                        case 3:
                            if (betteam == "h")
                            {
                                str.Append(Resources.Language.Over.ToUpper() + "@");
                            }
                            else if (betteam == "a")
                            {
                                str.Append(Resources.Language.Under.ToUpper() + "@");
                            }
                            str.Append(FormatHandler.FormatNumber2(hdp1));
                            str.Append("/" + FormatHandler.FormatNumber2(dr["odds"]) + "<br/>");
                            break;

                        case 4:
                            str.Append(dr["hdp1"] + ":" + dr["hdp2"] + "@");
                            str.Append(FormatHandler.FormatNumber2(dr["odds"]) + "<br/>");
                            break;

                        case 5:
                        case 6:
                            str.Append(dr["betteam"] + "@");
                            str.Append(FormatHandler.FormatNumber2(dr["odds"]) + "<br/>");
                            break;

                        case 7:
                            if (betteam == "h")
                            {
                                str.Append(strHome + "@");
                                hdp = hdp2 - hdp1;
                                str.Append(FormatHandler.FormatNumber2(hdp));
                            }
                            else if (betteam == "a")
                            {
                                str.Append(strAway + "@");
                                hdp = hdp1 - hdp2;
                                str.Append(FormatHandler.FormatNumber2(hdp));
                            }
                            str.Append("/" + FormatHandler.FormatNumber2(dr["odds"]) + "<br/>");
                            break;
                    }

                    #endregion bettype

                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref row, "{refno}", str.ToString());
                    html.Append(row);
                }
            }
            else
            {
                html.Append(norow);
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            html.Append(norow);
        }
        html.Append("</table>");

        content.Text = html.ToString();
    }
}