<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DailyCreatedCustomers.aspx.cs"
    Inherits="Report_DailyCreatedCustomers" Theme="" %>

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
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script type="text/javascript">
        var toDay = '<%= ToDay %>';
        var fDay = '<%= FDay %>';
        var tDay = '<%= TDay %>';
        $(document).ready(function () {
            var options = {
                showOtherMonths: true,
                selectOtherMonths: true,
                checkMinMax: true
            };
            calendarUtils.renderCalendar("#fdate", null, "#fdate", "", fDay, options);
            calendarUtils.renderCalendar("#tdate", null, "#fdate", "#tdate", tDay, options);
        });

        function GetCreatedCustomer() {
            var params = '?ViewDL=0&fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val();
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                url: 'DailyCreatedCustomers.ashx' + params,
                dataType: 'html',
                success: function (data) {
                    $('#parentContent').html(data);
                    $('#childContent').html('');
                    //                        $(".tbllayout tr:not(:.title, .report-footer, .title1)").mouseover(function(){rowover(this)});
                    //                        $(".tbllayout tr:not(:.title, .report-footer, .title1)").mouseout(function() { rowout() });
                    parent.jQuery.unblockUI();
                }
            });
        }

        function View_DL(roleid, date) {
            var params = '?ViewDL=1&roleid=' + roleid + '&date=' + encodeURIComponent(date);
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                url: 'DailyCreatedCustomers.ashx' + params,
                dataType: 'html',
                success: function (data) {
                    $('#childContent').html(data);
                    //                        $(".tbllayout tr:not(:.title.report-footer)").mouseover(function(){rowover(this)});
                    //                        $(".tbllayout tr:not(:.title.report-footer)").mouseout(function(){rowout()});
                    parent.jQuery.unblockUI();
                }
            });
        }

        function ExportToExcel() {
            var params = '?ViewDL=0&fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val() + "&isexport=1";
            document.location.href = "DailyCreatedCustomers.aspx" + params;
        }

        function rowover(obj) { $(".tbllayout .overrow").removeClass("overrow"); $(obj).addClass("overrow"); }
        function rowout() { $(".tbllayout .overrow").removeClass("overrow"); }
    </script>
</head>
<body>
    <asp:literal id="reportHeader" runat="server"></asp:literal>
</body>
</html>
