<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GamesActiveMembers.aspx.cs"
    Inherits="Report_MemberStatistics_GamesActiveMembers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/Message.css" />
    <link rel="stylesheet" type="text/css" href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" />
    <link rel="stylesheet" type="text/css" href="Style/ActiveMembers.css" />
    <script src="../../js/jQuery/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/Utils/jquery.calendar.js" type="text/javascript"></script>
    <script src="JS/ActiveMembers.js" type="text/javascript"></script>
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
                                <li class="current"><span>Number of Active Members</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <span class="padright b">From:</span>
                                <input type="text" id="fdate" readonly="readonly" class="txtdate" style="width: 100px;
                                    background-color: white;" />
                                <span class="pL10 b">To:</span>
                                <input type="text" id="tdate" readonly="readonly" class="txtdate" style="width: 100px;
                                    background-color: white;" />
                                <span style="padding-left: 5px; vertical-align: middle;">
                                    <input type="button" id="btnRefesh" value="Report" onclick="getReport();" class="btn" />
                                </span><span style="padding-left: 5px; vertical-align: middle;">
                                    <input type="button" onclick="Export2Excel();" value="   " name="btnExport" id="btnExportIcon"
                                        class="btnExcel" title="Export to Excel" />
                                </span>
                            </div>
                            <div id="err">
                            </div>
                        </div>
                        <div class="contentBox activemember-form">
                            <div class="activemememer-mainform" style="display: none;" id="divMainActiveMembers">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
</body>
<script type="text/javascript">
    var toDay = '<%= ToDay %>';
    var fDay = '<%= FDay %>';
    var tDay = '<%= TDay %>';
</script>
</html>
