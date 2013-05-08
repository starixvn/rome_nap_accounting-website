<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MonthlyWinloss.aspx.cs" Inherits="Report_NumberGame_MonthlyWinloss" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Number Game Monthly Winloss</title>
    <link type="text/css" rel="stylesheet" href="../../App_Themes/Public/commonIOM.css" />
    <link type="text/css" rel="stylesheet" href="../../App_Themes/Public/table.css" />
    <link type="text/css" rel="stylesheet" href="../../App_Themes/Public/input.css" />
    <link type="text/css" rel="stylesheet" href="../../App_Themes/Public/Message.css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link type="text/css" rel="stylesheet" href="../../App_Themes/Report/numbergamewinloss.css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/Datetime.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="JS/monthlywinlost.js" type="text/javascript"></script>
</head>
<body>
    <form id="frmreport" action="">
    <div class="main">
        <table style="width: 100%" border="0" cellpadding="0px" cellspacing="0px">
            <tr>
                <td>
                    <div id="divMainHeader" class="">
                        <div id="left_tab1">
                            <ul>
                                <li class="lefticon"></li>
                                <li class="current"><span>Number Game Monthly Winloss</span></li>
                            </ul>
                        </div>
                    </div>
                    <div class="maincontent">
                        <div class="filterBox">
                            <div class="filterContent">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                Select month:
                                            </td>
                                            <td>
                                                <select id="cbMonth" style="width: 90px">
                                                </select>
                                            </td>
                                            <td>
                                                <input type="button" id="btnReport" name="btnReport" value="REPORT" class="btn" onclick="GetReport()" />
                                            </td>
                                            <td>
                                                <input type="button" title="Export to Excel" class="btnExcel" disabled="disabled"
                                                    id="btnExport" name="btnExport" value="  " onclick="ExportToExcel();" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div class="contentBox">
                            <div class="content-warper contentWidth" id="divContent" style="display: none">
                                <table class="tbl-header-scroll contentWidth">
                                    <tbody>
                                        <tr class="bgborder">
                                            <td>
                                                <div class="col100 c">
                                                    Winlost</div>
                                            </td>
                                            <td>
                                                <div class="col100 c">
                                                    Position</div>
                                            </td>
                                            <td>
                                                <div class="col100 c">
                                                    Commission</div>
                                            </td>
                                            <td>
                                                <div class="col100 c">
                                                    Line Total</div>
                                            </td>
                                            <td>
                                                <div class="col100 c">
                                                    Turnover</div>
                                            </td>
                                            <td>
                                                <div class="col100 c">
                                                    Margin</div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table id="tbContent" class="tbl-body-scroll contentWidth">
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <iframe id="frmExport" width="0" height="0" frameborder="0" style="display: none">
    </iframe>
    <script type="text/javascript">
        $(document).ready(function () {
            var currentMonth = <%=_month%>
            var currentYear = <%=_year %>
            RenderMonthly("cbMonth", 2010, currentYear, currentMonth);
        });
    </script>
</body>
</html>
