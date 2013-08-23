using System;
using System.Text;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// Summary description for ReportUtils
/// </summary>
public partial class ReportHTML
{
    #region Order by Currency

    /// <summary>
    /// RM[2],RMB[13],Euro[6],HK$[5],IN[15], IND[22], IDD[33], IDR[34], DDD[47],SD$[1],TB[4],THB[43],US$[3], $US[35], IRR[40], NTD[17], Yen[16]
    /// </summary>
    public static string strArrCurrencyOrder =
        "[2],[13],[6],[5],[15], [22],[33],[34], [47],[1],[4],[43],[3],[35],[40],[17],[16]";

    /// <summary>
    /// US$[3], $US[35], IRR[40], NTD[17], Yen[16]
    /// </summary>
    public static int[] ArrCurrencySubTotal_USD = { 3, 35, 40, 17, 16 };

    public static string strCurrencySubTotal_USD = "[3],[35],[40],[17],[16]";

    /// <summary>
    /// 4, 43
    /// TB[4], THB[43]
    /// </summary>
    public static int[] ArrCurrencySubTotal_THB = { 4, 43 };

    public static string strCurrencySubTotal_THB = "[4], [43]";

    /// <summary>
    /// 15, 22, 33, 34
    /// IN[15], IND[22], IDD[33], IDR[34], DDD[47]
    /// </summary>
    public static int[] ArrCurrencySubTotal_IN = { 15, 22, 33, 34, 47 };

    public static string strCurrencySubTotal_IN = "[15], [22], [33], [34], [47]";

    #endregion Order by Currency

    #region ValidDate

    /// <summary>
    /// date must be between balance up date and today
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string ValidDate(string date)
    {
        if (date == null || date.Length == 0)
            date = DateTime.Today.ToShortDateString();
        if (CastAide.AsDateTime(date) < ReportUtils.BalanceUp.BalanceUpDay || CastAide.AsDateTime(date) > DateTime.Today)
            date = DateTime.Today.ToShortDateString();
        return date;
    }

    #endregion ValidDate

    public static string Report2A_Excel
    {
        get { return "AccReport2A_Excel_" + AppSession.Username; }
    }

    public static string CSSExcelReport()
    {
        var content = new StringBuilder();
        content.AppendLine(@"<style>.text { mso-number-format:\@; }  ");
        content.AppendLine(".c {text-align:center;}");
        content.AppendLine(".r {text-align:right;}");
        content.AppendLine(".l {text-align:left;}");
        content.AppendLine(".b {font-weight:bold;}");
        content.AppendLine(
            "td {font-family:Tahoma; font-size:11px;vertical-align:middle; height:21px; border-color:#7C98A4; border-style:solid; border-width:thin;}");
        content.AppendLine(
            "th {font-family:Tahoma; font-size:11px;vertical-align:middle; height:21px; border-color:#7C98A4; border-style:solid; border-width:thin;}");
        content.AppendLine(
            "table.tbllayout {font-family:Tahoma,Verdana; border-collapse:collapse;border-color:#7C98A4; border-style:solid; border-width:thin;}");
        content.AppendLine(
            "table.bg {font-family:Tahoma,Verdana; border-collapse:collapse;border-color:#7C98A4; border-style:solid; border-width:thin;}");
        content.AppendLine(
            "tr.title {background:#B5C4D0 none repeat scroll 0%; color:#000; font-weight:bold; text-align:center;}");
        content.AppendLine(
            "tr.title1 {background:#587281  none repeat scroll 0%; color:#fff; font-weight:bold; text-align:center;}");
        content.AppendLine(
            "tr.footer1 {background:#D3E1C8  none repeat scroll 0%; color:#000; font-weight:bold; text-align:center;}");
        content.AppendLine(
            "tr.footer2 {background:#FFEEDD  none repeat scroll 0%; color:#000;text-align:center;font-weight:bold;}");
        content.AppendLine("tr.evenrow {background:#E6E9F0 none repeat scroll 0%;}");
        content.AppendLine(
            "tr.report-footer { background:#CCFFCC none repeat scroll 0%; color:#000; text-align:center; font-weight:bold}");
        content.AppendLine(
            "td.tdtitle {background:#B5C4D0 none repeat scroll 0%; color:#000; font-weight:bold; text-align:center;}");
        content.AppendLine(
            "td.tdtitle2 {background:#D0E8FB none repeat scroll 0%; color:#000; font-weight:bold; text-align:center;}");
        content.AppendLine(
            "td.report-rowspan {background:#8e8e8e none repeat scroll 0%; color:#fff; font-weight:bold; text-align:center;}");
        content.AppendLine(
            "td.total {background:#C4D0D7 none repeat scroll 0%; color:#000; font-weight:bold; text-align:right;}");
        content.AppendLine(
            "th.ag {background:#B5C4D0 none repeat scroll 0%; color:#000; font-weight:bold; text-align:center;}");
        content.AppendLine(
            "th.ibc {background:#D0E8FB none repeat scroll 0%; color:#000; font-weight:bold; text-align:center;}");

        content.AppendLine("td.a1 {background-color:#CCFFFF !important;}");
        content.AppendLine("td.a3 {background-color:#FFFF99 !important;}");
        content.AppendLine("td.a2 {background-color:#CCFFFF !important;}");
        content.AppendLine("td.a6 {background-color:#FFDDFF !important;}");
        content.AppendLine("td.a5 {background-color:#DFEAFF !important;}");
        content.AppendLine("td.a4 {background-color:#FFFFEE !important;}");

        content.AppendLine("td.rpt-red-color {color:red !important;}");
        content.AppendLine("td.tdhidden {border-style:none;}");
        content.Append(
            ".ag {background:none repeat scroll 0 0 #B5C4D0;} .ibc {background:none repeat scroll 0 0 #D0E8FB;} .phu {background:none repeat scroll 0 0 #B5C4D0;}");
        content.Append(".tbllayout .bgnobold { background:#dddddd !important; color:#000; padding:1px;}");
        content.Append(".tbllayout .bgnobold td{background:#dddddd !important;}	");
        content.Append(".tbllayout .bgnobold1 { background:#99cccc !important; color:#000; padding:1px; }");
        content.Append(".tbllayout .bgnobold1 td{background:#99cccc !important;}");
        content.Append(".bgTotal td { background:none repeat scroll 0 0 #C4D0D7; color:#000000; text-align:right;}");
        content.Append(".footernobold td{background:#d3e1c8 none repeat scroll 0 0;color:#000;}");
        content.Append(".sportLeg td{background:#DEFFDE none repeat scroll 0 0;color:#000;font-weight:bold}");
        content.Append(".limit td{background: #eaeaea  none repeat scroll 0%;}");
        content.Append(".limit-header td{background:  #fde9d9 none repeat scroll 0%;}");
        content.AppendLine("</style>");
        return content.ToString();
    }
}