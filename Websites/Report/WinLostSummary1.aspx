<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WinLostSummary1.aspx.cs"
    Inherits="Report_WinLostSummary1" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link rel="stylesheet" type="text/css" href="../Report/WinLostSummary/Styles/WinLossSummary.css?ver=<%=WebSettings.Version%>" />
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="WinLostSummary/Scripts/WinLostSummary.js" type="text/javascript"></script>
</head>
<body>
    <div class="main">
        <table style="width: 100%" border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <div id="divMainHeader" class="">
                        <div id="left_tab1">
                            <ul>
                                <li class="lefticon"></li>
                                <li class="current"><span>Win Loss Summary</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <form id="frm" name="frm" method="post" action="WinLostSummary1.aspx">
                            <asp:literal id="lblFromToDate" runat="server"></asp:literal>
                            </form>
                        </div>
                        <div class="contentBox">
                            <asp:literal id="content" runat="server"></asp:literal>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</body>
<script type="text/javascript">
    var toDay = '<%= ToDay %>';
    var fDay = '<%= FDay %>';
    var tDay = '<%= TDay %>';
    var minDay = '<%= MinDay %>';
    function GetData() {
        var params = '?fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val();
        location.href = "WinLostSummary1.aspx" + params;
    }

    function ExportToExcel() {
        var params = '?fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val() + "&isexport=1";
        document.location.href = "WinLostSummary1.aspx" + params;
    }

    function rowover(obj) { $(".tbllayout .overrow").removeClass("overrow"); $(obj).addClass("overrow"); }
    function rowout() { $(".tbllayout .overrow").removeClass("overrow"); }
</script>
</html>
