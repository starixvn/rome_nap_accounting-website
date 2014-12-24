<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerStatement.aspx.cs"
    Inherits="Statement_CustomerStatement" Title="CustomerIndex" Theme="" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>
<%@ Import Namespace="Fanex.Module.Statement.Resources" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="ReportTitle" runat="server"></title>
    <link href="~/App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="~/App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="Stylesheet" href="~/js/jQuery/autocomplete/styles.css" />
    <link rel="stylesheet" type="text/css" href="~/Scripts/_Libs/FanexGrid/Fanex.Grid.css" />
    <link rel="stylesheet" type="text/css" href="~/Scripts/_Libs/FanexGrid/Fanex.Pagination.css" />
    <link rel="Stylesheet" type="text/css" href="~/App_Themes/Report/Statement.css?v=2.1" />
    <link rel="stylesheet" type="text/css" href="~/Areas/Statement/Views/Report/Styles/Statement.css?ver=1" />
    <%--<link href="../js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />--%>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" id="bannerFilter" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        <asp:Literal ID="reportHeader" runat="server"></asp:Literal>
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box" id="bannerFilter">
                <asp:Literal ID="searchContent" runat="server"></asp:Literal>
            </div>
            <div class="bg_table" style="display: none;" id="statementCotent">
                <div id="sData">
                    <%--Loading...--%>
                </div>
            </div>
            <div id="GridContainer" class="freegrid" style="margin-top: 10px;">
            </div>
            <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
            <div id="popup" style="display: none;">
            </div>
        </div>
    </div>
    <div id="betListContent" style="margin-top: 10px;">
    </div>
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="WebResources/js/CustomerStatement.js?ver=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/_Libs/FanexGrid/Fanex.Grid.js"></script>
    <script type="text/javascript" src="../Scripts/_Libs/FanexGrid/Fanex.Pagination.js"></script>
    <script type="text/javascript" src="../Scripts/_Libs/FanexGrid/moment.js"></script>
    <script type="text/javascript" src="../Scripts/_Libs/FanexGrid/numeral.js"></script>
    <script src="../Areas/Statement/Views/Report/Scripts/StatementReportManager.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        window.rootUrl = '<%=rootUrl%>'; 
        var isInternalCustomer = <%=isInternalCustomer%>;
        var balanceUpDay = '<%=balanceUpDay%>';
        var moveBetsDay = '<%=moveBetsDay%>'; 
        var newLayoutDate = '<%=newLayoutDate%>';
        var domain =  window.rootUrl;
        var statementResources = {
            Invalid_Date_Range: '<%=Labels.Invalid_Date_Range%>',
            Date: "<%=Labels.Date%>",
            Opening_Balance: "<%=Labels.Opening_Balance%>",
            Turnover: "<%=Labels.Turnover%>",
            Void_Cancelled: '<%=(Labels.Void + "/" + Labels.Rejected)%>',
            Outstanding: "<%=Labels.Outstanding%>",
            Win_Loss: "<%=Labels.Win_Loss%>",
            Commission: "<%=Labels.Commission%>",
            Transfer_Amt: "<%=Labels.Transfer_Amt%>",
            Accumulated_Total: "<%=Labels.Accumulated_Total%>",
            Symbol_NoData: "<%=Labels.Symbol_NoData%>",
            Transfer_Details: "<%=Labels.Transfer_Details%>",
            Bet_List: '<%=Labels.Bet_List%>',
            Statement_Guidelines: '<%=Labels.Statement_Guidelines%>'
        };

        GetDataReport();
        jQuery(document).ready(function() {
            $("#notemessage").click(function() {
                viewGuidelines();
            });
        });
    </script>
    <script src="../js/jQuery/autocomplete/jquery.autocomplete.min.js" type="text/javascript"></script>
    <%--<script src="../js/IBCWnd/IBCWnd.js" type="text/javascript"></script>--%>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery-jtemplates.js" type="text/javascript"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script src="../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <script src="../js/jQuery/GridScroller/jquery.gridscroller.1.0.js" type="text/javascript"></script>
</body>
</html>
