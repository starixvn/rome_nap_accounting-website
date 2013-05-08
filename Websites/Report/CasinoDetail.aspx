<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CasinoDetail.aspx.cs" Inherits="Report_HRWLDetails"
    Theme="" %>

<%@ Import Namespace="Sunplus.Accounting.DataAccess.Report" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=Resources.Language.lblAccountingSystem%></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../js/Utils/jquery.calendar.js" type="text/javascript"></script>
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
                                <li class="current"><span>Casino Win Lost</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <asp:literal id="headerDatetime" runat="server"></asp:literal>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div id="container" style="height: 655px;">
                                <iframe src="CasinoDetail_1.aspx?<%=param %>" frameborder="0" name="mainfr" id="mainfr"
                                    style="width: 100%; height: 90%;" onload="resize_iframe();"></iframe>
                            </div>
                            <input type="hidden" id="movebetsday" name="movebetsday" value="<%=ReportUtils.BalanceUp.MoveBetsDay %>" />
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
    });

    function resize_iframe() {
        var fheight = $('#container').height() - 10;
        $('#mainfr').height(fheight + "px");
    }
    // this will resize the iframe every
    // time you change the size of the window.
    window.onresize = resize_iframe;

    function obj(id) {
        return document.getElementById(id);
    }

    function showProgressBar(show) {
        if (show)
            obj("inProgressBar").style.display = "";
        else
            obj("inProgressBar").style.display = "none";
    }

    function ViewReport() {
        // var db  = (obj('rdMain').checked) ? 'main' : 'rep';            
        showProgressBar(true);
        var fdate = obj('fdate').value;
        var frm = obj('mainfr');
        //var url = 'CasinoDetail_1.aspx?type=Direct&click=1&bettype=Casino&WL=WL&wldate=' + fdate + '&db=' + db;            
        var url = 'CasinoDetail_1.aspx?type=Direct&click=1&bettype=Casino&WL=WL&wldate=' + fdate + '&db=rep';
        //            var movebetsday = new Date(obj('movebetsday').value);
        //            if (movebetsday >= new Date(fdate))
        //                obj('rdRep').checked = 'checked';
        //            
        frm.src = url;
    }

    function ExportToExcel() {
        //var db = (obj('rdMain').checked) ? 'main' : 'rep';
        var fdate = obj('fdate').value;
        var frm = obj('mainfr');
        var url = 'CasinoDetail_1.aspx?type=Direct&click=1&bettype=Casino&WL=WL&wldate=' + fdate + '&db=rep&IsExport=1';

        //            var movebetsday = new Date(obj('movebetsday').value);
        //            if (movebetsday >= new Date(fdate))
        //                obj('rdRep').checked = 'checked';

        frm.src = url;
    }
       
</script>
</html>
