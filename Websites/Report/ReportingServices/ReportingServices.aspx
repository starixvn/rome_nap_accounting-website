<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportingServices.aspx.cs"
    Inherits="Report_ReportingServices" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Report/Report.css" />
    <link rel="stylesheet" type="text/css" href="~/App_Themes/Public/commonIOM.css" />
    <script type="text/javascript" src="../../js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="../../js/jQuery/jquery.blockUI.1.33.js"></script>
</head>
<body>
    <div id="dvContent">
        <%=html%>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
<script type="text/javascript">

    function GotoSubFolder(path) {
        parent.BlockUI();
        var param = 'path=' + path + '&mode=0';
        $.ajax({
            type: 'POST',
            url: 'GetDataReportingServices.ashx',
            data: param,
            async: true,
            success: function (rs) {
                $('#dvContent').html(rs);
                parent.jQuery.unblockUI();
            }
        });
    }

    function DownLoadFile(path) {
        $("#frm").attr('src', 'DownloadExcelFile.aspx?path=' + path);
    }
</script>
</html>
