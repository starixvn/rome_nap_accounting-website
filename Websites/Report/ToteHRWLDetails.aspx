<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ToteHRWLDetails.aspx.cs" Inherits="Report_HRWLDetails" Theme=""  %>
<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <%-- <meta http-equiv="Cache-Control" content="no-cache" />--%>
    
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css"/> 
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css"/>               
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css"/>      
    <link rel="stylesheet" type="text/css" href="../App_Themes/Report/Report.css"/>
    <link href="../js/jQuery/Bubblepopup/css/jquery.bubblepopup.v2.3.1.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/Bubblepopup/jquery.bubblepopup.v2.3.1.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../js/Report.js" type="text/javascript"></script>
   
</head>
<body>
 
   <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC">Racing Daily Report (for Tote)</td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="sHeader" style="display:<%=display%>">
                    <asp:Literal ID="headerDatetime" runat="server"></asp:Literal>                        
                </div>
            </div>
            <div class="bg_table">
                <div id="sErrMsg" class="error-message" style="font-weight:bold"></div>
                <div id="sContent"></div>
                <input type="hidden" id="movebetsday" name="movebetsday" value="<%=ReportUtils.BalanceUp.MoveBetsDay %>"/>
                <input type="hidden" id="type" name="type" value="<%=Request["type"] %>" />
            </div>
        </div>
    </div> 
     <script type="text/javascript">
         var widthmainpage = 2320;
         var betlistpage = 'ToteHRBL.aspx';
         var mode = 8;
         var bettypename = 'Racing Daily Report (for Tote)';
         ViewReport();

         $(function () {
             $("#btnReport, #btnNetIncomeHelp, #btnAmountDueHelp").CreateBubblePopup();
            //             $("#sContent").delegate("#btnNetIncomeHelp, #btnAmountDueHelp", "hover", function () {
            //                 
            //             })
            //             $("#sContent").delegate("#btnAmountDueHelp", "hover", function () {
            //                 showPopup($("#btnAmountDueHelp"), "= [PT Win/Loss IBC] + [Comm Income IBC] - [Comm Expense] - [PGI/Amtote Fee]");
            //             })
            //             $("#btnReport").mouseover(function () {
            //                 showPopup($(this), "= [PT Win/Loss IBC] + [Comm Income IBC] - [Comm Expense] - [PGI/Amtote Fee]");
            //             });
         });
         function showPopup(obj, content) {
             obj.ShowBubblePopup({
                 innerHtml: content,
                 alwaysVisible: false,
                 closingDelay: 200,
                 themeName: 'azure',
                 themePath: '../js/jQuery/Bubblepopup/images/jquerybubblepopup-theme'
             });
         }
     </script>
</body>
</html>
