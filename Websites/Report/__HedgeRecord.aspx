<%@ Page Language="C#" AutoEventWireup="true" CodeFile="__HedgeRecord.aspx.cs" Inherits="Report_HedgeRecord"
    Title="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/image.css" />
    <link rel="stylesheet" type="text/css" href="../js/IBCWnd/IBCWnd.css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/IBCWnd/IBCWnd.js"></script>
    <script src="../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script type="text/javascript">
        var toDay = '<%= ToDay %>';
        var fDay = '<%= FDay %>';
        var tDay = '<%= TDay %>';
        var minDay = '<%= MinDay %>';
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

        function confirm_delete(TransID) {
            if (confirm('Warning!!! Are you sure you want to delete this record?')) {
                var param = '?TransID=' + TransID;
                parent.BlockUI();
                $.ajax({
                    url: '../Handlers/DeleteHedgeRecord.ashx' + param,
                    dataType: 'html',
                    success: function (data) {
                        if (data.split('|')[0] == 'ok')
                            GetData();
                        else
                            $('#spErr').html("*" + data.split('|')[1]);
                        $(".tbllayout tr:not(:.title.report-footer)").mouseover(function () { rowover(this) });
                        $(".tbllayout tr:not(:.title.report-footer)").mouseout(function () { rowout() });
                        parent.jQuery.unblockUI();
                    }
                });
            }
        }

        function OpenEditCustInfo(id, type) {
            var url = "EditCustomerInfo.aspx?custId=" + id + "&type=" + type;
            window.open(url, "EditCust", "height=330,width=600,top=200,left=200");
        }
        function EditInfo(TransID, UserName) {
            var url = 'HedgeRecordEdit.aspx?TransID=' + TransID + '&UserName=' + UserName;
            ibcWnd.open(url, 'EditSuper', 150, 150, 600, 220);
        }

        $(document).ready(function () {
            $("#spErr").addClass("hidden");
        });
        function GetData() {
            var params = '?fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val();
            parent.BlockUI();
            $.ajax({
                url: '../Handlers/HedgeRecord.ashx' + params,
                dataType: 'html',
                success: function (data) {
                    $('#sData').html(data);
                    $(".tbllayout tr:not(:.title.report-footer)").mouseover(function () { rowover(this) });
                    $(".tbllayout tr:not(:.title.report-footer)").mouseout(function () { rowout() });
                    parent.jQuery.unblockUI();
                }
            });
        }

        function ExportToExcel() {
            var params = '?fdate=' + $('#fdate').val() + '&tdate=' + $('#tdate').val() + "&isexport=1";
            document.location.href = "HedgeRecord.aspx" + params;
        }

        function rowover(obj) { $(".tbllayout .overrow").removeClass("overrow"); $(obj).addClass("overrow"); }
        function rowout() { $(".tbllayout .overrow").removeClass("overrow"); }
    </script>
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
                                <li class="current"><span>Betting Graph</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <asp:literal id="SearchStandard" runat="server"></asp:literal>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="sData">
                                <span style="padding-left: 15px; color: Red;" id="spErr"></span>
                                <asp:literal id="content" runat="server"></asp:literal>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
