using System;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// Summary description for MatchWL
/// </summary>

namespace BLL.Report.MatchWL
{
    public class MatchWLManager
    {
        #region MatchWL2a

        public string MatchWL2a(string db, string type, string fdate)
        {
            string colspanHeader = "14";

            var html = new StringBuilder();

            string norow = WebUtils.LoadTemplate("NoInformation.htm");
            TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
            DateTime wldate = CastAide.AsDateTime(fdate, DateTime.Today);
            decimal winlost_8rm = 0, winlost_8rm_virtual = 0, FI_3rdShare = 0;
            try
            {
                #region //header , rows, totalrows

                string tpl = WebUtils.LoadTemplate("WinLost/Report2AMatchWL.htm");
                string tplHeader = TemplateHandler.CopyBlock(tpl, "Header");
                //Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate("WinLost/MatchWinLoss2AllHeaderV2.htm");
                string rows = TemplateHandler.CopyBlock(tpl, "Rows");
                //Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate("WinLost/MatchWinLoss2AllRowV2.htm");

                switch (type)
                {
                    case "v":
                    case "a":
                        TemplateHandler.Assign(ref tplHeader, "{WidthMath}", "405");
                        TemplateHandler.Assign(ref rows, "{WidthHome}", "192");
                        TemplateHandler.Assign(ref rows, "{WidthAway}", "192");
                        break;

                    default:
                        TemplateHandler.Assign(ref tplHeader, "{WidthMath}", "250");
                        TemplateHandler.Assign(ref rows, "{WidthHome}", "120");
                        TemplateHandler.Assign(ref rows, "{WidthAway}", "120");
                        break;
                }

                string tplTotal1 = TemplateHandler.CopyBlock(tpl, "Total1");
                //Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate("WinLost/MatchWinLoss2Total1V2.htm"); // for bettype and leaguename
                string tplTotal2 = TemplateHandler.CopyBlock(tpl, "Total2");
                //Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate("WinLost/MatchWinLoss2Total2V2.htm");// for 8RM
                string tplTotal3 = TemplateHandler.CopyBlock(tpl, "Total3");
                //Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate("WinLost/MatchWinLoss2Total3V2.htm"); // for grand total

                TemplateHandler.Assign(ref tplHeader, "{fdate}", wldate.ToShortDateString());
                TemplateHandler.Assign(ref tplHeader, "{tdate}", wldate.ToShortDateString());
                string[] arrBlock_V = { "Virtual-Comm", "ExRisk", "Virtual-ExRisk", "Virtual-LineTotal", "Variance" };
                string[] arrBlock_A = { "Comm", "ExRisk", "Virtual-ExRisk", "LineTotal", "Variance" };
                string[] arrBlock_VA = {
                                           "Comm", "ExRisk", "LineTotal", "Variance", "Virtual-Comm", "Virtual-ExRisk",
                                           "Virtual-LineTotal"
                                       };
                string[] arrBlock_VA_Exrisk_VirtualExrisk = { "ExRisk", "Virtual-ExRisk" };

                switch (type)
                {
                    case "a":
                        Reports.RemoveBlock(ref tplHeader, arrBlock_V);
                        Reports.RemoveBlock(ref rows, arrBlock_V);
                        Reports.RemoveBlock(ref tplTotal1, arrBlock_V);
                        Reports.RemoveBlock(ref tplTotal2, arrBlock_V);
                        Reports.RemoveBlock(ref tplTotal3, arrBlock_V);

                        Reports.RemoveTag(ref tplHeader, arrBlock_A);
                        Reports.RemoveTag(ref rows, arrBlock_A);
                        Reports.RemoveTag(ref tplTotal1, arrBlock_A);
                        Reports.RemoveTag(ref tplTotal2, arrBlock_A);
                        Reports.RemoveTag(ref tplTotal3, arrBlock_A);

                        TemplateHandler.Assign(ref norow, "{tdno}", "9");
                        TemplateHandler.Assign(ref tplHeader, "{cols}", "9");
                        TemplateHandler.Assign(ref tplTotal1, "{cols}", "8");
                        TemplateHandler.Assign(ref tplTotal2, "{cols}", "8");
                        colspanHeader = "9";
                        break;

                    case "v":
                        Reports.RemoveBlock(ref tplHeader, arrBlock_A);
                        Reports.RemoveBlock(ref rows, arrBlock_A);
                        Reports.RemoveBlock(ref tplTotal1, arrBlock_A);
                        Reports.RemoveBlock(ref tplTotal2, arrBlock_A);
                        Reports.RemoveBlock(ref tplTotal3, arrBlock_A);

                        Reports.RemoveTag(ref tplHeader, arrBlock_V);
                        Reports.RemoveTag(ref rows, arrBlock_V);
                        Reports.RemoveTag(ref tplTotal1, arrBlock_V);
                        Reports.RemoveTag(ref tplTotal2, arrBlock_V);
                        Reports.RemoveTag(ref tplTotal3, arrBlock_V);

                        TemplateHandler.Assign(ref norow, "{tdno}", "9");
                        TemplateHandler.Assign(ref tplHeader, "{cols}", "9");
                        TemplateHandler.Assign(ref tplTotal1, "{cols}", "7");
                        TemplateHandler.Assign(ref tplTotal2, "{cols}", "8");
                        colspanHeader = "9";
                        break;

                    default:
                        Reports.RemoveBlock(ref tplHeader, arrBlock_VA_Exrisk_VirtualExrisk);
                        Reports.RemoveBlock(ref rows, arrBlock_VA_Exrisk_VirtualExrisk);
                        Reports.RemoveBlock(ref tplTotal1, arrBlock_VA_Exrisk_VirtualExrisk);
                        Reports.RemoveBlock(ref tplTotal2, arrBlock_VA_Exrisk_VirtualExrisk);
                        Reports.RemoveBlock(ref tplTotal3, arrBlock_VA_Exrisk_VirtualExrisk);

                        Reports.RemoveTag(ref tplHeader, arrBlock_VA);
                        Reports.RemoveTag(ref rows, arrBlock_VA);
                        Reports.RemoveTag(ref tplTotal1, arrBlock_VA);
                        Reports.RemoveTag(ref tplTotal2, arrBlock_VA);
                        Reports.RemoveTag(ref tplTotal3, arrBlock_VA);

                        TemplateHandler.Assign(ref norow, "{tdno}", "12");
                        TemplateHandler.Assign(ref tplHeader, "{cols}", "12");
                        TemplateHandler.Assign(ref tplTotal1, "{cols}", "9");
                        TemplateHandler.Assign(ref tplTotal2, "{cols}", "9");
                        colspanHeader = "12";
                        break;
                }

                html.Append(tplHeader);

                #endregion //header , rows, totalrows

                DbResult rs = null;
                if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                    rs = IBCProvider.ReportProvider.MatchWL(DataAccess.DAOClient.AdoNet, DataAccess.DataSource.SqlServer).MatchWL(wldate, db);
                else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                    rs = IBCProvider.ReportProvider.MatchWL(DataAccess.DAOClient.AdoNet, DataAccess.DataSource.SqlServer).MatchWL14(wldate);
                else
                    rs = IBCProvider.ReportProvider.MatchWL(DataAccess.DAOClient.AdoNet, DataAccess.DataSource.SqlServer).MatchWL_WH(wldate);

                DataTable dt1 = rs.ResultSet.Tables[0];
                DataTable dt2 = rs.ResultSet.Tables[1];

                //all rows

                string odd_even = "";
                string leaguename = "empty", bettypename = "";
                bool llive = false;
                decimal grandtotal = 0, com = 0, grandvirtual = 0, grandvariance = 0, comvirtual = 0;
                decimal grandtotalbet = 0, combet = 0, grandvirtualbet = 0, grandvariancebet = 0, comvirtualbet = 0;
                decimal grandtotalbetb = 0, combetb = 0, grandvirtualbetb = 0, grandvariancebetb = 0, comvirtualbetb = 0;

                int index = 0;
                string oldleague = "";
                string color1 = "", color2 = "";
                int count = 0;

                // for (int k = 0; k <= 2; k++ )
                {
                    html.Append("<tbody>");
                    foreach (DataRow dr in dt1.Rows)
                    {
                        count++;

                        #region //total footer

                        grandtotal += CastAide.AsDecimal(dr["LineTotal"]);
                        //CastAide.AsDecimal(dr["winlost"]) + CastAide.AsDecimal(dr["pos"]) +  CastAide.AsDecimal(dr["comm"]);
                        com += CastAide.AsDecimal(dr["comm"]);
                        grandvirtual += CastAide.AsDecimal(dr["VirtualLineTotal"]);
                        grandvariance += (CastAide.AsDecimal(dr["LineTotal"]) -
                                          CastAide.AsDecimal(dr["VirtualLineTotal"]));

                        comvirtual += CastAide.AsDecimal(dr["VirtualComm"]);

                        #endregion //total footer

                        #region bettype or live

                        if ((!CacheRef.GetBettype(dr["bettype"]).Equals(bettypename))
                            || (CastAide.AsBoolean(dr["liveindicator"], false) != llive)
                            || (CastAide.AsString(dr["leaguenametc"]) != oldleague)
                            )
                        {
                            bettypename = CacheRef.GetBettype(dr["bettype"]);
                            llive = CastAide.AsBoolean(dr["liveindicator"], false);
                            if (index != 0)
                            {
                                string totalrow = tplTotal1.Clone().ToString();
                                color1 = "#efefef";
                                color2 = "#dedede";
                                ToTalRow(ref totalrow, combet, grandtotalbet, grandvirtualbet, grandvariancebet, color1,
                                         color2, "TYPE", "", "ralign", comvirtualbet);
                                html.Append(totalrow);
                                combet = 0;
                                grandtotalbet = 0;
                                comvirtualbet = 0;
                                grandvirtualbet = 0;
                                grandvariancebet = 0;
                            }
                            index++;
                        }

                        #endregion bettype or live

                        #region leaguename

                        leaguename = Convert.ToString(dr["leaguenametc"]);
                        if (leaguename != oldleague)
                        {
                            if (count > 1)
                            {
                                string totalrow = tplTotal1.Clone().ToString();

                                color1 = "#deffde";
                                color2 = "#cdffcd";
                                ToTalRow(ref totalrow, combetb, grandtotalbetb, grandvirtualbetb, grandvariancebetb,
                                         color1, color2, oldleague.ToUpper(), "", "ralign", comvirtualbetb);
                                html.Append(totalrow);
                                combetb = 0;
                                grandtotalbetb = 0;
                                grandvirtualbetb = 0;
                                grandvariancebetb = 0;
                                comvirtualbetb = 0;
                            }
                            oldleague = leaguename;
                        }

                        #endregion leaguename

                        #region total of bettype or live

                        combet += CastAide.AsDecimal(dr["comm"]);
                        grandtotalbet += CastAide.AsDecimal(dr["LineTotal"]);
                        grandvirtualbet += CastAide.AsDecimal(dr["VirtualLineTotal"]);
                        grandvariancebet += (CastAide.AsDecimal(dr["LineTotal"]) -
                                             CastAide.AsDecimal(dr["VirtualLineTotal"]));

                        comvirtualbet += CastAide.AsDecimal(dr["VirtualComm"]);

                        #endregion total of bettype or live

                        #region total of league

                        combetb += CastAide.AsDecimal(dr["comm"]);
                        grandtotalbetb += CastAide.AsDecimal(dr["LineTotal"]);
                        //CastAide.AsDecimal(dr["winlost"]) + CastAide.AsDecimal(dr["pos"]) + CastAide.AsDecimal(dr["comm"]);
                        grandvirtualbetb += CastAide.AsDecimal(dr["VirtualLineTotal"]);
                        grandvariancebetb += (CastAide.AsDecimal(dr["LineTotal"]) -
                                              CastAide.AsDecimal(dr["VirtualLineTotal"]));

                        comvirtualbetb += CastAide.AsDecimal(dr["VirtualComm"]);

                        #endregion total of league

                        #region earch row

                        string row = rows.Clone().ToString();
                        TemplateHandler.Assign(ref row, "{style}", "style='height:30px;' bgcolor='white'");
                        string bettype = "";
                        //rows
                        if (CastAide.AsBoolean(dr["liveindicator"], false)) bettype += "<b>!Live- </b>";
                        bettype += CacheRef.GetBettype(dr["bettype"]) + "<br>" + Convert.ToString(dr["matchcode"]);
                        TemplateHandler.Assign(ref row, "{bettype}", bettype);
                        string eventdate = CastAide.AsString(dr["eventdate"]);
                        string kickofftime = CastAide.AsString(dr["kickofftime"]);
                        string strTime = "";

                        if (CastAide.AsDateTime(dr["eventdate"]).ToShortDateString() !=
                            CastAide.AsDateTime(dr["kickofftime"]).ToShortDateString())
                            strTime = CastAide.AsDateTime(eventdate).ToShortDateString() + "<br>" + kickofftime;
                        else
                            strTime = CastAide.AsDateTime(eventdate).ToShortDateString() + "<br>" +
                                      CastAide.AsDateTime(kickofftime).ToShortTimeString();
                        TemplateHandler.Assign(ref row, "{date}", strTime);

                        if (CastAide.AsInt32(dr["awayid"]) == 0)
                        {
                            string td = "<td colspan='3' align='center'>" + CacheRef.GetTeamName(dr["homeid"]) + "</td>";
                            TemplateHandler.ReplaceBlock(ref row, "hr", td);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "{away}", CacheRef.GetTeamName(dr["awayid"]));
                            TemplateHandler.Assign(ref row, "{home}",
                                                   CacheRef.GetTeamName(dr["homeid"]) +
                                                   (CastAide.AsBoolean(dr["isNeutral"])
                                                        ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral")
                                                        : string.Empty));
                        }

                        TemplateHandler.Assign(ref row, "{winlost}", NumberFormat.DecFormat((dr["winlost"]), 2));
                        TemplateHandler.Assign(ref row, "{position}", NumberFormat.DecFormat((dr["pos"]), 2));
                        TemplateHandler.Assign(ref row, "{com}", NumberFormat.DecFormat((dr["comm"]), 2));

                        TemplateHandler.Assign(ref row, "{virtual-com}", NumberFormat.DecFormat((dr["VirtualComm"]), 2));

                        TemplateHandler.Assign(ref row, "{linetotal}",
                                               "<b>" + NumberFormat.DecFormat((dr["linetotal"]), 2) + "</b>");
                        TemplateHandler.Assign(ref row, "{virtual-linetotal}",
                                               "<b>" + NumberFormat.DecFormat((dr["VirtualLineTotal"]), 2) + "</b>");
                        double variance = CastAide.AsDouble(dr["LineTotal"]) - CastAide.AsDouble(dr["VirtualLineTotal"]);
                        TemplateHandler.Assign(ref row, "{variance}", NumberFormat.DecFormat(variance, 2));
                        if (count <= 1)
                        {
                            TemplateHandler.Assign(ref row, "{140}", "140");
                            TemplateHandler.Assign(ref row, "{20}", "20");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "{140}", "");
                            TemplateHandler.Assign(ref row, "{20}", "");
                        }

                        html.Append(row);

                        #endregion earch row
                    }
                }

                #region total of bettype

                string totalrowend = tplTotal1.Clone().ToString();
                color1 = "";
                color2 = "";
                ToTalRow(ref totalrowend, combet, grandtotalbet, grandvirtualbet, grandvariancebet, color1, color2,
                         "TYPE", "", "calign", comvirtualbet);
                html.Append(totalrowend);
                combet = 0;
                grandtotalbet = 0;
                grandvirtualbet = 0;
                grandvariancebet = 0;

                #endregion total of bettype

                #region total of league

                totalrowend = tplTotal1.Clone().ToString();
                color1 = "#deffde";
                color2 = "";
                ToTalRow(ref totalrowend, combetb, grandtotalbetb, grandvirtualbetb, grandvariancebetb, color1, color2,
                         oldleague.ToUpper(), "", "calign", comvirtualbetb);
                html.Append(totalrowend);
                combetb = 0;
                grandtotalbetb = 0;
                grandvirtualbetb = 0;
                grandvariancebetb = 0;

                #endregion total of league

                #region grand total

                totalrowend = tplTotal1.Clone().ToString();
                color1 = "#88aaff";
                color2 = "white";
                ToTalRow(ref totalrowend, com, grandtotal, grandvirtual, grandvariance, color1, color2, "GRAND", "GRAND",
                         "ralign", comvirtual);
                html.Append(totalrowend);

                #endregion grand total

                #region winlost_8rm

                winlost_8rm = CastAide.AsDecimal(rs.Outputs["winlost_8rm"]);
                winlost_8rm_virtual = winlost_8rm;
                decimal Variance8RM = (winlost_8rm - winlost_8rm_virtual);

                TemplateHandler.Assign(ref tplTotal2, "{Total-LineTotal}", NumberFormat.DecFormat(winlost_8rm, 2));
                TemplateHandler.Assign(ref tplTotal2, "{Total-Virtual-LineTotal}",
                                       NumberFormat.DecFormat(winlost_8rm_virtual, 2));
                TemplateHandler.Assign(ref tplTotal2, "{Total-Variance}", NumberFormat.DecFormat(Variance8RM, 2));
                html.Append(tplTotal2);

                #endregion winlost_8rm

                #region Mixparlay - OutRigth - FI - HR - 12BET - 3rdparty - Casino;

                grandtotal = grandtotal + winlost_8rm;
                grandvirtual = grandvirtual + winlost_8rm_virtual;
                decimal parlayGrandTotal = 0,
                        parlayTotal = 0,
                        parlayCommission = 0,
                        parlayVirtualCommission = 0,
                        parlayVirtualTotal = 0,
                        parlayVarianceTotal = 0;
                decimal palayvariance = 0;

                FI_3rdShare = CastAide.AsDecimal(rs.Outputs["fi_3rdshare"]);
                DataRow drFI_3rdShare = dt2.NewRow();
                drFI_3rdShare["bettype"] = "FI 3rd-Party";
                drFI_3rdShare["LineTotal"] = FI_3rdShare;
                dt2.Rows.Add(drFI_3rdShare);

                //string WL = context.Request["WL"];
                foreach (DataRow dr in dt2.Rows)
                {
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    string row = rows.Clone().ToString();
                    TemplateHandler.Assign(ref row, "{style}", "bgcolor='white'");
                    row = row.Replace("-vs-", "");

                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);

                    string bettype = CastAide.AsString(dr["bettype"]);

                    #region Link to Casino and ToteHR

                    if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                    {
                        if (bettype.ToLower().Equals("fodds hr"))
                        {
                            bettype = "<a href='../FOddsHRWLDetails.aspx?wldate=" +
                                      Convert.ToDateTime(dr["winlostdate"]).ToShortDateString() + "&bettype=" + bettype +
                                      "&db=" + db + "' target='_blank'>" + bettype + "</a>";
                        }
                        if (bettype.ToLower().Equals("tote hr"))
                        {
                            bettype = "<a href='../ToteHRWLDetails.aspx?wldate=" +
                                      Convert.ToDateTime(dr["winlostdate"]).ToShortDateString() + "&bettype=" + bettype +
                                      "&db=" + db + "' target='_blank'>" + bettype + "</a>";
                        }
                        if (bettype.ToLower().Equals("finance"))
                        {
                            bettype = "<a href='../FIWLDetails_1.aspx?wldate=" +
                                      Convert.ToDateTime(dr["winlostdate"]).ToShortDateString() + "&bettype=" + bettype +
                                      "&db=" + db + "' target='_blank'>" + bettype + "</a>";
                        }
                    }

                    if (bettype.ToLower().Equals("casino") || bettype.ToLower().Equals("p2p"))
                    {
                        bettype = "<a href='../CasinoDetail_1.aspx?wldate=" +
                                  Convert.ToDateTime(dr["winlostdate"]).ToShortDateString() + "&bettype=" + bettype +
                                  "&db=" + db + "' target='_blank'>" + bettype + "</a>";
                    }
                    if (bettype.ToLower().Equals("casino mini game"))
                    {
                        bettype = "<a href='../MiniGamesDetail_1.aspx?wldate=" +
                                  Convert.ToDateTime(dr["winlostdate"]).ToShortDateString() + "&db=" + db +
                                  "' target='_blank'>" + bettype + "</a>";
                    }

                    #endregion Link to Casino and ToteHR

                    if (CastAide.AsString(dr["bettype"]).ToLower().Equals("12bet agents")
                        || CastAide.AsString(dr["bettype"]).ToLower().Equals("fi 3rd-party"))
                    {
                        if (CastAide.AsDecimal(dr["LineTotal"]) == 0 && CastAide.AsDecimal(dr["VirtualLineTotal"]) == 0)
                            row = "";

                        if (CastAide.AsString(dr["bettype"]).ToLower().Equals("12bet agents")) //#dbeef3
                        {
                            TemplateHandler.Assign(ref row, "{style}", "bgcolor='#ffff99'");
                            continue;
                        }
                        else if (CastAide.AsString(dr["bettype"]).ToLower().Equals("fi 3rd-party"))
                        {
                            TemplateHandler.Assign(ref row, "{style}", "bgcolor='#dbeef3'");
                            bettype = "<div title='Finance 3rd-Party Share'>" + bettype + "</div>";
                        }
                        string td = "";
                        switch (type)
                        {
                            case "a":
                                td = "<td colspan='7' align='center'></td>";
                                break;

                            case "v":
                                td = "<td colspan='7' align='center'></td>";
                                break;

                            default:
                                td = "<td colspan='8' align='center'></td>";
                                break;
                        }

                        TemplateHandler.ReplaceBlock(ref row, "S12bet", td);
                    }
                    else
                    {
                        TemplateHandler.Assign(ref row, "{style}", "");
                        if (CastAide.AsString(dr["bettype"]).ToLower().Equals("finance"))
                        {
                            if (CastAide.AsDecimal(dr["LineTotal"]) == 0 &&
                                CastAide.AsDecimal(dr["VirtualLineTotal"]) == 0)
                                row = "";
                        }
                    }

                    TemplateHandler.Assign(ref row, "{bettype}", bettype);
                    TemplateHandler.Assign(ref row, "{date}", CastAide.AsDateTime(dr["winlostdate"]).ToShortDateString());
                    TemplateHandler.Assign(ref row, "{home}", "");
                    TemplateHandler.Assign(ref row, "-vs-", "");
                    TemplateHandler.Assign(ref row, "{away}", "");
                    TemplateHandler.Assign(ref row, "{winlost}", NumberFormat.DecFormat((dr["winlost"]), 2));
                    TemplateHandler.Assign(ref row, "{position}", NumberFormat.DecFormat((dr["pos"]), 2));
                    TemplateHandler.Assign(ref row, "{com}", NumberFormat.DecFormat((dr["comm"]), 2));

                    if (CastAide.AsString(dr["bettype"]).ToLower().Equals("fodds hr")
                        || CastAide.AsString(dr["bettype"]).ToLower().Equals("tote hr")
                        || CastAide.AsString(dr["bettype"]).ToLower().Equals("finance")
                        || CastAide.AsString(dr["bettype"]).ToLower().Equals("casino")
                        || CastAide.AsString(dr["bettype"]).ToLower().Equals("p2p")
                        || CastAide.AsString(dr["bettype"]).ToLower().Equals("fi 3rd-party"))
                    {
                        TemplateHandler.Assign(ref row, "{virtual-com}", "N/A");
                        TemplateHandler.Assign(ref row, "{virtual-linetotal}", "N/A");
                        TemplateHandler.Assign(ref row, "{variance}", "N/A");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref row, "{virtual-com}", NumberFormat.DecFormat((dr["VirtualComm"]), 2));
                        TemplateHandler.Assign(ref row, "{virtual-linetotal}",
                                               "<b>" + NumberFormat.DecFormat(dr["VirtualLineTotal"], 2) + "</b>");
                        palayvariance = CastAide.AsDecimal(dr["LineTotal"]) - CastAide.AsDecimal(dr["VirtualLineTotal"]);
                        TemplateHandler.Assign(ref row, "{variance}", NumberFormat.DecFormat(palayvariance, 2));

                        parlayVirtualTotal += CastAide.AsDecimal(dr["VirtualLineTotal"]);
                        parlayVirtualCommission += CastAide.AsDecimal(dr["VirtualComm"]);
                        parlayVarianceTotal += palayvariance;
                    }

                    parlayTotal = CastAide.AsDecimal(dr["LineTotal"]);
                    TemplateHandler.Assign(ref row, "{linetotal}",
                                           "<b>" + NumberFormat.DecFormat(parlayTotal, 2) + "</b>");

                    html.Append(row);
                    parlayCommission += CastAide.AsDecimal(dr["comm"]);
                    parlayGrandTotal += parlayTotal;
                }

                #endregion Mixparlay - OutRigth - FI - HR - 12BET - 3rdparty - Casino;

                html.Append("</tbody>");
                html.Append("<tfoot>");

                #region // Grand total footer;

                decimal TotalComMP = com + parlayCommission;
                decimal TotalComVirtualMP = comvirtual + parlayVirtualCommission;
                decimal TotalLineMP = grandtotal + parlayGrandTotal;
                decimal TotalVirtalLineMP = grandvirtual + parlayVirtualTotal;
                decimal TotalVarianceMP = grandvariance + parlayVarianceTotal; //TotalLineMP - TotalVirtalLineMP;

                TemplateHandler.Assign(ref tplTotal3, "{TotalCom}", NumberFormat.DecFormat(TotalComMP, 2));
                TemplateHandler.Assign(ref tplTotal3, "{total-linetotal}", NumberFormat.DecFormat(TotalLineMP, 2));
                TemplateHandler.Assign(ref tplTotal3, "{total-virtual-linetotal}",
                                       NumberFormat.DecFormat(TotalVirtalLineMP, 2));
                TemplateHandler.Assign(ref tplTotal3, "{total-variance}", NumberFormat.DecFormat(TotalVarianceMP, 2));
                TemplateHandler.Assign(ref tplTotal3, "{Total-Virtual-Comm}",
                                       NumberFormat.DecFormat(TotalComVirtualMP, 2));

                html.Append(tplTotal3);

                #endregion // Grand total footer;

                html.Append("</tfoot>");
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                string msg = ex.Message;

                TemplateHandler.Assign(ref norow, "{nodata}", msg);

                html.Append(norow);
                html.Append("</table>");
            }
            html.Append("</table>");

            #region add summary bettype

            //html.Append("<p>&nbsp;</p>");

            string str = "<script type ='text/javascript'>";
            str += "var done = '1';";
            str += "</script>";
            html.Append(str);

            #endregion add summary bettype

            // add more to export to excel
            //html.Append(Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate("TableBlank.tpl"));
            return html.ToString();
        }

        #region void ToTalRow(ref string totalrow, double combet, double grandtotalexbet, double grandtotalbet)

        private static void ToTalRow(ref string totalrow, decimal combet, decimal grandtotalbet, decimal grandvirtualbet,
                                     decimal grandvariancebet, string color1, string color2, string TypeorLeague,
                                     string grand, string align, decimal comvirtual)
        {
            TemplateHandler.Assign(ref totalrow, "{TotalCom}", NumberFormat.DecFormat(combet, 2));
            TemplateHandler.Assign(ref totalrow, "{TotalLine}", NumberFormat.DecFormat(grandtotalbet, 2));

            TemplateHandler.Assign(ref totalrow, "{Total-virtual-TotalCom}", NumberFormat.DecFormat(comvirtual, 2));

            TemplateHandler.Assign(ref totalrow, "{Total-virtual-linetotal}", NumberFormat.DecFormat(grandvirtualbet, 2));
            TemplateHandler.Assign(ref totalrow, "{Total-variance}", NumberFormat.DecFormat(grandvariancebet, 2));
            TemplateHandler.Assign(ref totalrow, "{grand}",
                                   (string.Compare(TypeorLeague.Trim(), "type", true) == 0) ? grand : TypeorLeague);
            //TypeorLeague);
            TemplateHandler.Assign(ref totalrow, "{color1}", color1);
            TemplateHandler.Assign(ref totalrow, "{color2}", color2);
            //TemplateHandler.Assign(ref totalrow, "{grand}", grand);
            TemplateHandler.Assign(ref totalrow, "{align}", align);
        }

        #endregion void ToTalRow(ref string totalrow, double combet, double grandtotalexbet, double grandtotalbet)

        #endregion MatchWL2a

        #region WLByBettype

        public string WLByBettype(string db, string fdate)
        {
            DateTime wldate = CastAide.AsDateTime(fdate);
            db = "rep";
            var html = new StringBuilder();
            string tplMain = "", tplNoData = "";
            bool IsNoData = false;
            int CountHiddenRows = 0;

            try
            {
                tplMain = WebUtils.LoadTemplate("WinLost/MatchWinLoss2SummaryBettypeV2.htm");
                string tplRows = TemplateHandler.CopyBlock(tplMain, "content");
                tplNoData = TemplateHandler.CopyBlock(tplMain, "nodata");
                DbResult rs;

                if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                    rs = IBCProvider.ReportProvider.MatchWL(DataAccess.DAOClient.AdoNet, DataAccess.DataSource.SqlServer).WinLossByBettype(wldate, db);
                else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                    rs = IBCProvider.ReportProvider.MatchWL(DataAccess.DAOClient.AdoNet, DataAccess.DataSource.SqlServer).WinLossByBettype14(wldate);
                else
                    rs = IBCProvider.ReportProvider.MatchWL(DataAccess.DAOClient.AdoNet, DataAccess.DataSource.SqlServer).WinLossByBettype_WH(wldate);

                DataTable dt = rs.ResultSet.Tables[0];
                decimal FI_3rdShare = CastAide.AsDecimal(rs.Outputs["fi_3rdshare"]);

                DataRow drFI_3rdShare = dt.NewRow();
                drFI_3rdShare["bettype"] = 150;
                drFI_3rdShare["LineTotal_others"] = FI_3rdShare;
                dt.Rows.Add(drFI_3rdShare);

                decimal LineTotal_soccer_live = 0,
                        VirtualLineTotal_soccer_live = 0,
                        LineTotal_soccer = 0,
                        VirtualLineTotal_soccer = 0,
                        Real = 0,
                        Virtual = 0,
                        Variance = 0;
                decimal GrandTotal_LineTotal_soccer_live = 0,
                        GrandTotal_VirtualLineTotal_soccer_live = 0,
                        GrandTotal_LineTotal_soccer = 0,
                        GrandTotal_VirtualLineTotal_soccer = 0;
                decimal LineTotal_others_live = 0,
                        VirtualLineTotal_others_live = 0,
                        LineTotal_others = 0,
                        VirtualLineTotal_others = 0,
                        GrandTotal_Real = 0,
                        GrandTotal_Virtual = 0,
                        GrandTotal_Variance = 0;
                decimal GrandTotal_LineTotal_others_live = 0,
                        GrandTotal_VirtualLineTotal_others_live = 0,
                        GrandTotal_LineTotal_others = 0,
                        GrandTotal_VirtualLineTotal_others = 0;

                if (dt.Rows.Count == 0)
                {
                    TemplateHandler.Assign(ref tplMain, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                    TemplateHandler.RemoveBlock(ref tplMain, "content");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref tplMain, "nodata");
                    var rows = new StringBuilder();
                    string row = "";

                    #region for each row

                    var dt2 = new DataTable();
                    dt2 = dt.Clone();

                    int index = 0;
                    string typename = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        switch (CastAide.AsInt32(dr["bettype"]))
                        {
                            case 9: //mp
                            case 10: //or
                            case 31: //win Tote Odds
                            case 32: //place Tote Odds
                            case 33: //winplace Tote Odds
                            case 41: //win Fixed Odds
                            case 42: //place Fixed Odds
                            case 43: //winplace Fixed Odds
                            case 400: //8rm
                            case 150: //3rdparty
                            case 200: //12bet
                            case 71: //CASINO- 71
                            case 72: //P2P-72
                                DataRow dr2 = dt2.NewRow();
                                for (int i = 0; i < dr.ItemArray.Length; i++)
                                {
                                    dr2[i] = dr[i];
                                }
                                dt2.Rows.Add(dr2);
                                continue;
                        }

                        row = tplRows.Clone().ToString();

                        LineTotal_soccer_live = CastAide.AsDecimal(dr["LineTotal_soccer_live"]);
                        LineTotal_soccer = CastAide.AsDecimal(dr["LineTotal_soccer"]);
                        LineTotal_others_live = CastAide.AsDecimal(dr["LineTotal_others_live"]);
                        LineTotal_others = CastAide.AsDecimal(dr["LineTotal_others"]);

                        VirtualLineTotal_soccer_live = CastAide.AsDecimal(dr["VirtualLineTotal_soccer_live"]);
                        VirtualLineTotal_soccer = CastAide.AsDecimal(dr["VirtualLineTotal_soccer"]);
                        VirtualLineTotal_others_live = CastAide.AsDecimal(dr["VirtualLineTotal_others_live"]);
                        VirtualLineTotal_others = CastAide.AsDecimal(dr["VirtualLineTotal_others"]);

                        switch (CastAide.AsInt32(dr["bettype"]))
                        {
                            case 51: //5 min
                            case 52: //15 min O/U
                            case 53: //30 min O/U
                            case 54: //45 min O/U
                            case 55: //60 min O/U
                            case 56: //End of day O/U

                            case 61: //5 min OE
                            case 62: //15 min OE
                            case 63: //30 min OE
                            case 64: //45 min OE
                            case 65: //60 min OE
                            case 66: //End of day OE
                                VirtualLineTotal_soccer_live = 0;
                                VirtualLineTotal_soccer = 0;
                                VirtualLineTotal_others_live = 0;
                                VirtualLineTotal_others = 0;

                                TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer_live}", "N/A");
                                TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer}", "N/A");
                                TemplateHandler.Assign(ref row, "{VirtualLineTotal_others_live}", "N/A");
                                TemplateHandler.Assign(ref row, "{VirtualLineTotal_others}", "N/A");
                                TemplateHandler.Assign(ref row, "{Virtual}", "N/A");
                                TemplateHandler.Assign(ref row, "{Variance}", "N/A");

                                IsNoData = true;
                                for (int i = 2; i < dr.ItemArray.Length; i++)
                                {
                                    if (CastAide.AsDecimal(dr.ItemArray[i]) != 0)
                                        IsNoData = false;
                                }
                                if (IsNoData)
                                {
                                    row = "";
                                    CountHiddenRows++;
                                }

                                Real = LineTotal_soccer_live + LineTotal_soccer + LineTotal_others_live +
                                       LineTotal_others;
                                Virtual = 0;
                                Variance = 0;
                                break;

                            default:
                                Real = LineTotal_soccer_live + LineTotal_soccer + LineTotal_others_live +
                                       LineTotal_others;
                                Virtual = VirtualLineTotal_soccer_live + VirtualLineTotal_soccer +
                                          VirtualLineTotal_others_live + VirtualLineTotal_others;
                                Variance = Real - Virtual;
                                break;
                        }

                        typename = CacheRef.GetBettype(dr["bettype"]);

                        if (CastAide.AsInt32(dr["bettype"]) == 1001)
                            typename = "Kick Off";

                        TemplateHandler.Assign(ref row, "{Bettype}", typename + " " + dr["Hedge"]);
                        TemplateHandler.Assign(ref row, "{LineTotal_soccer_live}",
                                               NumberFormat.DecFormat(LineTotal_soccer_live, 2));
                        TemplateHandler.Assign(ref row, "{LineTotal_soccer}",
                                               NumberFormat.DecFormat(LineTotal_soccer, 2));
                        TemplateHandler.Assign(ref row, "{LineTotal_others_live}",
                                               NumberFormat.DecFormat(LineTotal_others_live, 2));
                        TemplateHandler.Assign(ref row, "{LineTotal_others}",
                                               NumberFormat.DecFormat(LineTotal_others, 2));

                        TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer_live}",
                                               NumberFormat.DecFormat(VirtualLineTotal_soccer_live, 2));
                        TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer}",
                                               NumberFormat.DecFormat(VirtualLineTotal_soccer, 2));
                        TemplateHandler.Assign(ref row, "{VirtualLineTotal_others_live}",
                                               NumberFormat.DecFormat(VirtualLineTotal_others_live, 2));
                        TemplateHandler.Assign(ref row, "{VirtualLineTotal_others}",
                                               NumberFormat.DecFormat(VirtualLineTotal_others, 2));

                        TemplateHandler.Assign(ref row, "{Real}", NumberFormat.DecFormat(Real, 2));
                        TemplateHandler.Assign(ref row, "{Virtual}", NumberFormat.DecFormat(Virtual, 2));
                        TemplateHandler.Assign(ref row, "{Variance}", NumberFormat.DecFormat(Variance, 2));

                        index++;

                        if (index == 1)
                        {
                            TemplateHandler.Assign(ref row, "{rowspan}", "<td rowspan='{rowspan}'></td>");
                        }
                        else
                            TemplateHandler.Assign(ref row, "{rowspan}", "");

                        GrandTotal_LineTotal_soccer_live += LineTotal_soccer_live;
                        GrandTotal_VirtualLineTotal_soccer_live += VirtualLineTotal_soccer_live;
                        GrandTotal_LineTotal_soccer += LineTotal_soccer;
                        GrandTotal_VirtualLineTotal_soccer += VirtualLineTotal_soccer;

                        GrandTotal_LineTotal_others_live += LineTotal_others_live;
                        GrandTotal_VirtualLineTotal_others_live += VirtualLineTotal_others_live;
                        GrandTotal_LineTotal_others += LineTotal_others;
                        GrandTotal_VirtualLineTotal_others += VirtualLineTotal_others;

                        GrandTotal_Real += Real;
                        GrandTotal_Virtual += Virtual;
                        GrandTotal_Variance += Variance;

                        rows.Append(row);
                    }

                    #endregion for each row

                    #region for each row for bettype = 9, 10, 31, 32, 33, 400, 150, 200, CASINO- 71 AND P2P-72

                    int index2 = 0;
                    foreach (DataRow dr in dt2.Rows)
                    {
                        index2++;
                        row = tplRows.Clone().ToString();

                        if (index == 0 && index2 == 1)
                        {
                            TemplateHandler.Assign(ref row, "{rowspan}", "<td rowspan='{rowspan}'></td>");
                        }
                        else
                            TemplateHandler.Assign(ref row, "{rowspan}", "");

                        if (dr["bettype"].ToString() == "200") //12BET Agents
                        {
                            TemplateHandler.Assign(ref row, "{style}", "bgcolor='#ffff99'");
                            continue;
                        }
                        else if (dr["bettype"].ToString() == "150") //FI 3rd-Party
                            TemplateHandler.Assign(ref row, "{style}", "bgcolor='#dbeef3'");
                        else
                            TemplateHandler.Assign(ref row, "{style}", "");

                        LineTotal_soccer_live = CastAide.AsDecimal(dr["LineTotal_soccer_live"]);
                        VirtualLineTotal_soccer_live = CastAide.AsDecimal(dr["VirtualLineTotal_soccer_live"]);
                        LineTotal_soccer = CastAide.AsDecimal(dr["LineTotal_soccer"]);
                        VirtualLineTotal_soccer = CastAide.AsDecimal(dr["VirtualLineTotal_soccer"]);

                        LineTotal_others_live = CastAide.AsDecimal(dr["LineTotal_others_live"]);
                        VirtualLineTotal_others_live = CastAide.AsDecimal(dr["VirtualLineTotal_others_live"]);
                        LineTotal_others = CastAide.AsDecimal(dr["LineTotal_others"]);
                        VirtualLineTotal_others = CastAide.AsDecimal(dr["VirtualLineTotal_others"]);

                        TemplateHandler.Assign(ref row, "{style}", "");

                        typename = "";
                        switch (CastAide.AsInt32(dr["bettype"]))
                        {
                            case 150:
                                typename = "FI 3rd-Party";
                                break;

                            case 400:
                                typename = "8RM";
                                break;

                            case 71:
                                typename = "CASINO";
                                break; //CASINO- 71
                            case 72:
                                typename = "P2P";
                                break; //P2P-72
                            default:
                                typename = CacheRef.GetBettype(dr["bettype"]);
                                break;
                        }
                        typename += " " + dr["Hedge"];

                        TemplateHandler.Assign(ref row, "{Bettype}", typename);

                        switch (CastAide.AsInt32(dr["bettype"]))
                        {
                            case 31: //win Tote Odds
                            case 32: //place Tote Odds
                            case 33: //win-place Tote Odds
                            case 41: //win Fixed Odds
                            case 42: //place Fixed Odds
                            case 43: //win-place Fixed Odds
                            case 150: //3rdparty
                            case 71: //casino
                            case 72: //p2p
                                TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer_live}", "N/A");
                                TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer}", "N/A");
                                TemplateHandler.Assign(ref row, "{VirtualLineTotal_others_live}", "N/A");
                                TemplateHandler.Assign(ref row, "{VirtualLineTotal_others}", "N/A");

                                TemplateHandler.Assign(ref row, "{Virtual}", "N/A");
                                TemplateHandler.Assign(ref row, "{Variance}", "N/A");
                                VirtualLineTotal_soccer_live = 0;
                                VirtualLineTotal_soccer = 0;
                                VirtualLineTotal_others_live = 0;
                                VirtualLineTotal_others = 0;

                                if (CastAide.AsInt32(dr["bettype"]) == 150)
                                {
                                    IsNoData = true;
                                    for (int i = 2; i < dr.ItemArray.Length; i++)
                                    {
                                        if (CastAide.AsDecimal(dr.ItemArray[i]) != 0)
                                            IsNoData = false;
                                    }
                                    if (IsNoData)
                                    {
                                        row = "";
                                        CountHiddenRows++;
                                    }
                                }
                                break;

                            case 200: //12bet
                                IsNoData = true;
                                for (int i = 2; i < dr.ItemArray.Length; i++)
                                {
                                    if (CastAide.AsDecimal(dr.ItemArray[i]) != 0)
                                        IsNoData = false;
                                }
                                if (IsNoData)
                                {
                                    row = "";
                                    CountHiddenRows++;
                                }
                                break;
                        }

                        Real = LineTotal_soccer_live + LineTotal_soccer + LineTotal_others_live + LineTotal_others;

                        switch (CastAide.AsInt32(dr["bettype"]))
                        {
                            case 31: //win Tote Odds
                            case 32: //place Tote Odds
                            case 33: //win-place Tote Odds
                            case 41: //win Fixed Odds
                            case 42: //place Fixed Odds
                            case 43: //win-place Fixed Odds
                            case 150: //3rdparty
                            case 71: //casino
                            case 72: //p2p
                                Virtual = 0;
                                Variance = 0;
                                break;

                            default:
                                Virtual = VirtualLineTotal_soccer_live + VirtualLineTotal_soccer +
                                          VirtualLineTotal_others_live + VirtualLineTotal_others;
                                Variance = Real - Virtual;
                                break;
                        }

                        TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer_live}",
                                               NumberFormat.DecFormat(VirtualLineTotal_soccer_live, 2));
                        TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer}",
                                               NumberFormat.DecFormat(VirtualLineTotal_soccer, 2));
                        TemplateHandler.Assign(ref row, "{VirtualLineTotal_others_live}",
                                               NumberFormat.DecFormat(VirtualLineTotal_others_live, 2));
                        TemplateHandler.Assign(ref row, "{VirtualLineTotal_others}",
                                               NumberFormat.DecFormat(VirtualLineTotal_others, 2));

                        TemplateHandler.Assign(ref row, "{LineTotal_soccer_live}",
                                               NumberFormat.DecFormat(LineTotal_soccer_live, 2));
                        TemplateHandler.Assign(ref row, "{LineTotal_soccer}",
                                               NumberFormat.DecFormat(LineTotal_soccer, 2));
                        TemplateHandler.Assign(ref row, "{LineTotal_others_live}",
                                               NumberFormat.DecFormat(LineTotal_others_live, 2));
                        TemplateHandler.Assign(ref row, "{LineTotal_others}",
                                               NumberFormat.DecFormat(LineTotal_others, 2));

                        TemplateHandler.Assign(ref row, "{Virtual}", NumberFormat.DecFormat(Virtual, 2));
                        TemplateHandler.Assign(ref row, "{Variance}", NumberFormat.DecFormat(Variance, 2));
                        TemplateHandler.Assign(ref row, "{Real}", NumberFormat.DecFormat(Real, 2));

                        GrandTotal_LineTotal_soccer_live += LineTotal_soccer_live;
                        GrandTotal_VirtualLineTotal_soccer_live += VirtualLineTotal_soccer_live;
                        GrandTotal_LineTotal_soccer += LineTotal_soccer;
                        GrandTotal_VirtualLineTotal_soccer += VirtualLineTotal_soccer;

                        GrandTotal_LineTotal_others_live += LineTotal_others_live;
                        GrandTotal_VirtualLineTotal_others_live += VirtualLineTotal_others_live;
                        GrandTotal_LineTotal_others += LineTotal_others;
                        GrandTotal_VirtualLineTotal_others += VirtualLineTotal_others;

                        GrandTotal_Real += Real;
                        GrandTotal_Virtual += Virtual;
                        GrandTotal_Variance += Variance;

                        rows.Append(row);
                    }

                    #endregion for each row for bettype = 9, 10, 31, 32, 33, 400, 150, 200, CASINO- 71 AND P2P-72

                    #region Grand Total

                    row = tplRows.Clone().ToString();
                    TemplateHandler.Assign(ref row, "{rowspan}", "");
                    TemplateHandler.Assign(ref row, "{style}", "style='font-weight:bold;'");
                    TemplateHandler.Assign(ref row, "{Bettype}", "Grand Total");

                    TemplateHandler.Assign(ref row, "{LineTotal_soccer_live}",
                                           NumberFormat.DecFormat(GrandTotal_LineTotal_soccer_live, 2));
                    TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer_live}",
                                           NumberFormat.DecFormat(GrandTotal_VirtualLineTotal_soccer_live, 2));
                    TemplateHandler.Assign(ref row, "{LineTotal_soccer}",
                                           NumberFormat.DecFormat(GrandTotal_LineTotal_soccer, 2));
                    TemplateHandler.Assign(ref row, "{VirtualLineTotal_soccer}",
                                           NumberFormat.DecFormat(GrandTotal_VirtualLineTotal_soccer, 2));

                    TemplateHandler.Assign(ref row, "{LineTotal_others_live}",
                                           NumberFormat.DecFormat(GrandTotal_LineTotal_others_live, 2));
                    TemplateHandler.Assign(ref row, "{VirtualLineTotal_others_live}",
                                           NumberFormat.DecFormat(GrandTotal_VirtualLineTotal_others_live, 2));
                    TemplateHandler.Assign(ref row, "{LineTotal_others}",
                                           NumberFormat.DecFormat(GrandTotal_LineTotal_others, 2));
                    TemplateHandler.Assign(ref row, "{VirtualLineTotal_others}",
                                           NumberFormat.DecFormat(GrandTotal_VirtualLineTotal_others, 2));

                    TemplateHandler.Assign(ref row, "{Real}", NumberFormat.DecFormat(GrandTotal_Real, 2));
                    TemplateHandler.Assign(ref row, "{Virtual}", NumberFormat.DecFormat(GrandTotal_Virtual, 2));
                    TemplateHandler.Assign(ref row, "{Variance}", NumberFormat.DecFormat(GrandTotal_Variance, 2));

                    rows.Append(row);

                    int rsp = dt.Rows.Count - CountHiddenRows + 1;
                    rows.Replace("{rowspan}", rsp.ToString());

                    #endregion Grand Total

                    TemplateHandler.ReplaceBlock(ref tplMain, "content", rows.ToString());
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                string msg = ex.Message;
                TemplateHandler.Assign(ref tplMain, "{nodata}", msg);
                TemplateHandler.ReplaceBlock(ref tplMain, "content", "");
            }
            //Reports.RemoveCharacterNewLine(ref tplMain);
            html.Append(tplMain);
            return html.ToString();
        }

        #endregion WLByBettype
    }
}