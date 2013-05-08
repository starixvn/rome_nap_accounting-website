<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MatchWinLostByBetType.aspx.cs"
    Inherits="MatchWinLostByBetType" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../../js/FrozenHeader.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var limitFromDate = {
                minDate: minDay,
                maxDate: toDay
            };
            var options = {
                showOtherMonths: true,
                selectOtherMonths: true,
                checkMinMax: true
            };
            calendarUtils.renderCalendar("#fdate", limitFromDate, "#fdate", "", fDay, options);
            calendarUtils.renderCalendar("#tdate", limitFromDate, "#fdate", "#tdate", tDay, options);
        });
        function GetData() {
            var params = '?fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val();
            parent.BlockUI();
            $.ajax({
                url: 'MatchWinLostByBetType.ashx' + params,
                dataType: 'html',
                success: function (data) {
                    $('#sData').html(data);
                    InitFixedHeader(600, 'table', 'trFooter', 'trHeader');
                    parent.jQuery.unblockUI();
                }
            });
        }

        function ExportToExcel() {
            var params = '?fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val() + "&isexport=1";
            document.location.href = "MatchWinLostByBetType.aspx" + params;
        }
    
    </script>
</head>
<body>
    <asp:literal id="content" runat="server"></asp:literal>
</body>
<script type="text/javascript">
    var toDay = '<%= ToDay %>';
    var fDay = '<%= FDay %>';
    var tDay = '<%= TDay %>';
    var minDay = '<%= MinDay %>';
</script>
</html>
