<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyWinLoss.aspx.cs" Inherits="Report_WinLoss_DailyWinLoss" %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>

<%--<%@ Import Namespace="SquishIt.Framework" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
  <link href="../../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../../js/Calendar2/css/jscal2.css" type="text/css" rel="stylesheet" />
    <link href="../../../js/Calendar2/css/border-radius.css" type="text/css" rel="stylesheet" />
    <link href="../../../js/Calendar2/css/blue/blue.css" type="text/css" rel="stylesheet" />
    <link href="WebResources/reportWinLoss.css" rel="stylesheet" type="text/css" />
<%--      <%= Bundle.Css()
        .Add("~/App_Themes/Public/common.css")
        .Add("~/App_Themes/Public/table.css")
        .Add("~/App_Themes/Public/input.css")
        .Add("~/js/Calendar2/css/jscal2.css")
        .Add("~/js/Calendar2/css/border-radius.css")
        .Add("~/js/Calendar2/css/border-radius.css")
        .Add("~/js/Calendar2/css/blue/blue.css")
        .Add("~/Report/WinLoss/WebResources/reportWinLoss.css")      
        .Render("~/WebResources/Combined/Css/dailywinloss_#.css")
        %>--%>
</head>
<body onload="parent.jQuery.unblockUI();">
    <form id="form1" runat="server">
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        <%=ReportTitle%>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <table width="480px">
                    <tr class="b">
                        <td>
                            Date:
                        </td>
                        <td>
                            <input type="text" name="fdate" readonly="readonly" class="dateTime" id="fdate" />
                            <input type="image" src="../../../App_Themes/Images/calendar.gif" class="img-date-selector"
                                title="Select date" id="fdatetrigger" alt="" onclick="return false;" />
                        </td>
                        <td>
                            <input type="button" value="View report" class="btn" id="btnViewReport" />
                            <input type="button" class="btn" id="btnViewReportToday" />
                            <input type="button" class="btn" id="btnViewReportYesterday" />
                            <asp:ImageButton ID="btnExport" ImageUrl="~/App_Themes/Images/excel.gif" runat="server"
                                ToolTip="Export excel file" AlternateText="" OnClick="btnExport_Click" Style="width: 16px" />
                            <%--<input type="button" title="Export to excel file" onclick="ViewReport()" name="btnExport"
                                id="btnExport" class="btnExcel" />--%>
                        </td>
                    </tr>
                </table>
                
                <div id="totalMemberOnline" style="display:none">
                    <br />
                    <b>Total online members: <span style="color: #ff6600">
                        <%--<%= (new Sunplus.Accounting.Services.ReportServices.WinLossService().GetTotalOnlineMember(Sunplus.Accounting.Core.Common.AppSession.CurrentSystem))%>--%>
                    </span></b>
                </div>
                
            </div>
        </div>
        
       
        
        <asp:Repeater ID="rptWinLoss" runat="server" EnableViewState="false" EnableTheming="true">
            <HeaderTemplate>
                <div class="bg_table">                    
                    <table class="tbllayout" cellspacing="1" id="rptGrid">
                        <thead>
                            <tr class="title">
                                <th colspan="11">
                                    <b>Turnover & Position Taking </b>
                                </th>
                                <th colspan="8">
                                    <b>Win/Loss & Margin </b>
                                </th>
                            </tr>
                            <tr class="title" id="rptHeader">
                                <%--Turnovers & Position Taking--%>
                                <td class="No">
                                    No.
                                </td>
                                <td>
                                    Username
                                </td>
                                <td>
                                    Total T/O
                                </td>
                                <td>
                                    Company
                                </td>
                                <td class="Percent">
                                    %
                                </td>
                                <td>
                                    Super
                                </td>
                                <td class="Percent">
                                    %
                                </td>
                                <td>
                                    Master
                                </td>
                                <td class="Percent">
                                    %
                                </td>
                                <td>
                                    Agent
                                </td>
                                <td class="Percent">
                                    %
                                </td>
                                <%--Win/Lost Margin--%>
                                <td>
                                    Company
                                </td>
                                <td class="Percent">
                                    %
                                </td>
                                <td>
                                    Super
                                </td>
                                <td class="Percent">
                                    %
                                </td>
                                <td>
                                    Master
                                </td>
                                <td class="Percent">
                                    %
                                </td>
                                <td>
                                    Agent
                                </td>
                                <td class="Percent">
                                    %
                                </td>
                            </tr>
                        </thead>
                        <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="item">
                    <%#RenderUserName(Container.DataItem)%>
                    <%#RenderRowItem(Container.DataItem)%>
                </tr>
            </ItemTemplate>
          
            <FooterTemplate>
                </tbody>
                <tfoot>
                    <%--lasteditem--%>
                    <tr id="rptFooter">
                        <%--Turnovers & Position Taking--%>
                        <td colspan="2">
                            <span style="padding-right: 2px">Grand Total</span>
                        </td>
                        <%=RenderFooter()%>
                    </tr>
                </tfoot>
                </table></div>
            </FooterTemplate>
        </asp:Repeater>
         <div class="bg_table" runat="server" visible="false" id="NoInfoPanel" enableviewstate="false">
        <table class="tbllayout" cellspacing="1" id="rptGrid">
            <thead>
                <tr class="title">
                    <th colspan="11">
                        <b>Turnover & Position Taking </b>
                    </th>
                    <th colspan="8">
                        <b>Win/Loss & Margin </b>
                    </th>
                </tr>
                <tr class="title" id="rptHeader">
                    <%--Turnovers & Position Taking--%>
                    <td class="No">
                        No.
                    </td>
                    <td>
                        Username
                    </td>
                    <td>
                        Total T/O
                    </td>
                    <td>
                        Company
                    </td>
                    <td class="Percent">
                        %
                    </td>
                    <td>
                        Super
                    </td>
                    <td class="Percent">
                        %
                    </td>
                    <td>
                        Master
                    </td>
                    <td class="Percent">
                        %
                    </td>
                    <td>
                        Agent
                    </td>
                    <td class="Percent">
                        %
                    </td>
                    <%--Win/Lost Margin--%>
                    <td>
                        Company
                    </td>
                    <td class="Percent">
                        %
                    </td>
                    <td>
                        Super
                    </td>
                    <td class="Percent">
                        %
                    </td>
                    <td>
                        Master
                    </td>
                    <td class="Percent">
                        %
                    </td>
                    <td>
                        Agent
                    </td>
                    <td class="Percent">
                        %
                    </td>
                </tr>
            </thead>
            <tr>
                <td colspan="19">
                    No information is available
                </td>
            </tr>
        </table>
        </div>
    </div>
    </form>
</body>


<script src="../../../js/jQuery/jquery.min.js" type="text/javascript"></script>
<%--
<%= Bundle.JavaScript()
    .Add("~/js/Calendar2/js/jscal2.js")
    .Add("~/js/Calendar2/js/lang/en.js")
    .Add("~/js/jQuery/scrolltable/webtoolkit.jscrollable.js")
    .Add("~/js/jQuery/jquery.blockUI.1.33.js")
    .Add("~/Report/WinLoss/WebResources/Js/date.format.js")
    .Add("~/Report/WinLoss/WebResources/Js/WinLossReport.js")
    .ForceRelease()
        .Render("~/WebResources/Combined/Js/combined_#.js")%> --%>

<script src="../../../js/Calendar2/js/jscal2.js" type="text/javascript"></script>

<script src="../../../js/Calendar2/js/lang/en.js" type="text/javascript"></script>

<script src="../../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>

<script src="../../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>

<script src="../../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>

<script src="WebResources/Js/date.format.js" type="text/javascript"></script>

<script src="WebResources/Js/WinLossReport.js" type="text/javascript"></script>

<script type="text/javascript" language="javascript">
var uplineId = <%=UplineId %>;
var currentReportLevel = <%=GetCurrentReportLevel().ToString()%>;
var toDay = <%="'"+DateTime.Now.ToString("MM/dd/yyyy")+"'"%>;
var yesterday = <%="'"+DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy")+"'"%>;
var fromDate =  <%="'"+FromDate.ToString("MM/dd/yyyy")+"'"%>;

$(document).ready(function() {
    //setup value button search today, and yesterday
    //document.getElementById('dafadsf').setAttribute("style", "background-color: #FFEEDD");
    $("#btnViewReportToday").attr("value",toDay);  
    $("#btnViewReportYesterday").attr("value", yesterday);        
   //set value date time input
     
   $("#fdate").attr("value", fromDate);          
   $("#btnViewReport").click(function(){
        ViewDailyWLReport(uplineId, $("#fdate").val(), currentReportLevel);
    });
    
   $("#btnViewReportToday").click(function(){
        ViewDailyWLReport(uplineId, toDay, currentReportLevel);
   });    
    
   $("#btnViewReportYesterday").click(function(){
        ViewDailyWLReport(uplineId, yesterday, currentReportLevel);
   });     
   
     
});

//setup calendar
var fromCalendar =  Calendar.setup
({
    animation: false,
    showTime: false,
    bottomBar: true,
    trigger: "fdatetrigger",
    inputField: "fdate",
    dateFormat: "%m/%d/%Y",
    min: <%=ReportUtils.BalanceUp.BalanceUpDay.ToString("yyyyMMdd") %>, 
    max: <%=DateTime.Now.ToString("yyyyMMdd") %>,          
    onSelect: function() {          
        this.hide();            
    }
});    
    
   
 
function ViewDownLineReport(uplineId) {
    var nextReportLevel = parseInt(currentReportLevel) + 1;    
    ViewDailyWLReport(uplineId, $("#fdate").val(), nextReportLevel); 
}  

  
   
</script>

</html>
