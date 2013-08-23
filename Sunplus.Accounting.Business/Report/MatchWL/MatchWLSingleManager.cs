using System;
using System.Collections;
using System.Data;
using System.Text;

using DNAide.Core;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;

namespace BLL.Report.MatchWL
{
    public class MatchWLSingleManager : ReportBase
    {
        private readonly string _matchWinLossSingleKey = "accRptMatchWinLossSingleBet." + AppSession.AccUserName;
        private string msg = "<span class='error-message'>{content}</span>";

        public string LoadHeaderMatchWLSingle(Hashtable input)
        {
            string tpl = WebUtils.LoadTemplate("Report/MatchWL/MatchWLSingle/MatchWLSingleBetAllHeader.htm");
            try
            {
                TemplateHandler.Assign(ref tpl, "{Title}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MatchWinLostSingleBet"));
                TemplateHandler.Assign(ref tpl, "{fdate}", "<input type=\"text\" id=\"fdate\" readonly=\"readonly\" class=\"txtdate\" style=\"width: 100px; background-color: white;\" />");
                TemplateHandler.Assign(ref tpl, "{tdate}", "<input type=\"text\" id=\"tdate\" readonly=\"readonly\" class=\"txtdate\" style=\"width: 100px; background-color: white;\" />");
                TemplateHandler.Assign(ref tpl, "{mainContent}", RenderHTML(input));
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return tpl;
        }

        public string MatchWLSingle(Hashtable input)
        {
            string tpl = WebUtils.LoadTemplate("Report/MatchWL/MatchWLSingle/MatchWLSingleBetAllRow.htm");
            try
            {
                //Get data
                var rs =
                    IBCProvider.ReportProvider.MatchWL(DataAccess.DAOClient.AdoNet, DataAccess.DataSource.SqlServer).
                        MatchWLSingleBet(CastAide.AsDateTime(input["fdate"]), CastAide.AsDateTime(input["tdate"]));

                //Load Rows
                if (rs != null && rs.Tables.Count > 0 && rs.Tables[0].Rows.Count > 0)
                {
                    DataRowCollection dtRow = rs.Tables[0].Rows;
                    var html = new StringBuilder();
                    string tpl_row = TemplateHandler.CopyBlock(tpl, "Rows");
                    string odd_even = "bgodd";
                    double total = 0;
                    int index = 0;
                    foreach (DataRow dr in dtRow)
                    {
                        index++;
                        string row = tpl_row;
                        odd_even = (odd_even == "bgodd") ? "bgeven" : "bgodd";

                        TemplateHandler.Assign(ref row, "{Order}", NumberFormat.DecFormat(index, 0));
                        TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                        TemplateHandler.Assign(ref row, "{home}",
                                               CacheRef.GetTeamName(dr["homeid"]) +
                                               (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                        TemplateHandler.Assign(ref row, "{away}", CacheRef.GetTeamName(dr["awayid"]));
                        TemplateHandler.Assign(ref row, "{base}", NumberFormat.DecFormat(dr["matchwinlost"], 2));
                        html.Append(row);
                        total += CastAide.AsDouble(dr["matchwinlost"]);
                    }
                    string totalrow = WebUtils.LoadHtml("Report/MatchWL/MatchWLSingle/MatchWLSingleBetTotal.htm",
                                                        new[] { "{total}" }, new[] { NumberFormat.DecFormat(total, 2) });
                    html.Append(totalrow);
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", html.ToString());
                }
                else
                {
                    string norow = WebUtils.LoadTemplate("NoInformation.htm");
                    TemplateHandler.Assign(ref norow, "{tdno}", "5");
                    TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", norow);
                }
            }
            catch (InvalidOperationException invalidOEx)
            {
                throw invalidOEx;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            }

            string strTitle = "<center><b>Match Win Loss Single Bet from " +
                              CastAide.AsDateTime(input["fdate"]).ToShortDateString() + " to " +
                              CastAide.AsDateTime(input["tdate"]).ToShortDateString() + "</b></center>";
            SetCacheReport(_matchWinLossSingleKey, strTitle + tpl, DateTime.Now.AddMinutes(30));
            return tpl;
        }

        public void ExportExcelMatchWLSingle(Hashtable input)
        {
            string data = GetCachedReport(_matchWinLossSingleKey);
            if (string.IsNullOrEmpty(data))
            {
                data = MatchWLSingle(input);
            }
            data = "<table><tr><td>" + data + "</td></tr></table>";
            var content = new StringBuilder();
            content.Append("<html>");
            content.Append("<head>");
            content.Append("<META http-EQUIV='Content-Type' CONTENT='text/html; charset=unicode-utf8'>");
            content.Append("</head>");
            content.Append(ReportHTML.CSSExcelReport());
            content.Append(data);
            content.Append("<table><tr><td class='tdhidden'></td></tr></table>");
            content.Append("</html>");

            ExcelContent = content.ToString();
            base.Export2Excel(input);
        }

        public override string RenderHTML(Hashtable input)
        {
            return string.Empty;
        }
    }
}