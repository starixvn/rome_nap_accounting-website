using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Core;
using Sunplus.Accounting.Business.BetList;
using Sunplus.Accounting.Core.Extensions;

public partial class BetList_ExportExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            DateTime fdt;
            fdt = CastAide.AsDateTime(Request["fdate"]);
            var userName = Request["username"];
            var transTypeId = CastAide.AsInt32(Request["transtypeid"]);

            var sessionData = Session["statement.betlist." + userName + ".fdate" + fdt + "." + transTypeId];
            var data = string.Empty;
            if (null != sessionData)
                data = Convert.ToString(sessionData);
            else
            {
                BetlistByStatement betlist = new BetlistByStatement();
                data = betlist.GenReportStatement(fdt, userName, transTypeId);
            }
            //Response.Write(betListHtml);

            ExportExcel(data, userName, fdt);

        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }



    }

    private static void ExportExcel(string data, string userName, DateTime date)
    {
        var fileName = "DailyStatement-" + date.ToShortDateString() + "-" + userName;
        var title =  "Daily Statement - " + date.ToShortDateString() + " - " + userName;
        StringBuilder content = new StringBuilder();
        content.AppendLine("<html>");
        content.AppendLine("<head>");
        content.AppendLine("<META http-EQUIV='Content-Type' CONTENT='text/html; charset=unicode-utf8'>");

        content.AppendLine(RenderStyle());
        content.AppendLine("</head>");
        content.Append("<body>");



        content.Append("<div style='font-size:16px;'><b>"+ title  +"</b></div>");
        content.Append(data.Replace("href", ""));
        content.Append("</body>");
        content.Append("</hmtl> ");


        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".xls");
        HttpContext.Current.Response.Charset = "";
        //data = data.Replace("href", "");
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

        HttpContext.Current.Response.Write(content.ToString());
        //HttpContext.Current.Response.Write(data);
        //HttpContext.Current.Response.Write("<table><tr><td class='tdhidden'></td></tr></table>");

        //HttpContext.Current.Response.End();
    }

    private static string RenderStyle()
    {
        var content = new StringBuilder();
        content.AppendLine(@"<style>.text { mso-number-format:\@; }  ");

        content.AppendLine("body{font-family:Tahoma,Arial ,helvetica ,sans-serif;font-size:8pt;margin:0px;} " +
                           "button::-moz-focus-inner,input[type='reset']::-moz-focus-inner,input[type=\"button\"]::-moz-focus-inner,input[type='submit']::-moz-focus-inner,input[type=\"file\"] >input[type=\"button\"]::-moz-focus-inner{border:none} #err, #errMsg, #errfunc, #errTransfer, .errMsg {color:Red;font-weight:bold;padding:0px;margin:0px;display:inline; overflow:hidden;} #clear{clear:both} #clear2{clear:both;height:2px;padding:0px;margin:0px;display:inline;overflow:hidden;} #fdate, #tdate {width:80px !important;} .l{text-align:left;} .c{text-align:center} .r{text-align:right} .b{font-weight:bold} .mid{vertical-align:middle} .top{vertical-align:top} .bot{vertical-align:bottom} .hidden, .hide{display:none !important;} .allhidden{display:none !important;} .UCase{text-transform:uppercase} a.hilight{color:#e00 !important; text-decoration:none} a.hilight:hover{text-decoration:underline} span.a{color:#0066CC; text-decoration:none} span.hilight{color:#e00 !important; text-decoration:none} span.canchange{color:#0066CC; cursor:pointer} span.canchange:hover{color:#D66D35; cursor:pointer} li.canchange{color:#0066CC; cursor:pointer} li.canchange:hover{color:#D66D35; cursor:pointer} span.change .current{color:#D66D35; cursor:pointer} input{outline: none 0;} a{outline: none 0; color:#0066CC; text-decoration:none} a:hover{color:#D66D35; text-decoration:none;} a.current{color:#D66D35 !important; cursor:pointer} .lnk{color:blue;cursor:pointer;} " +
                                                                                                                                                                                             ".hand {cursor:pointer} .pad_tb_4{padding:4px 0px;} span.cannotchange{color:#0066CC; cursor:pointer} span.cannotchange:hover{color:#0066CC} span.cannotchangeinternal{color: #0000ff;} span.cannotchangeinternal:hover{color: #0000ff;} .fixfont{font-family: Tahoma,Arial ,helvetica ,sans-serif;font-size: 11px;} .main{width:100%;height:100%;} .maincontent{padding: 2px 0px 0px 1px;} .maincontent_header{height:23px;} .maincontent_headerL{height:23px;width:24px;background:url(../images/L_content_tab.png) no-repeat right bottom;} .maincontent_headerR{height:23px;width:5px;background:url(../images/R_content_tab.png) no-repeat right bottom;} .maincontent_headerC{height:23px;min-width:120px;font-weight:bold;color:#fff;background:url(../images/C_content_tab.png) repeat-x bottom;text-align:left;} .maincontent_box{background:#FFF url(../images/banner/bg_head_tb.jpg) repeat-x left bottom;border:solid 1px #7c98a4;padding:4px 2px;display:table;width:99%;text-align:left;} .bg_table{background:#e6e9f0;padding:4px 2px;border:solid 1px #7c98a4;display:table;width:99%;margin-top:2px;} .submenu{width: 100%;height:23px;background-color:#e6e9f0;} .submenucontent{padding: 0px 5px;} .child-c{background:url(images/child_menu_c.jpg) repeat-x top center;} .child-l{background:url(images/child_menu_l.jpg) no-repeat;height:20px;width:3px;} .child-r{background:url(images/child_menu_r.jpg) no-repeat;width:6px;} .blockUI{padding:0px;border-spacing:0px;} .blockUI td{white-space:nowrap;} .tdloading{height:100px;width:100px;background: url(../images/loading51.gif) no-repeat center center;} .viewicon{background:url(\"../Images/icn_view.png\") no-repeat scroll right top transparent;cursor:pointer;height:18px;margin:0 0px 0 0;vertical-align:middle;width:16px;} .ui-datepicker-week-end{color: #FF0000 !important;} ")
        ;

        content.AppendLine(
           ".tbllayout{border-spacing: 1px;background-color: #7c98a4;width: 100%;text-align: center;line-height: 18px;font-family: Tahoma,Arial ,helvetica ,sans-serif;font-size: 11px;} .tbllayout td, .tbllayout th{background-color:#fff;padding:1px;!padding-bottom:2px;} .tbllayout a{cursor:pointer} .tbllayout .title{background-color: #7c98a4 !important;font-weight: bold;color: #000;padding: 1px;} .tbllayout .title td{background-color:#b5c4d0 !important;} .tbllayout .title th{background-color:#b5c4d0 !important;} .a1 {background-color:#CCFFFF !important;} .a2 {background-color:#CCFFFF !important;} .a3 {background-color:#FFFF99 !important;} .a4 {background-color:#FFFFEE !important;} .a5 {background-color:#FFDDFF !important;} .tbllayout .title1{background-color:#7c98a4 !important;font-weight:bold;color:#fff;padding:1px;} .tbllayout .title1 td{background-color:#587281 !important;} .tbllayout .bgnobold{background-color:#dddddd !important;color:#000;padding:1px;} .tbllayout .bgnobold1 td{background-color:#99cccc !important;} .tbllayout .bgnobold1{background-color:#99cccc !important;color:#000;padding:1px;} .tbllayout .bgnobold td{background:#dddddd !important;} .report-rowspan{background:#7c98a4 !important;color:#fff;font-weight:bold;text-align:center;} .tbllayout .report-rowspan td{background:#8e8e8e !important;} .tbllayout .row-total td{background:#357EC7 !important;} .tbllayout .title2{background-color:#7c98a4 !important;font-weight:bold;color:#000;padding:1px;} .tbllayout .title2 td{background-color:#e1effa !important;} .tbllayout .titleall td{background-color:#7c98a4 !important;font-weight:bold;color:#000;padding:1px;} .tbllayout .tdtitle{background-color:#b5c4d0 !important;font-weight:bold;color:#000;padding:1px;} .tblcollapse{background-color:#fff;border-collapse: collapse;border:solid 1px #7c98a4;width:100%;text-align:center;line-height:18px;font-family:Tahoma,Arial ,helvetica ,sans-serif;font-size:11px;border:solid 1px #7c98a4;} .tblcollapse td, .tblcollapse th{border:solid 1px #7c98a4;line-height:18px;padding:1px;} .tblcollapse a{cursor:pointer} .tblcollapse .title{font-weight:bold;color:#000;padding:1px;} .tblcollapse .title td, .tblcollapse .title th{border:solid 1px #7c98a4;} .tblcollapse .title td{background-color:#b5c4d0 !important;} .tblcollapse .title th{background-color:#b5c4d0 !important;} .tblcollapse .title1{font-weight:bold;color:#fff;padding:1px;} .tblcollapse .title1 td{background-color:#587281 !important;} .tblcollapse .title2{font-weight:bold;color:#000;padding:1px;} .tblcollapse .title2 td{background-color:#e1effa !important;} .noneborder td, .noneborder th{border:none 0px !important;} .tdtitle2{background-color:#d0e8fb !important;} .tdtitle{text-align:center;line-height:18px;font-family:Tahoma,Arial ,helvetica ,sans-serif;font-size:11px;background-color:#b5c4d0 !important;font-weight:bold;color:#000;padding:1px;} .whitebg{background-color:#fff;} .evenrow td{background-color:#DFEAFF;} .overrow td{background-color:#f8ff8d !important;} .rowhighlight td{background-color:#f8ff8d !important;font-weight:bold !important;} .new-rate-evenrow {background-color:#F7E9F8 !important;} .statement-accu-row td{background-color:#fdf8b5;color:#0000ff} .limit td{background: #eaeaea none repeat scroll 0%;} .limit-header td{background: #fde9d9 none repeat scroll 0%;} .report-footer td{background:#ccffcc none repeat scroll 0 0;color:#000;font-weight:bold;} .footer1, .report-footer, .footer2{background-color:#7c98a4;} .footer1 td{background:#d3e1c8 none repeat scroll 0 0;color:#000; font-weight:bold;} .footer2 td{background:#ffeedd none repeat scroll 0 0;color:#000; font-weight:bold;} .footernobold td{background:#d3e1c8 none repeat scroll 0 0;color:#000;} .sportLeg td{background:#DEFFDE none repeat scroll 0 0;color:#000;font-weight:bold} .colGroup1 td{background:#f7f0f7 none repeat scroll 0 0;} .colGroup2 td{background:#fafaf3 none repeat scroll 0 0;} .collap{border-collapse:collapse} .separate{background:#FFF; border:1px solid #526A80; border-collapse:separate} .layout_table{border-collapse: collapse;border-spacing: 0} .f10{font-size:10px} .f11{font-size:11px} .w30{width:20px} .w35{width:35px} .w40{width:40px} .w50{width:50px} .w60{width:60px} .w70{width:70px} .w80{width:80px} .w90{width:90px} .w100{width:100px} .w120{width:120px} .w130{width:130px} .w150{width:150px} .w160{width:160px} .w200{width:200px} .w250{width:250px} .w300{width:300px} .w350{width:350px} .pL10{padding-left:10px} .pL20{padding-left:20px} .pL30{padding-left:30px} .rh-100{width:100px} .mT5{margin-top:5px} .fullwidth{width:100%;} .mL2{margin-left:2px;} .tblTitleLeft{text-align: left;background-color: #B5C4D0;color: #000000;font-weight: bold;font-size: 8pt;height: 22px;} .tblTitleRight{text-align: right;background-color: #B5C4D0;color: #000000;font-weight: bold;font-size: 8pt;height: 22px;} .tblfixed{text-align: center;line-height: 18px;font-family: Tahoma,Arial ,helvetica ,sans-serif;font-size: 11px;border-collapse: collapse;} .tblfixed td, .tblfixed th{border:solid 1px #7c98a4;padding:1px;!padding-bottom:2px;} .tblfixed a{cursor:pointer} .tblfixed .title{background-color: #7c98a4 !important;font-weight: bold;color: #000;} .tblfixed .title td{background-color:#B5C4D0 !important;} .tblfixed .title th{background-color:#B5C4D0 !important;} .tblfixed .total td{background-color:#88AAFF !important;} .tblfixed .total th{background-color:#88AAFF !important;} .tblfixed .title1{background-color:#7c98a4 !important;font-weight:bold;color:#fff;padding:1px;} .tblfixed .title1 td{background-color:#587281 !important;} .tblfixed .bgnobold{background-color:#dddddd !important;color:#000;padding:1px;} .tblfixed .bgnobold1 td{background-color:#99cccc !important;} .tblfixed .bgnobold1{background-color:#99cccc !important;color:#000;padding:1px;} .tblfixed .bgnobold td{background:#dddddd !important;} .report-rowspan{background:#7c98a4 !important;color:#fff;font-weight:bold;text-align:center;} .tblfixed .report-rowspan td{background:#8e8e8e !important;} .tblfixed .row-total td{background:#357EC7 !important;} .tblfixed .title2{background-color:#7c98a4 !important;font-weight:bold;color:#000;padding:1px;} .tblfixed .title2 td{background-color:#e1effa !important;} .tblfixed .titleall td{background-color:#7c98a4 !important;font-weight:bold;color:#000;padding:1px;} .tblfixed .tdtitle{background-color:#b5c4d0 !important;font-weight:bold;color:#000;padding:1px;} .rowactive td{background:#f8ff8d !important; cursor: pointer;} table.tablesorter {margin:10px 0pt 15px;width: 100%;text-align: center;} table.tablesorter thead tr th, table.tablesorter tfoot tr th {} table.tablesorter thead tr .header {background-image: url(../images/sorting_bg.gif);background-repeat: no-repeat;background-position: center right;cursor: pointer;} table.tablesorter tbody td {} table.tablesorter tbody tr.odd td {} table.tablesorter thead tr .headerSortUp {background-image: url(../images/sorting_asc.gif);} table.tablesorter thead tr .headerSortDown {background-image: url(../images/sorting_desc.gif);} table.tablesorter thead tr .headerSortDown, table.tablesorter thead tr .headerSortUp {}  ");


      

        content.AppendLine(
            ".bl-choice {color:#FF0000;font-family:Tahoma;font-size:13px;font-weight:bold;} .bl-bettype {color:#003399;font-family:Tahoma;font-size:11px;font-weight:600;} .bl-sporttype {color:#0066CC;font-family:Tahoma;font-size:10px;font-weight:bold;} .bl-leaguename {color:#666666;font-family:Tahoma;font-size:11px;font-weight:bold;} .bl-event {color:#000000;font-family:Tahoma;font-size:11px;font-weight:bold;} .event-live{background-color:#ffffcc;} .s-datavalue{font-size: 11px;font-family: Tahoma;color: #000000;font-weight: 600;} .s-datavalue-minus{color: #FF0000;} .s-total-minus{color: #FF0000;font-weight: bold} .s-total{color: #003399;font-weight: bold;} span.link {color:#16178D;cursor:pointer;text-decoration:none;} span.link:hover {color:#FF0000;cursor:pointer;text-decoration:none;} .hand{cursor:pointer;} .subTotal td{background:#fff !important;} .bl_stype{color: #0066CC;} .bl_lname{color: #666666;line-height:15px;margin-top:1px;} .b_mix{font-weight: bold;width: auto;line-height: 16px;float: left;height: 20px;} .bl_time{color: #434343;font-size: 11px !important;font-weight:normal;text-align:center;font-family:Tahoma;} .FirstGoal {background:transparent url(../Images/firstGoal.gif) no-repeat scroll center center;display:inline;height:10px;padding:0 4px;width:14px;} .LastGoal {background:transparent url(../Images/lastGoal.gif) no-repeat scroll center center;display:inline;height:10px;padding:0 4px;width:14px;} .exrate2{color: #0066CC;font-size: 9px;} .bgpending{background-color: Yellow !important;} .bl-sporttypeLC {color:#8C373C;font-family:Tahoma;font-size:11px;font-weight:bold;} .bl-odds {color:#B5D0E5;font-family:Tahoma;font-size:10px;} .bl-columnheader{border-width: 1px;border-style: solid;border-right: none; border-color: #7C98A4;} .bl-columncontent{border-width: 1px;border-style: solid;border-right: none;border-top: none; border-color: #7C98A4;} ");

       


        content.AppendLine("</style>");
        return content.ToString();
    }
}