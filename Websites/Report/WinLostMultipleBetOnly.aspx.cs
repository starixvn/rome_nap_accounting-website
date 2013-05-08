using System;
using System.Data;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;


/// <summary>
/// CITS
/// </summary>
public partial class Report_WinLostMultipleBetOnly : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Resources.Language.WinLostMultipleBetOnLy;
        header.Text = Resources.Language.WinLostMultipleBetOnLy;
        string lang = "E";
        if (!string.IsNullOrEmpty(Request["lang"])) lang = Request["lang"];
        content.Text = gencontent(lang);
    }
    string gencontent(string lang)
    {
        StringBuilder html = new StringBuilder();
        string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
        TemplateHandler.Assign(ref norow, "{tdno}", "6");

        try
        {
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WinLostMultipleBetOnlyAllHeader.htm", new string[] { "{username}", "{refno}", "{winlost}", "{position}", "{commission}", "{linetotal}" },
                new string[] { Resources.Language.UserName, Resources.Language.RefNo, Resources.Language.WinLost, Resources.Language.Position, Resources.Language.Commission, Resources.Language.LineTotal }));
            DateTime fdate, tdate;
            if (!(IBCUtils.Validator.ValidDateMMDDYYYYNotNull(Request["fdate"], out fdate).IsSuccess() && IBCUtils.Validator.ValidDateMMDDYYYYNotNull(Request["tdate"], out tdate).IsSuccess()))
            {
                TemplateHandler.Assign(ref norow, "{nodata}", AlertMessage.GetMsg(IbcError.DATE_INVALID_FORMAT_MSG, AlertMessage.MsgType.Error));
                TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
                html.Append(norow);
                html.Append("</table>");
                return html.ToString(); ;
            }

            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/WinLostMultipleBetOnlyAllRow.htm");
            DataTable dt = AccountReport.RptSelParlayBets(fdate, tdate).ResultSet.Tables[0];
            double total= 0, totalcomm = 0, totalline = 0;
            if (dt.Rows.Count != 0)
            {
                string odd_even = "";
                foreach (DataRow dr in dt.Rows)
                {
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    string row = rows.Clone().ToString();
                    totalline = FormatHandler.GetDouble(getvalue(dr["matchwinlost"])) + FormatHandler.GetDouble(getvalue(dr["positionwinlost"])) + FormatHandler.GetDouble(getvalue(dr["comwinlost"]));
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref row, "{username}", Convert.ToString(dr["username"]));
                    TemplateHandler.Assign(ref row, "{refno}", Convert.ToString(dr["refno"]));
                    TemplateHandler.Assign(ref row, "{lang}", lang);
                    TemplateHandler.Assign(ref row, "{winlost}", FormatHandler.FormatNumber20((getvalue(dr["matchwinlost"]))));
                    TemplateHandler.Assign(ref row, "{position}", FormatHandler.FormatNumber20(((getvalue(dr["positionwinlost"])))));
                    TemplateHandler.Assign(ref row, "{com}", FormatHandler.FormatNumber20(((getvalue(dr["comwinlost"])))));
                    TemplateHandler.Assign(ref row, "{linetotal}", FormatHandler.FormatNumber20(totalline));
                    html.Append(row);

                    totalcomm += (getvalue(dr["comwinlost"]));
                    total += totalline;
                }
                string totalrow = Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/WinLostMultipleBetOnlyFinalTotal.htm", new string[] { "{GrandTotal}", "{ComTotal}", "{LineTotal}"},
                    new string[] { Resources.Language.GrandToTal, FormatHandler.FormatNumber20(totalcomm), FormatHandler.FormatNumber20(total) });
                html.Append(totalrow);
            }
            else
            {
                TemplateHandler.Assign(ref norow, "{nodata}", Resources.Language.thereisnodata);
                TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
                html.Append(norow);
                html.Append("</table>");
            }
            html.Append("</table>");
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            TemplateHandler.Assign(ref norow, "{nodata}", AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error));
            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
            html.Append(norow);
            html.Append("</table>");
            return html.ToString(); ;
        }
        return html.ToString();
    }
    static double getvalue(object value)
    {
        if (value == null) return 0;
        if (Convert.IsDBNull(value)) return 0;
        return FormatHandler.GetDouble(value);
    }
}
